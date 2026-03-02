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
                string formattedName = Name.Replace(" ", "_").ToLower();
                return formattedName + ".jpg";
            }
        }
    }
}
