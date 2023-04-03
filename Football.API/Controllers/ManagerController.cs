using Football.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System;

namespace Football.API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        readonly FootballContext footballContext;

        private readonly ILogger<ManagerController> _logger;
        
        public ManagerController(FootballContext footballContext, ILogger<ManagerController> logger)
        {
            this.footballContext = footballContext;
            _logger = logger;
        }


        [HttpGet]        
        public ActionResult<IEnumerable<Manager>> Get()
        {
            try
            {
                return this.Ok(footballContext.Managers);
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

            var response = footballContext.Managers.Find(id);
            
            if (response == default)
                return this.NotFound();
            
            return this.Ok(response.Name);
        }


        [HttpPost]
        public ActionResult Post(Manager manager)
        {
            if (manager == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = footballContext.Managers.Add(manager).Entity;

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
        public ActionResult Update(int id, Manager manager)
        {
            if(id <= 0 || manager == null || !ModelState.IsValid) 
                return BadRequest(ModelState);

            var managerTemp = footballContext.Managers.Find(id);

            if (managerTemp == default)
                return this.NotFound();

            managerTemp.Name = manager.Name;
            managerTemp.RedCard = manager.RedCard;
            managerTemp.YellowCard = manager.YellowCard;

            try
            {
                footballContext.Managers.Update(managerTemp);
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

            var response = footballContext.Managers.Find(id);
            if (response == default)
                this.NotFound();

            try
            {
                footballContext.Managers.Remove(response);
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
