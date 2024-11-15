using System.ComponentModel.DataAnnotations;

namespace Service;

public sealed class AppOptions
{
    [Required] public string DefaultConnection { get; set; }
}