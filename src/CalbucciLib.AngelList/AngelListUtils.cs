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
            if (string.IsNullOrWhiteSpace(angelListUrl))
                return null;

            // https://angel.co/calbucci
            if (angelListUrl.StartsWith(AngelListBaseUrl, StringComparison.CurrentCultureIgnoreCase))
                return null;

            int pos = angelListUrl.IndexOfAny(new char[] { '/', '?', '#'}, AngelListBaseUrl.Length);
            string slug = null;
            if (pos > 0)
            {
                slug = angelListUrl.Substring(AngelListBaseUrl.Length, pos - AngelListBaseUrl.Length);
            }
            else
            {
                slug = angelListUrl.Substring(AngelListBaseUrl.Length);
            }
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

            string slug = linkOrSlug;
            if (IsValidSlug(slug))
                return slug;

            if (linkOrSlug.Length > 16)
            {
                // It's probably a link
                if (!IsAngelListLink(linkOrSlug))
                    return null;

                try
                {
                    Uri link;
                    if (!Uri.TryCreate(linkOrSlug, UriKind.Absolute, out link))
                        return null;

                    if (link.Segments.Length == 0)
                        return null;

                    slug = link.Segments[1];
                    if (string.IsNullOrEmpty(slug))
                        return null;

                    if (slug.EndsWith("/"))
                        slug = slug.Substring(0, slug.Length - 1);

                }
                catch (Exception)
                {
                    return null;
                }
            }

            if (!IsValidSlug(slug))
                return null;

            return slug;
        }

    }
}
