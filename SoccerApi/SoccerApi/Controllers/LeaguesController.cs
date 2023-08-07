using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoccerApi.Data;
using SoccerApi.Migrations;
using SoccerApi.Models.DTO;
using SoccerApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace SoccerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController,Authorize]
    public class LeaguesController : ControllerBase
    {
        private readonly SoccerDbContext dbContext;
        public LeaguesController(SoccerDbContext dbContext) {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll() {

            var leagues = await dbContext.Leagues.ToListAsync();

            //Map Models to DTOs
            var leagueDto = new List<LeagueDto>();
            foreach (var league in leagues)
            {
                leagueDto.Add(new LeagueDto()
                {
                    Id = league.Id,
                    HomeLeague = league.HomeLeague,
                    EuropeLeague = league.EuropeLeague
                }) ;
            }
            //Return Dto back to client
            return Ok(leagueDto);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<ActionResult> GetById([FromRoute] Guid id) {
            var league = await dbContext.Leagues.FindAsync(id);
            if(league == null)
            {
                return NotFound();
            }
            //Map domain model to Dto
            var leagueDto = new LeagueDto
            {
                Id = league.Id,
                HomeLeague = league.HomeLeague,
                EuropeLeague = league.EuropeLeague
            };

            return Ok(leagueDto);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] LeagueRequest leagueRequest)
        {
            //Map Dto to model
            var leagueModel = new League{
                EuropeLeague = leagueRequest.EuropeLeague,
                HomeLeague = leagueRequest.HomeLeague
            };

            //Use model to create League, add to database
            dbContext.Leagues.Add(leagueModel);
            await dbContext.SaveChangesAsync();

            //Map model back to Dto
            var leagueDto = new LeagueDto
            {
                Id = leagueModel.Id,
                HomeLeague = leagueModel.HomeLeague,
                EuropeLeague = leagueModel.EuropeLeague
            };
            //Returns 201 after creation, sends the new Id of the item just created
            return CreatedAtAction(nameof(GetById),new {id = leagueModel.Id},leagueDto);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            var league = await dbContext.Leagues.FindAsync(id);
            if (league != null)
            {
                dbContext.Remove(league);
                await dbContext.SaveChangesAsync();
                return Ok(league);
            }
            return NotFound();
        }


        [HttpPut]
        [Route("{id:guid}")]
        public async Task<ActionResult> Update([FromRoute] Guid id, LeagueRequest leagueRequest)
        {
            //Try to find the id in the database
            var league = await dbContext.Leagues.FindAsync(id);
            if (league != null)
            {

         

                league.HomeLeague = leagueRequest.HomeLeague;
                league.EuropeLeague = leagueRequest.EuropeLeague;


                //save the database after making changes
                await dbContext.SaveChangesAsync();
                return Ok(league);
            }

            //return saying the id is not found in the database
            return NotFound();
        }
    }
}
