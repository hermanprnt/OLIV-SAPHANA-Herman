using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common.Data
{
    public class CANCEL_INV_TABLE
    {
        //private string _noreg = null;
        //public string NOREG
        //{
        //    get { return _noreg; }
        //    set { _noreg = value; }
        //}

        public List<CANCEL_INV_IN> INPUT { get; set; }
        public List<CANCEL_INV_OUT> OUTPUT { get; set; }


        public CANCEL_INV_TABLE()
        {
            INPUT = new List<CANCEL_INV_IN>();
            OUTPUT = new List<CANCEL_INV_OUT>();
          
        }

        public bool isEmpty()
        {
            return ((INPUT == null || INPUT.Count < 1)
                && (OUTPUT == null || OUTPUT.Count < 1));
        }

    }
}
