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
        public void Test()
        {
            var mockUsers = new Mock<DbSet<Role>>();

            var dbContext = new Mock<DatabaseContext>();
            dbContext.Setup(x => x.Roles).Returns(mockUsers.Object);


            var repoMoq = new RolesRepository(dbContext.Object);
            
           // var repo = new RolesRepository(dbContext.Object);
            repoMoq.Insert(new Role
            {
                Name = "Admin",
                AccessLevel = 300
            });
            repoMoq.Save();

            var test = repoMoq.GetAll().ToList();
        }
    }
}
