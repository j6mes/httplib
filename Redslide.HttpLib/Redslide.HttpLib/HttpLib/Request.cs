using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Redslide.HttpLib
{
    /*
     * A deligate void to be called if there's a network issue
     */
    public delegate void ConnectionIssue(WebException ex);


    /// <summary>
    /// Request is a static class that holds the http methods
    /// </summary>
    public static class Request
    {
        

        /*
         *  Events
         */
        public static event ConnectionIssue ConnectFailed = delegate { };



        /*
         * Cookie Container
         */
        private static CookieContainer cookies = new CookieContainer();
                

        /*
         * Methods
         */
        #region Methods

        #region GET
        /// <summary>
        /// Performs a HTTP get operation
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="successCallback">A function that is called on success</param>
        public static void Get(string url, Action<string> successCallback)
        {
            Get(url, new { }, StreamToStringCallback(successCallback));
        }

        /// <summary>
        /// Performs a HTTP get operation with parameters
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">KVP Array of parameters</param>
        /// <param name="successCallback">A function that is called on success</param>
        public static void Get(string url, object parameters, Action<String> successCallback)
        {
            Get(url, parameters, StreamToStringCallback(successCallback), (webEx) =>
            {
                ConnectFailed(webEx);
            });
        }

        /// <summary>
        /// Performs a HTTP get operation with parameters and a function that is called on failure
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">KVP Array of parameters</param>
        /// <param name="successCallback">Function that is called on success</param>
        /// <param name="failCallback">Function that is called on failure</param>
        public static void Get(string url, object parameters, Action<string> successCallback, Action<WebException> failCallback)
        {
            Get(url, parameters, StreamToStringCallback(successCallback), failCallback);
        }

        /// <summary>
        /// Performs a HTTP get request
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="successCallback">A function that is called on success</param>
        public static void Get(string url, Action<WebHeaderCollection, Stream> successCallback)
        {
            Get(url, new {}, successCallback);
        }

        /// <summary>
        /// Performs a HTTP get request with parameters
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">KVP Array of parameters</param>
        /// <param name="successCallback">A function that is called on success</param>
        public static void Get(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback)
        {
            Get(url, parameters, successCallback, (webEx) =>
                {
                    ConnectFailed(webEx);
                });
        }
         
        /// <summary>
        /// Performs a HTTP get request with parameters and a function that is called on failure
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">KVP Array of parameters</param>
        /// <param name="successCallback">Function that is called on success</param>
        /// <param name="failCallback">Function that is called on failure</param>
        public static void Get(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback, Action<WebException> failCallback)
        {
            UriBuilder b = new UriBuilder(url);
            
            /*
             * Append Paramters to the url
             */
            if (parameters != null)
            {
                if (!string.IsNullOrWhiteSpace(b.Query))
                {
                    b.Query = b.Query.Substring(1) + "&" + Utils.SerializeQueryString(parameters);
                }
                else
                {
                    b.Query =Utils.SerializeQueryString(parameters);
                }
                
            }


            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Get, b.Uri.ToString(), null, new { }, successCallback, failCallback);
        }
        #endregion

        #region HEAD
        /// <summary>
        /// Performs a HTTP head operation
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="successCallback">A function that is called on success</param>
        public static void Head(string url, Action<string> successCallback)
        {
            Head(url, new { }, StreamToStringCallback(successCallback));
        }

        /// <summary>
        /// Performs a HTTP head operation with parameters
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">KVP Array of parameters</param>
        /// <param name="successCallback">A function that is called on success</param>
        public static void Head(string url, object parameters, Action<String> successCallback)
        {
            Head(url, parameters, StreamToStringCallback(successCallback), (webEx) =>
            {
                ConnectFailed(webEx);
            });
        }

        /// <summary>
        /// Performs a HTTP head operation with parameters and a function that is called on failure
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">KVP Array of parameters</param>
        /// <param name="successCallback">Function that is called on success</param>
        /// <param name="failCallback">Function that is called on failure</param>
        public static void Head(string url, object parameters, Action<string> successCallback, Action<WebException> failCallback)
        {
            Head(url, parameters, StreamToStringCallback(successCallback), failCallback);
        }

        /// <summary>
        /// Performs a HTTP head operation
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="successCallback">A function that is called on success</param>
        public static void Head(string url, Action<WebHeaderCollection, Stream> successCallback)
        {
            Head(url, new { }, successCallback);
        }

        /// <summary>
        /// Performs a HTTP head operation with parameters
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">KVP Array of parameters</param>
        /// <param name="successCallback">A function that is called on success</param>
        public static void Head(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback)
        {
            Head(url, parameters, successCallback, (webEx) =>
            {
                ConnectFailed(webEx);
            });
        }

        /// <summary>
        /// Performs a HTTP head operation with parameters and a function that is called on failure
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">KVP Array of parameters</param>
        /// <param name="successCallback">Function that is called on success</param>
        /// <param name="failCallback">Function that is called on failure</param>
        public static void Head(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback, Action<WebException> failCallback)
        {
            UriBuilder b = new UriBuilder(url);

            /*
             * Append Paramters to the url
             */


            #if NETFX_CORE
            if (parameters.GetType().GetTypeInfo().DeclaredProperties.ToArray().Length > 0)
            #else
            if (parameters.GetType().GetProperties().Length > 0)
            #endif
            {
                if (!string.IsNullOrWhiteSpace(b.Query))
                {
                    b.Query = b.Query.Substring(1) + "&" + Utils.SerializeQueryString(parameters);
                }
                else
                {
                    b.Query = Utils.SerializeQueryString(parameters);
                }

            }


            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Head, b.Uri.ToString(), null, new { }, successCallback, failCallback);
        }
        #endregion

        #region POST
        /// <summary>
        /// Performs a HTTP post request on a target with parameters
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">Array of parameters</param>
        /// <param name="successCallback">Function that is called on success</param>
        public static void Post(string url, object parameters, Action<string> successCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Post, url, null, parameters, StreamToStringCallback(successCallback), (webEx) =>
                {
                    ConnectFailed(webEx);

                });
        }

        /// <summary>
        /// Performs a HTTP post request on a target with parameters
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">Array of parameters</param>
        /// <param name="successCallback">Function that is called on success</param>
        public static void Post(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Post, url, null, parameters, successCallback, (webEx) =>
            {
                ConnectFailed(webEx);

            });
        }


        /// <summary>
        /// Performs a HTTP post request with parameters and a fail function
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">Array of parameters</param>
        /// <param name="successCallback">Function that is called on success</param>
        /// <param name="failCallback">Function that is called on failure</param>
        public static void Post(string url, object parameters, Action<string> successCallback, Action<WebException> failCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Post, url, null, parameters,StreamToStringCallback(successCallback), failCallback);
        }

        /// <summary>
        /// Performs a HTTP post request with parameters and a fail function
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">Array of parameters</param>
        /// <param name="successCallback">Function that is called on success</param>
        /// <param name="failCallback">Function that is called on failure</param>
        public static void Post(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback, Action<WebException> failCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Post, url, null, parameters, successCallback, failCallback);
        }

        /// <summary>
        /// Performs a HTTP post request with parameters and a fail function
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">Array of parameters</param>
        /// <param name="successCallback">Function that is called on success</param>
        /// <param name="failCallback">Function that is called on failure</param>
        public static void Post(string url, IDictionary<String,String> headers,object parameters, Action<WebHeaderCollection, Stream> successCallback, Action<WebException> failCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Post, url, headers, parameters, successCallback, failCallback);
        }
        #endregion

        #region PATCH
        /// <summary>
        /// Performs a HTTP patch request on a target with parameters
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">Array of parameters</param>
        /// <param name="successCallback">Function that is called on success</param>
        public static void Patch(string url, object parameters, Action<string> successCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Patch, url, null, parameters, StreamToStringCallback(successCallback), (webEx) =>
            {
                ConnectFailed(webEx);

            });
        }

        /// <summary>
        /// Performs a HTTP patch request on a target with parameters
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">Array of parameters</param>
        /// <param name="successCallback">Function that is called on success</param>
        public static void Patch(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Patch, url, null, parameters, successCallback, (webEx) =>
            {
                ConnectFailed(webEx);

            });
        }

        /// <summary>
        /// Performs a HTTP patch request with parameters and a fail function
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">Array of parameters</param>
        /// <param name="successCallback">Function that is called on success</param>
        /// <param name="failCallback">Function that is called on failure</param>
        public static void Patch(string url, object parameters, Action<string> successCallback, Action<WebException> failCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Patch, url, null, parameters, StreamToStringCallback(successCallback), failCallback);
        }

        /// <summary>
        /// Performs a HTTP patch request with parameters and a fail function
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">Array of parameters</param>
        /// <param name="successCallback">Function that is called on success</param>
        /// <param name="failCallback">Function that is called on failure</param>
        public static void Patch(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback, Action<WebException> failCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Patch, url, null, parameters, successCallback, failCallback);
        }
        #endregion

        #region PUT
        /// <summary>
        /// Performs a HTTP put request on a target with parameters
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">Array of parameters</param>
        /// <param name="successCallback">Function that is called on success</param>
        public static void Put(string url, object parameters, Action<string> successCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Put, url, null, parameters, StreamToStringCallback(successCallback), (webEx) =>
            {
                ConnectFailed(webEx);

            });
        }

        /// <summary>
        /// Performs a HTTP put request on a target with parameters
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">Array of parameters</param>
        /// <param name="successCallback">Function that is called on success</param>
        public static void Put(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Put, url, null, parameters, successCallback, (webEx) =>
            {
                ConnectFailed(webEx);

            });
        }

        /// <summary>
        /// Performs a HTTP put request with parameters and a fail function
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">Array of parameters</param>
        /// <param name="successCallback">Function that is called on success</param>
        /// <param name="failCallback">Function that is called on failure</param>
        public static void Put(string url, object parameters, Action<string> successCallback, Action<WebException> failCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Put, url, null, parameters, StreamToStringCallback(successCallback), failCallback);
        }

        /// <summary>
        /// Performs a HTTP put request with parameters and a fail function
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">Array of parameters</param>
        /// <param name="successCallback">Function that is called on success</param>
        /// <param name="failCallback">Function that is called on failure</param>
        public static void Put(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback, Action<WebException> failCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Put, url, null, parameters, successCallback, failCallback);
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Performs a HTTP delete request with parameters and a fail function
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">Array of parameters</param>
        /// <param name="successCallback">Function that is called on success</param>
        /// <param name="failCallback">Function that is called on failure</param>
        public static void Delete(string url,object parameters, Action<string> successCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Delete, url, null, parameters, StreamToStringCallback(successCallback), (webEx) =>
            {
                ConnectFailed(webEx);

            });
        }

        /// <summary>
        /// Performs a HTTP delete request with parameters and a fail function
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">Array of parameters</param>
        /// <param name="successCallback">Function that is called on success</param>
        /// <param name="failCallback">Function that is called on failure</param>
        public static void Delete(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Delete, url, null, parameters, successCallback, (webEx) =>
            {
                ConnectFailed(webEx);

            });
        }

        /// <summary>
        /// Performs a HTTP delete request with parameters and a fail function
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">Array of parameters</param>
        /// <param name="successCallback">Function that is called on success</param>
        /// <param name="failCallback">Function that is called on failure</param>
        public static void Delete(string url, object parameters, Action<string> successCallback, Action<WebException> failCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Delete, url, null, parameters, StreamToStringCallback(successCallback), failCallback);
        }

        /// <summary>
        /// Performs a HTTP delete request with parameters and a fail function
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">Array of parameters</param>
        /// <param name="successCallback">Function that is called on success</param>
        /// <param name="failCallback">Function that is called on failure</param>
        public static void Delete(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback, Action<WebException> failCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Delete, url, null, parameters, successCallback, failCallback);
        }
        #endregion

        #region UPLOAD
        /// <summary>
        /// Upload an array of files to a remote host using the HTTP post multipart method
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">Parmaters</param>
        /// <param name="files">An array of files</param>
        /// <param name="successCallback">funciton that is called on success</param>
        public static void Upload(string url, object parameters, NamedFileStream[] files, Action<string> successCallback)
        {
            Upload(url, parameters, files, successCallback, (webEx) =>
                {
                    ConnectFailed(webEx);
                });
        }

        /// <summary>
        /// Upload an array of files to a remote host using the HTTP post multipart method
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">Parmaters</param>
        /// <param name="files">An array of files</param>
        /// <param name="successCallback">funciton that is called on success</param>
        public static void Upload(string url, object parameters, NamedFileStream[] files, Action<WebHeaderCollection, Stream> successCallback)
        {
            Upload(url, parameters, files, successCallback, (webEx) =>
            {
                ConnectFailed(webEx);
            });
        }

        /// <summary>
        /// Upload an array of files to a remote host using the HTTP post multipart method
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">Parmaters</param>
        /// <param name="files">An array of files</param>
        /// <param name="successCallback">Funciton that is called on success</param>
        /// <param name="failCallback">Function that is called on failure</param>
        public static void Upload(string url, object parameters, NamedFileStream[] files, Action<string> successCallback, Action<WebException> failCallback)
        {
            Upload(url, HttpVerb.Post, parameters, files, successCallback, failCallback);
        }

        /// <summary>
        /// Upload an array of files to a remote host using the HTTP post multipart method
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="parameters">Parmaters</param>
        /// <param name="files">An array of files</param>
        /// <param name="successCallback">Funciton that is called on success</param>
        /// <param name="failCallback">Function that is called on failure</param>
        public static void Upload(string url, object parameters, NamedFileStream[] files, Action<WebHeaderCollection, Stream> successCallback, Action<WebException> failCallback)
        {
            Upload(url, HttpVerb.Post, parameters, files, successCallback, failCallback);
        }

        /// <summary>
        /// Upload an array of files to a remote host using the HTTP post or put multipart method
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="method">Request Method - POST or PUT</param>
        /// <param name="parameters">Parmaters</param>
        /// <param name="files">An array of files</param>
        /// <param name="successCallback">Funciton that is called on success</param>
        /// <param name="failCallback">Function that is called on failure</param>
        public static void Upload(string url, HttpVerb method, object parameters, NamedFileStream[] files, Action<string> successCallback, Action<WebException> failCallback)
        {
            Upload(url, method, parameters, files, StreamToStringCallback(successCallback), failCallback);
        }

        /// <summary>
        /// Upload an array of files to a remote host using the HTTP post or put multipart method
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="method">Request Method - POST or PUT</param>
        /// <param name="parameters">Parmaters</param>
        /// <param name="files">An array of files</param>
        /// <param name="successCallback">Funciton that is called on success</param>
        /// <param name="failCallback">Function that is called on failure</param>
        public static void Upload(string url, HttpVerb method, object parameters, NamedFileStream[] files, Action<WebHeaderCollection, Stream> successCallback,Action<WebException> failCallback)
        {
            if (method != HttpVerb.Post && method != HttpVerb.Put)
            {
                throw new ArgumentException("Request method must be POST or PUT");
            }

            try
            {
                /*
                 * Generate a random boundry string
                 */
                string boundary = RandomString(12);

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.Method = method.ToString();
                request.ContentType = "multipart/form-data, boundary=" + boundary;
                request.CookieContainer = cookies;
     
                request.BeginGetRequestStream(new AsyncCallback((IAsyncResult asynchronousResult) =>
                {
                    /*
                     * Create a new request
                     */
                    HttpWebRequest tmprequest = (HttpWebRequest)asynchronousResult.AsyncState;

                    /*
                     * Get a stream that we can write to
                     */
                    Stream postStream = tmprequest.EndGetRequestStream(asynchronousResult);
                    string querystring = "\n";

                    /*
                     * Serialize parameters in multipart manner
                     */
                    #if NETFX_CORE
                    foreach (var property in parameters.GetType().GetTypeInfo().DeclaredProperties)
                    #else
                    foreach (var property in parameters.GetType().GetProperties())                    
                    #endif
                    {
                        querystring += "--" + boundary + "\n";
                        querystring += "content-disposition: form-data; name=\"" + System.Uri.EscapeDataString(property.Name) + "\"\n\n";
                        querystring += System.Uri.EscapeDataString(property.GetValue(parameters, null).ToString());
                        querystring += "\n";
                    }

                
                    /*
                     * Then write query string to the postStream
                     */
                    byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(querystring);
                    postStream.Write(byteArray, 0, byteArray.Length);
                
                    /*
                     * A boundary string that we'll reuse to separate files
                     */ 
                    byte[] closing = System.Text.Encoding.UTF8.GetBytes("\n--" + boundary + "--\n");


                    /*
                     * Write each files to the postStream
                     */
                    foreach (NamedFileStream file in files)
                    {
                        /*
                         * A temporary buffer to hold the file stream
                         * Not sure if this is needed ???
                         */
                        Stream outBuffer = new MemoryStream();

                        /*
                         * Additional info that is prepended to the file
                         */
                        string qsAppend;
                        qsAppend = "--" + boundary + "\ncontent-disposition: form-data; name=\""+file.Name +"\"; filename=\"" + file.Filename + "\"\r\nContent-Type: " + file.ContentType + "\r\n\r\n";
       
                        /*
                         * Read the file into the output buffer
                         */
                        StreamReader sr = new StreamReader(file.Stream);
                        outBuffer.Write(System.Text.Encoding.UTF8.GetBytes(qsAppend), 0, qsAppend.Length);

                        int bytesRead = 0;
                        byte[] buffer = new byte[4096];

                        while ((bytesRead = file.Stream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            outBuffer.Write(buffer, 0, bytesRead);

                        }


                        /*
                         * Write the delimiter to the output buffer
                         */
                        outBuffer.Write(closing, 0, closing.Length);



                        /*
                         * Write the output buffer to the post stream using an intemediate byteArray
                         */
                        outBuffer.Position = 0;
                        byte[] tempBuffer = new byte[outBuffer.Length];
                        outBuffer.Read(tempBuffer, 0, tempBuffer.Length);
                        postStream.Write(tempBuffer, 0, tempBuffer.Length);
                        postStream.Flush();           
                    }

                
                    postStream.Flush();
                    postStream.Dispose();

                    tmprequest.BeginGetResponse(ProcessCallback(successCallback, failCallback), tmprequest);

                }), request);
            }
            catch (WebException webEx)
            {
                failCallback(webEx);
            }

        }
        #endregion

        #region Private Methods

        private static Action<WebHeaderCollection, Stream> StreamToStringCallback(Action<string> stringCallback)
        {
            return (WebHeaderCollection headers ,Stream resultStream) =>
            {
                using (StreamReader sr = new StreamReader(resultStream))
                {
                    stringCallback(sr.ReadToEnd());
                }
            };
        }


        private static void MakeRequest(string contentType, HttpVerb method, string url, IDictionary<String,String> headers, object parameters, Action<WebHeaderCollection, Stream> successCallback, Action<WebException> failCallback)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters object cannot be null");
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("url is empty");
            }
            

            try
            {
                /*
                 * Create new Request
                 */
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.CookieContainer = cookies;
                request.Method = method.ToString();


                if (headers != null)
                {
                   
                    
                    foreach (var key in headers.Keys)

                    {
#if NETFX_CORE

#else
                        request.Headers.Add("");
#endif
                    }

                }
                
                

                /*
                 * Asynchronously get the response
                 */
                if (method == HttpVerb.Delete || method == HttpVerb.Post || method == HttpVerb.Put || method == HttpVerb.Patch)
                {
                    request.ContentType = contentType;
                    request.BeginGetRequestStream(new AsyncCallback((IAsyncResult callbackResult) =>
                    {                 
                        HttpWebRequest tmprequest = (HttpWebRequest)callbackResult.AsyncState;
                        Stream postStream;

                        postStream = tmprequest.EndGetRequestStream(callbackResult);


                        string postbody = "";

                        
                        postbody = Utils.SerializeQueryString(parameters);
                        

                        // Convert the string into a byte array. 
                        byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(postbody);

                        // Write to the request stream.
                        postStream.Write(byteArray, 0, byteArray.Length);
                        postStream.Flush();
                        postStream.Dispose();

                        // Start the asynchronous operation to get the response
                        tmprequest.BeginGetResponse(ProcessCallback(successCallback,failCallback), tmprequest);


                    }), request);
                }
                else if (method == HttpVerb.Get || method == HttpVerb.Head)
                {
                    request.BeginGetResponse(ProcessCallback(successCallback,failCallback), request);
                }
            }
            catch (WebException webEx)
            {
                failCallback(webEx);
            }
        }

        /*
         * Muhammad Akhtar @StackOverflow
         */
        private static string RandomString(int length)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            char[] chars = new char[length];
            Random rd = new Random();

            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }



        private static AsyncCallback ProcessCallback(Action<WebHeaderCollection, Stream> success, Action<WebException> fail)
        {
            return new AsyncCallback((callbackResult) =>
                {
                    HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;

                    try
                    {
                        using (HttpWebResponse myResponse = (HttpWebResponse)myRequest.EndGetResponse(callbackResult))
                        {

                                
                            success(myResponse.Headers, myResponse.GetResponseStream());
                           
                        }

                    }
                    catch (WebException webEx)
                    {
                        if (ConnectFailed != null)
                        {
                            fail(webEx);

                        }

                    }
                });
        }
        #endregion




        #endregion
    }


}
