using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models.Message
{
    public class MessageRepository
    {
        private MessageRepository() { }
        private static MessageRepository instance = null;

        public static MessageRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MessageRepository();
                }
                return instance;
            }
        }

        public Message GetMessageById(string msgId)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                MSG_ID = msgId
            };

            Message result = db.SingleOrDefault<Message>("GetMessageById", args);

            db.Close();

            return result;
        }
    }
}