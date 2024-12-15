using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using Service.Services.Interfaces;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;

namespace Service.Services;

public class BoardService(IBoardRepository boardRepository, IPriceRepository priceRepository, IGameRepository gameRepository) : IBoardService
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
        var newBoard = boardRepository.PlayBoard(board);
        return new BoardResponseDTO().FromBoard(newBoard);
    }

    public List<BoardResponseDTO> GetBoards()
    {
        var boards = boardRepository.GetBoards();
        var boardsDto = boards.Select(b => new BoardResponseDTO().FromBoard(b)).ToList();
        return boardsDto;
    }
    
    public List<BoardResponseDTO> IdentifyWinners(Guid gameId, int winningNumber)
    {
        var boards = boardRepository.GetBoards().Where(b => b.Gameid == gameId).ToList();
        var winningBoards = boards.Where(b => b.Chosennumbers.Any(n => n.Number == winningNumber)).ToList();
        var winners = winningBoards.Select(b => new BoardResponseDTO().FromBoard(b)).ToList();
            
        return winners;
    }

}