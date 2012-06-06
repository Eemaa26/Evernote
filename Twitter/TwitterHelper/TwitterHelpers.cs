using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Evernote.TwitterHelper.Server
{
    public class TwitterHelpers
    {

        private string _twitterJsonUrl = "https://twitter.com/statuses/update.json"; 
        public string TwitterJsonUrl
        { 
            get { return _twitterJsonUrl; }
            set { _twitterJsonUrl = value; }
        } 
        
        private string _twitterUser = string.Empty; 
        public string TwitterUser
        {
            get { return _twitterUser; }
            set { _twitterUser = value; }
        } 
        
        private string _twitterPass = string.Empty; 
        public string TwitterPass
        {
            get { return _twitterPass; }
            set { _twitterPass = value; }
        } 
        
        private string _proxyServer = string.Empty; 
        public string ProxyServer
        {
            get { return _proxyServer; }
            set { _proxyServer = value; }
        } 
        
        public string SendTwitterMessage(string message)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(TwitterJsonUrl);
                System.Net.ServicePointManager.Expect100Continue = false; 
                string post = string.Empty;
                
                using (TextWriter writer = new StringWriter())
                {
                    if (message.Length > 140)
                    {
                        message = message.Substring(0, 140);
                    }
                    writer.Write("status={0}", System.Web.HttpUtility.UrlEncode(message)); 
                    post = writer.ToString();
                } 
                
                SetRequestParams(request); 
                request.Credentials = new NetworkCredential(TwitterUser, TwitterPass); 
                
                using (Stream requestStream = request.GetRequestStream())
                {
                    using (StreamWriter writer = new StreamWriter(requestStream))
                    {
                        writer.Write(post);
                    }
                } 
                
                WebResponse response = request.GetResponse();
                string content; 
                
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        content = reader.ReadToEnd();
                    }
                } 
                return content; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        
        private void SetRequestParams(HttpWebRequest request)
        {
            request.Timeout = 500000;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "EvernoteTest"; 
            
            if (!string.IsNullOrEmpty(_proxyServer))
            {
                request.Proxy = new WebProxy(_proxyServer, false);
            } 
        } 
        
        public TwitterHelpers(string userName, string userPassword, string proxyServer)
        {
            _twitterUser = userName;
            _twitterPass = userPassword;
            _proxyServer = proxyServer;
        } 
        
        public TwitterHelpers(string userName, string userPassword)
        {
            _twitterUser = userName;
            _twitterPass = userPassword;
        }
    }
}
