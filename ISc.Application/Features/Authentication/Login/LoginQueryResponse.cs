namespace ISc.Application.Features.Authentication.Login
{
    public class LoginQueryResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public IList<string> Roles { get; set; }
        public string Token { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
