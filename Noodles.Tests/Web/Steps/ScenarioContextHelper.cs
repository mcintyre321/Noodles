using System;
using CassiniDev;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace Noodles.Tests.Web.Steps
{
    public static class ScenarioContextHelper
    {
        public static CassiniDevServer WebServer
        {
            get
            {
                if (!FeatureContext.Current.ContainsKey("webserver"))
                {
                    var server = new CassiniDevServer();
                    server.StartServer(@"..\..\..\Noodles.Example.Web");
                    FeatureContext.Current["webserver"] = server;
                }
                return (CassiniDevServer) FeatureContext.Current["webserver"];
            }
        }

        public static void StopWebServer()
        {
            if (FeatureContext.Current["webserver"] != null)
            {
                WebServer.Dispose();
            }
        }

        public static void StopDriverIfExists()
        {
            if (FeatureContext.Current["webdriver"] != null)
            {
                WebDriver.Quit();
                WebDriver.Dispose();
            }
        }
        
        public static IWebDriver WebDriver
        {
            get
            {
                if (!FeatureContext.Current.ContainsKey("webdriver"))
                {
                    var webDriver = new OpenQA.Selenium.PhantomJS.PhantomJSDriver();
//                    var webDriver = new OpenQA.Selenium.Chrome.ChromeDriver();
                    Console.WriteLine(WebServer.RootUrl);
                    FeatureContext.Current["webdriver"] = webDriver;
                }
                return (IWebDriver)FeatureContext.Current["webdriver"]; ;
            }
        }

    }
}
