using System.Globalization;
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
    
    public List<WinnerResponseDTO> IdentifyWinners(Guid gameId, List<int> winningNumbers)
    {
        var boards = boardRepository.GetBoards()
            .Where(b => b.Gameid == gameId)
            .ToList();
        
        var winningBoards = boards.Where(b =>
        {
            var chosenNumbers = b.Chosennumbers.Select(n => n.Number ?? 0).ToList();
            var matchedNumbersCount = winningNumbers.Intersect(chosenNumbers).Count();
            return matchedNumbersCount == 3;
        }).ToList();

        var totalRevenue = boards.Sum(b => b.Price.Price1); // Calculate total revenue (sum of all board prices)
        
        var prizePool = totalRevenue * 0.7m; // Calculate prize pool: 70% of revenue
        
        var totalWinners = winningBoards.Count;
        
        var wonAmount = totalWinners > 0 ? prizePool / totalWinners : 0m;  // Divide prize pool between winners
        var winnerEntities = winningBoards.Select(b => new Winner
        {
            Id = Guid.NewGuid(),
            Gameid = gameId,
            Userid = b.Userid,
            Wonamount = wonAmount
        }).ToList();

        gameRepository.SaveWinners(winnerEntities);

        var winnersResponse = winnerEntities.Select(w => new WinnerResponseDTO
        {
            UserName = w.User.Name,  
            WonAmount = w.Wonamount,
            WeekNumber = ISOWeek.GetWeekOfYear(DateTime.Now)
        }).ToList();

        return winnersResponse;
    }
}

