using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common.Data
{
    public class TCICO
    {
        public string PERNR { get; set; }  // Personnel Number 
        public string LDATE { get; set; }
        public string LTIME { get; set; } 
        public string SATZA  { get; set; } // Time Event Type, Clock Type
        public string DALLF { get; set; }  // Day assignment 
        public string TERID { get; set; }  // Terminal ID 
        public string STATS { get; set; }  // Status Time Events 
    }
}
