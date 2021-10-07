using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common.Data
{
    [Serializable]
    public class CREATE_INV_GL_ACCOUNT
    {
        public string GL_ACCOUNT { get; set; }
        public string AMOUNT { get; set; }
        public string SHKZG { get; set; }
    }
}
