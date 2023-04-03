using Football.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Football.API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        readonly FootballContext footballContext;
        private readonly ILogger<MatchController> _logger;
        public PlayerController(FootballContext footballContext, ILogger<MatchController> logger)
        {
            this.footballContext = footballContext;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Player>> Get()
        {
            try
            {
                return this.Ok(footballContext.Players);
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

            var response = footballContext.Players.Find(id);

            if (response == default)
                return this.NotFound();

            return this.Ok();
        }

        [HttpPost]
        public ActionResult Post(Player player)
        {            
            if (player == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = footballContext.Players.Add(player).Entity;

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
        public ActionResult Update(int id, Player player)
        {            
            if (id <= 0 || player == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var playerTemp = footballContext.Players.Find(id);

            if (playerTemp == default)
                return this.NotFound();

            playerTemp.Name = player.Name;
            playerTemp.MinutesPlayed = player.MinutesPlayed;
            playerTemp.YellowCard = player.YellowCard;
            playerTemp.RedCard = player.RedCard;

            try
            {
                footballContext.Players.Update(playerTemp);
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

            var response = footballContext.Players.Find(id);
            if (response == default)
                this.NotFound();

            try
            {
                footballContext.Players.Remove(response);
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
