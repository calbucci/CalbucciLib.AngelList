using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalbucciLib.AngelList
{
    public static class AngelListUtils
    {
        private static string AngelListBaseUrl = "https://angel.co/";

        private static string[] AngelListLinks = new string[]
        {
            "http://angel.co",
            "https://www.angel.co",
            "https://angel.co",
            "http://www.angel.co"
        };

        private static HashSet<string> _NotValidSlugs;

        static AngelListUtils()
        {
            _NotValidSlugs = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
            {
                "candidates",
                "jobs",
                "syndicates",
                "companies",
                "people",
                "investors",
                "markets",
                "locations",
                "salaries",
                "valuations",
                "accelerators",
                "search",
                "notifications",
                "messages",
                "help",
                "terms",
                "risks",
                "news",

            };
        }

        public static string GetSlug(string angelListUrl)
        {
            if (string.IsNullOrWhiteSpace(angelListUrl) || !Uri.IsWellFormedUriString(angelListUrl, UriKind.Absolute))
                return null;

            // https://angel.co/calbucci
            var uri = new Uri(angelListUrl);
            if (!uri.Host.Equals("angel.co", StringComparison.CurrentCultureIgnoreCase)
                && !uri.Host.Equals("www.angel.co", StringComparison.CurrentCultureIgnoreCase))
                return null;

            if (uri.Segments.Length < 2)
                return null;

            var slug = uri.Segments[1].Trim('/');

            if (!IsValidSlug(slug))
                return null;
            return slug;
        }

        public static bool IsValidSlug(string slug)
        {
            // TODO: Double check this
            if (string.IsNullOrEmpty(slug))
                return false;

            if (slug.Length > 32) // ???
                return false;

            foreach (char c in slug)
            {
                if (c >= 'a' && c <= 'z')
                    continue;
                if (c >= 'A' && c <= 'Z')
                    continue;
                if (c >= '0' && c <= '9')
                    continue;
                if (c == '-')
                    continue;
                return false;
            }

            if (_NotValidSlugs.Contains(slug))
                return false;


            return true;
            
        }

        public static bool IsAngelListLink(string link)
        {
            if (string.IsNullOrWhiteSpace(link))
                return false;

            foreach (var prefix in AngelListLinks)
            {
                if (link.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        public static string ExtractSlug(string linkOrSlug)
        {
            if (string.IsNullOrWhiteSpace(linkOrSlug))
                return null;

            linkOrSlug = linkOrSlug.Trim();
            if (linkOrSlug.StartsWith("@"))
                linkOrSlug = linkOrSlug.Substring(1);

            if (IsValidSlug(linkOrSlug))
                return linkOrSlug;

            if (linkOrSlug.Length > 16)
            {
                return GetSlug(linkOrSlug);
            }

            return null;

        }

    }
}
