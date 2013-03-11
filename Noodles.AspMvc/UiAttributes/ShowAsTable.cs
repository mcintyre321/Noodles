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
            UiHint = "Noodles/Table.";
        }
    }
}