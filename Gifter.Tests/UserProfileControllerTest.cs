using Gifter.Controllers;
using Gifter.Models;
using Gifter.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Gifter.Tests
{
    public class UserProfileControllerTests
    {
        [Fact]
        public void Get_Returns_All_Users()
        {
            var userCount = 10;
            var users = CreateTestUsers(userCount);

            var repo = new InMemoryUserProfileRepository(users);
            var controller = new UserProfileController(repo);

            var result = controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualUsers = Assert.IsType<List<UserProfile>>(okResult.Value);

            Assert.Equal(userCount, actualUsers.Count);
            Assert.Equal(users, actualUsers);
        }
        [Fact]
        public void Get_Returns_User_By_Id()
        {
            var testUserId = 999;
            var users = CreateTestUsers(5);
            users[0].Id = testUserId;

            var repo = new InMemoryUserProfileRepository(users);
            var controller = new UserProfileController(repo);

            var result = controller.Get(testUserId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualUser = Assert.IsType<UserProfile>(okResult.Value);

            Assert.Equal(testUserId, actualUser.Id);
        }

        [Fact]
        public void Post_Method_Adds_New_User()
        {
            var userCount = 10;
            var users = CreateTestUsers(userCount);

            var repo = new InMemoryUserProfileRepository(users);
            var controller = new UserProfileController(repo);

            controller.Post(CreateTestUsers(1)[0]);

            Assert.Equal(userCount + 1, repo.InternalData.Count);
        }
        private List<UserProfile> CreateTestUsers(int count)
        {
            var users = new List<UserProfile>();

            for (var i = 1; i <= count; i++)
            {
                users.Add(new UserProfile()
                {
                    Id = i,
                    Name = $"Name {i}",
                    Email = $"Name{i}@bob.comx",
                    ImageUrl = $"http://post.image.url/{i}",
                    DateCreated = DateTime.Today.AddDays(-i),
                    Bio = $"Bob{i} really likes cheese",
                    Posts = CreatePostList(i)
                });
            }
            return users;
        }
        private List<Post> CreatePostList(int id)
        {
            var posts = new List<Post>();
            for (var i = 1; i <= 2; i++)
            {
                posts.Add(new Post()
                {
                    Id = i,
                    Caption = $"Caption {i}",
                    Title = $"Title {i}",
                    ImageUrl = $"http://post.image.url/{i}",
                    DateCreated = DateTime.Today.AddDays(-i),
                    UserProfileId = id
                });
            }
            return posts;
        }

    }
}

