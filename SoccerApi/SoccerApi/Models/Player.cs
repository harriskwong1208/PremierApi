namespace SoccerApi.Models
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Goal { get; set; }
        public int Assist { get; set; }
        public string Country { get; set; }

        public Guid TeamId { get; set; } 
        public Guid LeagueId { get; set; }
        public Team Team { get; set; }
        public League League { get; set; }  
   
    }
}
