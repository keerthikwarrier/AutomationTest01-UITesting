using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;

namespace ResolutionTesting
{
    [TestFixture("Chrome", "1920x1080")]
    [TestFixture("Chrome", "1366x768")]
    [TestFixture("Chrome", "1536x864")]
    [TestFixture("Firefox", "1920x1080")]
    [TestFixture("Firefox", "1366x768")]
    [TestFixture("Firefox", "1536x864")]
    [TestFixture("Safari", "1920x1080")]
    [TestFixture("Safari", "1366x768")]
    [TestFixture("Safari", "1536x864")]
    //[TestFixture("MobileChrome", "360x640")]
    //[TestFixture("MobileChrome", "414x896")]
    //[TestFixture("MobileChrome", "375x667")]
    public class ResolutionTests
    {
        private IWebDriver driver;
        private string browser;
        private string resolution;
        public static String dir = AppDomain.CurrentDomain.BaseDirectory;
        public static String testResultPath = dir.Replace("bin\\Debug\\net8.0", "Desktop-Screenshots");
        public ResolutionTests(string browser, string resolution)
        {
            this.browser = browser;
            this.resolution = resolution;
        }
        [SetUp]
        public void Setup()
        {
            switch (browser)
            {
                case "Chrome":
                    driver = new ChromeDriver();
                    break;
                case "Firefox":
                    driver = new FirefoxDriver();
                    break;
                case "Safari":
                    driver = new SafariDriver();
                    break;
                //case "MobileChrome":
                //    ChromeOptions mobileOptions = new ChromeOptions();
                //    mobileOptions.EnableMobileEmulation(new ChromeMobileEmulationDeviceSettings
                //    {
                //        Width = int.Parse(resolution.Split('x')[0]),
                //        Height = int.Parse(resolution.Split('x')[1]),
                //        UserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Mobile Safari/537.36"
                //    });
                //    driver = new ChromeDriver(mobileOptions);
                //    break;
                default:
                    throw new ArgumentException("Invalid browser specified");
            }

            driver.Manage().Window.Size = ParseResolution(resolution);
        }

        [Test]
        
        public void TestWebsite()
        {
            // Navigate to the website URL
            driver.Navigate().GoToUrl("https://www.getcalley.com/page-sitemap.xml");

            //This will store all the links that need to be clicked
            string[] linksToClick = { "https://www.getcalley.com/", "https://www.getcalley.com/calley-call-from-browser/", "https://www.getcalley.com/calley-pro-features/", "https://www.getcalley.com/best-auto-dialer-app/", "https://www.getcalley.com/how-calley-auto-dialer-app-works/" };

            //foreach loop will iterate through all the links and takes the screenshots with appropriate names
            foreach (string linkText in linksToClick)
            {
                // Find the link by its text
                IWebElement link = driver.FindElement(By.LinkText(linkText));
                link.Click();

                // Take a screenshot of the entire page
                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();

                // Create folder structure based on device and resolution
                string folderPath = Path.Combine(testResultPath, $"{browser}_{resolution}");
                Directory.CreateDirectory(folderPath);

                // Save the screenshot with timestamp
                string screenshotName = $"Screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                string screenshotPath = Path.Combine(folderPath, screenshotName);
                screenshot.SaveAsFile(screenshotPath);

                // Log the screenshot path for reference
                Console.WriteLine($"Screenshot saved: {screenshotPath}");
                driver.Navigate().Back();

                //Validating the File Exists in FilePath
                Assert.IsTrue(File.Exists(screenshotPath), "Screenshot file does not exist");
            }
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
        private System.Drawing.Size ParseResolution(string resolution)
        {
            string[] parts = resolution.Split('x');
            int width = int.Parse(parts[0]);
            int height = int.Parse(parts[1]);
            return new System.Drawing.Size(width, height);
        }
    }
}