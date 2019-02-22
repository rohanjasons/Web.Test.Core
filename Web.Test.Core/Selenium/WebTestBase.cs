using System;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using Web.Test.Core.Constants;
using Web.Test.Core.Enums;

namespace Web.Test.Core.Selenium
{
    /// <summary>
    /// Base class used for all tests to inherit.
    /// </summary>
    public abstract class WebTestBase
    {
        /// <summary>
        /// Gets instance of the WebDriver used during the tests.
        /// </summary>
        protected IWebDriver WebBrowserDriver { get; private set; }

        /// <summary>
        /// Start up the WebDriver and navigate to the URL specified.
        /// </summary>
        /// <param name="url">The Url that will be loaded in the web page.</param>
        /// <param name="deleteAllCookies">Should the cookies be deleted before starting the browser.</param>
        /// <param name="webDriver">The webdriver that will be used during the test.</param>
        protected void CommonTestSetup(Uri url, bool deleteAllCookies = true, WebDriver webDriver = WebDriver.Firefox)
        {
            switch (webDriver)
            {
                case WebDriver.Firefox:
                    InitialiseFirefoxLocal(url, deleteAllCookies);
                    break;
                case WebDriver.FirefoxBuild:
                    InitialiseFirefox(url, deleteAllCookies);
                    break;
                case WebDriver.FirefoxHeadless:
                    InitialiseFirefoxHeadless(url, deleteAllCookies);
                    break;
                case WebDriver.InternetExplorer:
                    InitialiseInternetExplorerLocal(url, deleteAllCookies);
                    break;
                case WebDriver.InternetExplorerBuild:
                    InitialiseInternetExplorer(url, deleteAllCookies);
                    break;
                case WebDriver.Chrome:
                    InitialiseChromeLocal(url, deleteAllCookies);
                    break;
                case WebDriver.ChromeBuild:
                    InitialiseChromeBuild(url, deleteAllCookies);
                    break;
                case WebDriver.ChromeHeadless:
                    InitialiseChromeHeadless(url, deleteAllCookies);
                    break;
            }
        }

