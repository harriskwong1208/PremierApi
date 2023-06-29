namespace SoccerApi.Models.DTO
{
    public class LeagueDto
    {
        public Guid Id { get; set; }
        public string HomeLeague { get; set; }
        public string? EuropeLeague { get; set; }
    }
}
