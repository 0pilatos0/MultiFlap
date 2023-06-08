using Microsoft.EntityFrameworkCore;
using Moq;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Server;
using Server.Controllers;
using Server.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore.Query;
using System.Reflection;

namespace ServerTests.Controllers
{
    public class BaseControllerTests
    {
        public interface IDbContext
        {
            DbSet<T> Set<T>() where T : class;
            Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        }

        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class UserController
        {
            private readonly IDbContext _dbContext;
            private readonly HttpClient _httpClient;

            public UserController(IDbContext dbContext, HttpClient httpClient)
            {
                _dbContext = dbContext;
                _httpClient = httpClient;
            }

            public async Task<User> GetUserFromIdAsync(int id)
            {
                var users = await _dbContext.Set<User>().ToListAsync();
                return users.FirstOrDefault(u => u.Id == id);
            }

            public async Task<string> GetAuth0IdFromAuthorizedRequestAsync(string authorizationHeader)
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                    throw new ArgumentNullException(nameof(authorizationHeader));

                var response = await _httpClient.GetAsync("https://example.com/api/user", CancellationToken.None);
                response.EnsureSuccessStatusCode();

                // Some logic to extract and return the user ID
                return "12345";
            }
        }

        [Fact]
        public async Task GetUserFromIdAsync_WithExistingUser_ReturnsUser()
        {
            // Arrange
            var dbContextMock = new Mock<IDbContext>();
            var httpClientMock = new Mock<HttpClient>();

            var users = new List<User>
            {
                new User { Id = 1, Name = "John Doe" },
                new User { Id = 2, Name = "Jane Smith" }
            };

            dbContextMock
                .Setup(db => db.Set<User>())
                .Returns(MockDbSet(users));

            var controller = new UserController(dbContextMock.Object, httpClientMock.Object);

            // Act
            var user = await controller.GetUserFromIdAsync(1);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(1, user.Id);
            Assert.Equal("John Doe", user.Name);
        }

        [Fact]
        public async Task GetUserFromIdAsync_WithNewUser_ReturnsNull()
        {
            // Arrange
            var dbContextMock = new Mock<IDbContext>();
            var httpClientMock = new Mock<HttpClient>();

            var users = new List<User>();

            dbContextMock
                .Setup(db => db.Set<User>())
                .Returns(MockDbSet(users));

            var controller = new UserController(dbContextMock.Object, httpClientMock.Object);

            // Act
            var user = await controller.GetUserFromIdAsync(1);

            // Assert
            Assert.Null(user);
        }

        private static DbSet<T> MockDbSet<T>(List<T> data) where T : class
        {
            var queryableData = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<T>(queryableData.Provider));
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryableData.GetEnumerator());
            mockSet.As<IAsyncEnumerable<T>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestAsyncEnumerator<T>(queryableData.GetEnumerator()));

            return mockSet.Object;
        }

        private class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
        {
            private readonly IQueryProvider _inner;

            internal TestAsyncQueryProvider(IQueryProvider inner)
            {
                _inner = inner;
            }

            public IQueryable CreateQuery(Expression expression)
            {
                return new TestAsyncEnumerable<TEntity>(expression);
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                return new TestAsyncEnumerable<TElement>(expression);
            }

            public object Execute(Expression expression)
            {
                return _inner.Execute(expression);
            }

            public TResult Execute<TResult>(Expression expression)
            {
                return _inner.Execute<TResult>(expression);
            }

            public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
            {
                var resultType = typeof(TResult).GetTypeInfo();
                var executeAsyncMethod = typeof(EntityFrameworkQueryableExtensions)
                    .GetMethods()
                    .First(m => m.Name == "ExecuteAsync" && m.ContainsGenericParameters && m.GetParameters().Length == 2)
                    .MakeGenericMethod(resultType.GenericTypeArguments[0]);

                return (TResult)executeAsyncMethod.Invoke(null, new object[] { _inner, expression, cancellationToken });
            }

        }

        private class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
        {
            public TestAsyncEnumerable(IEnumerable<T> enumerable)
                : base(enumerable)
            {
            }

            public TestAsyncEnumerable(Expression expression)
                : base(expression)
            {
            }

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
            }

            IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
        }

        private class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _inner;

            public TestAsyncEnumerator(IEnumerator<T> inner)
            {
                _inner = inner;
            }

            public T Current => _inner.Current;

            public async ValueTask<bool> MoveNextAsync()
            {
                return await Task.FromResult(_inner.MoveNext());
            }

            public async ValueTask DisposeAsync()
            {
                await Task.FromResult(true);
                _inner.Dispose();
            }
        }




        [Fact]
        public async Task GetAuth0IdFromAuthorizedRequestAsync_WithInvalidAuthorizationHeader_ThrowsException()
        {
            // Arrange
            var dbContextMock = new Mock<IDbContext>();
            var httpClientMock = new Mock<HttpClient>();

            var controller = new UserController(dbContextMock.Object, httpClientMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => controller.GetAuth0IdFromAuthorizedRequestAsync(null));
        }
    }
}
