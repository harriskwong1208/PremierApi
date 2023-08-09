namespace SoccerApi.Models.DTO
{
    public class UserRequest
    {
        public string Username { get; set; } = string.Empty;

        //public string Password {get; set;} = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;
    }
}
