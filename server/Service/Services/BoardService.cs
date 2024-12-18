using API.Exceptions;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.Types.Enums;
using Service.Services.Interfaces;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;

namespace Service.Services;

public class BoardService(IBoardRepository boardRepository, IPriceRepository priceRepository, IWinnersRepository winnersRepository ,IUserService userService, IGameService gameService) : IBoardService
{
    public BoardResponseDTO PlayBoard(PlayBoardDTO playBoardDTO)
    {
        if (gameService.GetActiveGame().Enddate > DateTime.Now)
        {
            var board = new Board();
            board.Userid = playBoardDTO.Userid;
            board.Dateofpurchase = playBoardDTO.Dateofpurchase;
            board.Gameid = gameService.GetActiveGame().Id;
            board.Id = Guid.NewGuid();
            board.Priceid = priceRepository.GetPrice(playBoardDTO.Numbers.Count).Id;
            board.Chosennumbers = playBoardDTO.Numbers.Select(n => new Chosennumber
                {
                    Id = Guid.NewGuid(), 
                    Boardid = board.Id, 
                    Number = n 
                })
                .ToList();
            var price = priceRepository.GetPrice(playBoardDTO.Numbers.Count).Price1;
            var newBoard = boardRepository.PlayBoard(board);
            if (newBoard != null)
            {
                userService.UpdateUserBalance(price,newBoard.Userid);
                var prize = GetBoards().Where(b=> b.Gameid == gameService.GetActiveGame().Id).Sum(b => b.Price) + gameService.GetActiveGame().StartingPrizepool;
                gameService.UpdatePrizePool(prize);
                return new BoardResponseDTO().FromBoard(newBoard);
            }
            throw new ErrorException("BoardService","Board could not be played");
        }
        else
        {
            throw new ErrorException("BoardService","Board could not be played because game is not active");
        }
        
    }

    public List<BoardResponseDTO> GetBoards()
    {
        var boards = boardRepository.GetBoards();
        var boardsDto = boards.Select(b => new BoardResponseDTO().FromBoard(b)).ToList();
        return boardsDto;
    }

    public List<BoardGameResponseDTO> GetBoardsFromGame(Guid gameId)
    {
        var boards = boardRepository.GetBoardsFromGame(gameId);
        var boardsDto = boards.Select(b => new BoardGameResponseDTO().FromBoard(b)).ToList();
        return boardsDto;
    }

    public AutoplayBoardDTO AutoplayBoard(PlayAutoplayBoardDTO playAutoplayBoardDTO)
    {
        var board = new BoardAutoplay();
        board.Id = Guid.NewGuid();
        board.UserId = playAutoplayBoardDTO.Userid;
        board.LeftToPlay = playAutoplayBoardDTO.LeftToPlay;
        board.ChosenNumbersAutoplays = playAutoplayBoardDTO.Numbers.Select(n => new ChosenNumbersAutoplay()
            {
                Id = Guid.NewGuid(), 
                BoardId = board.Id, 
                Number = n 
            })
            .ToList();

        var newBoard = boardRepository.AutoplayBoard(board);
        return new AutoplayBoardDTO().FromBoard(newBoard);
    }

    public void PlayAllAutoplayBoards()
    {
        var boardsToPlay = boardRepository.GetAutoplayBoards();
        var adjustedBoards = AdjustLeftToPlay(boardsToPlay);
        

        var boards = adjustedBoards.Select(b => new PlayBoardDTO().fromAutoplay(b)).ToList();

        foreach (var board in boards)
        {
            var userBalance = userService.CheckUsersBalance(board.Userid);
          
            var price = priceRepository.GetPrice(board.Numbers.Count());

            if (userBalance >= price.Price1)
            {
                PlayBoard(board);
            }
        }

        foreach (var board in adjustedBoards)
        {
            board.LeftToPlay -= 1;
            boardRepository.AdjustLeftToPlay(board);
        }
    }


    public List<BoardAutoplay> AdjustLeftToPlay(List<BoardAutoplay> boards)
    {
        var boardsLeft = new List<BoardAutoplay>();
        foreach (var board in boards)
        {
            if (board.LeftToPlay-1 <1)
            {
                boardRepository.DeleteBoardLeftToPlay(board);
            }
            else
            {
                boardsLeft.Add(board);
            }
        }

        return boardsLeft;
    }

