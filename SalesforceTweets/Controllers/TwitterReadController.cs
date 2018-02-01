using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;

namespace SalesforceTweets.Controllers
{
    
    public class TwitterReadController : Controller
    {
        /// <summary>
        /// Static variable to store the access token so we shouldn't make the call with every refresh
        /// </summary>
        public static string _accesstoken = null;

        /// <summary>
        /// Method used for authentication.
        /// </summary>
        /// <returns></returns>
        public string Authentication()
        {
            /* Access token is needed for a valid (OAuth) twitter authentication, and
                #1. We can either pass token directly OR
                #2. Get them programmatically while application runs (best practice)
             * we will be using the 2nd approach here
            */

            // If the access token is yet to get 
            // (Out of scope handling: If the token gets expired when can handle within a login screen or make a direct call )
            if (string.IsNullOrEmpty(_accesstoken))
            {
                // Get the authentication url, fom web config
                var AuthenticationUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["AuthenticationUrl"];
                if (string.IsNullOrWhiteSpace(AuthenticationUrl))
                {
                    throw new Exception("No URL provided for authentication.");
                }

                // Get the credentials from web.config. The Best practice is to store in database in encrypted form.
                var consumerKey = System.Web.Configuration.WebConfigurationManager.AppSettings["ConsumerKey"];
                var consumerSecret = System.Web.Configuration.WebConfigurationManager.AppSettings["ConsumerSecret"];
                if (string.IsNullOrWhiteSpace(consumerKey) || string.IsNullOrWhiteSpace(consumerSecret))
                {
                    throw new Exception("No credential provided for authentication.");
                }

                string strCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(consumerKey + ":" + consumerSecret)); // Base64 encoded string

                // Create the post request to get the access token.            
                var postrequest = GetPostRequestInstance(AuthenticationUrl, strCredentials, "grant_type=client_credentials");

                try
                {
                    // Get the response stream.
                    using (var responseStream = postrequest.GetResponse().GetResponseStream())
                    {
                        // Read the response stream.
                        using (var responseReader = new StreamReader(responseStream))
                        {
                            _accesstoken = responseReader.ReadToEnd();
                        }
                    }
                    // Parse the response stream.
                    _accesstoken = ParseResponseString(_accesstoken);
                }
                catch
                {
                    throw new Exception("Error while getting the access token");
                }
            }

            // Return the token to the caller
            return _accesstoken;
        }        

        /// <summary>
        /// Creates a post request instance which will be used to get the response stream
        /// </summary>
        /// <param name="url">Url to invoke</param>
        /// <param name="authHeader">Credentials need for authentication</param>
        /// <param name="requestbodyType">Body type of the request to create</param>
        /// <returns></returns>
        public HttpWebRequest GetPostRequestInstance(string url, string authHeader, string requestbodyType)
        {
            var postrequest = WebRequest.Create(url) as HttpWebRequest;
            postrequest.Method = "POST";
            postrequest.ContentType = "application/x-www-form-urlencoded";
            postrequest.Headers[HttpRequestHeader.Authorization] = "Basic " + authHeader;
            var requestBody = Encoding.UTF8.GetBytes(requestbodyType);
            postrequest.ContentLength = requestBody.Length;

            // Create and fill the request stream
            using (var requestStream = postrequest.GetRequestStream())
            {
                requestStream.Write(requestBody, 0, requestBody.Length);
            }

            return postrequest;
        }

        /// <summary>
        /// Parses the response string
        /// </summary>
        /// <param name="response">String to be parsed</param>
        /// <returns></returns>
        private string ParseResponseString(string response)
        {
            return response.Substring(response.IndexOf("access_token\":\"") + "access_token\":\"".Length, response.IndexOf("\"}")
                                        - (response.IndexOf("access_token\":\"") + "access_token\":\"".Length));
        }


        /// <summary>
        /// Method to read the tweets for an account.
        /// </summary>
        /// <param name="numberofRecords">Number of records to read</param>
        /// <param name="screenname">Account whose tweets need to be read</param>
        /// <returns>Tweet content details</returns>
        public string ReadResponse(int numberofRecords, string screenname)
        {
            /* 
              This method will perform the 2 steps here:
             1. Autheticate the request and get access token.
             2. Use the access token and GET request, read the twitter contents 
            */

            string strResponse = null; // Variable to store the tweets.
            // Make an authentication call and get the access token
            string strAccessToken = Authentication();

            if (!string.IsNullOrWhiteSpace(strAccessToken))
            {
                // Region to get the tweets per the passed screen name           
                var TwitterReadUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["TwitterReadUrl"]; // Get the url required to get the tweets
                var gettweets = WebRequest.Create(TwitterReadUrl + "?count=" + numberofRecords + "&screen_name=" + screenname) as HttpWebRequest;
                gettweets.Method = "GET";
                gettweets.Headers[HttpRequestHeader.Authorization] = "Bearer " + strAccessToken;
                try
                {
                    //  "using" disposes the object regardless of an error
                    using (var responseStream = gettweets.GetResponse().GetResponseStream())
                    {
                        // Read the stream to a string object
                        using (var responseReader = new StreamReader(responseStream))
                        {
                            strResponse = responseReader.ReadToEnd();
                        }
                    }
                }
                catch (Exception exc)
                {
                    throw exc;
                }
            }

            // Return the response to the caller.
            return strResponse;
        }


       
    }
}
