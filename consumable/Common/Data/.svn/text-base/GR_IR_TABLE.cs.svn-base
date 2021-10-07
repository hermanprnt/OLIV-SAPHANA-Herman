using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common.Data
{
    public class GR_IR_TABLE
    {
        //private string _noreg = null;
        //public string NOREG
        //{
        //    get { return _noreg; }
        //    set { _noreg = value; }
        //}

        public List<GR_IR_DOC_NO> DOC_NO { get; set; }
        public List<GR_IR_DOC_DATE> DOC_DATE { get; set; }
        public List<GR_IR_DATA> DATA { get; set; }
        public List<GR_IR_RETURN> RETURN { get; set; }

        public GR_IR_TABLE()
        {
            DOC_NO = new List<GR_IR_DOC_NO>();
            DOC_DATE = new List<GR_IR_DOC_DATE>();
            DATA = new List<GR_IR_DATA>();
            RETURN = new List<GR_IR_RETURN>();
        }

        public bool isEmpty()
        {
            return ((DOC_NO == null || DOC_NO.Count < 1)
                && (DOC_DATE == null || DOC_DATE.Count < 1)
                && (DATA == null || DATA.Count < 1)
                && (RETURN == null || RETURN.Count < 1));
        }

    }
}
