﻿namespace SoccerApi.Models.DTO
{
    public class PlayerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Goal { get; set; }
        public int Assist { get; set; }
        public string Country { get; set; }

        public Guid TeamId { get; set; }
        public Guid LeagueId { get; set; }

    }
}
