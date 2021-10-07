using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable
{
    public enum Doing
    {
        Help = 0,

        /* RFC Methods */
        Get = 1,              /// ZHRTFM_GET_EMPLOYEE_PROFILE
        Put = 2,              /// ZHRTFM_UPD_EMPLOYEE_PROFILE
        PutObject = 3,        /// ZRH_OBJECT_MAINTAIN
        PutRelation = 4,      /// ZRH_RELATION_MAINTAIN
        RhDirOrgStruGet = 5,  /// RH_DIR_ORG_STRUC_GET
        GetOrgStru = 6,       /// ZHRTFM_RH_DIR_ORG_STRUC_GET
        UploadCico = 17,      /// ZHRTFM_PI_UPLD_CICO

        /* PI Methods */
        PiCico = 101,         /// MIOS_CICO
        PiShift = 102,        /// MIOS_UPDATE_SHIFTDATA
        PiOvertime = 103,     /// MIOS_UPDATE_OVERTIME
        PiBdjk = 104,         /// MIOS_UPDATE_BDJK 
        PiLeave = 105,        /// MIOS_UPDATE_LEAVE 
        PiAction = 107,       /// MIOS_UPDATE_ACTION     

        /* Test Methods */ 
        Xml = 1025,
        Map = 1026,
        TopoSort = 32768
    }
}
