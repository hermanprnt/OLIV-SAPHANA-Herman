using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common.Data
{
    public class PO_TABLE
    {
        //private string _noreg = null;
        //public string NOREG
        //{
        //    get { return _noreg; }
        //    set { _noreg = value; }
        //}

        public List<SEND_PO_NO> PO_NO { get; set; }
        public List<SEND_PO_DATE> PO_DATE { get; set; }

        public List<SEND_PO_HEADER> PO_HEADER { get; set; } 

        public List<SEND_PO_RETURN> RETURN { get; set; }

        public PO_TABLE()
        {
            PO_NO = new List<SEND_PO_NO>();
            PO_DATE = new List<SEND_PO_DATE>();
            PO_HEADER = new List<SEND_PO_HEADER>();
            RETURN = new List<SEND_PO_RETURN>();
        }

        

    }
}
