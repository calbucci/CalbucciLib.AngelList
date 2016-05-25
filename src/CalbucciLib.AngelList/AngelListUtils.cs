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
            return true;
            
        }

    }
}
