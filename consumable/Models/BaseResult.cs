using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models
{
    public class BaseResult
    {
        public const string VALUE_SUCCESS = "SUCCESS";
        public const string VALUE_ERROR = "ERROR";
        public string ValueSuccess { get { return VALUE_SUCCESS; } }
        public string ValueError { get { return VALUE_ERROR; } }

        public string Result { set; get; }
        public string Role { set; get; }
        public string ProcessId { set; get; }
        public string[] SuccMesgs { set; get; }
        public string[] ErrMesgs { set; get; }
        public object[] Params { set; get; }
    }
}