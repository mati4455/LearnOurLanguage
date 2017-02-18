using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.Models;
using Model.Models.Database;
using Model.Repositories;
using Moq;
using Xunit;

namespace UnitTests.DbFixtures
{
    public class RepositoryFixture
    {
        [Fact]
        public void DbInsertsItemAndSavesChanges()
        {
            Mock<DbSet<Role>> dbSetMock = new Mock<DbSet<Role>>();
            Mock<Logger<RolesRepository>> loggerMock = new Mock<Logger<RolesRepository>>();
            Mock<DatabaseContext> dbMock = new Mock<DatabaseContext>();
            dbMock.Setup(context => context.Set<Role>()).Returns(dbSetMock.Object);


            RolesRepository repository = new RolesRepository(dbMock.Object);

            repository.Insert(new Role
            {
                Name = "Admin",
                AccessLevel = 300
            });
            repository.Save();

            dbSetMock.Verify(set => set.Add(It.IsAny<Role>()), Times.Once);
            dbMock.Verify(context => context.SaveChanges(), Times.Once);
        }

    }
}
