using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace Noodles.Tests.Web.Steps
{
    [Binding]
    public class NodeMethodsSteps
    {
        [Given(@"I am on a page with a node method link")]
        public void GivenIAmOnAPageWithANodeMethodLink()
        {
            ScenarioContextHelper.WebDriver.Navigate().GoToUrl(ScenarioContextHelper.WebServer.RootUrl);
//            ScenarioContextHelper.WebDriver.WaitForReady();
        }

        [When(@"I click the node method link")]
        public void WhenIClickTheNodeMethodLink()
        {
            ScenarioContextHelper.WebDriver.FindElement(By.LinkText("Sign in"), 10).Click();
        }

        [Then(@"a node method form appears")]
        public void ThenANodeMethodFormAppears()
        {
            // <button class="submitMethod btn btn-primary">Sign in</button>
            Assert.NotNull(ScenarioContextHelper.WebDriver.FindElements(By.CssSelector("button.submitMethod"), 10)
                                                .SingleOrDefault(x => x.Text == "Sign in"));
        }

        [AfterTestRun]
        public static void CleanUp()
        {
            ScenarioContextHelper.StopDriverIfExists();
            ScenarioContextHelper.StopWebServer();
        }
    }
}
