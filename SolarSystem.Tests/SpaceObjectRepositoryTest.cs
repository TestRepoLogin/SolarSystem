﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Configuration;
using DataLayer;
using DataLayer.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolarSystemWeb.Models.Entities;
using SolarSystemWeb.Models.Repositories;
// ReSharper disable InconsistentNaming

namespace SolarSystem.Tests
{
    [TestClass]
    public class SpaceObjectRepositoryTest
    {
        private readonly ICrudRepository<SpaceObjectDto, SpaceObject> repository = new SpaceObjectRepository();
        private readonly  Expression<Func<SpaceObject, bool>> testExpression = x => x.Id > 3;
        private readonly SpaceObjectDto toAdd = new SpaceObjectDto
        {
            Name = "test",
            Description = "text description",
            Distance = 0,
            Mass = 0,
            Radius = 0,  
            WikiLink = "some link",  
            OrbitPeriod = 1,
            SiderealPeriod = 1
        };

        [TestMethod]
        public async Task GetAllTest()
        {
            var res = repository.GetAll().ToList();
            var asyncRes = (await repository.GetAllAsync()).ToList();

            Assert.AreEqual(res.Count(), asyncRes.Count());

            for (int i = 0 ; i < res.Count(); i++)
            {
               Assert.AreEqual(res[i].Id, asyncRes[i].Id);
            }
        }

        [TestMethod]
        public async Task GetTest()
        {            
            var res = repository.Get(testExpression).ToList();
            var asyncRes = (await repository.GetAsync(testExpression)).ToList();

            Assert.AreEqual(res.Count(), asyncRes.Count());

            for (int i = 0; i < res.Count(); i++)
            {
                Assert.AreEqual(res[i].Id, asyncRes[i].Id);
            }           
        }

        [TestMethod]
        public async Task GetSingleTest()
        {
            var existing = repository.GetAll().FirstOrDefault();
            int? existingId = existing != null ? (int?)existing.Id : null;

            if (!existingId.HasValue)
                return;

            var res = repository.Get(existingId.Value);
            var asyncRes = (await repository.GetAsync(existingId.Value));

            Assert.AreEqual(res.Id, asyncRes.Id);           
        }

        [TestMethod]
        public async Task GetSingleNotExistingTest()
        {
            var data = repository.GetAll();
            if (data == null || !data.Any())
                return;

            int minId = data.Min(x => x.Id);

            int notExistingId = minId - 1;

            var res = repository.Get(notExistingId);
            Assert.IsNull(res);

            var asyncRes = (await repository.GetAsync(notExistingId));            
            Assert.IsNull(asyncRes);
        }

        [TestMethod]
        public void AddDeleteTest()
        {
            int countOld = repository.Count;
            var first = repository.GetAll().FirstOrDefault();
            int typeId = first == null ? 1 : first.TypeId;
            int ownerId = first == null ? 1 : first.Id;
            toAdd.TypeId = typeId;
            toAdd.OwnerId = ownerId;

            using (var transaction = new TransactionScope())
            {                
                repository.Add(toAdd);

                int countAfterAdding = repository.Count;
                Assert.IsTrue(countAfterAdding == countOld + 1);

                int lastId = repository.GetAll().Max(x => x.Id);
                repository.Delete(lastId);

                int countAfterDeleting = repository.Count;
                Assert.IsTrue(countAfterDeleting == countOld);

                transaction.Complete();
            }
            
        }

        [TestMethod]
        public async Task AddDeleteAsyncTest()
        {
            int countOld = repository.GetAll().Count();
            var first = repository.GetAll().FirstOrDefault();
            int typeId = first == null ? 1 : first.TypeId;
            int ownerId = first == null ? 1 : first.Id;
            toAdd.TypeId = typeId;
            toAdd.OwnerId = ownerId;
             
            await repository.AddAsync(toAdd);

            int countAfterAdding = repository.GetAll().Count();
            Assert.IsTrue(countAfterAdding == countOld + 1);

            int lastId = repository.GetAll().Max(x => x.Id);
            await repository.DeleteAsync(lastId);

            int countAfterDeleting = repository.GetAll().Count();
            Assert.IsTrue(countAfterDeleting == countOld);
           
        }
    }
}
