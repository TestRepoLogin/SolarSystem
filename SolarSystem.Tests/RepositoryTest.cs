using System.Collections.Generic;
using DataLayer;
using DataLayer.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SolarSystemWeb.Models.Entities;

namespace SolarSystem.Tests
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void Test()
        {
            var mock = new Mock<ICrudRepository<SpaceObjectDto, SpaceObject>> ();
            mock.Setup(a => a.GetAll()).Returns(new List<SpaceObjectDto>() { new SpaceObjectDto() });


        }
    }
}
