namespace Tesco.ClubcardProducts.MCA.Web.RestClient
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class Helper.
    /// </summary>
    public class Helper
    {
        private const char QueryIdentifier = '?';
        private const char QueryParamseperator = '&';
        private const char EqualOperator = '=';

        /// <summary>
        /// construct the REST service URL.
        /// Appends the URL query string.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="queryKeyValuePairs">The query key value pairs.</param>
        /// <returns>System.String.</returns>
        public static string AppendUrlQueryString(string url, List<KeyValuePair<string,string>> queryKeyValuePairs)
        {
            string resUrl = url ?? string.Empty;

            if (queryKeyValuePairs != null && queryKeyValuePairs.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(resUrl);
                bool appendQuerySep = false;

                if (resUrl.Length > 0)
                {
                    if (!resUrl.Contains(QueryIdentifier))
                    {
                        sb.Append(QueryIdentifier);
                    }
                    else if (resUrl[resUrl.Length - 1] != QueryParamseperator && resUrl[resUrl.Length - 1] != QueryIdentifier)
                    {
                        appendQuerySep = true;
                    }
                }

                foreach (var param in queryKeyValuePairs)
                {
                    if (appendQuerySep)
                    {
                        sb.Append(QueryParamseperator);
                    }

                    sb.AppendFormat("{0}{1}{2}", param.Key, EqualOperator, param.Value);
                    appendQuerySep = true;
                }

                resUrl = sb.ToString();
            }

            return resUrl;
        }
    }
}