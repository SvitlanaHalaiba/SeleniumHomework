using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace SeleniumHomework
{
    public class TestClass
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private LoginPage loginPage;
        private ReportListPage reportListPage;
        private ReportPage reportPage;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            loginPage = new LoginPage(driver);
            reportListPage = new ReportListPage(driver);
            reportPage = new ReportPage(driver);
        }

        [TearDown]
        public void OneTimeTearDown()
        {
            driver.Quit();
        }

        [Test]
        public void VerifyReportGeneration()
        {
            string baseUrl = Environment.GetEnvironmentVariable("ENT_QA_BASE_URL");
            string username = Environment.GetEnvironmentVariable("ENT_QA_USER");
            string password = Environment.GetEnvironmentVariable("ENT_QA_PASS");
            string company = Environment.GetEnvironmentVariable("ENT_QA_COMPANY");
            string reportName = "reportName";

            loginPage.GoToLoginPage(baseUrl);
            loginPage.FillLoginCredentials(username, password, company);
            loginPage.ClickLoginSubmitButton();

            var originalWindow = driver.CurrentWindowHandle;

            reportListPage.GoToReportListPage(baseUrl);
            reportListPage.ClickReportLink();

            reportPage.EnableHeaders();
            reportPage.GenerateReport();

            Assert.IsTrue(reportPage.IsHeaderDisplayed(reportName), "Header not found");

            reportPage.CloseReportWindow();

            driver.SwitchTo().Window(originalWindow);
            driver.FindElement(By.CssSelector(".id-btn-close")).Click();
        }
    }
}
