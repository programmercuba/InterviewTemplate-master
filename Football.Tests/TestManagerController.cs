
using Football.API;
using Football.API.Controllers;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Football.Tests
{
    [TestFixture]    
    public class TestManagerController
    {        
        FootballContext context;
        ILogger<ManagerController> _logger;
        ManagerController manager;

        [SetUp]
        public void SetUp()
        {
            context = new FootballContext(new Microsoft.EntityFrameworkCore.DbContextOptions<FootballContext>());
            manager = new ManagerController(context, _logger); 
        }

        [Test]
        public void GetManager()
        {
            //Arrange
            //Act
            var result = manager.Get();
            //Assert
            Assert.NotNull(result);
        }
    }
}