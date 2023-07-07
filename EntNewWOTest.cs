using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers; // Added namespace for ExpectedConditions
using System;
namespace SeleniumHomework
{
    [TestFixture]
    public class EntNewWOTest
    {
        private IWebDriver drv;
        private WebDriverWait wait;
        [SetUp]
        public void Setup()
        {
            drv = new ChromeDriver();
            wait = new WebDriverWait(drv, TimeSpan.FromSeconds(10));
        }
        [TearDown]
        public void Teardown()
        {
            drv.Quit();
        }
        [Test]
        public void TestWOflow()
        {
            // Go to login page
            drv.Navigate().GoToUrl(Environment.GetEnvironmentVariable("ENT_QA_BASE_URL") + "/CorpNet/Login.aspx");
            // Fill in login credentials
            drv.FindElement(By.Id("username")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_USER"));
            drv.FindElement(By.Id("password")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_PASS"));
            drv.FindElement(By.Name("_companyText")).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_COMPANY"));
            drv.FindElement(By.CssSelector("input.btn.login-submit-button")).Click();
            // Wait for menu drop-down to be visible
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.menu-secondary ul li.menu-user a.menu-drop")));
            var pickUpComment = "Pick-Up Comment From Autotest";
            // Go to WO List page
            drv.Navigate().GoToUrl(Environment.GetEnvironmentVariable("ENT_QA_BASE_URL") + "/corpnet/workorder/workorderlist.aspx");
            // Wait for the first New WO link to be visible
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//td[@data-column='WOStatus'][contains(text(), 'New')][1]/following-sibling::td/a")));
            var woLink = By.XPath("//td[@data-column='WOStatus'][contains(text(), 'New')][1]/following-sibling::td/a");
            var woNumber = drv.FindElement(woLink).Text;
            // Click on the New WO link
            drv.FindElement(woLink).Click();
            // Wait for Pick-Up button to be visible
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@data-role='woqvdialog']//span[@title='Pick-Up']")));
            // Click on the Pick-Up button
            drv.FindElement(By.XPath("//div[@data-role='woqvdialog']//span[@title='Pick-Up']")).Click();
            // Wait for Comment area to be visible
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//form[@class='corrigo-form']/div/textarea")));
            // Click on the Comment area
            drv.FindElement(By.XPath("//form[@class='corrigo-form']/div/textarea")).Click();
            // Enter the Pick-Up comment
            drv.FindElement(By.CssSelector("form.corrigo-form textarea")).SendKeys(pickUpComment);
            // Click the Save button to close the dialog
            drv.FindElement(By.CssSelector("div[data-role=woactionpickupeditdialog] button.id-btn-save")).Click();
            // Wait for the Action in the activity log to be "Picked Up"
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@data-role='woactivityloggrid']//tbody//tr[1]//td[@data-column='ActionTitle']")));
            var action = drv.FindElement(By.XPath("//*[@data-role='woactivityloggrid']//tbody//tr[1]//td[@data-column='ActionTitle']"));
            NUnit.Framework.Assert.That(action.Text, Is.EqualTo("Picked Up"));
            // Check that the Comment in the activity log matches the Pick-Up comment
            var comment = drv.FindElement(By.XPath("//*[@data-role='woactivityloggrid']//tbody//tr[1]//td[@data-column='Comment']"));
            NUnit.Framework.Assert.That(comment.Text, Is.EqualTo(pickUpComment));
            // Close the Quick View dialog
            drv.FindElement(By.XPath("//button[@class='close btn-dismiss']")).Click();
            // Wait for the WO Status to be "Open" on the WO list page
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//td[@data-column='Number']/a[contains(text(), '" + woNumber + "')]/../../td[@data-column='WOStatus']")));
            var woStatus = drv.FindElement(By.XPath("//td[@data-column='Number']/a[contains(text(), '" + woNumber + "')]/../../td[@data-column='WOStatus']"));
            NUnit.Framework.Assert.That(woStatus.Text, Is.EqualTo("Open"));
        }
    }
}
