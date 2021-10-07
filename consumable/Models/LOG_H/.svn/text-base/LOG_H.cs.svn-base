using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models
{
    public class LOG_H
    {
        public int NUMBER { get; set; }
        public string PROCESS_ID { get; set; }
        public DateTime START_DT { get; set; }
        public DateTime? END_DT { get; set; }
        public string MODULE_ID { get; set; }
        public string FUNCTION_ID { get; set; }
        public string PROCESS_STATUS { get; set; }
        public string USER_ID { get; set; }
        public string READ_FLAG { get; set; }
        public string REMARK { get; set; }

        public string FUNCTION_NAME
        {
            get
            {
                string function = FUNCTIONRepository.Instance.GetDataByID(MODULE_ID, FUNCTION_ID.Trim());
                return function;
            }
        }

        public string PROCESS_STATUS_NAME
        {
            get
            {
                string result = SystemRepository.Instance.GetLOG_H_STS(PROCESS_STATUS);
                return result;
            }
        }
    }
}