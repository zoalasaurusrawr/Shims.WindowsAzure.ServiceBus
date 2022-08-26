using System.Collections.Concurrent;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Microsoft.ServiceBus
{
    public static class ServiceBusEnvironment
    {
        public static Uri CreateServiceUri(string scheme, string serviceNamespace, string servicePath)
        {
            return CreateServiceUri(scheme, serviceNamespace, servicePath, suppressRelayPathPrefix: false, RelayEnvironment.RelayHostRootName);
        }

        public static Uri CreateServiceUri(string scheme, string serviceNamespace, string servicePath, bool suppressRelayPathPrefix)
        {
            return CreateServiceUri(scheme, serviceNamespace, servicePath, suppressRelayPathPrefix, RelayEnvironment.RelayHostRootName);
        }

        private static Uri CreateServiceUri(string scheme, string serviceNamespace, string servicePath, bool suppressRelayPathPrefix, string hostName)
        {
            string serviceNamespace2 = serviceNamespace.Trim();
            ValidateSchemeAndNamespace(scheme, serviceNamespace2);
            if (!servicePath.EndsWith("/", StringComparison.Ordinal))
            {
                servicePath += "/";
            }

            Uri uri = ServiceBusUriHelper.CreateServiceUri(scheme, serviceNamespace2, hostName, servicePath, suppressRelayPathPrefix);
            if (!ServiceBusUriHelper.IsSafeBasicLatinUriPath(uri))
            {
                throw new ArgumentException(servicePath);
            }

            return uri;
        }

        private static void ValidateSchemeAndNamespace(string scheme, string serviceNamespace)
        {
            if (!ValidateScheme(scheme))
            {
                throw new ArgumentException("scheme");
            }

            if (!ValidateServiceNamespace(serviceNamespace))
            {
                throw new ArgumentException("serviceNamespace");
            }
        }

        private static bool ValidateScheme(string scheme)
        {
            if (!string.IsNullOrEmpty(scheme))
            {
                if (!scheme.Equals("http") && !scheme.Equals("https"))
                {
                    return scheme.Equals("sb");
                }

                return true;
            }

            return false;
        }

        private static bool ValidateServiceNamespace(string serviceNamespace)
        {
            if (!string.IsNullOrEmpty(serviceNamespace))
            {
                return ServiceBusUriHelper.IsBasicLatinNonControlString(serviceNamespace);
            }

            return false;
        }
    }
}



namespace Microsoft.ServiceBus
{
    internal static class ServiceBusUriHelper
    {
        //
        // Summary:
        //     TODO: See if we can remove this API. This was moved from Microsoft.ServiceBus.Channels.dll
        //     so that Microsoft.Cloud.ServiceBus.Common.dll can use it.
        private static class ServiceBusStringExtension
        {
            private const int MinServiceNamespaceLength = 6;

            private const int MaxServiceNamespaceLength = 50;

            private static readonly string ServiceNamespacePattern = "^[a-zA-Z][a-zA-Z0-9-]{" + 4 + "," + 48 + "}[a-zA-Z0-9]$";

            private static readonly Regex ServiceNamespaceRegex = new Regex(ServiceNamespacePattern, RegexOptions.Compiled);

            private static IEnumerable<string> ReservedHostnameSuffixes = new string[3] { "-sb", "-mgmt", "-sb-mgmt" };

