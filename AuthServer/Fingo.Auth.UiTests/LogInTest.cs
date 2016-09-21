using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using Xunit;

namespace Fingo.Auth.UiTests
{
    public class LogInTest
    {
        [Fact]
        public void ShouldBeAbleToLogInUsingLoginQAtQAndPasswordQ()
        {
            var service = PhantomJSDriverService.CreateDefaultService();
            service.IgnoreSslErrors = true;

            using (IWebDriver wdriver = new PhantomJSDriver(service))
            {
                wdriver.Navigate().GoToUrl("http://192.168.2.94:1251");

                var loginBox =
                    wdriver.FindElement(
                        By.XPath(
                            "/html/body/div[@class='container body-content']/form/div[@id='login']/div[@class='col-md-offset-4 col-md-10'][1]/div/input[@id='Login']"));
                loginBox.SendKeys("q@q");

                var passwordBox =
                    wdriver.FindElement(
                        By.XPath(
                            "/html/body/div[@class='container body-content']/form/div[@id='login']/div[@class='col-md-offset-4 col-md-10'][2]/div/input[@id='Password']"));
                passwordBox.SendKeys("q");

                var logInButton =
                    wdriver.FindElement(
                        By.XPath(
                            "/html/body/div[@class='container body-content']/form/div[@id='login']/div[3]/div[@class='col-md-offset-4 col-md-10']/input[@class='btn btn-success']"));
                logInButton.Click();

                var signOutButton =
                    wdriver.FindElement(
                        By.XPath(
                            "/html/body/header/div[@class='navbar navbar-inverse navbar-fixed-top']/div[@class='container']/div[@class='navbar-collapse collapse']/ul[@class='nav navbar-nav navbar-right']/li[1]/a"));

                wdriver.Quit();
            }
        }
    }
}