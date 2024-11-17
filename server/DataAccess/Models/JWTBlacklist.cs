namespace DataAccess.Models;

public partial class JWTBlacklist
{
    public Guid Id { get; set; }
    public string Jti { get; set; }
    public DateTime BlacklistExpiry { get; set; }
}