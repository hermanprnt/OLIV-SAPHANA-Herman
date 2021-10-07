using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models
{
    public class AjaxResult : BaseResult
    {
        public string[] ExceptionErrors { get; set; }
        public object Data { get; set; }
    }
}