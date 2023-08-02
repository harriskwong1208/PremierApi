using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoccerApi.Data;
using SoccerApi.Models.DTO;
using SoccerApi.Models;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult> GetAll()
        {

            var teams = await dbContext.Teams.ToListAsync();

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
        public async Task<ActionResult> GetById([FromRoute] Guid id)
        {
            var team = await dbContext.Teams.FindAsync(id);
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
        public async Task<ActionResult> Create([FromBody] TeamRequest teamRequest)
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
            await dbContext.SaveChangesAsync();

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

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            var team =  await dbContext.Teams.FindAsync(id);
            if (team != null)
            {
                dbContext.Remove(team);
                 await dbContext.SaveChangesAsync();
                return Ok(team);
            }
            return NotFound();
        }


        [HttpPut]
        [Route("{id:guid}")]  
        public async Task<ActionResult> Update([FromRoute] Guid id, TeamRequest teamRequest)
        {
            //Try to find the id in the database
            var team = await dbContext.Teams.FindAsync(id);
            if (team != null)
            {
                team.City = teamRequest.City;
                team.Country = teamRequest.Country;
                team.Name = teamRequest.Name;


                //save the database after making changes
               await  dbContext.SaveChangesAsync();
                return Ok(team);
            }

            //return saying the id is not found in the database
            return NotFound();
        }
    }

}
