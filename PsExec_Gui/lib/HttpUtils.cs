using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;// for check file exist
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace PsExec_Gui.lib
{
    class HttpUtils
    {
        public string HttpGet(string url)
        {
            try
            {
                WebRequest webRequest = WebRequest.Create(url);
                webRequest.Credentials = CredentialCache.DefaultCredentials;

                //allows for validation of SSL certificates 

                ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);
                WebResponse webResp = webRequest.GetResponse();


                Encoding myEncoding = Encoding.GetEncoding("UTF-8");

                string data;
                using (StreamReader myStreamReader = new StreamReader(webResp.GetResponseStream(), myEncoding))
                {

                    data = myStreamReader.ReadToEnd();
                }
            return data;
            }
            catch (WebException wex)
            {

                // Exception message and stack trace
                Console.WriteLine("错误: "+url+"----"+wex.Message);
                //Console.WriteLine(wex.StackTrace);

                // Inspect the reason of communication failure
                switch (wex.Status)
                {
                    case WebExceptionStatus.ConnectFailure:
                        // handle failure to connect to server
                        break;

                    case WebExceptionStatus.ReceiveFailure:
                        // handle failure to receive complete
                        // HTTP response from server
                        break;

                    case WebExceptionStatus.Timeout:
                        // handle timeout when waiting for
                        // HTTP response from server
                        break;

                    case WebExceptionStatus.ConnectionClosed:
                        // handle connection with server closed 
                        // prematurely
                        break;

                    // This is where we can examine HTTP status
                    // codes other than 2xx and the server response 
                    // body
                    case WebExceptionStatus.ProtocolError:
                        // Examine the HTTP response returned
                        HttpWebResponse response = (HttpWebResponse)wex.Response;
                        StreamReader responseReader =
                            new StreamReader(response.GetResponseStream());
                        string responseFromServer = responseReader.ReadToEnd();
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.BadRequest:
                                // handle bad request (http status code 400)
                                break;
                            case HttpStatusCode.InternalServerError:
                                // handle internal server error (http status code 500)
                                break;
                            default:
                                // handle other http status code returned by
                                // the server.
                                break;
                        } // end switch(res.StatusCode)
                        response.Close();
                        break;

                    case WebExceptionStatus.NameResolutionFailure:
                        // handle DNS error
                        break;

                    default:
                        // handle other errors not in this switch statement
                        break;
                } // end switch(wex.Status)
                return null;
            } // end try-catch
        }


        public string getResultByPost(string _url, string _postData)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(_url);
            httpWebRequest.Method = "POST"; // 设定METHOD

            httpWebRequest.ContentType = "application/x-www-form-urlencoded"; // 设定ContentType

            //httpWebRequest.Headers["Authorization"] = authHeader; // 设定Header

            //httpWebRequest.ProtocolVersion = HttpVersion.Version10; //设定用HTTPS

            byte[] byteArray = Encoding.UTF8.GetBytes(_postData);
            httpWebRequest.ContentLength = byteArray.Length;
            using (Stream postStream = httpWebRequest.GetRequestStream())
            {
                postStream.Write(byteArray, 0, byteArray.Length);
            }

            string responseFromServer;
            using (WebResponse response = httpWebRequest.GetResponse())
            {
                using (StreamReader responseReader = new StreamReader(response.GetResponseStream()))
                {
                    responseFromServer = responseReader.ReadToEnd();
                }
            }

            while (responseFromServer.Length == 0) ; //记得空迴圈等待

            return responseFromServer;
        }




        //for testing purpose only, accept any dodgy certificate... 
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
