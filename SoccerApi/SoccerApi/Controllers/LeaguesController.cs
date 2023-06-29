using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoccerApi.Data;
using SoccerApi.Migrations;
using SoccerApi.Models.DTO;
using SoccerApi.Models;

namespace SoccerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaguesController : ControllerBase
    {
        private readonly SoccerDbContext dbContext;
        public LeaguesController(SoccerDbContext dbContext) {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAll() {

            var leagues = dbContext.Leagues.ToList();

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
        public IActionResult GetById([FromRoute] Guid id) {
            var league = dbContext.Leagues.Find(id);
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
        public IActionResult Create([FromBody] LeagueRequest leagueRequest)
        {
            //Map Dto to model
            var leagueModel = new League{
                EuropeLeague = leagueRequest.EuropeLeague,
                HomeLeague = leagueRequest.HomeLeague
            };

            //Use model to create League, add to database
            dbContext.Leagues.Add(leagueModel);
            dbContext.SaveChanges();

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
    }
}
