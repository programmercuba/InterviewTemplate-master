using Microsoft.AspNetCore.Mvc;
using System;
using Football.API.Models.Output;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Football.API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        readonly FootballContext footballContext;
        private readonly ILogger<MatchController> _logger;

        public StatisticsController(FootballContext footballContext, ILogger<MatchController> logger)
        {
            this.footballContext = footballContext;
            _logger = logger;
        }


        [HttpGet("yellowcards")]       
        public ActionResult GetYellowCards()
        {
            List<Statistics> result = new List<Statistics>();
            
            try
            {
                result = GetStatistics(footballContext.Managers, StatisticsType.YellowCard);
                result.AddRange(GetStatistics(footballContext.Players, StatisticsType.YellowCard));
                return this.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error system" + ex.Message);
                footballContext.Dispose();
                return this.NotFound();
            }

        }

        //Método privado que recibe un IEnumerable y el tipo de estadística a consultar 
        private List<Statistics> GetStatistics(IEnumerable<dynamic> list, StatisticsType statisticsType) 
        {
            List<Statistics> result = new List<Statistics>();
           
            foreach (var item in list)
            {
                var statistics = new Statistics { Id = item.Id, Name = item.Name };

                switch (statisticsType)
                {
                    case StatisticsType.YellowCard:
                        statistics.Total = item.YellowCard;
                        break;
                    case StatisticsType.RedCard:
                       statistics.Total = item.RedCard;
                        break;
                    case StatisticsType.MinutesPlayed:
                        statistics.Total = item.MinutesPlayed;
                        break;
                    default:
                        break;
                }

                result.Add(statistics);
            }

            return  result;

        }

        [HttpGet("redcards")]        
        public ActionResult GetRedCards()
        {
            List<Statistics> result = new List<Statistics>();

            try
            {
                result = GetStatistics(footballContext.Managers, StatisticsType.RedCard);
                result.AddRange(GetStatistics(footballContext.Players, StatisticsType.RedCard));
                return this.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error system" + ex.Message);
                footballContext.Dispose();
                return this.NotFound();
            }


        }

        [HttpGet("minutesplayed")]        
        public ActionResult GetMinutesPlayed()
        {
            List<Statistics> result = new List<Statistics>();                    

            try
            {
                result = GetStatistics(footballContext.Referees, StatisticsType.MinutesPlayed);
                result.AddRange(GetStatistics(footballContext.Players, StatisticsType.MinutesPlayed));
                return this.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error system" + ex.Message);
                footballContext.Dispose();
                return this.NotFound();
            }
        }
    }
}
