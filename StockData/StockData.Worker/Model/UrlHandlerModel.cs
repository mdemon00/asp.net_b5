using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StockData.Worker.Model
{
    public static class UrlHandlerModel
    {
        public static HtmlDocument GetUrlResponse(string url)
        {
            var urlResponse = "";
            HtmlDocument htmlDoc = null;

            try 
            {
                urlResponse = URLRequest(url);

                htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(urlResponse);
            }
            catch { } //Error not handled intentionally

            return htmlDoc;
        }
        
        private static string URLRequest(string url)
        {
            //This is General Function to request data from a Server

            // Prepare the Request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            // Set method to GET to retrieve data
            request.Method = "GET";
            request.Timeout = 6000; //60 second timeout
            request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)";

            string responseContent = null;

            // Get the Response
            using (WebResponse response = request.GetResponse())
            {
                // Retrieve a handle to the Stream
                using (Stream stream = response.GetResponseStream())
                {
                    // Begin reading the Stream
                    using (StreamReader streamreader = new StreamReader(stream))
                    {
                        // Read the Response Stream to the end
                        responseContent = streamreader.ReadToEnd();
                    }
                }
            }

            return (responseContent);
        }
    }
}