            //
            // Summary:
            //     Validate if a given servcie namespace candidate is a valid service namespace
            //
            // Parameters:
            //   serviceNamespaceCandidate:
            //     servcie namespace candidate to be validated
            //
            //   isReservedSuffixAllowed:
            //     true if reserved hostname suffixes are allowed in the Uri, false otherwise
            //
            // Returns:
            //     true if the given candidate is a valid service namespace, false otherwise
            public static bool IsValidServiceNamespace(string serviceNamespaceCandidate, bool isReservedSuffixAllowed)
            {
                if (serviceNamespaceCandidate == null)
                {
                    return false;
                }

                string suffix;
                bool flag = HasReservedSuffix(serviceNamespaceCandidate, out suffix);
                if (!isReservedSuffixAllowed && flag)
                {
                    return false;
                }

                serviceNamespaceCandidate = (flag ? StripReservedSuffix(serviceNamespaceCandidate, suffix) : serviceNamespaceCandidate);
                if (!ServiceNamespaceRegex.IsMatch(serviceNamespaceCandidate))
                {
                    return false;
                }

                if (serviceNamespaceCandidate.StartsWith("xn--", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                return true;
            }

            private static string StripReservedSuffix(string serviceNamespaceCandidate, string suffix)
            {
                return serviceNamespaceCandidate.Substring(0, serviceNamespaceCandidate.Length - suffix.Length);
            }

            private static bool HasReservedSuffix(string serviceNamespaceCandidate, out string suffix)
            {
                suffix = ReservedHostnameSuffixes.FirstOrDefault((string s) => serviceNamespaceCandidate.EndsWith(s, StringComparison.Ordinal)) ?? string.Empty;
                return suffix != null;
            }
        }

        private const int MaxCacheItem = 10000;

        public static readonly Regex SafeBasicLatinUriSegmentExpression = new Regex("^[\\u0020\\u0021\\u0024-\\u002E\\u0030-\\u003B\\u003D\\u0040-\\u005B\\u005D-\\u007D\\u007E]*/?$", RegexOptions.Compiled);

        public static readonly Regex SafeMessagingEntityNameExpression = new Regex("^[\\w-\\.\\$]*/?$", RegexOptions.Compiled | RegexOptions.ECMAScript);

        private static readonly Regex BasicLatinNonControlStringExpression = new Regex("^[\\u0020-\\u007E]*$", RegexOptions.Compiled);

        private static readonly ConcurrentDictionary<Uri, string> UriCache = new ConcurrentDictionary<Uri, string>();

        private static bool IsOneboxEnvironment => !string.IsNullOrEmpty(RelayEnvironment.RelayPathPrefix);

        internal static Uri CreateServiceUri(string scheme, string authority, string servicePath)
        {
            return CreateServiceUri(scheme, authority, servicePath, suppressRelayPathPrefix: false);
        }

        internal static Uri CreateServiceUri(string scheme, string authority, string servicePath, bool suppressRelayPathPrefix)
        {
            string uri = "tempscheme://" + authority;
            UriBuilder uriBuilder = new UriBuilder(uri);
            if (uriBuilder.Port == -1)
            {
                uriBuilder.Port = GetSchemePort(scheme);
            }

            uriBuilder.Scheme = scheme;
            uriBuilder.Path = RefinePath(servicePath, suppressRelayPathPrefix);
            return uriBuilder.Uri;
        }

        internal static Uri CreateServiceUri(string scheme, string serviceNamespace, string hostName, string servicePath)
        {
            return CreateServiceUri(scheme, serviceNamespace, hostName, servicePath, suppressRelayPathPrefix: false);
        }

        internal static Uri CreateServiceUri(string scheme, string serviceNamespace, string hostName, string servicePath, bool suppressRelayPathPrefix)
        {
            string authority = (string.IsNullOrEmpty(serviceNamespace) ? hostName : string.Format(CultureInfo.InvariantCulture, "{0}.{1}", serviceNamespace, hostName));
            return CreateServiceUri(scheme, authority, servicePath, suppressRelayPathPrefix);
        }

        private static int GetSchemePort(string scheme)
        {
            return scheme switch
            {
                "http" => RelayEnvironment.RelayHttpPort,
                "https" => RelayEnvironment.RelayHttpsPort,
                _ => -1,
            };
        }

        internal static bool IsRequestAuthorityIpAddress(Uri requestUri)
        {
            if (requestUri == null)
            {
                return false;
            }

            if (requestUri.HostNameType != UriHostNameType.IPv4)
            {
                return requestUri.HostNameType == UriHostNameType.IPv6;
            }

            return true;
        }

        internal static string RemoveServicePathPrefix(string path)
        {
            TryRemoveServicePathPrefix(path, out var refinedPath, throwOnFailure: true);
            return refinedPath;
        }

        internal static bool TryRemoveServicePathPrefix(string path, out string refinedPath)
        {
            return TryRemoveServicePathPrefix(path, out refinedPath, throwOnFailure: false);
        }

        private static bool TryRemoveServicePathPrefix(string path, out string refinedPath, bool throwOnFailure)
        {
            refinedPath = path;
            if (!IsOneboxEnvironment)
            {
                return true;
            }

            if (!path.StartsWith(RelayEnvironment.RelayPathPrefix, StringComparison.OrdinalIgnoreCase))
            {
                if (throwOnFailure)
                {
                    throw new ArgumentException(path);
                }

                return false;
            }

            refinedPath = path.Substring(RelayEnvironment.RelayPathPrefix.Length);
            return true;
        }

        public static Uri EnsurePathEndsWith(this Uri uri, string value, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (uri.AbsolutePath.EndsWith(value, comparisonType))
            {
                return uri;
            }

            UriBuilder uriBuilder = new UriBuilder(uri);
            uriBuilder.Path += value;
            return uriBuilder.Uri;
        }

        internal static Uri NormalizeUri(Uri uri, bool ensureTrailingSlash = false)
        {
            return NormalizeUri(uri.AbsoluteUri, uri.Scheme, stripQueryParameters: true, stripPath: false, ensureTrailingSlash);
        }

        internal static Uri NormalizeUri(string uri, string scheme, bool stripQueryParameters = true, bool stripPath = false, bool ensureTrailingSlash = false)
        {
            UriBuilder uriBuilder = new UriBuilder(uri)
            {
                Scheme = scheme,
                Port = -1,
                Fragment = string.Empty,
                Password = string.Empty,
                UserName = string.Empty
            };
            if (stripPath)
            {
                uriBuilder.Path = string.Empty;
            }

            if (stripQueryParameters)
            {
                uriBuilder.Query = string.Empty;
            }

            if (ensureTrailingSlash && !uriBuilder.Path.EndsWith("/", StringComparison.Ordinal))
            {
                uriBuilder.Path += "/";
            }

            return uriBuilder.Uri;
        }

        internal static bool IsSafeUri(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            string text = WebUtility.UrlDecode(uri.AbsoluteUri);
            int length = text.Length;
            bool flag = false;
            for (int i = 0; i < length; i++)
            {
                char c = text[i];
                if (!flag && ((c >= ' ' && c <= '~') || (c >= '\u00a0' && c <= '\ud7ff') || (c >= '\ue000' && c <= '\ufffd')))
                {
                    continue;
                }

                if (!flag && '\ud800' <= c && c <= '\udbff')
                {
                    flag = true;
                    continue;
                }

                if (flag && '\udc00' <= c && c <= '\udfff')
                {
                    flag = false;
                    continue;
                }

                return false;
            }

            return !flag;
        }

        //
        // Summary:
        //     Remove a queryString parameter and while leaving the rest of the queryString
        //     properly formatted including only those ampersands which are required after the
        //     query string parameter identified by removeStartIndex+removeLength is removed.
        //     Example1: queryString = "a=b&c=d&e=f", removeStartIndex = 4, removeLength = 3
        //     results in "a=b&e=f"
        //     Example2: queryString = "a=b&c=d", removeStartIndex = 0, removeLength = 3 results
        //     in "c=d"
        //     Example3: queryString = "a=b&c=d", removeStartIndex = 4, removeLength = 3 results
        //     in "a=b"
        internal static string FilterQueryString(string queryString, int removeStartIndex, int removeLength)
        {
            StringBuilder stringBuilder = new StringBuilder(queryString.Length);
            if (removeStartIndex > 0)
            {
                stringBuilder.Append(queryString.Substring(0, removeStartIndex - 1));
            }

            if (removeStartIndex + removeLength < queryString.Length)
            {
                int num = removeStartIndex + removeLength;
                if (stringBuilder.Length == 0)
                {
                    num++;
                }

                stringBuilder.Append(queryString.Substring(num));
            }

            return stringBuilder.ToString();
        }

        internal static Uri ReplaceQueryString(Uri uri, string queryString)
        {
            UriBuilder uriBuilder = new UriBuilder(uri);
            uriBuilder.Query = queryString;
            return uriBuilder.Uri;
        }

        internal static bool IsSafeBasicLatinUriPath(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            string[] segments = uri.Segments;
            for (int i = 1; i < segments.Length; i++)
            {
                string text = segments[i];
                string input = WebUtility.UrlDecode(text);
                if (!SafeBasicLatinUriSegmentExpression.IsMatch(input))
                {
                    return false;
                }
            }

            return true;
        }

        internal static bool IsSafeMessagingEntityUriPath(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            if (!string.IsNullOrWhiteSpace(uri.Fragment))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(uri.UserInfo))
            {
                return false;
            }

            string[] segments = uri.Segments;
            for (int i = 1; i < segments.Length; i++)
            {
                string text = segments[i];
                string input = WebUtility.UrlDecode(text);
                if (!SafeMessagingEntityNameExpression.IsMatch(input))
                {
                    return false;
                }
            }

            return true;
        }

