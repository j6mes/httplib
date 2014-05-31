using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace Redslide.HttpLib
{
    /// <summary>
    /// This abstract class is a container for utility functions used by HttpLib
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Serialize an array of Key-Value pairs into a URL encoded query string
        /// </summary>
        /// <param name="Parameters">The key-value pair array</param>
        /// <returns>The URL encoded query string</returns>
        public static string SerializeQueryString(object Parameters)
        {
            string querystring = "";
            int i = 0;
            try
            {

                PropertyInfo[] properties;
                #if NETFX_CORE
                properties = Parameters.GetType().GetTypeInfo().DeclaredProperties.ToArray();
                #else
                properties =  Parameters.GetType().GetProperties();
                #endif

                

                foreach (var property in properties)
                {
                    querystring += property.Name + "=" + System.Uri.EscapeDataString(property.GetValue(Parameters, null).ToString());

                    if (++i < properties.Length)
                    {
                        querystring += "&";
                    }
                }

                

            }
            catch (NullReferenceException e)
            {
                throw new ArgumentNullException("Paramters cannot be a null object",e);
            }
          
            return querystring;
        }
    }
}
