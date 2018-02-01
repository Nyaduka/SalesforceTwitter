using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalesforceTweets.Controllers
{
    public class HomeController : TwitterReadController
    {
        public ActionResult Index()
        {
          return View();
        }

        /// <summary>
        /// Method to get the details from twitter
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTweetsDetails(int count, string name)
        {
            return Json(ReadTweets(count, name), JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            //ViewBag.Message = "Description";

            return View();
        }

        #region Private Method

        /// <summary>
        /// Read the tweets from the twitter api
        /// </summary>
        /// <param name="count">Number of tweets needed</param>
        /// <param name="name">Name of the account</param>
        /// <returns>Return the tweets</returns>
        private string ReadTweets(int count, string name)
        {
            string strResponse = string.Empty; // Variable to store the response and will be returned back to the caller
            // If no value is passed then return nothing
            if (count <= 0 || string.IsNullOrWhiteSpace(name))
            {
                return strResponse;
            }

            strResponse = base.ReadResponse(count, name);

            // Return the response back to caller
            return strResponse;
        }

        #endregion
    }
}
