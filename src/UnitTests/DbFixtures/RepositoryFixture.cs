using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.Models;
using Model.Models.Database;
using Model.Repositories;
using Model.Repositories.Interfaces;
using Moq;
using Xunit;

namespace UnitTests.DbFixtures
{
    public class RepositoryFixture
    {
        [Fact]
        public void DbInsertsItemAndSavesChanges()
        {
            var dbSetMock = new Mock<DbSet<Role>>();

            var dbMock = new Mock<DatabaseContext>();
            dbMock.Setup(x => x.Roles).Returns(dbSetMock.Object);


            RolesRepository repository = new RolesRepository(dbMock.Object);
            
            repository.Insert(new Role
            {
                Name = "Admin",
                AccessLevel = 300
            });
            repository.Save();
            
            dbMock.Verify(context => context.SaveChanges(), Times.Once);
        }
        
    }
}
