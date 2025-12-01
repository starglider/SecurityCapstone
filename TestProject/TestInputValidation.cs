

using SecurityCapstone.Pages;

namespace TestProject {

    [TestFixture]
    public class TestInputValidation
    {
        [Test]
        public void TestForSQLInjection()
        {
            // Arrange
            string userInput = "'; DROP TABLE Users; --";
            string cleanInput = IndexModel.SanitizeInput(userInput);

            // Act & Assert
            Assert.That(!cleanInput.Contains('\''), "Sanitized input should not contain single quote.");
            Assert.That(!cleanInput.Contains(';'), "Sanitized input should not contain semicolon.");
        }

        [Test]
        public void TestForXSS()
        {
            // Arrange
            string userInput = "<script>alert('XSS');</script>";
            string cleanInput = IndexModel.SanitizeInput(userInput);
            // Act & Assert
            Assert.That(!cleanInput.Contains("<script>"), "Sanitized input should not contain script tags.");
        }


    }
}