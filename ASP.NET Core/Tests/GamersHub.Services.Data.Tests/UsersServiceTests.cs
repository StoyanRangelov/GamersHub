using System;
using GamersHub.Data;
using GamersHub.Data.Models;
using GamersHub.Data.Repositories;
using GamersHub.Services.Data.Users;
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
            this.usersRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            this.rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(new ApplicationDbContext(options.Options));
            this.usersService = new UsersService(this.usersRepository, this.rolesRepository);
        }
    }
}