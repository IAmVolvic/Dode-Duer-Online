using Service.TransferModels.Requests;
using Service.TransferModels.Responses;

namespace Service.Services.Interfaces;

public interface IBoardService
{
    public BoardResponseDTO PlayBoard(PlayBoardDTO playBoardDTO);
    public List<BoardResponseDTO> GetBoards();
    public List<BoardResponseDTO> IdentifyWinners(Guid gameId, int winningNumber);

}