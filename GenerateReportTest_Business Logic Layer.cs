using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace SeleniumHomework
{
    public class LoginPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void GoToLoginPage(string baseUrl)
        {
            driver.Navigate().GoToUrl(Environment.GetEnvironmentVariable("ENT_QA_BASE_URL") + "/CorpNet/Login.aspx");
        }

        public void FillLoginCredentials(string username, string password, string company)
        {
            driver.FindElement(By.Id("username")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_USER"));
            driver.FindElement(By.Id("password")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_PASS"));
            driver.FindElement(By.Name("_companyText")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_COMPANY"));
        }

        public void ClickLoginSubmitButton()
        {
            driver.FindElement(By.CssSelector("input.btn.login-submit-button")).Click();
        }
    }

    public class ReportListPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public ReportListPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void GoToReportListPage(string baseUrl)
        {
            driver.Navigate().GoToUrl($"{Environment.GetEnvironmentVariable("ENT_QA_BASE_URL")}/corpnet/report/reportlist.aspx");
        }

        public void ClickReportLink()
        {
            var reportLink = By.XPath("//td[@data-column='DisplayAs'][1]/a");
            wait.Until(driver => driver.FindElement(reportLink));
            driver.FindElement(reportLink).Click();
        }
    }

    public class ReportPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public ReportPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void EnableHeaders()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".showHeaders .k-switch-container"))).Click();
        }

        public void GenerateReport()
        {
            var currWindows = driver.WindowHandles.Count;
            driver.FindElement(By.CssSelector(".id-generate-button")).Click();
            wait.Until(driver => driver.WindowHandles.Count == currWindows);
            driver.SwitchTo().Window(driver.WindowHandles.Last());
        }

        public bool IsHeaderDisplayed(string reportName)
        {
            wait.Until(driver => driver.FindElement(By.CssSelector("[arial-label= 'Report table']")));
            return driver.FindElements(By.XPath($"//span[contains(text(),'{reportName}')]")).Count > 0;
        }

        public void CloseReportWindow()
        {
            driver.Close();
        }
    }
}