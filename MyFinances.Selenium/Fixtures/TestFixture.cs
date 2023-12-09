using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MyFinances.Selenium.Fixtures;

public class TestFixture : IDisposable
{
    public IWebDriver Driver { get; set; }

    // Setup
    public TestFixture()
    {
        Driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
    }
    
    // Tear Down
    public void Dispose()
    {
        Driver.Quit();
    }
}