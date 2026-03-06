using System;
using System.Collections.Generic;
using System.Text;

namespace Prog4GodMAUI.Models
{
    public class Category
    {
        public string Name { get; set; }

        public string ImageName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Name))
                {
                    return null;
                }

                var lower = Name.ToLower();
                var builder = new System.Text.StringBuilder();
                foreach (var ch in lower)
                {
                    if (char.IsLetterOrDigit(ch)) builder.Append(ch);
                    else if (char.IsWhiteSpace(ch)) builder.Append('_');
                }

                return builder.ToString() + ".jpg";
            }
        }
    }
}
