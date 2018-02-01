using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalesforceTweets;
using SalesforceTweets.Controllers;

namespace SalesforceTweets.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);

            // Assert
            //Assert.AreEqual("Description:", result.ViewBag.Message);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception), "Error while getting the access token")]
        public void GetFailForIncorrectCredentials()
        {
            // Arrange
            HomeController controller = new HomeController();

            System.Web.Configuration.WebConfigurationManager.AppSettings["AuthenticationUrl"] = "https://api.twitter.com/oauth2/token";
            System.Web.Configuration.WebConfigurationManager.AppSettings["ConsumerKey"] = "test";
            System.Web.Configuration.WebConfigurationManager.AppSettings["ConsumerSecret"] = "test";
            System.Web.Configuration.WebConfigurationManager.AppSettings["TwitterReadUrl"] = "https://api.twitter.com/1.1/statuses/user_timeline.json";

            TwitterReadController contentController = new TwitterReadController();
            contentController.ReadResponse(10, "Salesforce");
        }

        [TestMethod]
        public void GetRecordsForSalesForceUserSuccess()
        {
            // Arrange
            HomeController controller = new HomeController();

            System.Web.Configuration.WebConfigurationManager.AppSettings["AuthenticationUrl"] = "https://api.twitter.com/oauth2/token";
            System.Web.Configuration.WebConfigurationManager.AppSettings["ConsumerKey"] = "PxrPt87pjbvTxwFX31WjaSdDN";
            System.Web.Configuration.WebConfigurationManager.AppSettings["ConsumerSecret"] = "i4fSXklY9U4BKl2za1VKJ9oLUhaPfv8U9VsxnezFNRuuxfnCes";
            System.Web.Configuration.WebConfigurationManager.AppSettings["TwitterReadUrl"] = "https://api.twitter.com/1.1/statuses/user_timeline.json";

            TwitterReadController contentController = new TwitterReadController();
            string response = contentController.ReadResponse(10, "Salesforce");
            Assert.IsNotNull(response);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException), "The remote server returned an error: (401) Unauthorized.")]
        public void GetFailsForIncorrectUsername()
        {
            // Arrange
            HomeController controller = new HomeController();

            System.Web.Configuration.WebConfigurationManager.AppSettings["AuthenticationUrl"] = "https://api.twitter.com/oauth2/token";
            System.Web.Configuration.WebConfigurationManager.AppSettings["ConsumerKey"] = "PxrPt87pjbvTxwFX31WjaSdDN";
            System.Web.Configuration.WebConfigurationManager.AppSettings["ConsumerSecret"] = "i4fSXklY9U4BKl2za1VKJ9oLUhaPfv8U9VsxnezFNRuuxfnCes";
            System.Web.Configuration.WebConfigurationManager.AppSettings["TwitterReadUrl"] = "https://api.twitter.com/1.1/statuses/user_timeline.json";

            TwitterReadController contentController = new TwitterReadController();
            contentController.ReadResponse(10, "Test");
        }

        [TestMethod]        
        public void GetTestForNumberofTweets()
        {
            // Arrange
            HomeController controller = new HomeController();

            System.Web.Configuration.WebConfigurationManager.AppSettings["AuthenticationUrl"] = "https://api.twitter.com/oauth2/token";
            System.Web.Configuration.WebConfigurationManager.AppSettings["ConsumerKey"] = "PxrPt87pjbvTxwFX31WjaSdDN";
            System.Web.Configuration.WebConfigurationManager.AppSettings["ConsumerSecret"] = "i4fSXklY9U4BKl2za1VKJ9oLUhaPfv8U9VsxnezFNRuuxfnCes";
            System.Web.Configuration.WebConfigurationManager.AppSettings["TwitterReadUrl"] = "https://api.twitter.com/1.1/statuses/user_timeline.json";

            TwitterReadController contentController = new TwitterReadController();
            string response = contentController.ReadResponse(2, "Salesforce");
            Assert.IsNotNull(response);
        }
    }
}
