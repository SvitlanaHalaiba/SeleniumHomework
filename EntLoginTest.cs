using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumHomework
{
    [TestFixture]
    public class EntLoginTest
    {
        private IWebDriver drv;

        [SetUp]
        public void Setup()
        {
            drv = new ChromeDriver();
        }

        [Test]
        public void PositiveLoginTest()
        {
            drv.Navigate().GoToUrl(Environment.GetEnvironmentVariable("ENT_QA_BASE_URL") + "/CorpNet/Login.aspx");
            drv.FindElement(By.Id("username")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_USER"));
            drv.FindElement(By.Id("password")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_PASS"));
            drv.FindElement(By.Name("_companyText")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_COMPANY"));
            drv.FindElement(By.CssSelector("input.btn.login-submit-button")).Click();

            WebDriverWait wait = new WebDriverWait(drv, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.menu-secondary ul li.menu-user a.menu-drop")));

            Assert.IsTrue(IsElementVisible(By.CssSelector("div.menu-secondary ul li.menu-user a.menu-drop")));
        }

        [Test]
        public void NegativeLoginTest()
        {
            drv.Navigate().GoToUrl(Environment.GetEnvironmentVariable("ENT_QA_BASE_URL") + "/CorpNet/Login.aspx");
            drv.FindElement(By.Id("username")).SendKeys("invalid_username");
            drv.FindElement(By.Id("password")).SendKeys("invalid_password");
            drv.FindElement(By.Name("_companyText")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_COMPANY"));
            drv.FindElement(By.CssSelector("input.btn.login-submit-button")).Click();

            WebDriverWait wait = new WebDriverWait(drv, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.login-error-message")));

            Assert.IsFalse(IsElementVisible(By.CssSelector("div.menu-secondary ul li.menu-user a.menu-drop")));
        }

        private bool IsElementVisible(By locator)
        {
            return drv.FindElements(locator).Count > 0;
        }

        [TearDown]
        public void Cleanup()
        {
            drv.Quit();
        }
    }
}