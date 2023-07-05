using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Net;
using System.Runtime.InteropServices;
namespace SeleniumTraining
{
    public class Tests
    {
        IWebDriver drv;
        [SetUp]
        public void Setup()
        {
            ChromeOptions opt = new ChromeOptions();
            //opt.AddArguments("start-fullscreen", "auto-open-devtools-for-tabs");
            drv = new ChromeDriver(opt);
        }
        [TearDown]
        public void Teardown()
        {
            drv.Quit();
        }
        [Test]
        public void Test1()
        {
            drv.Navigate().GoToUrl("https://google.com");
            foreach (var cookies in drv.Manage().Cookies.AllCookies)
            {
                Console.WriteLine("drv cookies default: " + cookies);
            }
            drv.Manage().Cookies.AddCookie(new OpenQA.Selenium.Cookie(name: "myCookie", value: "myValue"));
            foreach (var cookies in drv.Manage().Cookies.AllCookies)
            {
                Console.WriteLine("drv cookies added: " + cookies);
            }
            drv.Manage().Cookies.DeleteCookieNamed(name: "myCookie");
            foreach (var cookies in drv.Manage().Cookies.AllCookies)
            {
                Console.WriteLine("drv cookies deleted: " + cookies);
            }
            drv.Manage().Cookies.DeleteAllCookies();
            foreach (var cookies in drv.Manage().Cookies.AllCookies)
            {
                Console.WriteLine("drv cookies deleted ALL: " + cookies);
            }
            drv.FindElement(By.CssSelector("[name=q]")).SendKeys(text: "Selenium");
        }
    }
}