    public List<AutoplayBoardDTO> GetAutoplayBoards(Guid userId)
    {
        var autoplay = boardRepository.GetAutoplayBoards(userId);
        return autoplay.Select(b => new AutoplayBoardDTO().FromBoard(b)).ToList();
    }
    
    
    public MyBoards[] GetAllMyBoards(Guid userId)
    {
        var userBoards = boardRepository.GetBoards().Where(b => b.Userid == userId).ToList();
        var gameIds = new HashSet<Guid>();
        var myBoardsList = new List<MyBoards>();
        
        // Check if user has played any boards
        if (!userBoards.Any()) { return null; }
        
        // foreach games are played under this user
        foreach (var board in userBoards)
        {
            if (!gameIds.Contains(board.Gameid))
            {
                gameIds.Add(board.Gameid);

                var game = board.Game;
                var winningBoards = GetWinningBoardsFromGame(game.Id);
                
                var playerBoardsUnderGame = new List<UserBoard>();

                foreach (var userBoard in userBoards)
                {
                    if (board.Gameid == userBoard.Gameid)
                    {
                        var prize = 0m;
                        if (winningBoards.Any(b=> b.Id == userBoard.Id) && game.Status == GameStatus.Inactive)
                        {
                            prize = game.Prizepool * 0.7m / winningBoards.Count();
                        }
                        playerBoardsUnderGame.Add(UserBoard.FromEntity(userBoard, prize));
                    }
                }
                
                myBoardsList.Add(MyBoards.FromEntity(game, playerBoardsUnderGame));
            }
        }

        return myBoardsList.ToArray();
    }

    public List<WinnersDto> EstablishWinners(Guid gameId)
    {
        var game = gameService.getGameById(gameId);
        var prize = game.Prize * 0.7m;
        var boards = GetBoardsFromGame(gameId);
        var winningNumbers = gameService.GetWinningNumbers(gameId);
        var winningBoards = new List<BoardGameResponseDTO>();
        
        foreach (var b in boards)
        {
            if (winningNumbers.All(win => b.Numbers.Contains(win.Number)))
            {
                winningBoards.Add(b);
            }
        }

        var prizePerBoard = 0m;
        if (winningBoards.Any())
        {
            prizePerBoard = prize / winningBoards.Count;
        }
        
        if (prizePerBoard>5000)
        {
            prizePerBoard = 5000;
        }
        var winners = new List<WinnersDto>();
        foreach (var b in winningBoards)
        {
            if (!winners.Any() || !winners.Any(w => w.UserId == b.userId))
            {
                var newWinner = new WinnersDto()
                {
                    Gameid = gameId,
                    Name = b.User,
                    Prize = prizePerBoard,
                    UserId = b.userId,
                    NumberOfWinningBoards = 1,
                };
                newWinner.WinningBoards.Add(b);
                winners.Add(newWinner);
            }
            else
            {
                var winner = winners.Find(w => w.UserId == b.userId);
                winner.NumberOfWinningBoards += 1;
                if (winner.Prize + prizePerBoard > 5000)
                {
                    winner.Prize = 5000;
                }
                else
                {
                    winner.Prize += prizePerBoard;
                }
                winner.WinningBoards.Add(b);
            }
        }

        var prizeLeft = 0m;
        if (!winners.Any())
        {
            prizeLeft = prize;
        }
        else
        {
            var prizeGiven = winners.Sum(w => w.Prize);
            prizeLeft = (prize - prizeGiven);
            winnersRepository.AddWinners(winners.Select(w => w.ToWinner()).ToList());
        }
        gameService.NewGame(prizeLeft);
        PlayAllAutoplayBoards();
        return winners;
    }

    public List<BoardGameResponseDTO> GetWinningBoardsFromGame(Guid gameId)
    {
        var boards = boardRepository.GetBoardsFromGame(gameId).Select(b => new BoardGameResponseDTO().FromBoard(b)).ToList();
        var winningNumbers = gameService.GetWinningNumbers(gameId);
        var winningBoards = new List<BoardGameResponseDTO>();
        
        foreach (var b in boards)
        {
            if (winningNumbers.All(win => b.Numbers.Contains(win.Number)))
            {
                winningBoards.Add(b);
            }
        }

        return winningBoards;
    }
}