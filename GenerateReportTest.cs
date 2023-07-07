using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace SeleniumHomework
{
    public class TestClass
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }

        [TearDown]
        public void OneTimeTearDown()
        {
            driver.Quit();
        }

        [Test]
        public void VerifyReportGeneration()
        {
            // Go to login page
            driver.Navigate().GoToUrl(Environment.GetEnvironmentVariable("ENT_QA_BASE_URL") + "/CorpNet/Login.aspx");
            // Fill in login credentials
            driver.FindElement(By.Id("username")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_USER"));
            driver.FindElement(By.Id("password")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_PASS"));
            driver.FindElement(By.Name("_companyText")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_COMPANY"));
            driver.FindElement(By.CssSelector("input.btn.login-submit-button")).Click();

            var originalWindow = driver.CurrentWindowHandle;
            driver.Navigate().GoToUrl($"{Environment.GetEnvironmentVariable("ENT_QA_BASE_URL")}/corpnet/report/reportlist.aspx");

            var reportLink = By.XPath("//td[@data-column='DisplayAs'][1]/a");

            wait.Until(driver => driver.FindElement(reportLink));

            driver.FindElement(reportLink).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".showHeaders .k-switch-container"))).Click();
            var currWindows = driver.WindowHandles.Count;

            driver.FindElement(By.CssSelector(".id-generate-button")).Click();

            wait.Until(driver => driver.WindowHandles.Count == currWindows);

            driver.SwitchTo().Window(driver.WindowHandles.Last());

            wait.Until(driver => driver.FindElement(By.CssSelector("[arial-label= 'Report table']")));
            Assert.IsTrue(driver.FindElements(By.XPath($"//span[contains(text(),'reportName')]")).Count > 0, "Header not found");

            driver.Close();

            driver.SwitchTo().Window(originalWindow);
            driver.FindElement(By.CssSelector(".id-btn-close")).Click();
        }
    }
}