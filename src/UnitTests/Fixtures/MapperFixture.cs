using AutoMapper;
using Model.Helpers;
using Model.Models.Account;
using Model.Models.Database;
using Xunit;

namespace UnitTests.Fixtures
{
    public class MapperFixture
    {
        public MapperFixture()
        {
            MapperHelper.InitializeMaps();
        }

        [Theory]
        [InlineData(1, "Jan", "Kowalski", 100, "User", 1)]
        [InlineData(2, "Jan", "Nowak", 300, "Admin", 2)]
        [InlineData(3, "Aleksander", "Wójcikowski", 100, "User", 1)]
        public void MapUserToAppUserVo(int id, string firstName, string lastName, int accessLevel, string roleName, int roleId)
        {
            var user = new User
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Role = new Role
                {
                    Id = roleId,
                    AccessLevel = accessLevel,
                    Name = roleName
                }
            };

            var mappedUser = Mapper.Map<AppUserVo>(user);
            Assert.Equal(user.Id, mappedUser.UserId);
            Assert.Equal(user.Role.Id, mappedUser.RoleId);
            Assert.Equal(user.Role.AccessLevel, mappedUser.AccessLevel);
            Assert.Equal(user.Role.Name, mappedUser.RoleName);
            Assert.Equal($"{user.FirstName} {user.LastName}", mappedUser.FullName);
        }
    }
}
