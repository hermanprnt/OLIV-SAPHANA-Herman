using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models.Announcement
{
    public class AnnouncementRepository
    {
        private AnnouncementRepository() { }
        private static AnnouncementRepository instance = null;

        public static AnnouncementRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AnnouncementRepository();
                }
                return instance;
            }
        }

        public List<Announcement> GetDashboardAnnouncement(string status, int fromNumber, int toNumber)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new 
            {
                STATUS = status,
                NumberFrom = fromNumber.ToString(),
                NumberTo = toNumber.ToString()
            };
            List<Announcement> result = db.Fetch<Announcement>("GetDashboardAnnouncement", args);
            db.Close();

            return result;
        }

        public int CountDashboardAnnouncement(string status)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                STATUS = status
            };

            return db.SingleOrDefault<int>("CountDashboardAnnouncement", args);
        }

        public int countAnnouncement(string docTitleSearch, string releaseDateSearch, string statusSearch)
        {
            string releaseDateSearchFrom = "";
            string releaseDateSearchTo = "";
            if (releaseDateSearch != null && !"".Equals(releaseDateSearch))
            {
                string[] receivingDateSearchArray = releaseDateSearch.Split('-');
                releaseDateSearchFrom = receivingDateSearchArray[0].Trim();
                releaseDateSearchTo = receivingDateSearchArray[1].Trim();
            }

            if (docTitleSearch != null && !"".Equals(docTitleSearch))
            {
                string[] docTitleSearchArray = docTitleSearch.Split(';');

                for (int i = 0; i < docTitleSearchArray.Count(); i++)
                {
                    if (i == 0)
                    {
                        docTitleSearch = "'" + docTitleSearchArray[i] + "'";
                    }
                    else
                    {
                        docTitleSearch = docTitleSearch + ",'" + docTitleSearchArray[i] + "'";
                    }
                }
            }

            if (statusSearch != null && !"".Equals(statusSearch))
            {
                string[] statusSearchArray = statusSearch.Split(';');

                for (int i = 0; i < statusSearchArray.Count(); i++)
                {
                    if (i == 0)
                    {
                        statusSearch = "'" + statusSearchArray[i] + "'";
                    }
                    else
                    {
                        statusSearch = statusSearch + ",'" + statusSearchArray[i] + "'";
                    }
                }
            }

            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                DOC_TITLE = docTitleSearch,
                RELEASE_DATE_FROM = releaseDateSearchFrom,
                RELEASE_DATE_TO = releaseDateSearchTo,
                STATUS = statusSearch,
            };
            return db.SingleOrDefault<int>("CountAnnouncement", args);
        }

        public List<Announcement> GetAnnouncement(string docTitleSearch, string releaseDateSearch, string statusSearch, int fromnumber, int tonumber)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            string releaseDateFromStr = null;
            string releaseDateToStr = null;
            DateTime? releaseDateFrom = null;
            DateTime? releaseDateTo = null;
            if (releaseDateSearch != null && !"".Equals(releaseDateSearch))
            {
                string[] releaseDateArray = releaseDateSearch.Split('-');

                releaseDateFromStr = releaseDateArray[0].Trim();
                releaseDateToStr = releaseDateArray[1].Trim();
                releaseDateFrom = DateTime.ParseExact(releaseDateFromStr, "dd.MM.yyyy", null);
                releaseDateTo = DateTime.ParseExact(releaseDateToStr, "dd.MM.yyyy", null);
            }

            if (docTitleSearch != null && !"".Equals(docTitleSearch))
            {
                string[] docTitleSearchArray = docTitleSearch.Split(';');

                for (int i = 0; i < docTitleSearchArray.Count(); i++)
                {
                    if (i == 0)
                    {
                        docTitleSearch = "'" + docTitleSearchArray[i] + "'";
                    }
                    else
                    {
                        docTitleSearch = docTitleSearch + ",'" + docTitleSearchArray[i] + "'";
                    }
                }
            }

            if (statusSearch != null && !"".Equals(statusSearch))
            {
                string[] statusSearchArray = statusSearch.Split(';');

                for (int i = 0; i < statusSearchArray.Count(); i++)
                {
                    if (i == 0)
                    {
                        statusSearch = "'" + statusSearchArray[i] + "'";
                    }
                    else
                    {
                        statusSearch = statusSearch + ",'" + statusSearchArray[i] + "'";
                    }
                }
            }

            dynamic args = new
            {
                DOC_TITLE = docTitleSearch,
                RELEASE_DATE_FROM = releaseDateFrom,
                RELEASE_DATE_TO = releaseDateTo,
                STATUS = statusSearch,
                NUMBERFROM = fromnumber,
                NUMBERTO = tonumber
            };

            List<Announcement> result = db.Fetch<Announcement>("GetAnnouncement", args);
            db.Close();
            return result;
        }

        public List<Announcement> GetDocumentTitle()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {

            };

            List<Announcement> result =
                db.Fetch<Announcement>("GetAnnounceTitleList", args);

            db.Close();

            return result;
        }

        public Announcement GetAnnouncementByKey(Announcement announcement)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                DOCUMENT_ID = announcement.DOCUMENT_ID
            };

            Announcement result =
                db.SingleOrDefault<Announcement>("GetAnnouncementByKey", args);

            db.Close();

            return result;
        }

        public AjaxResult SaveData(IDBContext db, string filelocation, Announcement announcement, string NoReg)
        {
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    DOCUMENT_TITLE = announcement.ANNOUNCEMENT_TITLE,
                    RELEASE_DATE = DateTime.ParseExact(announcement.DATE_RELEASE_STR, "dd.MM.yyyy", null),
                    FILE_LOCATION = filelocation,
                    STATUS = announcement.STATUS,
                    CREATED_BY = NoReg,
                };

                db.Execute("InsertAnnouncement", args);

                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;

            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new String[] {
                                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message),
                };
            }
            finally
            {
                db.Close();
            }

            return ajaxResult;
        }

        public AjaxResult EditData(IDBContext db, string filelocation, Announcement announcement, string NoReg)
        {
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    DOCUMENT_ID = announcement.DOCUMENT_ID,
                    DOCUMENT_TITLE = announcement.ANNOUNCEMENT_TITLE,
                    RELEASE_DATE = DateTime.ParseExact(announcement.DATE_RELEASE_STR, "dd.MM.yyyy", null),
                    FILE_LOCATION = filelocation,
                    STATUS = announcement.STATUS,
                    CHANGED_BY = NoReg
                };

                db.Execute("UpdateAnnouncement", args);

                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;

            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new String[] {
                                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message),
                };
            }

            return ajaxResult;
        }

        public AjaxResult DeleteData(string doc_id)
        {
            AjaxResult ajaxResult = new AjaxResult();

            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                DOCUMENT_ID = doc_id
            };
            db.Execute("DeleteAnnouncement", args);

            ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
            ajaxResult.SuccMesgs = new String[] { "Delete Announcement Success" };

            return ajaxResult;
        }
    }
}