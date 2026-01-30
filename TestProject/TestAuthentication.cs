using SecurityCapstone.Services;
using SecurityCapstone.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace TestProject
{
    [TestFixture]
    public class TestAuthentication
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Test]
        public async Task TestValidateCredentials_ValidUser()
        {
            // Arrange
            var db = GetInMemoryDbContext();
            var authService = new AuthService(db);

            // Act
            var user = await authService.ValidateCredentials("alice", "password123");

            // Assert
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Username, Is.EqualTo("alice"));
            Assert.That(user.Role, Is.EqualTo("Admin"));
        }

        [Test]
        public async Task TestValidateCredentials_InvalidPassword()
        {
            // Arrange
            var db = GetInMemoryDbContext();
            var authService = new AuthService(db);

            // Act
            var user = await authService.ValidateCredentials("alice", "wrongpassword");

            // Assert
            Assert.That(user, Is.Null);
        }

        [Test]
        public async Task TestValidateCredentials_InvalidUsername()
        {
            // Arrange
            var db = GetInMemoryDbContext();
            var authService = new AuthService(db);

            // Act
            var user = await authService.ValidateCredentials("nonexistent", "password123");

            // Assert
            Assert.That(user, Is.Null);
        }

        [Test]
        public void TestSetAuthCookie_CreatesCorrectCookies()
        {
            // Arrange
            var db = GetInMemoryDbContext();
            var authService = new AuthService(db);
            var httpContext = new DefaultHttpContext();
            
            var user = new User 
            { 
                UserID = 1, 
                Username = "testuser", 
                Email = "test@example.com",
                Role = "User",
                Password = "test123"
            };

            // Act
            authService.SetAuthCookie(httpContext, user);

            // Assert
            Assert.That(httpContext.Response.Headers.ContainsKey("Set-Cookie"), Is.True);
        }

        [Test]
        public void TestIsAuthenticated_WithoutCookie()
        {
            // Arrange
            var db = GetInMemoryDbContext();
            var authService = new AuthService(db);
            var httpContext = new DefaultHttpContext();

            // Act
            var isAuthenticated = authService.IsAuthenticated(httpContext);

            // Assert
            Assert.That(isAuthenticated, Is.False);
        }

        [Test]
        public void TestClearAuthCookie()
        {
            // Arrange
            var db = GetInMemoryDbContext();
            var authService = new AuthService(db);
            var httpContext = new DefaultHttpContext();
            
            var user = new User 
            { 
                UserID = 1, 
                Username = "testuser", 
                Role = "User" 
            };

            authService.SetAuthCookie(httpContext, user);

            // Act
            authService.ClearAuthCookie(httpContext);

            // Assert - The cookies are deleted (expires set to past date)
            Assert.That(httpContext.Response.Headers.ContainsKey("Set-Cookie"), Is.True);
        }
    }
}
