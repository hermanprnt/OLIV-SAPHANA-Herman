using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common.Data
{
    public class CREATE_INV_TABLE
    {
        //private string _noreg = null;
        //public string NOREG
        //{
        //    get { return _noreg; }
        //    set { _noreg = value; }
        //}

        public List<CREATE_INV_IN> INPUT { get; set; }
        public List<CREATE_INV_GR_DATA> GR_DATA { get; set; }
        public List<CREATE_INV_GL_ACCOUNT> GL_ACCOUNT { get; set; }
        public List<CREATE_INV_OUT> OUTPUT { get; set; }


        public CREATE_INV_TABLE()
        {
            INPUT = new List<CREATE_INV_IN>();
            GR_DATA = new List<CREATE_INV_GR_DATA>();
            GL_ACCOUNT = new List<CREATE_INV_GL_ACCOUNT>();
            OUTPUT = new List<CREATE_INV_OUT>();
          
        }

        public bool isEmpty()
        {
            return ((INPUT == null || INPUT.Count < 1)
                && (GL_ACCOUNT == null || GL_ACCOUNT.Count < 1)
                && (OUTPUT == null || OUTPUT.Count < 1));
        }

    }
}
