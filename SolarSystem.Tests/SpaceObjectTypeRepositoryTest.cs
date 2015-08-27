using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using DataLayer;
using DataLayer.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolarSystemWeb.Models.Entities;
using SolarSystemWeb.Models.Repositories;
// ReSharper disable InconsistentNaming

namespace SolarSystem.Tests
{
    [TestClass]
    public class SpaceObjectTypeRepositoryTest
    {       
        private readonly ICrudRepository<SpaceObjectTypeDto, SpaceObjectType> repository = new SpaceObjectTypeRepository();
        private readonly Expression<Func<SpaceObjectType, bool>> testExpression = x => x.Id > 3;
        private readonly SpaceObjectTypeDto toAdd = new SpaceObjectTypeDto { Name = "test", Plural = "test plural" };

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
            int? existingId = repository.GetAll().FirstOrDefault()?.Id;

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
            int countOld = repository.Count;

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {                
                await repository.AddAsync(toAdd);

                int countAfterAdding = repository.Count;
                Assert.IsTrue(countAfterAdding == countOld + 1);

                int lastId = repository.GetAll().Max(x => x.Id);
                await repository.DeleteAsync(lastId);

                int countAfterDeleting = repository.Count;
                Assert.IsTrue(countAfterDeleting == countOld);

                transaction.Complete();
            }

        }
    }
}
