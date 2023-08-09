namespace SoccerApi.Models.DTO
{
    public class UserDto
    {

        public Guid Id { get; set; }

        public required string Username { get; set; } = string.Empty;

        //public string PasswordHash { get; set; } = string.Empty;

        public string Password { get; set; }    
    }
}