        /// <summary>
        /// Initialise Firefox webdriver
        /// </summary>
        /// <param name="url">The url that will be navigated to</param>
        /// <param name="deleteAllCookies">Boolean to determin whether you want to delete cookies prior to openinig browser</param>
        private void InitialiseFirefoxLocal(Uri url, bool deleteAllCookies)
        {
            var firefoxOptions = new FirefoxOptions();
            firefoxOptions.SetPreference("browser.download.folderList", 2);
            firefoxOptions.SetPreference("browser.download.dir", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\");
            firefoxOptions.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/zip");
            firefoxOptions.AddArguments("--disable-gpu");  // This should only be enabled if you wish to execute the tests headless
            var driverService = FirefoxDriverService.CreateDefaultService(@"C:\Selenium", "geckodriver.exe");
            driverService.FirefoxBinaryPath = @"C:\Program Files\Firefox Developer Edition\firefox.exe";
            driverService.HideCommandPromptWindow = true;
            driverService.SuppressInitialDiagnosticInformation = true;
            WebBrowserDriver = new FirefoxDriver(driverService, firefoxOptions, TimeSpan.FromSeconds(Timeouts.DefaultTimeout));
            InitialiseWebDriver(url, deleteAllCookies);
        }

        /// <summary>
        /// Initialise Firefox webdriver
        /// </summary>
        /// <param name="url">The url that will be navigated to</param>
        /// <param name="deleteAllCookies">Boolean to determin whether you want to delete cookies prior to openinig browser</param>
        private void InitialiseFirefoxHeadless(Uri url, bool deleteAllCookies)
        {
            var firefoxOptions = new FirefoxOptions();
            firefoxOptions.SetPreference("browser.download.folderList", 2);
            firefoxOptions.SetPreference("browser.download.dir", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\");
            firefoxOptions.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/zip");
            firefoxOptions.AddArguments("--headless", "--disable-gpu");  // This should only be enabled if you wish to execute the tests headless
            var driverService = FirefoxDriverService.CreateDefaultService(@"C:\Selenium", "geckodriver.exe");
            driverService.FirefoxBinaryPath = @"C:\Program Files\Firefox Developer Edition\firefox.exe";
            driverService.HideCommandPromptWindow = true;
            driverService.SuppressInitialDiagnosticInformation = true;
            WebBrowserDriver = new FirefoxDriver(driverService, firefoxOptions, TimeSpan.FromSeconds(Timeouts.DefaultTimeout));
            InitialiseWebDriver(url, deleteAllCookies);
        }

        /// <summary>
        /// Initialise firefox on the build server
        /// </summary>
        /// <param name="url">The url that will be navigated to</param>
        /// <param name="deleteAllCookies">Boolean to determin whether you want to delete cookies prior to openinig browser</param>
        private void InitialiseFirefox(Uri url, bool deleteAllCookies)
        {
            var firefoxOptions = new FirefoxOptions();
            firefoxOptions.SetPreference("browser.download.folderList", 2);
            firefoxOptions.SetPreference("browser.download.dir", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\");
            firefoxOptions.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/zip");
            firefoxOptions.AddArguments("--headless");
            WebBrowserDriver = new FirefoxDriver(Environment.GetEnvironmentVariable("GeckoWebDriver"), firefoxOptions, TimeSpan.FromSeconds(Timeouts.DefaultTimeout));
            InitialiseWebDriver(url, deleteAllCookies);
        }

        /// <summary>
        /// Initialise Internet explorer on the local webdriver
        /// </summary>
        /// <param name="url">The url that will be navigated to</param>
        /// <param name="deleteAllCookies">Boolean to determin whether you want to delete cookies prior to openinig browser</param>
        private void InitialiseInternetExplorerLocal(Uri url, bool deleteAllCookies)
        {
            var internetExplorerOptions = new InternetExplorerOptions();
            internetExplorerOptions.AddAdditionalCapability(CapabilityType.Platform, new Platform(PlatformType.Windows));
            internetExplorerOptions.AddAdditionalCapability("IgnoreZoomLevel", true);
            internetExplorerOptions.AddAdditionalCapability("InitialBrowserUrl", "about:blank");
            internetExplorerOptions.AddAdditionalCapability("IntroduceInstabilityByIgnoringProtectedModeSettings", true);
            internetExplorerOptions.AddAdditionalCapability("EnableNativeEvents", true);
            internetExplorerOptions.AddAdditionalCapability("EnsureCleanSession", true);
            WebBrowserDriver = new InternetExplorerDriver(internetExplorerOptions);
            InitialiseWebDriver(url, deleteAllCookies);
        }

        /// <summary>
        /// Initialise internet explorer on selenium grid
        /// </summary>
        /// <param name="url">The url that will be navigated to</param>
        /// <param name="deleteAllCookies">Boolean to determin whether you want to delete cookies prior to openinig browser</param>
        private void InitialiseInternetExplorer(Uri url, bool deleteAllCookies)
        {
            var capabilities = new InternetExplorerOptions();
            capabilities.AddAdditionalCapability(CapabilityType.Platform, new Platform(PlatformType.Windows));
            capabilities.AddAdditionalCapability("IgnoreZoomLevel", true);
            capabilities.AddAdditionalCapability("InitialBrowserUrl", "about:blank");
            capabilities.AddAdditionalCapability("IntroduceInstabilityByIgnoringProtectedModeSettings", true);
            capabilities.AddAdditionalCapability("EnableNativeEvents", true);
            capabilities.AddAdditionalCapability("EnsureCleanSession", true);
            WebBrowserDriver = new RemoteWebDriver(new Uri(ConfigurationManager.AppSettings["browserDriver"]), capabilities.ToCapabilities());
            InitialiseWebDriver(url, deleteAllCookies);
        }

        /// <summary>
        /// Initialise chrome on the local webdriver
        /// </summary>
        /// <param name="url">The url that will be navigated to</param>
        /// <param name="deleteAllCookies">Boolean to determin whether you want to delete cookies prior to openinig browser</param>
        private void InitialiseChromeLocal(Uri url, bool deleteAllCookies)
        {
            var options = new ChromeOptions();
            options.AddUserProfilePreference("download.prompt_for_download", false);
            options.AddUserProfilePreference("download.directory_upgrade", true);
            options.AddUserProfilePreference("safebrowsing.enabled", true);
            options.AddUserProfilePreference("credentials_enable_service", false);
            options.AddUserProfilePreference("profile.password_manager_enabled", false);
            options.AddArguments("chrome.switches", "--disable-gpu", "--disable-popup-blocking", "--disable-extensions", "--disable-extensions-http-throttling", "--disable-extensions-file-access-check", "--disable-infobars", "--enable-automation", "--safebrowsing-disable-download-protection ", "--safebrowsing-disable-extension-blacklist", "--start-maximized");
            WebBrowserDriver = new ChromeDriver(options);
            InitialiseWebDriver(url, deleteAllCookies);
        }

        /// <summary>
        /// Initialise chrome on the local webdriver
        /// </summary>
        /// <param name="url">The url that will be navigated to</param>
        /// <param name="deleteAllCookies">Boolean to determin whether you want to delete cookies prior to openinig browser</param>
        private void InitialiseChromeHeadless(Uri url, bool deleteAllCookies)
        {
            var options = new ChromeOptions();
            options.AddUserProfilePreference("download.prompt_for_download", false);
            options.AddUserProfilePreference("download.directory_upgrade", true);
            options.AddUserProfilePreference("safebrowsing.enabled", true);
            options.AddUserProfilePreference("credentials_enable_service", false);
            options.AddUserProfilePreference("profile.password_manager_enabled", false);
            options.AddArguments("chrome.switches", "headless", "--disable-gpu", "--disable-popup-blocking", "--disable-extensions", "--disable-extensions-http-throttling", "--disable-extensions-file-access-check", "--disable-infobars", "--enable-automation", "--safebrowsing-disable-download-protection ", "--safebrowsing-disable-extension-blacklist", "--start-maximized");
            WebBrowserDriver = new ChromeDriver(options);
            InitialiseWebDriver(url, deleteAllCookies);
        }

        /// <summary>
        /// Initialise chrome on the build server
        /// </summary>
        /// <param name="url">The url that will be navigated to</param>
        /// <param name="deleteAllCookies">Boolean to determin whether you want to delete cookies prior to openinig browser</param>
        private void InitialiseChromeBuild(Uri url, bool deleteAllCookies)
        {
            var options = new ChromeOptions();
            options.AddUserProfilePreference("download.prompt_for_download", false);
            options.AddUserProfilePreference("download.directory_upgrade", true);
            options.AddUserProfilePreference("safebrowsing.enabled", true);
            options.AddUserProfilePreference("credentials_enable_service", false);
            options.AddUserProfilePreference("profile.password_manager_enabled", false);
            options.AddArguments("chrome.switches", "--disable-popup-blocking", "--disable-extensions", "--disable-extensions-http-throttling", "--disable-extensions-file-access-check", "--disable-infobars", "--enable-automation", "--safebrowsing-disable-download-protection ", "--safebrowsing-disable-extension-blacklist", "--start-maximized");
            options.AddArguments("headless");
            WebBrowserDriver = new ChromeDriver(Environment.GetEnvironmentVariable("ChromeWebDriver"), options);
            InitialiseWebDriver(url, deleteAllCookies);
        }

        /// <summary>
        /// Navigate to inputted Url and maximise browser
        /// </summary>
        /// <param name="url">The url that will be navigated to</param>
        /// <param name="deleteAllCookies">Boolean to determin whether you want to delete cookies prior to openinig browser</param>
        private void InitialiseWebDriver(Uri url, bool deleteAllCookies)
        {
            const int maxAttempts = 3;
            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                var message = string.Empty;
                try
                {
                    SetTimeout.PageLoad(WebBrowserDriver, TimeSpan.FromSeconds(TimeoutInSeconds.DefaultTimeout));

                    if (deleteAllCookies)
                    {
                        WebBrowserDriver.Manage().Cookies.DeleteAllCookies();
                    }

                    WebBrowserDriver.Manage().Cookies.DeleteAllCookies();
                    WebBrowserDriver.Navigate().GoToUrl(url);
                    break;
                }
                catch (WebDriverException exception)
                {
                    message = message + $"Exception {attempt}:" + exception.Message;
                    if (attempt >= maxAttempts)
                    {
                        throw new WebDriverException(string.Format($"Failed to start Web Browser in timely manner. - {message}"));
                    }
                }
            }
        }
    }
}
