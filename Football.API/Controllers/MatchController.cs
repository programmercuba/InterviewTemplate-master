using Football.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System;

namespace Football.API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly FootballContext footballContext;

        private readonly ILogger<MatchController> _logger;
        public MatchController(FootballContext footballContext, ILogger<MatchController> logger)
        {
            this.footballContext = footballContext;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Match>> Get()
        {
            try
            {
                return this.Ok(footballContext.Matches);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error system" + ex.Message);
                footballContext.Dispose();
                return this.NotFound();
            }

        }

        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            if (id <= 0) return this.NotFound();

            var response = footballContext.Matches.Find(id);

            if (response == default)
                return this.NotFound();

            return this.Ok();
        }

        [HttpPost]
        public ActionResult Post(Match match)
        {
            if (match == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = footballContext.Matches.Add(match).Entity;

            try
            {
                footballContext.SaveChanges();

                return this.CreatedAtAction("GetById", response.Id, response);
            }
            catch (Exception ex)
            {

                _logger.LogError("System error" + ex.Message);
                footballContext.Dispose();
                return NotFound();
            }


        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, Match match)
        {            
            if (id <= 0 || match == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var matchTemp = footballContext.Matches.Find(id);

            if (matchTemp == default)
                return this.NotFound();

            matchTemp.Referee = match.Referee;
            matchTemp.AwayManager = match.AwayManager;
            matchTemp.AwayPlayers = match.AwayPlayers;
            matchTemp.HouseManager = match.HouseManager;
            matchTemp.HousePlayers = match.HousePlayers;

            try
            {
                footballContext.Matches.Update(matchTemp);
                footballContext.SaveChanges();
                return this.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("System error" + ex.Message);
                footballContext.Dispose();
                return NotFound();
            }


        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id) 
        {            
            if (id <= 0)
                return BadRequest();

            var response = footballContext.Matches.Find(id);
            if (response == default)
                this.NotFound();

            try
            {
                footballContext.Matches.Remove(response);
                footballContext.SaveChanges();
                return this.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("System error" + ex.Message);
                footballContext.Dispose();
                return NotFound();
            }
        }


    }
}
