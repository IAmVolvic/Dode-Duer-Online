using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DataAccess.Models;
using DataAccess.Types.Enums;

namespace Service.TransferModels.Responses;

public class WinningNumbersResponseDTO
{ 
    public Guid Gameid { get; set; }
    
    [MinLength(3, ErrorMessage = "Winning Numbers must be at least 3 numbers.")]
    [MaxLength(3, ErrorMessage = "Winning Numbers must be no more than 3 numbers.")]
    public List<int> Winningnumbers { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public GameStatus Status { get; set; }

    
    public static WinningNumbersResponseDTO FromGame(Game game, List<int> winningNumbers)
    {
        return new WinningNumbersResponseDTO()
        {
            Gameid = game.Id,
            Winningnumbers = winningNumbers,
            Status = game.Status
        };
    }
}