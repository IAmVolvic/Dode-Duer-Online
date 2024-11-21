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
        
    }
}