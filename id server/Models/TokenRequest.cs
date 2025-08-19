namespace DA.WI.NSGHSM.IdentityServer.Models
{
    public class TokenRequest
    {
        public required string GrantType { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? RefreshToken { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
