using System;
using Xunit;
using Moq;
using System.IO;
using System.Linq;
using System.Reflection;
using BCryptNet = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Mvc;
using UserMgmt.Authorization;
using UserMgmt.Controllers;
using UserMgmt.Helpers;
using UserMgmt.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UserMgmt.Test
{
    public class TestService
    {
        private readonly DataContext _dbContext;
        private readonly IJwtUtils _jwtUtils;
        private readonly IConfigurationRoot _configuration;
        private readonly IService _service;

        public TestService()
        {
            _configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
            .AddJsonFile("appsettings.json")
            .Build();
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase(databaseName: "Users");
            _dbContext = new DataContext(builder.Options);
            _dbContext.Database.EnsureCreated();
            _jwtUtils = new JwtUtils(configuration: _configuration);
            _service = new Service(_dbContext,_jwtUtils);
            _dbContext.Users.AddRange(
                    new User
                    {
                        Id = 1,
                        FirstName = "Ravi",
                        LastName = "Ahuja",
                        Username = "RaviAhuja",
                        PasswordHash = BCryptNet.HashPassword("ravipassword"),
                        Role = Role.Edit
                    },
                    new User
                    {
                        Id = 2,
                        FirstName = "Test",
                        LastName = "User",
                        Username = "TestUser",
                        PasswordHash = BCryptNet.HashPassword("testpassword"),
                        Role = Role.View
                    },
                    new User
                    {
                        Id = 3,
                        FirstName = "Admin",
                        LastName = "Admin",
                        Username = "Admin",
                        PasswordHash = BCryptNet.HashPassword("admin"),
                        Role = Role.Admin
                    },
                    new User
                    {
                        Id = 4,
                        FirstName = "New",
                        LastName = "User",
                        Username = "NewUser",
                        PasswordHash = BCryptNet.HashPassword("View"),
                        Role = Role.View
                    });
            _dbContext.SaveChanges();
        }

        [Fact]
        public void Test_IncorrectUserNameOrPassword()
        {
            //Setup
            string id = "fakeId";
            string secret = "fakeSecret";
            //Act
            Action act = () => _service.GetToken(id, secret);
            //assert
            AppException e = Assert.Throws<AppException>(act);
            //The thrown exception can be used for even more detailed assertions.
            Assert.Equal("Username or password is incorrect", e.Message);
        }

        [Fact]
        public void Test_CorrectUserNameOrPassword()
        {
            //Setup
            string id = "admin";
            string secret = "admin";
            //Act
            var output = _service.GetToken(id, secret);            
            //The thrown exception can be used for even more detailed assertions.
            Assert.NotEmpty(output.Token);
        }

       

        [Fact]
        public void Test_AddNewUser()
        {
            //Setup
            var countBefore = _dbContext.Users.CountAsync<User>().Result;

                        
            User user = new User();
            user.Username = "UserName";
            user.FirstName = "FirstName";
            user.LastName = "LastName";
            user.Role = Role.View;
            string password = "password";
            //Act
            _service.AddNewUser(user, password);
            var countAfter = _dbContext.Users.CountAsync<User>().Result;
            //The thrown exception can be used for even more detailed assertions.
            Assert.Equal(countBefore +1,countAfter);
        }

        [Fact]
        public void Test_UpdateUserWithAlreadyAvailableUserName()
        {            
            //Setup
            User u = new User();
            u.Id = 1;
            u.Username = "admin";
            u.FirstName = "Ravi";
            u.LastName = "Ahuja";
            u.Role = Role.Edit;  //Change in role from Edit to Admin         
            //Act
            Action act = () => _service.UpdateUser(u);
            //assert
            AppException e = Assert.Throws<AppException>(act);
            //The thrown exception can be used for even more detailed assertions.
            Assert.Equal("Username admin is already taken", e.Message);
          
        }

        [Fact]
        public void Test_UpdateUserRole()
        {
            //Setup
            User u = new User();
            u.Id = 1;
            u.Username = "RaviAhuja";
            u.FirstName = "Ravi";
            u.LastName = "Ahuja";
            u.Role = Role.Admin;  //Change in role from Edit to Admin         
            //Act
            _service.UpdateUser(u);
            var user = _dbContext.Users.Find(u.Id);
            Assert.Equal(u.Role, user.Role);
        }

        [Fact]
        public void Test_DeleteUser()
        {
            //Setup
            var countBefore = _dbContext.Users.CountAsync<User>().Result;
            //Act
            _service.Delete(1);
            var countAfter = _dbContext.Users.CountAsync<User>().Result;
            //The thrown exception can be used for even more detailed assertions.
            Assert.Equal(countBefore -1,countAfter);
        }


    }
}