        internal static bool IsBasicLatinNonControlString(string str)
        {
            return BasicLatinNonControlStringExpression.IsMatch(str);
        }

        //
        // Summary:
        //     TODO: See if we can remove this API. This was moved from Microsoft.ServiceBus.Channels.dll
        //     so that Microsoft.Cloud.ServiceBus.Common.dll can use it. Validate the service
        //     namespace and the hostname in the input Uri and return the service namespace
        //     on successful validation
        //
        // Parameters:
        //   uri:
        //     Uri to validate
        //
        //   expectedHostnameSuffix:
        //     expected hostname to compare against (e.g. ".servicebus.windows.net")
        //
        //   isReservedSuffixAllowed:
        //     true if reserved hostname suffixes are allowed in the Uri, false otherwise
        //
        // Returns:
        //     the validated service namespace
        internal static string ParseServiceNamespace(this Uri uri, string expectedHostnameSuffix, bool isReservedSuffixAllowed)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            if (expectedHostnameSuffix == null)
            {
                throw new ArgumentNullException("expectedHostnameSuffix");
            }

            string host = uri.Host;
            string text = string.Empty;
            if (host.EndsWith(expectedHostnameSuffix, StringComparison.OrdinalIgnoreCase))
            {
                text = host.Replace(expectedHostnameSuffix, string.Empty);
            }

