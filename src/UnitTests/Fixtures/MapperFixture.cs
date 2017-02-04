using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [InlineData("Jan", "Kowalski", 100, "User")]
        [InlineData("Jan", "Nowak", 300, "Admin")]
        [InlineData("Aleksander", "Wójcikowski", 100, "User")]
        public void MapUserToAppUserVo(string firstName, string lastName, int accessLevel, string roleName)
        {
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Role = new Role
                {
                    AccessLevel = accessLevel,
                    Name = roleName
                }
            };

            var mappedUser = Mapper.Map<AppUserVo>(user);
            Assert.Equal(user.Role.AccessLevel, mappedUser.AccessLevel);
            Assert.Equal(user.Role.Name, mappedUser.RoleName);
            Assert.Equal($"{user.FirstName} {user.LastName}", mappedUser.FullName);
        }
    }
}
