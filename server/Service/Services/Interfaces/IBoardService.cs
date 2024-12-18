using Service.TransferModels.Requests;
using Service.TransferModels.Responses;

namespace Service.Services.Interfaces;

public interface IBoardService
{
    public BoardResponseDTO PlayBoard(PlayBoardDTO playBoardDTO);
    public List<BoardResponseDTO> GetBoards();
    public List<BoardGameResponseDTO> GetBoardsFromGame(Guid gameId);
    public AutoplayBoardDTO AutoplayBoard(PlayAutoplayBoardDTO playAutoplayBoardDTO);
    public void PlayAllAutoplayBoards();
    public List<AutoplayBoardDTO> GetAutoplayBoards(Guid userId);
    
    public MyBoards[] GetAllMyBoards(Guid userId);
}