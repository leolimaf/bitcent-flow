using MyFinances.Selenium.Fixtures;
using OpenQA.Selenium;

namespace MyFinances.Selenium;

public class UnitTest1 : IClassFixture<TestFixture>
{
    private IWebDriver _driver;

    public UnitTest1(TestFixture fixture)
    {
        _driver = fixture.Driver;
    }

    [Fact]
    public void Test1()
    {
        // Given
        
        // When
        _driver.Navigate().GoToUrl("https://localhost:44330/swagger/index.html");
        
        // Then
        Assert.Contains("Swagger UI", _driver.Title);
    }
}