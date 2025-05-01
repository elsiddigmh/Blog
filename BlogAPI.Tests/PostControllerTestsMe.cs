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
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Tests
{
    public class PostControllerTestsMe
    {
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;
        private Mock<IPostRepository> _postRepositoryMock;
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private Mock<IMapper> _mapperMock;

        private PostAPIController _postController;

        [SetUp]
        public void Setup()
        {
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _postRepositoryMock = new Mock<IPostRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();

            _postController = new PostAPIController(_webHostEnvironmentMock.Object, _postRepositoryMock.Object, _categoryRepositoryMock.Object, _mapperMock.Object);
        }


        // GetPosts() -> Scenario (1) --- When posts is null return NotFound 
        [Test]
        public async Task GetPosts_WhenPostsIsNull_ShouldReturnNotFound()
        {
            // Arrange
            List<Post> posts = null;
            _postRepositoryMock.Setup(p => p.GetAllAsync(null, It.IsAny<string[]>())).ReturnsAsync(posts);

            // Act
            var result = await _postController.GetPosts();
            var actionResult = result.Result as NotFoundObjectResult;

            // Assert
            var apiResponse = result.Value;
            ClassicAssert.IsNull(apiResponse);
            ClassicAssert.That(actionResult, Is.TypeOf<NotFoundObjectResult>());

        }

        // GetPosts() -> Scenario (2) --- When posts is empty return NotFound 
        [Test]
        public async Task GetPosts_WhenPostsIsEmpty_ShouldReturnNotFound()
        {
            // Arrange
            List<Post> posts = new List<Post> { };
            _postRepositoryMock.Setup(p => p.GetAllAsync(null, It.IsAny<string[]>())).ReturnsAsync(posts);
            // Act
            var result = await _postController.GetPosts();
            var actionResult = result.Result as NotFoundObjectResult;

            // Assert
            var apiResponse = result.Value;
            ClassicAssert.IsNull(apiResponse);
            ClassicAssert.That(actionResult, Is.TypeOf<NotFoundObjectResult>());
        }

        // GetPosts() -> Scenario (3) --- When posts is exist return posts
        [Test]
        public async Task GetPosts_WhenPostsExist_ShouldReturnPosts()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post { Title = "Test 1", CategoryId = 1, Content = "Test content", Author = new User(), Category = new Category() },
                new Post { Title = "Test 2", CategoryId = 2, Content = "More test content", Author = new User(), Category = new Category() }
            };

            _postRepositoryMock
                .Setup(p => p.GetAllAsync(null, It.IsAny<string[]>()))
                .ReturnsAsync(posts);

            _mapperMock
                .Setup(m => m.Map<List<PostDTO>>(It.IsAny<List<Post>>()))
                .Returns(posts.Select(p => new PostDTO { Title = p.Title, Content = p.Content }).ToList());

            // Act
            var result = await _postController.GetPosts();

            // Assert
            var apiResponse = result.Value;
            ClassicAssert.IsNotNull(apiResponse);
            ClassicAssert.IsTrue(apiResponse.IsSuccess);
            ClassicAssert.That(apiResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var postDtos = apiResponse.Result as List<PostDTO>;
            ClassicAssert.IsNotNull(postDtos);
            ClassicAssert.That(postDtos.Count, Is.EqualTo(2));
        }



        // GetPost() -> Scenario (1) --- When post is not exist return NotFound
        [Test]
        public async Task GetPost_WhenPostIsNotExist_ShouldReturnNotFound()
        {
            // Arrange
            int postId = 1;
            var post = new Post
            {
                Id = postId,
                Author = new User(),
                CategoryId = 1,
                Category = new Category(),
                Title = "Test 1",
                Content = "Test ....... rerere 1"
            };

            _postRepositoryMock.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Post, bool>>>(), true,It.IsAny<string[]>())).ReturnsAsync(post);

            //_mapperMock.Setup(m => m.Map<PostDTO>(It.IsAny<Post>()))
            //           .Returns(new PostDTO { Title = post.Title, Content = post.Content });

            // Act
            var result = await _postController.GetPost(2);
            var actionResult = result.Result;

            ClassicAssert.That(actionResult, Is.TypeOf<NotFoundObjectResult>());

        }

        // GetPost() -> Scenario (2) --- When post is exist return Post
        [Test]
        public async Task GetPost_WhenPostIsExist_ShouldReturnPost()
        {
            // Arrange
            int postId = 1;
            var post = new Post
            {
                Id = postId,
                Author = new User(),
                CategoryId = 1,
                Category = new Category(),
                Title = "Test 1",
                Content = "Test ....... rerere 1"
            };

            _postRepositoryMock.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Post, bool>>>(), true, It.IsAny<string[]>())).ReturnsAsync(post);

            var postDto = new PostDTO { Title = post.Title, Content = post.Content };
            _mapperMock.Setup(m => m.Map<PostDTO>(It.IsAny<Post>()))
                       .Returns(postDto);

            // Act
            var result = await _postController.GetPost(postId);
            var apiResponse = result.Value;
            ClassicAssert.IsNotNull(apiResponse);

            //var postDto = apiResponse.Result as PostDTO;
            //ClassicAssert.AreEqual(postDto, _mapperMock.Object);

        }
    }
}
