using PetaPoco;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceJournaltoSAP.Models
{
    class LibraryRepo
    {
        string ConnString = "InterfaceJournaltoSAP.ConnectionString";
        static string Dir = AppDomain.CurrentDomain.BaseDirectory;

        private LibraryRepo() { }
        private static LibraryRepo instance = null;
        public static LibraryRepo Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LibraryRepo();
                }
                return instance;
            }
        }

        #region Param
        public List<SapParamH> GetParamH()
        {
            string sql = @"SELECT DISTINCT A.INVOICE_ID
                    FROM TB_R_INVOICE A
		            JOIN TB_R_INVOICE_SAP_INPUT B ON A.INVOICE_ID = B.INVOICE_ID
		            JOIN TB_R_INVOICE_SAP_GR_DATA D ON A.INVOICE_ID = D.INVOICE_ID
		            JOIN TB_R_GR_IR_FROM_SAP E ON A.INVOICE_ID = E.INVOICE_ID
			            AND D.GR_NUMBER = E.MATDOC_NUMBER
			            AND D.GR_ITEM = E.MATDOC_ITEM
		            WHERE A.ON_PROCESS_SAP_POST = 'Y'
			            AND B.ON_PROCESS = 'Y'
			            AND D.ON_PROCESS = 'Y'
			            AND B.SAP_STATUS IS NULL";

            using (var db = new Database(ConnString))
            {
                db.CommandTimeout = 0;
                List<SapParamH> result = db.Fetch<SapParamH>(sql, new { });
                db.CloseSharedConnection();
                return result;
            }
        }

        public List<SapParamH> MandatoryChecking(string ProcessId)
        {
            string sql = System.IO.File.ReadAllText(System.IO.Path.Combine(Dir + @"\Sql\MandatoryChecking.sql"));
            using (var db = new Database(ConnString))
            {
                db.CommandTimeout = 0;
                List<SapParamH> result = db.Fetch<SapParamH>(sql, new
                {
                    ProcessId = ProcessId
                });
                db.CloseSharedConnection();
                return result;
            }
        }
        #endregion

        #region Update
        public void CreateStaging()
        {
            //string sql = System.IO.File.ReadAllText(System.IO.Path.Combine(Dir + @"\Sql\CreateStaging.sql"));
            //using (var db = new Database(ConnString))
            //{
            //    db.CommandTimeout = 0;
            //    string res = db.ExecuteScalar<string>(sql, new
            //    {
            //    });
            //    db.CloseSharedConnection();
            //}

            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnString].ConnectionString);
            SqlDataReader reader = null;

            try
            {

                connect.Open();

                SqlCommand sqlSelect = new SqlCommand("[dbo].[SP_BH00021_CREATE_STAGING]", connect);
                sqlSelect.CommandType = CommandType.StoredProcedure;
                sqlSelect.CommandTimeout = 0;

                reader = sqlSelect.ExecuteReader();

                while (reader.Read())
                {
                }

                connect.Close();
            }
            catch (Exception e)
            {
                connect.Close();
            }

        }

        public void UpdateFileName(string filename)
        {
            string sql = System.IO.File.ReadAllText(System.IO.Path.Combine(Dir + @"\Sql\UpdateFileName.sql"));
            using (var db = new Database(ConnString))
            {
                db.CommandTimeout = 0;
                string res = db.ExecuteScalar<string>(sql, new
                {
                    filename = filename
                });
                db.CloseSharedConnection();
            }

        }

        public void UpdateSequence()
        {
            string sql = System.IO.File.ReadAllText(System.IO.Path.Combine(Dir + @"\Sql\UpdateSequence.sql"));
            using (var db = new Database(ConnString))
            {
                db.CommandTimeout = 0;
                string res = db.ExecuteScalar<string>(sql, new
                {
                });
                db.CloseSharedConnection();
            }

        }
        #endregion


        #region Get System Master
        public List<SystemMasterModel> GetSystemMaster(string Type, string Code = "")
        {
            string sql = @"SELECT SYSTEM_CD, SYSTEM_VALUE_TEXT SYSTEM_VALUE, SYSTEM_DESC SYSTEM_REMARK FROM TB_M_SYSTEM WHERE SYSTEM_TYPE = @Type";
            List<SystemMasterModel> result;

            if (!string.IsNullOrEmpty(Code))
                sql = @"SELECT SYSTEM_CD, SYSTEM_VALUE_TEXT SYSTEM_VALUE, SYSTEM_DESC SYSTEM_REMARK FROM TB_M_SYSTEM WHERE SYSTEM_TYPE = @Type AND SYSTEM_CD = @Code";

            using (var db = new Database(ConnString))
            {
                db.CommandTimeout = 0;
                result = db.Fetch<SystemMasterModel>(sql, new
                {
                    Type = Type
                    , Code = Code
                });
                db.CloseSharedConnection();
                return result;
            }
        }
        #endregion

        #region Get Process Id
        public string GetProcessID()
        {
            string sql = @"declare @@PROCESS_ID bigint
                        exec [dbo].[SP_LOG_WRITE_HEADER] 
	                        @@PROCESS_ID OUTPUT, 
	                        '0',
	                        '0',
	                        '4',
	                        'SYSTEM'

                        INSERT INTO TB_R_LOG_D (
	                        PROCESS_ID
	                        ,SEQ_NO
	                        ,MESSAGE_ID
	                        ,LOCATION
	                        ,MESSAGE
	                        ,CREATED_DT
	                        ,CREATED_BY
	                        ,MESSAGE_TYPE
	                        )
                        VALUES (
	                        @@PROCESS_ID
	                        ,1
	                        ,'MCSTSTD001I'
	                        ,'Invoice Posting'
	                        ,'Invoice Posting is started'
	                        ,GETDATE()
	                        ,'SYSTEM'
	                        ,'I'
	                        );

                        select CONVERT(VARCHAR,@@PROCESS_ID) PROCESS_ID";
            using (var db = new Database(ConnString))
            {
                db.CommandTimeout = 0;
                string result = db.SingleOrDefault<string>(sql, new { });
                db.CloseSharedConnection();
                return result;
            }
        }
        #endregion

        #region Get Message
        public MessageModel GetMessageById(string MsgId)
        {
            string sql = @"SELECT MSG_ID as MsgId, MSG_TEXT as MsgText, MSG_TYPE as MsgType FROM TB_M_MESSAGE WHERE MSG_ID = @MSG_ID";

            using (var db = new Database(ConnString))
            {
                db.CommandTimeout = 0;
                MessageModel result = db.SingleOrDefault<MessageModel>(sql, new
                {
                    MSG_ID = MsgId
                });
                db.CloseSharedConnection();
                return result;
            }

            
        }
        #endregion

        #region Generate Log
        public void GenerateLog(string PROCESS_ID,
            string MOD_ID,
            string FUNC_ID,
            string MSG_ID,
            string MSG_TEXT,
            string MSG_TYPE,
            string PROCESS_NM,
            int PROCESS_STATUS,
            string USER_ID)
        {
            string sql = @"EXEC [dbo].[SP_Generate_Log] 
			                            @PROCESS_ID,
			                            @MOD_ID,
			                            @FUNC_ID,
			                            @MSG_ID,
			                            @MSG_TEXT,
			                            @MSG_TYPE,
			                            @PROCESS_NM,
			                            @PROCESS_STATUS,
			                            @USER_ID";
            using (var db = new Database(ConnString))
            {
                db.CommandTimeout = 0;
                db.Execute(sql, new {
                    PROCESS_ID = PROCESS_ID,
                    MOD_ID = MOD_ID,
                    FUNC_ID = FUNC_ID,
                    MSG_ID = MSG_ID,
                    MSG_TEXT = MSG_TEXT,
                    MSG_TYPE = MSG_TYPE,
                    PROCESS_NM = PROCESS_NM,
                    PROCESS_STATUS = PROCESS_STATUS,
                    USER_ID = USER_ID
                });
                db.CloseSharedConnection();
            }
        }

        #endregion
    }
}
