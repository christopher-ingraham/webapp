namespace DA.WI.NSGHSM.IdentityServer.Models
{
    public class UserInfo
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Department { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
