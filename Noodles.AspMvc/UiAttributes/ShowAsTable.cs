using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Noodles.AspMvc.UiAttributes
{
    public class ShowAsTable : ShowCollectionAttribute
    {
        public ShowAsTable()
        {
            UiHint = "Table";
            LinkColumn = "";
        }

        public string LinkColumn { get; set; }
    }
    public class NotInTable : Attribute
    {
        
    }
}