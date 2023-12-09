using MyFinances.Selenium.Fixtures;
using OpenQA.Selenium;

namespace MyFinances.Selenium;

public class TestesDeApiPelaInterfaceDoSwagger : IClassFixture<TestFixture>
{
    private IWebDriver _driver;

    public TestesDeApiPelaInterfaceDoSwagger(TestFixture fixture)
    {
        _driver = fixture.Driver;
    }

    [Fact(Skip = "Realizar os testes de UI através da interface que for desenvolvida.")]
    public void Test1()
    {
        // Given
        
        // When
        _driver.Navigate().GoToUrl("https://localhost:44330/swagger/index.html");
        
        // Then
        Assert.Contains("Swagger UI", _driver.Title);
    }
}