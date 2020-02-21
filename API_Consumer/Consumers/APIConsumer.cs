using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using API_Consumer;
using System.Text;
using System.Web;

namespace SQLAPI_Consumer
{
    /// <summary>
    /// This class will be used for consume API using the URL provided.
    /// Actually only GET & POST Method are supported.
    /// </summary>
    public static class APIConsumer
    {
        /// <summary>
        /// Fixed Context type supported.
        /// </summary>
        private const string CONTENTTYPE = "application/json; charset=utf-8";
        private const string CONTENTTYPE_XXX_FORM_URL_ENCODED = "application/x-www-form-urlencoded";

        /// <summary>
        /// Fixed string for POST method
        /// </summary>
        private const string POST_WebMethod = "POST";

        /// <summary>
        /// Fixed string for GET method
        /// </summary>
        private const string GET_WebMethod = "GET";

        /// <summary>
        /// POST to Resful API sending Json body.
        /// </summary>
        /// <param name="url">API URL</param>
        /// <param name="JsonBody">Content Application By Default Json</param>
        /// <param name="Authorization">Header Authorization token, user-passwrod, JWT, etc.</param>
        /// <returns>String Api result</returns>
        public static string POSTMethod(string url, string JsonBody, string Authorization = "")
        {
            string ContentResult = string.Empty;
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                       | SecurityProtocolType.Ssl3;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = CONTENTTYPE;
                request.Method = POST_WebMethod;

                if (!String.IsNullOrEmpty(Authorization))
                    request.Headers.Add(HttpRequestHeader.Authorization, Authorization);

                if (!String.IsNullOrEmpty(JsonBody))
                {
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(JsonBody);
                        streamWriter.Flush();
                    }
                }

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    ContentResult = result;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    var result = reader.ReadToEnd();
                    ContentResult = result;
                }
            }
            catch (Exception ex)
            {
                ContentResult = ex.Message.ToString();
                throw ex;
            }

            return ContentResult;
        }

        /// <summary>
        /// POST to Resful API sending Json body.
        /// </summary>
        /// <param name="url">API URL</param>
        /// <param name="JsonBody">Content Application By Default Json</param>
        /// <returns>String Api result</returns>
        public static string POSTMethod(string url, string JsonBody)
        {
            string ContentResult = string.Empty ;
            try
            {
                SetSSL();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = CONTENTTYPE;
                request.Method = POST_WebMethod;

                if (!String.IsNullOrEmpty(JsonBody))
                {
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(JsonBody);
                        streamWriter.Flush();
                    }
                }

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader?.ReadToEnd();
                    ContentResult = result;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response?.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var result = reader.ReadToEnd();
                            ContentResult = result;
                        }
                    }
                    else
                    {
                        ContentResult = ex.Message.ToString();
                    }
                    
                }
                
            }
            catch (Exception ex)
            {
                ContentResult = ex.Message.ToString();
                throw ex;
            }

            return ContentResult;
        }

        /// <summary>
        /// POST to Resful API sending Json body.
        /// </summary>
        /// <param name="url">API URL</param>
        /// <param name="JsonBody">Content Application By Default Json</param>
        /// <returns>String Api result</returns>
        public static string POSTMethod_UrlEncoded(string url, string scopeurl, string client_id, string client_secret)
        {
            string ContentResult = string.Empty;
            try
            {
                SetSSL();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //request.ContentType = CONTENTTYPE_XXX_FORM_URL_ENCODED;
                request.Method = POST_WebMethod;
                request.ContentType = CONTENTTYPE_XXX_FORM_URL_ENCODED;
                //request.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                StringBuilder postData = new StringBuilder();
                postData.AppendUrlEncoded("grant_type", "client_credentials");
                postData.AppendUrlEncoded("scope", scopeurl);
                postData.AppendUrlEncoded("client_id", client_id);
                postData.AppendUrlEncoded("client_secret", client_secret);
                /*
                 * 
                if (!String.IsNullOrEmpty(postData))
                {*/
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(postData);
                        streamWriter.Flush();
                    }
                //}

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader?.ReadToEnd();
                    ContentResult = result;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response?.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var result = reader.ReadToEnd();
                            ContentResult = result;
                        }
                    }
                    else
                    {
                        ContentResult = ex.Message.ToString();
                    }

                }

            }
            catch (Exception ex)
            {
                ContentResult = ex.Message.ToString();
                throw ex;
            }

            return ContentResult;
        }

        public static void AppendUrlEncoded(this StringBuilder sb, string name, string value)
        {
            if (sb.Length != 0)
                sb.Append("&");
            sb.Append(HttpUtility.UrlEncode(name));
            sb.Append("=");
            sb.Append(HttpUtility.UrlEncode(value));
        }

        /// <summary>
        /// POST to Resful API sending Json body.
        /// </summary>
        /// <param name="url">API URL</param>
        /// <param name="JsonBody">Content Application By Default Json</param>
        /// <param name="JsonHeaders">Headers added in Json format: Authorization token, user-passwrod, JWT, etc.</param>
        /// <returns>String Api result</returns>
        public static string POSTMethod_Header(string url, string JsonBody = "", string JsonHeaders = "")
        {
            string ContentResult = string.Empty;
            try
            {
                SetSSL();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = CONTENTTYPE;
                request.Method = POST_WebMethod;

                if (!string.IsNullOrEmpty(JsonHeaders))
                {
                    List<Headers> _headers = JsonConvert.DeserializeObject<List<Headers>>(JsonHeaders);

                    foreach (var Header in _headers)
                    {
                        if (!string.IsNullOrEmpty(Header.Name) && !string.IsNullOrEmpty(Header.Value))
                            request.Headers.Add(Header.Name, Header.Value);
                    }
                }

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    if(!String.IsNullOrEmpty(JsonBody))
                        streamWriter.Write(JsonBody);

                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    ContentResult = result;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response?.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var result = reader.ReadToEnd();
                            ContentResult = result;
                        }
                    }
                    else
                    {
                        ContentResult = ex.Message.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ContentResult = ex.Message.ToString();
                throw ex;
            }

            return ContentResult;
        }

        /// <summary>
        /// Request GET Method to the URL API provided.
        /// </summary>
        /// <param name="url">API URL</param>
        /// <param name="Authorization">Header Authorization</param>
        /// <returns>String Api result</returns>
        public static string GETMethod(string url, string Authorization = "")
        {
            string ContentResult = string.Empty;
            try
            {
                SetSSL();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = CONTENTTYPE;
                request.Method = GET_WebMethod;

                if (!String.IsNullOrEmpty(Authorization))
                    request.Headers.Add(HttpRequestHeader.Authorization, Authorization);

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    ContentResult = result;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response?.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var result = reader.ReadToEnd();
                            ContentResult = result;
                        }
                    }
                    else
                    {
                        ContentResult = ex.Message.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ContentResult = ex.Message.ToString();
                throw ex;
            }

            return ContentResult;
        }

        /// <summary>
        /// Request GET Method to the URL API provided.
        /// </summary>
        /// <param name="url">API URL</param>
        /// <param name="Authorization">Header Authorization</param>
        /// <returns>String Api result</returns>
        public static string GETMethod_Headers(string url, string Headers)
        {
            string ContentResult = string.Empty;
            try
            {
                SetSSL();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = CONTENTTYPE;
                request.Method = GET_WebMethod;

                if (!string.IsNullOrEmpty(Headers))
                {
                    List<Headers> _headers = JsonConvert.DeserializeObject<List<Headers>>(Headers);

                    foreach (var Header in _headers)
                    {
                        if (!string.IsNullOrEmpty(Header.Name) && !string.IsNullOrEmpty(Header.Value))
                            request.Headers.Add(Header.Name, Header.Value);
                    }
                }

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    ContentResult = result;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response?.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var result = reader.ReadToEnd();
                            ContentResult = result;
                        }
                    }
                    else
                    {
                        ContentResult = ex.Message.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ContentResult = ex.Message.ToString();
                throw ex;
            }

            return ContentResult;
        }

        /// <summary>
        /// Request GET Method to the URL API provided.
        /// </summary>
        /// <param name="url">API URL</param>
        /// <param name="Authorization">Header Authorization</param>
        /// <returns>String Api result</returns>
        public static string GETMethod_Headers(string url,  string JsonBody = "", string Headers = "")
        {
            string ContentResult = string.Empty;
            try
            {
                SetSSL();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = CONTENTTYPE;
                request.Method = GET_WebMethod;

                if (!string.IsNullOrEmpty(Headers))
                {
                    List<Headers> _headers = JsonConvert.DeserializeObject<List<Headers>>(Headers);

                    foreach (var Header in _headers)
                    {
                        if (!string.IsNullOrEmpty(Header.Name) && !string.IsNullOrEmpty(Header.Value))
                            request.Headers.Add(Header.Name, Header.Value);
                    }
                }

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(JsonBody);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    ContentResult = result;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response?.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var result = reader.ReadToEnd();
                            ContentResult = result;
                        }
                    }
                    else
                    {
                        ContentResult = ex.Message.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ContentResult = ex.Message.ToString();
                throw ex;
            }

            return ContentResult;
        }

        /// <summary>
        /// Request GET Method to the URL API provided.
        /// </summary>
        /// <param name="url">API URL</param>
        /// <param name="Id">Id</param>
        /// <param name="Authorization">Authorization</param>
        /// <returns>String Api result</returns>
        public static string GETMethod(string url, string Id, string Authorization = "")
        {
            string ContentResult = string.Empty;
            try
            {
                SetSSL();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Concat(url,"/",Id));
                request.ContentType = CONTENTTYPE;
                request.Method = GET_WebMethod;

                if (!string.IsNullOrEmpty(Authorization))
                    request.Headers.Add(HttpRequestHeader.Authorization, Authorization);

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    ContentResult = result;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response?.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var result = reader.ReadToEnd();
                            ContentResult = result;
                        }
                    }
                    else
                    {
                        ContentResult = ex.Message.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ContentResult = ex.Message.ToString();
                throw ex;
            }

            return ContentResult;
        }

        /// <summary>
        /// Request GET Method to the URL API provided.
        /// </summary>
        /// <typeparam name="T">Object used to deserialize the result</typeparam>
        /// <param name="url">API URL</param>
        /// <param name="Authorization">Authorization header</param>
        /// <returns>String Api result</returns>
        public static string GETMethod<T>(string url, ref T ObjectResult, string Authorization = "")
        {
            string ContentResult = string.Empty;
            try
            {
                SetSSL();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = CONTENTTYPE;
                request.Method = GET_WebMethod;

                if (!string.IsNullOrEmpty(Authorization))
                    request.Headers.Add(HttpRequestHeader.Authorization, Authorization);

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    ObjectResult = JsonConvert.DeserializeObject<T>(result);
                    ContentResult = result;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response?.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var result = reader.ReadToEnd();
                            ContentResult = result;
                        }
                    }
                    else
                    {
                        ContentResult = ex.Message.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ContentResult = ex.Message.ToString();
                throw ex;
            }

            return ContentResult;
        }

        /// <summary>
        /// Request GET Method to the URL API provided.
        /// </summary>
        /// <typeparam name="T">Object used to deserialize the result</typeparam>
        /// <param name="url">API URL</param>
        /// <param name="Authorization">Authorization header</param>
        /// <returns>String Api result</returns>
        public static string GETMethod<T>(string url, string Id, ref T ObjectResult, string Authorization = "")
        {
            string ContentResult = string.Empty;
            try
            {
                SetSSL();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Concat(url,"/",Id));
                request.ContentType = CONTENTTYPE;
                request.Method = GET_WebMethod;

                if (!string.IsNullOrEmpty(Authorization))
                    request.Headers.Add(HttpRequestHeader.Authorization, Authorization);

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    ObjectResult = JsonConvert.DeserializeObject<T>(result);
                    ContentResult = result;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response?.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var result = reader.ReadToEnd();
                            ContentResult = result;
                        }
                    }
                    else
                    {
                        ContentResult = ex.Message.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ContentResult = ex.Message.ToString();
                throw ex;
            }

            return ContentResult;
        }

        private static void SetSSL()
        {
            ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                 | SecurityProtocolType.Ssl3 
                                                 | (SecurityProtocolType)3072;
        }
    }
}
