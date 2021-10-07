using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models
{
    public static class PagingModelHelper
    {
        public static PagingModel PagingModel(this PagedModel p)
        {
            if (p != null)
                return new PagingModel(p.Count, p.Page, p.Rows);
            else
                return new PagingModel(0, 1, 1);
        }
    }
}