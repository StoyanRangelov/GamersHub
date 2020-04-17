using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Data;
using GamersHub.Data.Models;
using GamersHub.Data.Repositories;
using GamersHub.Services.Data.Users;
using GamersHub.Services.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace GamersHub.Services.Data.Tests
{
    [TestFixture]
    public class UsersServiceTests
    {
        private EfDeletableEntityRepository<ApplicationUser> usersRepository;
        private EfDeletableEntityRepository<ApplicationRole> rolesRepository;
        private UsersService usersService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.usersRepository =
                new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            this.rolesRepository =
                new EfDeletableEntityRepository<ApplicationRole>(new ApplicationDbContext(options.Options));
            this.usersService = new UsersService(this.usersRepository, this.rolesRepository);
            AutoMapperConfig.RegisterMappings(typeof(UserTest).Assembly);
        }

        [Test]
        public async Task TestGetAllBannedUsersWithoutSkipAndTakeValues()
        {
            var dateTimeOffset = new DateTimeOffset(DateTime.UtcNow);
            var banLength = dateTimeOffset.AddDays(30);

            for (int i = 0; i < 3; i++)
            {
                await this.usersRepository.AddAsync(new ApplicationUser
                {
                    UserName = "fail",
                    LockoutEnd = null,
                });
            }

            for (int i = 0; i < 5; i++)
            {
                await this.usersRepository.AddAsync(new ApplicationUser
                {
                    UserName = "test",
                    LockoutEnd = banLength,
                });
            }

            await this.usersRepository.SaveChangesAsync();

            var users = this.usersService.GetAllBannedUsers<UserTest>().ToList();

            Assert.AreEqual(5, users.Count);

            foreach (var userTest in users)
            {
                Assert.AreEqual("test", userTest.Username);
            }
        }

        [Test]
        public async Task TestGetAllBannedUsersWithSkipAndTakValues()
        {
            var dateTimeOffset = new DateTimeOffset(DateTime.UtcNow);
            var banLength = dateTimeOffset.AddDays(30);

            for (int i = 0; i < 3; i++)
            {
                await this.usersRepository.AddAsync(new ApplicationUser
                {
                    UserName = "skip",
                    LockoutEnd = banLength.AddDays(1),
                });
            }

            for (int i = 0; i < 5; i++)
            {
                await this.usersRepository.AddAsync(new ApplicationUser
                {
                    UserName = "test",
                    LockoutEnd = banLength,
                });
            }

            await this.usersRepository.AddAsync(new ApplicationUser
            {
                UserName = "fail",
                LockoutEnd = null,
            });

            await this.usersRepository.SaveChangesAsync();

            var users = this.usersService.GetAllBannedUsers<UserTest>(5, 3).ToList();

            Assert.AreEqual(5, users.Count);

            foreach (var userTest in users)
            {
                Assert.AreEqual("test", userTest.Username);
            }
        }

        [Test]
        public async Task TestGetAllByRoleWorksCorrectly()
        {
            await this.rolesRepository.AddAsync(new ApplicationRole
            {
                Name = "test",
                Id = "test",
            });
            await this.rolesRepository.AddAsync(new ApplicationRole
            {
                Name = "fail",
                Id = "fail",
            });
            await this.rolesRepository.SaveChangesAsync();

            for (int i = 0; i < 3; i++)
            {
                await this.usersRepository.AddAsync(new ApplicationUser
                {
                    UserName = "test",
                    Roles = new List<IdentityUserRole<string>>
                    {
                        new IdentityUserRole<string>
                        {
                            RoleId = "test",
                        },
                    },
                });
            }

            for (int i = 0; i < 5; i++)
            {
                await this.usersRepository.AddAsync(new ApplicationUser
                {
                    UserName = "fail",
                    Roles = new List<IdentityUserRole<string>>
                    {
                        new IdentityUserRole<string>
                        {
                            RoleId = "fail",
                        },
                    },
                });
            }

            await this.usersRepository.SaveChangesAsync();

            var users = this.usersService.GetAllByRole<UserTest>("test").ToList();

            Assert.AreEqual(3, users.Count);

            foreach (var userTest in users)
            {
                Assert.AreEqual("test", userTest.Username);
            }
        }

        [Test]
        public async Task TestDeleteAsyncWorksCorrectly()
        {
            await this.usersRepository.AddAsync(new ApplicationUser
            {
                Id = "test",
                UserName = "test",
                NormalizedUserName = "test",
                Email = "test@email.com",
                NormalizedEmail = "test@email.com",
                DiscordUsername = "test",
            });
            await this.usersRepository.SaveChangesAsync();

            await this.usersService.DeleteAsync("test");

            var user = await this.usersRepository.AllWithDeleted().FirstAsync();

            Assert.IsTrue(user.IsDeleted);
            Assert.Null(user.NormalizedUserName);
            Assert.Null(user.Email);
            Assert.Null(user.NormalizedEmail);
            Assert.IsEmpty(user.DiscordUsername);
        }

        [Test]
        public async Task TestGetCountOfBannedUsersWorksCorrectly()
        {
            var dateTimeOffset = new DateTimeOffset(DateTime.UtcNow);
            var banLength = dateTimeOffset.AddDays(30);

            for (int i = 0; i < 3; i++)
            {
                await this.usersRepository.AddAsync(new ApplicationUser
                {
                    LockoutEnd = banLength,
                });
            }

            for (int i = 0; i < 5; i++)
            {
                await this.usersRepository.AddAsync(new ApplicationUser
                {
                    LockoutEnd = null,
                });
            }

            await this.usersRepository.SaveChangesAsync();

            var count = this.usersService.GetCountOfBannedUsers();

            Assert.AreEqual(3, count);
        }

        [Test]
        public async Task TestGetIdByName()
        {
            await this.usersRepository.AddAsync(new ApplicationUser
            {
                Id = "test",
                UserName = "test",
            });
            await this.usersRepository.AddAsync(new ApplicationUser
            {
                Id = "fail",
                UserName = "fail",
            });

            await this.usersRepository.SaveChangesAsync();

            var userId = this.usersService.GetIdByName("test");

            Assert.AreEqual("test", userId);
        }

        [Test]
        public async Task TestGetCountOfPromotableUsers()
        {
            await this.rolesRepository.AddAsync(new ApplicationRole
            {
                Name = GlobalConstants.AdministratorRoleName,
                Id = "admin",
            });
            await this.rolesRepository.AddAsync(new ApplicationRole
            {
                Name = GlobalConstants.ModeratorRoleName,
                Id = "mod",
            });
            await this.rolesRepository.SaveChangesAsync();

            for (int i = 0; i < 3; i++)
            {
                await this.usersRepository.AddAsync(new ApplicationUser
                {
                    Roles = new List<IdentityUserRole<string>>
                    {
                        new IdentityUserRole<string>
                        {
                            RoleId = "admin",
                        },
                    },
                });
            }

            for (int i = 0; i < 1; i++)
            {
                await this.usersRepository.AddAsync(new ApplicationUser
                {
                    Roles = new List<IdentityUserRole<string>>
                    {
                        new IdentityUserRole<string>
                        {
                            RoleId = "mod",
                        },
                    },
                });
            }

            for (int i = 0; i < 5; i++)
            {
                await this.usersRepository.AddAsync(new ApplicationUser());
            }

            await this.usersRepository.SaveChangesAsync();

            var count = this.usersService.GetCountOfPromotableUsers();

            Assert.AreEqual(5, count);
        }

        [Test]
        public async Task TestGetByNameWorksCorrectly()
        {
            await this.usersRepository.AddAsync(new ApplicationUser
            {
                UserName = "test",
            });
            await this.usersRepository.SaveChangesAsync();

            var user = this.usersService.GetByName<UserTest>("test");

            Assert.AreEqual("test", user.Username);
        }

        [Test]
        public async Task TestGetByIdWorksCorrectly()
        {
            await this.usersRepository.AddAsync(new ApplicationUser
            {
                UserName = "test",
                Id = "test",
            });
            await this.usersRepository.SaveChangesAsync();

            var user = this.usersService.GetById<UserTest>("test");

            Assert.AreEqual("test", user.Username);
        }

        [Test]
        public async Task TestGetAllPromotableUsers()
        {
            await this.rolesRepository.AddAsync(new ApplicationRole
            {
                Name = GlobalConstants.AdministratorRoleName,
                Id = "admin",
            });
            await this.rolesRepository.AddAsync(new ApplicationRole
            {
                Name = GlobalConstants.ModeratorRoleName,
                Id = "mod",
            });
            await this.rolesRepository.SaveChangesAsync();

            for (int i = 0; i < 3; i++)
            {
                await this.usersRepository.AddAsync(new ApplicationUser
                {
                    UserName = "fail",
                    Roles = new List<IdentityUserRole<string>>
                    {
                        new IdentityUserRole<string>
                        {
                            RoleId = "admin",
                        },
                    },
                });
            }

            for (int i = 0; i < 1; i++)
            {
                await this.usersRepository.AddAsync(new ApplicationUser
                {
                    UserName = "fail",
                    Roles = new List<IdentityUserRole<string>>
                    {
                        new IdentityUserRole<string>
                        {
                            RoleId = "mod",
                        },
                    },
                });
            }

            for (int i = 0; i < 5; i++)
            {
                await this.usersRepository.AddAsync(new ApplicationUser
                {
                    UserName = "test",
                });
            }

            await this.usersRepository.SaveChangesAsync();

            var users = this.usersService.GetAllPromotableUsers<UserTest>().ToList();

            Assert.AreEqual(5, users.Count);

            foreach (var userTest in users)
            {
                Assert.AreEqual("test", userTest.Username);
            }
        }

        [Test]
        public async Task TestGetAllPromotableUsersWithSkipAndTakeValues()
        {
            await this.rolesRepository.AddAsync(new ApplicationRole
            {
                Name = GlobalConstants.AdministratorRoleName,
                Id = "admin",
            });
            await this.rolesRepository.AddAsync(new ApplicationRole
            {
                Name = GlobalConstants.ModeratorRoleName,
                Id = "mod",
            });
            await this.rolesRepository.SaveChangesAsync();

            for (int i = 0; i < 3; i++)
            {
                await this.usersRepository.AddAsync(new ApplicationUser
                {
                    UserName = "fail",
                });
            }

            for (int i = 0; i < 8; i++)
            {
                await this.usersRepository.AddAsync(new ApplicationUser
                {
                    UserName = "test",
                });
            }

            await this.usersRepository.SaveChangesAsync();

            var users = this.usersService.GetAllPromotableUsers<UserTest>(5, 3).ToList();

            Assert.AreEqual(5, users.Count);

            foreach (var userTest in users)
            {
                Assert.AreEqual("test", userTest.Username);
            }
        }

        [Test]
        public async Task TestGetTopFiveWithValues([Values(
            GlobalConstants.Posts,
            GlobalConstants.Reviews,
            GlobalConstants.Banned,
            null)]string value)
        {
            var dateTimeOffset = new DateTimeOffset(DateTime.UtcNow);
            var banLength = dateTimeOffset.AddDays(30);

            for (int i = 0; i < 5; i++)
            {
                await this.usersRepository.AddAsync(new ApplicationUser
                {
                    UserName = "test",
                    CreatedOn = new DateTime(2020, 4, 17),
                    Posts = new List<Post> { new Post(), new Post(), new Post() },
                    Reviews = new List<Review> { new Review(), new Review(), new Review(), },
                    LockoutEnd = banLength,
                });
            }

            for (int i = 0; i < 3; i++)
            {
                await this.usersRepository.AddAsync(new ApplicationUser
                {
                    UserName = "fail",
                    CreatedOn = new DateTime(2020, 4, 16),
                    Posts = new List<Post> { new Post(), },
                    Reviews = new List<Review> { new Review(), },
                    LockoutEnd = null,
                });
            }

            await this.usersRepository.SaveChangesAsync();

            var users = this.usersService.GetTopFive<UserTest>(value).ToList();

            Assert.AreEqual(5, users.Count);

            foreach (var userTest in users)
            {
                Assert.AreEqual("test", userTest.Username);
            }
        }
    }

    public class UserTest : IMapFrom<ApplicationUser>
    {
        public string Username { get; set; }
    }
}