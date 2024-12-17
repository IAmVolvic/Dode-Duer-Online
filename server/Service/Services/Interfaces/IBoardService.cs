using DataAccess.Models;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;

namespace Service.Services.Interfaces;

public interface IBoardService
{
    public BoardResponseDTO PlayBoard(PlayBoardDTO playBoardDTO);
    public List<BoardResponseDTO> GetBoards();
    List<WinnerResponseDTO> IdentifyWinners(Guid gameId, List<int> winningNumbers);
    List<WinnerResponseDTO> GetWinners(Guid gameId);
   
}