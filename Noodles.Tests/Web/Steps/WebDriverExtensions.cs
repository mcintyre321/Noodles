using System;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Noodles.Tests.Web.Steps
{
    public static class WebDriverExtensions
    {
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(drv => drv.FindElement(by));
        }

        public static ReadOnlyCollection<IWebElement> FindElements(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(drv => (drv.FindElements(by).Any()) ? drv.FindElements(by) : null);
        }

        public static void WaitForReady(this IWebDriver driver, int timeoutInSeconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            ((IJavaScriptExecutor) driver).ExecuteScript("$(document).ready( function(){ $('body').addClass('jquery-ready'); } );");
            driver.FindElement(By.CssSelector("body.jquery-ready"), 10);
        }
    }
}