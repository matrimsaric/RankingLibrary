using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RankingLibrary.Security
{
    internal class TextValidator
    {
        private Regex checkString = new Regex(@"^[a-zA-Z]+$");//^[a-zA-Z]+$  [\\s\\w\\.]*

        internal virtual bool ValidateText(string unsafeText, out string safeText, object param = null)
        {
            bool passedSearch = false;
            safeText = String.Empty;

            // space is permitted but is an extra char
            if (!checkString.IsMatch(unsafeText.Replace(" ", String.Empty)))
            {
                safeText = unsafeText.Replace(checkString.ToString(), String.Empty);
                throw new AuthenticationException("unsafe input");
            }
            else
            {
                // passed
                safeText = unsafeText;// only time this it permitted
                passedSearch = true;
            }

            return passedSearch;


        }
    }
}
