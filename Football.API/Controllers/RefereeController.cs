using Football.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Football.API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class RefereeController : ControllerBase
    {
        readonly FootballContext footballContext;
        private readonly ILogger<MatchController> _logger;
        public RefereeController(FootballContext footballContext, ILogger<MatchController> logger)
        {
            this.footballContext = footballContext;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Referee>> Get()
        {            
            try
            {
                return this.Ok(footballContext.Referees);
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

            var response = footballContext.Referees.Find(id);

            if (response == default)
                return this.NotFound();

            return this.Ok();
        }

        [HttpPost]
        public ActionResult Post(Referee referee)
        {
            if (referee == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = footballContext.Referees.Add(referee).Entity;

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
        public ActionResult Update(int id, Referee referee)
        {
            if (id <= 0 || referee == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var refereeTemp = footballContext.Referees.Find(id);

            if (refereeTemp == default)
                return this.NotFound();

            refereeTemp.Name = referee.Name;
            refereeTemp.MinutesPlayed = referee.MinutesPlayed;

            try
            {
                footballContext.Referees.Update(refereeTemp);
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

            var response = footballContext.Referees.Find(id);
            if (response == default)
                this.NotFound();

            try
            {
                footballContext.Referees.Remove(response);
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
