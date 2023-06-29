﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoccerApi.Data;
using SoccerApi.Models.DTO;
using SoccerApi.Models;

namespace SoccerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly SoccerDbContext dbContext;
        public TeamsController(SoccerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {

            var teams = dbContext.Teams.ToList();

            //Map Models to DTOs
            var teamDto = new List<TeamDto>();
            foreach (var team in teams)
            {
                teamDto.Add(new TeamDto()
                {
                          
                    Id = team.Id,
                    Name = team.Name,
                    Country = team.Country,
                    City = team.City
                });
            }
            //Return Dto back to client
            return Ok(teamDto);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var team = dbContext.Teams.Find(id);
            if (team == null)
            {
                return NotFound();
            }
            //Map domain model to Dto
            var teamDto = new TeamDto
            {
                Id = team.Id,
                Name = team.Name,
                Country = team.Country,
                City = team.City
            };

            return Ok(teamDto);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TeamRequest teamRequest)
        {
            //Map Dto to model
            var teamModel = new Team
            {

                Name = teamRequest.Name,
                Country = teamRequest.Country,
                City = teamRequest.City
            };

            //Use model to create League, add to database
            dbContext.Teams.Add(teamModel);
            dbContext.SaveChanges();

            //Map model back to Dto
            var teamDto = new TeamDto
            {
                Id = teamModel.Id,
                Name = teamModel.Name,
                Country = teamModel.Country,
                City = teamModel.City

            };
            //Returns 201 after creation, sends the new Id of the item just created
            return CreatedAtAction(nameof(GetById), new { id = teamModel.Id }, teamDto);
        }
    }

}
