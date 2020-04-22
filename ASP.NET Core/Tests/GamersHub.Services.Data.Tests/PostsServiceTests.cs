using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Data;
using GamersHub.Data.Models;
using GamersHub.Data.Repositories;
using GamersHub.Services.Data.Posts;
using GamersHub.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GamersHub.Services.Data.Tests
{
    [TestFixture]
    public class PostsServiceTests
    {
        private EfDeletableEntityRepository<Post> postsRepository;
        private EfDeletableEntityRepository<Forum> forumsRepository;
        private EfDeletableEntityRepository<Reply> repliesRepository;
        private PostsService postsService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.postsRepository = new EfDeletableEntityRepository<Post>(new ApplicationDbContext(options.Options));
            this.forumsRepository = new EfDeletableEntityRepository<Forum>(new ApplicationDbContext(options.Options));
            this.repliesRepository = new EfDeletableEntityRepository<Reply>(new ApplicationDbContext(options.Options));
            this.postsService = new PostsService(this.postsRepository, this.forumsRepository, this.repliesRepository);
            AutoMapperConfig.RegisterMappings(typeof(TestPost).Assembly);
        }

        [Test]
        public async Task TestGetByNameUrl()
        {
            await this.postsRepository.AddAsync(new Post
                {Name = "name: test name", Content = "test content", Category = new Category()});
            await this.postsRepository.SaveChangesAsync();

            var nameAsUrl = "name-test-name";

            var post = this.postsService.GetByNameUrl<TestPost>(nameAsUrl);

            Assert.AreEqual("name: test name", post.Name);
            Assert.AreEqual("test content", post.Content);
        }

        [Test]
        public async Task TestById()
        {
            await this.postsRepository.AddAsync(
                new Post {Name = "name", Content = "content", Category = new Category()});
            await this.postsRepository.SaveChangesAsync();

            var post = this.postsService.GetById<TestPost>(1);

            Assert.AreEqual("name", post.Name);
            Assert.AreEqual("content", post.Content);
        }

        [Test]
        public async Task TestGetAll()
        {
            for (int i = 0; i < 5; i++)
            {
                await this.postsRepository.AddAsync(new Post
                    {Name = "name", Content = "content", Category = new Category()});
            }

            await this.postsRepository.SaveChangesAsync();

            var posts = this.postsService.GetAll<TestPost>().ToList();

            foreach (var testPost in posts)
            {
                Assert.AreEqual("name", testPost.Name);
                Assert.AreEqual("content", testPost.Content);
            }

            Assert.AreEqual(5, posts.Count);
        }

        [Test]
        public async Task TestGetAllWithSkipAndTakeValues()
        {
            for (int i = 0; i < 3; i++)
            {
                await this.postsRepository.AddAsync(new Post
                {
                    Name = "skip name", Content = "skip", Category = new Category(),
                    CreatedOn = new DateTime(2020, 04, 15)
                });
            }

            for (int i = 0; i < 5; i++)
            {
                await this.postsRepository.AddAsync(new Post
                {
                    Name = "name", Content = "take", Category = new Category(), CreatedOn = new DateTime(2020, 04, 14)
                });
            }

            await this.postsRepository.AddAsync(new Post
            {
                Name = "test name", Content = "content", Category = new Category(),
                CreatedOn = new DateTime(2020, 04, 13)
            });
            await this.postsRepository.AddAsync(new Post
            {
                Name = "test name", Content = "content", Category = new Category(),
                CreatedOn = new DateTime(2020, 04, 12)
            });
            await this.postsRepository.SaveChangesAsync();

            var posts = this.postsService.GetAll<TestPost>(5, 3).ToList();

            foreach (var testPost in posts)
            {
                Assert.AreEqual("name", testPost.Name);
                Assert.AreEqual("take", testPost.Content);
            }

            Assert.AreEqual(5, posts.Count);
        }

        [Test]
        public async Task TestGetTopFive()
        {
            for (int i = 0; i < 5; i++)
            {
                await this.postsRepository.AddAsync(new Post
                {
                    Name = "Post with five Replies",
                    Replies = new List<Reply>
                    {
                        new Reply(),
                        new Reply(),
                        new Reply(),
                        new Reply(), new Reply(),
                    },
                    Category = new Category(),
                });
            }

            await this.postsRepository.AddAsync(new Post {Name = "no replies"});
            await this.postsRepository.AddAsync(new Post {Name = "no replies"});
            await this.postsRepository.SaveChangesAsync();

            var posts = this.postsService.GetTopFive<TestPost>().ToList();

            foreach (var testPost in posts)
            {
                Assert.AreEqual("Post with five Replies", testPost.Name);
            }

            Assert.AreEqual(5, posts.Count);
        }

        [Test]
        public async Task TestGetAllByForumId()
        {
            for (int i = 0; i < 5; i++)
            {
                await this.postsRepository.AddAsync(new Post
                    {Name = "name", Content = "content", ForumId = 1, Category = new Category()});
            }

            for (int i = 2; i < 4; i++)
            {
                await this.postsRepository.AddAsync(new Post
                    {Name = "wrong name", Content = "wrong content", ForumId = i, Category = new Category()});
            }

            await this.postsRepository.SaveChangesAsync();

            var posts = this.postsService.GetAllByForumId<TestPost>(1).ToList();

            foreach (var testPost in posts)
            {
                Assert.AreEqual("name", testPost.Name);
                Assert.AreEqual("content", testPost.Content);
            }

            Assert.AreEqual(5, posts.Count);
        }

        [Test]
        public async Task TestGetAllByForumIdWithTakeAndSkipValues()
        {
            for (int i = 0; i < 3; i++)
            {
                await this.postsRepository.AddAsync(new Post
                {
                    Name = "skip name", Content = "skip", ForumId = 1, Category = new Category(),
                    CreatedOn = new DateTime(2020, 04, 15)
                });
            }

            for (int i = 0; i < 5; i++)
            {
                await this.postsRepository.AddAsync(new Post
                {
                    Name = "name", Content = "take", ForumId = 1, Category = new Category(),
                    CreatedOn = new DateTime(2020, 04, 14)
                });
            }

            await this.postsRepository.AddAsync(new Post
            {
                Name = "test name", Content = "content", ForumId = 2, Category = new Category(),
                CreatedOn = new DateTime(2020, 04, 13)
            });
            await this.postsRepository.AddAsync(new Post
            {
                Name = "test name", Content = "content", ForumId = 2, Category = new Category(),
                CreatedOn = new DateTime(2020, 04, 12)
            });
            await this.postsRepository.SaveChangesAsync();


            var allPosts = this.postsRepository.All().ToList();
            var posts = this.postsService.GetAllByForumId<TestPost>(1, 5, 3).ToList();

            foreach (var testPost in posts)
            {
                Assert.AreEqual("name", testPost.Name);
                Assert.AreEqual("take", testPost.Content);
            }

            Assert.AreEqual(5, posts.Count);
        }

        [Test]
        public async Task TestGetAllByCategoryNameAndForumId()
        {
            for (int i = 0; i < 10; i++)
            {
                await this.postsRepository.AddAsync(new Post
                    {Name = "name", Content = "content", ForumId = 1, Category = new Category {Name = "category"}});
            }

            await this.postsRepository.AddAsync(new Post
            {
                Name = "wrong name", Content = "wrong content", ForumId = 1,
                Category = new Category {Name = "wrong category"},
            });

            await this.postsRepository.AddAsync(new Post
            {
                Name = "wrong forum", Content = "wrong forum", ForumId = 2,
                Category = new Category {Name = "category"},
            });

            await this.postsRepository.SaveChangesAsync();


            var posts = this.postsService.GetAllByCategoryNameAndForumId<TestPost>("category", 1).ToList();

            foreach (var testPost in posts)
            {
                Assert.AreEqual("name", testPost.Name);
                Assert.AreEqual("content", testPost.Content);
            }

            Assert.AreEqual(10, posts.Count);
        }

        [Test]
        public async Task TestGetAllByCategoryNameAndForumIdWithSkipAndTakeValues()
        {
            for (int i = 0; i < 3; i++)
            {
                await this.postsRepository.AddAsync(new Post
                {
                    Name = "skip name",
                    Content = "skip",
                    ForumId = 1, Category = new Category {Name = "category"},
                    CreatedOn = new DateTime(2020, 04, 15),
                });
            }

            for (int i = 0; i < 5; i++)
            {
                await this.postsRepository.AddAsync(new Post
                {
                    Name = "name",
                    Content = "content",
                    ForumId = 1, Category = new Category {Name = "category"},
                    CreatedOn = new DateTime(2020, 04, 14),
                });
            }

            await this.postsRepository.AddAsync(new Post
            {
                Name = "name",
                Content = "content",
                ForumId = 2, Category = new Category {Name = "category"},
                CreatedOn = new DateTime(2020, 04, 14),
            });

            await this.postsRepository.AddAsync(new Post
            {
                Name = "name",
                Content = "content",
                ForumId = 1, Category = new Category {Name = "wrong category"},
                CreatedOn = new DateTime(2020, 04, 14),
            });

            await this.postsRepository.SaveChangesAsync();


            var posts = this.postsService.GetAllByCategoryNameAndForumId<TestPost>("category", 1, 5, 3).ToList();

            foreach (var testPost in posts)
            {
                Assert.AreEqual("name", testPost.Name);
                Assert.AreEqual("content", testPost.Content);
            }

            Assert.AreEqual(5, posts.Count);
        }

        [Test]
        public async Task TestCreateAsyncReturnsNullWithWrongForumId()
        {
            var postId = await this.postsService.CreateAsync(1, 1, "post", "content", "userId");

            Assert.Null(postId);
        }

        [Test]
        public async Task TestCreateAsyncWorksCorrectlyWithExistingForumAndCategory()
        {
            await this.forumsRepository.AddAsync(new Forum
                {Id = 1, ForumCategories = new List<ForumCategory> {new ForumCategory {CategoryId = 1}}});
            await this.forumsRepository.SaveChangesAsync();

            var postId = await this.postsService.CreateAsync(1, 1, "name", "content", "userId");

            var post = await this.postsRepository.All().FirstAsync();

            Assert.AreEqual(postId, post.Id);
            Assert.AreEqual(1, post.ForumId);
            Assert.AreEqual(1, post.CategoryId);
            Assert.AreEqual("name", post.Name);
            Assert.AreEqual("content", post.Content);
            Assert.AreEqual("userId", post.UserId);
        }

        [Test]
        public async Task TestCreateAsyncAddsNewCategoryToForumIfItDoesNotExistWhenCreatingPost()
        {
            await this.forumsRepository.AddAsync(new Forum {Id = 1});
            await this.forumsRepository.SaveChangesAsync();

            var postId = await this.postsService.CreateAsync(1, 1, "name", "content", "userId");

            var post = await this.postsRepository.All().FirstAsync();

            var forum = await this.forumsRepository.All().FirstAsync();

            var forumCategory = forum.ForumCategories.First();

            Assert.AreEqual(1, forumCategory.ForumId);
            Assert.AreEqual(1, forumCategory.CategoryId);

            Assert.AreEqual(1, postId);
            Assert.AreEqual(1, post.ForumId);
            Assert.AreEqual(1, post.CategoryId);
            Assert.AreEqual("name", post.Name);
            Assert.AreEqual("content", post.Content);
            Assert.AreEqual("userId", post.UserId);
        }


        [Test]
        public async Task TestEditAsyncReturnsNullWithWrongPostId()
        {
            var postId = await this.postsService.EditAsync(1, "name", "content");

            Assert.Null(postId);
        }

        [Test]
        public async Task TestEditAsyncWorksCorrectly()
        {
            await this.postsRepository.AddAsync(new Post {Name = "name", Content = "content"});
            await this.postsRepository.SaveChangesAsync();

            var postId = await this.postsService.EditAsync(1, "new name", "new content");

            var post = await this.postsRepository.All().FirstAsync();

            Assert.AreEqual(1, postId);
            Assert.AreEqual("new name", post.Name);
            Assert.AreEqual("new content", post.Content);
        }

        [Test]
        public async Task TestDeleteAsyncReturnsNullWithInvalidId()
        {
            var postId = await this.postsService.DeleteAsync(1);

            Assert.Null(postId);
        }

        [Test]
        public async Task TestDeleteAsync()
        {
            await this.postsRepository.AddAsync(new Post
            {
                Id = 5,
                Name = "name",
                Content = "content",
                Replies = new List<Reply> {new Reply {PostId = 5}, new Reply {PostId = 5}, new Reply {PostId = 5}},
            });
            await this.postsRepository.SaveChangesAsync();

            var postId = await this.postsService.DeleteAsync(5);

            var post = await this.postsRepository.AllWithDeleted().FirstAsync();

            Assert.AreEqual(5, postId);
            Assert.IsTrue(post.IsDeleted);

            foreach (var reply in post.Replies)
            {
                Assert.IsTrue(reply.IsDeleted);
            }
        }

        [Test]
        public async Task TaskGetCount()
        {
            for (int i = 0; i < 5; i++)
            {
                await this.postsRepository.AddAsync(new Post());
            }

            await this.postsRepository.SaveChangesAsync();

            var count = this.postsService.GetCount();

            Assert.AreEqual(5, count);
        }

        [Test]
        public async Task TestGetCountByForumId()
        {
            for (int i = 0; i < 5; i++)
            {
                await this.postsRepository.AddAsync(new Post {ForumId = 1});
            }

            for (int i = 2; i < 5; i++)
            {
                await this.postsRepository.AddAsync(new Post {ForumId = i});
            }

            await this.postsRepository.SaveChangesAsync();

            var count = this.postsService.GetCountByForumId(1);

            Assert.AreEqual(5, count);
        }

        [Test]
        public async Task TesGetCountByCategoryNameAndForumId()
        {
            for (int i = 0; i < 5; i++)
            {
                await this.postsRepository.AddAsync(new Post
                    {ForumId = 1, Category = new Category {Name = "category"}});
            }

            for (int i = 2; i < 5; i++)
            {
                await this.postsRepository.AddAsync(new Post
                    {ForumId = i, Category = new Category {Name = "category"}});
            }

            await this.postsRepository.AddAsync(new Post
                {ForumId = 1, Category = new Category {Name = "wrong category"}});
            await this.postsRepository.SaveChangesAsync();

            var count = this.postsService.GetCountByCategoryNameAndForumId("category", 1, null);

            Assert.AreEqual(5, count);
        }
    }

    public class TestPost : IMapFrom<Post>
    {
        public string CategoryName { get; set; }

        public int ForumId { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}