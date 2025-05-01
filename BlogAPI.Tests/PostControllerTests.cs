using AutoMapper;
using BlogAPI.Controllers;
using BlogAPI.Models;
using BlogAPI.Models.Dto;
using BlogAPI.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Tests
{
    [TestFixture]
    public class PostControllerTests
    {
        private Mock<IPostRepository> _postRepoMock;
        private Mock<ICategoryRepository> _categoryRepoMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IWebHostEnvironment> _webHostMock;
        private PostAPIController _controller;

        [SetUp]
        public void SetUp()
        {
            _postRepoMock = new Mock<IPostRepository>();
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();
            _webHostMock = new Mock<IWebHostEnvironment>();

            _controller = new PostAPIController(_webHostMock.Object, _postRepoMock.Object, _categoryRepoMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetPosts_ReturnsOk_WhenPostsExist()
        {
            var posts = new List<Post> { new Post { Id = 1, Title = "Test" } };
            var postDTOs = new List<PostDTO> { new PostDTO { Id = 1, Title = "Test" } };

            _postRepoMock.Setup(r => r.GetAllAsync(null, It.IsAny<string[]>())).ReturnsAsync(posts);
            _mapperMock.Setup(m => m.Map<List<PostDTO>>(posts)).Returns(postDTOs);

            var result = await _controller.GetPosts();

            ClassicAssert.IsInstanceOf<APIResponse>((result.Result as ObjectResult).Value);
            var response = (APIResponse)(result.Result as ObjectResult).Value;
            ClassicAssert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

    }
}