            if (text == null)
            {
                throw new FormatException(uri.ToString());
            }

            if (!ServiceBusStringExtension.IsValidServiceNamespace(text, isReservedSuffixAllowed))
            {
                throw new FormatException(uri.ToString());
            }

            return text;
        }

        internal static string GetEntityPerfCounterName(this Uri endpoint)
        {
            if (UriCache.TryGetValue(endpoint, out var value))
            {
                return value;
            }

            if (UriCache.Count > 10000)
            {
                UriCache.Clear();
            }

            UriBuilder uriBuilder = new UriBuilder(endpoint)
            {
                Port = -1
            };
            Uri uri = uriBuilder.Uri;
            int num = uri.AbsoluteUri.IndexOf("://", StringComparison.InvariantCultureIgnoreCase);
            value = ((num < 0 || uri.AbsoluteUri.Length <= num + 3) ? uri.AbsoluteUri.Replace("/", "|") : uri.AbsoluteUri.Substring(num + 3).Replace("/", "|"));
            UriCache.TryAdd(endpoint, value);
            return value;
        }

        private static string RefinePath(string path, bool suppressRelayPathPrefix)
        {
            string result = path;
            if (!IsOneboxEnvironment || suppressRelayPathPrefix)
            {
                return result;
            }

            if (string.IsNullOrEmpty(path))
            {
                return RelayEnvironment.RelayPathPrefix;
            }

            return path.StartsWith("/", StringComparison.OrdinalIgnoreCase) ? (RelayEnvironment.RelayPathPrefix + path) : (RelayEnvironment.RelayPathPrefix + "/" + path);
        }
    }
}