using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Noodles.AspMvc.UiAttributes
{
    public class ShowAsTable : ShowAttribute
    {
        public ShowAsTable()
        {
            UiHint = "Noodles/Table.";
        }
    }
}