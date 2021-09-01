using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;

namespace POC_FSK_Automation
{
    public class Tests
    {
        private ExtentReports extent;
       
        ExtentHtmlReporter htmlReporter;
        ExtentTest test;
        private ChromeDriver driver;

        [OneTimeSetUp]
        public void SetUpOnce()
        {
            htmlReporter = new ExtentHtmlReporter(@"C:\VWPOC\FSKAutomationReportSample.html");
            
            htmlReporter.Config.DocumentTitle = "Automated Test Report | FSK";
            htmlReporter.Config.ReportName = "Test Report | FSK";
            
            
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        
        }

        [SetUp]
        public void Setup()
        {
            //Initialize 
             driver = new ChromeDriver();

          
        }

        [Test]
        public void Test1()
        {
            test = extent.CreateTest("Passed Test Case");
            // This will open up the URL
            driver.Url = "https://www.demoblaze.com/";
            // Maxamize window
            driver.Manage().Window.Maximize();

            //Click login and enter credentials
            var loginbutton = driver.FindElement(By.Id("login2"));
            WebDriverWait wait = new WebDriverWait(driver, new System.TimeSpan(1000000000));
            DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver);
            
            // Wait till the button appears
            wait.Until(dr => driver.FindElement(By.Id("login2")).Displayed);

            //Click on login button
            loginbutton.Click();

            // Take Screenshot and save to disk
            // FUNC
           wait.Until(dr => driver.FindElement(By.Id("loginusername")).Displayed);
            
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            string screenshot = ss.AsBase64EncodedString;
            byte[] screenshotAsByteArray = ss.AsByteArray;
           
            ss.SaveAsFile("C:\\VWPOC\\loginsc.png", ScreenshotImageFormat.Png);
            ss.ToString();
            // FUNC

            // Write ID and password in TB
            //loginusername , loginpassword
            var login_username_tb = driver.FindElement(By.Id("loginusername"));
            var login_password_tb = driver.FindElement(By.Id("loginpassword"));

            login_username_tb.SendKeys("aksgaw");
            login_password_tb.SendKeys("aksgaw");

            //press login button 
            // driver.find_element_by_xpath("//input[@type='submit' and @value='something']").click()
            //finding element with xpath
            //*[@id="logInModal"]/div/div/div[3]/button[2]
            var login_button = driver.FindElementByXPath("//*[@id='logInModal']/div/div/div[3]/button[2]");


            login_button.Click();
          
            wait.Until(dr => !driver.FindElement(By.Id("loginusername")).Displayed);
            wait.Until(dr => driver.FindElement(By.Id("logout2")).Displayed);

            Screenshot ss2 = ((ITakesScreenshot)driver).GetScreenshot();
            string screenshot2 = ss.AsBase64EncodedString;
            byte[] screenshotAsByteArray2 = ss.AsByteArray;
            ss2.SaveAsFile("C:\\VWPOC\\loggedin.png", ScreenshotImageFormat.Png);
            ss2.ToString();
            test.Log(Status.Pass, "Test Case passed with some desc, please refer to the attached screenshot");
            test.AddScreenCaptureFromPath(@"C:\VWPOC\loginsc.png", "Passed Test Case Caption");
            Assert.Pass();
            
        }
        [Test]
        public void Test2()
        {
            test = extent.CreateTest("Failed Test");
            test.Log(Status.Fail, "Test Case failed with some desc, please refer to the attached screenshot");
            test.AddScreenCaptureFromPath(@"C:\VWPOC\loggedin.png","Failed Screenshot caption");
            Assert.Fail();
           
        }
        [TearDown]
        public void GetResult()
        {
            var nunutstatus = TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = TestContext.CurrentContext.Result.StackTrace;// to get stacktrace
            var errormessage = TestContext.CurrentContext.Result.Message;


            if (nunutstatus == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                test.Log(Status.Fail, nunutstatus + " " + errormessage);
            }
            if (nunutstatus == NUnit.Framework.Interfaces.TestStatus.Passed)
            {
                test.Log(Status.Pass, nunutstatus + " " + errormessage);
            }
            extent.Flush();

        }
       


    }
}