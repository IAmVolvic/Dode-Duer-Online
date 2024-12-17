using API.Exceptions;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using Service.Services.Interfaces;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;

namespace Service.Services;

public class BoardService(IBoardRepository boardRepository, IPriceRepository priceRepository, IGameRepository gameRepository, IUserService userService, IGameService gameService) : IBoardService
{
    public BoardResponseDTO PlayBoard(PlayBoardDTO playBoardDTO)
    {
        var board = new Board();
        board.Userid = playBoardDTO.Userid;
        board.Dateofpurchase = playBoardDTO.Dateofpurchase;
        board.Gameid = gameRepository.GetActiveGame().Id;
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
            var prize = GetBoards().Sum(b => b.Price);
            gameService.UpdatePrizePool(prize);
            return new BoardResponseDTO().FromBoard(newBoard);
        }
        throw new ErrorException("BoardService","Board could not be played");
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

    public List<WinnersDto> EstablishWinners(Guid gameId)
    {
        var game = gameService.getGameById(gameId);
        var prize = game.Prize * 0.7m;
        var boards = GetBoardsFromGame(gameId);
        var winningNumbers = gameService.GetWinningNumbers(gameId);
        var winningBoards = new List<BoardGameResponseDTO>();
        var winners = new List<WinnersDto>();
        var numberOfWinners = 0;
        foreach (var b in boards)
        {
            if (winningNumbers.All(win => b.Numbers.Contains(win.Number)))
            {
                winningBoards.Add(b);
                numberOfWinners++;
            }
        }
        foreach (var b in winningBoards)
        {
            if (winners.First() == null || winners.Where(w => w.UserId == b.userId) == null)
            {
                
            }
        }
    }
}