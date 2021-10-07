using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using consumable.Commons.Constants;
using consumable.Models;
using consumable.Models.Paging;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;
using consumable.Models.SystemProperty;
using consumable.Models.Announcement;
using consumable.Models.Message;
using consumable.Commons.Helpers;

namespace consumable.Controllers
{
    public class AnnouncementController : PageController
    {
        private DatabaseManager databaseManager = DatabaseManager.Instance;
        private AnnouncementRepository announcementRepo = AnnouncementRepository.Instance;
        private SystemPropertyRepository systemPropertyRepo = SystemPropertyRepository.Instance;
        private MessageRepository messageRepo = MessageRepository.Instance;

        protected override void Startup()
        {
            Settings.Title = "Announcement";
            getListAnnouncement(null, null, null, CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());

            constructComboBoxes();
        }

        public ActionResult search(string docTitle, string releaseDate, string status, int page, int size)
        {
            getListAnnouncement(docTitle, releaseDate, status, page, size);

            return PartialView("_GridView");
        }

        private void getListAnnouncement(string docTitle, string releaseDate, string status, int page, int size)
        {
            Paging pg = new Paging(announcementRepo.countAnnouncement(docTitle, releaseDate, status), page, size);
            ViewData["Paging"] = pg;
            List<Announcement> announcementList = announcementRepo.GetAnnouncement(docTitle, releaseDate, status, pg.StartData, pg.EndData);
            ViewData["AnnouncementList"] = announcementList;
        }

        public void constructComboBoxes()
        {
            List<SystemProperty> listSystemProperty = (List<SystemProperty>)systemPropertyRepo.GetBySystemPropertyType(CommonConstant.SYSTEM_TYPE_STATUS_ANNOUNCEMENT);
            List<Announcement> listDocumentTitle = (List<Announcement>)announcementRepo.GetDocumentTitle();
            ViewData["StatusList"] = listSystemProperty;
            ViewData["DocumentTitle"] = listDocumentTitle;

        }

        public ActionResult SaveInput(string screenMode, Announcement announcement, HttpPostedFileBase file)
        {
            AjaxResult ajaxResult = new AjaxResult();
            IDBContext db = databaseManager.GetContext();
            string path = null;

            if (file != null)
            {
                bool upload = true;

                //check file size
                decimal fileSize = file.ContentLength;
                decimal maxSize = int.Parse("3") * 1024 * 1024;

                if (fileSize > maxSize)
                {
                    upload = false;

                    Message msg = messageRepo.GetMessageById("INVCRE000003");

                    ajaxResult.Result = AjaxResult.VALUE_ERROR;
                    ajaxResult.ErrMesgs = new string[] {
                            string.Format(msg.MSG_TEXT, "3")
                        };
                    return Content(new JavaScriptSerializer().Serialize(ajaxResult), "text/plain", System.Text.Encoding.UTF8);
                }

                if (upload)
                {
                    try
                    {
                        string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;

                        SystemProperty sysProp = (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(
                                CommonConstant.SYSTEM_CD_UPLOAD_PATH_ANNOUNCEMENT, CommonConstant.SYSTEM_TYPE_UPLOAD_PATH);
                        path = sysProp.SYSTEM_VALUE_TEXT;

                        if (Directory.Exists(path))
                        {
                            //HousekeepingTempFolder(path, 2);
                        }
                        else
                        {
                            Directory.CreateDirectory(path);
                        }

                        string fileNameOrigin = file.FileName;
                        string fileNameFullPath = Path.Combine(path, fileNameOrigin);
                        file.SaveAs(fileNameFullPath);

                        //insert into temp table
                        if (CommonConstant.SCREEN_MODE_ADD.Equals(screenMode))
                        {
                            ajaxResult = announcementRepo.SaveData(db, fileNameFullPath, announcement, NoReg);
                        }
                        else if (CommonConstant.SCREEN_MODE_EDIT.Equals(screenMode))
                        {
                            //delete previous file
                            Announcement old = announcementRepo.GetAnnouncementByKey(announcement);
                            if(old != null && old.FILE_LOCATION != null)
                            {
                                if (System.IO.File.Exists(old.FILE_LOCATION))
                                {
                                    System.IO.File.Delete(old.FILE_LOCATION);
                                }
                            }
                            
                            ajaxResult = announcementRepo.EditData(db, fileNameFullPath, announcement, NoReg);
                        }

                    }
                    catch (Exception ex)
                    {
                        ajaxResult.Result = AjaxResult.VALUE_ERROR;
                        ajaxResult.ErrMesgs = new string[] {
                            string.Format("{0} = {1}", ex.GetType().FullName, ex.Message)
                        };
                    }
                }
            }
            else
            {
                if (CommonConstant.SCREEN_MODE_ADD.Equals(screenMode)) 
                {
                    ajaxResult.Result = AjaxResult.VALUE_ERROR;
                    ajaxResult.ErrMesgs = new string[] {
                        string.Format("Please choose file to upload!")
                    };
                }
                else if (CommonConstant.SCREEN_MODE_EDIT.Equals(screenMode))
                {
                    try
                    {
                        string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;

                        SystemProperty sysProp = (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(
                                CommonConstant.SYSTEM_CD_UPLOAD_PATH_ANNOUNCEMENT, CommonConstant.SYSTEM_TYPE_UPLOAD_PATH);
                        path = sysProp.SYSTEM_VALUE_TEXT;

                        string fileNameOriginEdit = announcement.FILE_NAME;
                        string fileNameFullPathEdit = Path.Combine(path, fileNameOriginEdit);

                        ajaxResult = announcementRepo.EditData(db, fileNameFullPathEdit, announcement, NoReg);
                    }
                    catch (Exception ex)
                    {
                        ajaxResult.Result = AjaxResult.VALUE_ERROR;
                        ajaxResult.ErrMesgs = new string[] {
                            string.Format("{0} = {1}", ex.GetType().FullName, ex.Message)
                        };
                    }

                }

            }

            return Content(new JavaScriptSerializer().Serialize(ajaxResult), "text/plain", System.Text.Encoding.UTF8);
        }

        protected void HousekeepingTempFolder(string path, int remainDays)
        {
            var directory = new DirectoryInfo(path);
            DateTime limitDt = DateTime.Now.Date.AddDays(-1 * remainDays);

            directory.GetFiles().Where(file => file.LastWriteTime.Date <= limitDt).ToList().ForEach(file => file.Delete());
        }
        public ActionResult GetByKey(Announcement announcement)
        {
            AjaxResult ajaxResult = new AjaxResult();
            Announcement result = null;
            try
            {
                result = announcementRepo.GetAnnouncementByKey(announcement);

                if (result == null)
                {
                    ajaxResult.Result = AjaxResult.VALUE_ERROR;
                    ajaxResult.ErrMesgs = new string[] {
                        string.Format("No Data with the selected key found, please refresh the screen first")
                    };

                    return Json(ajaxResult);
                }

                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
                ajaxResult.Params = new object[] {
                    result
                };
            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new string[] {
                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message)
                };
            }
            return Json(ajaxResult);
        }

        public ActionResult DeleteAnnouncement(Announcement announcement)
        {
            IDBContext db = databaseManager.GetContext();
            AjaxResult ajaxResult = new AjaxResult();
            Announcement result = null;
            string filelocation = null;

            try
            {
                result = announcementRepo.GetAnnouncementByKey(announcement);
                filelocation = result.FILE_LOCATION;

                if (System.IO.File.Exists(filelocation))
                {
                    System.IO.File.Delete(filelocation);
                }

                ajaxResult = announcementRepo.DeleteData(result.DOCUMENT_ID);
            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new string[] {
                        string.Format("{0} = {1}", ex.GetType().FullName, ex.Message)
                    };
            }

            return Content(new JavaScriptSerializer().Serialize(ajaxResult), "text/plain", System.Text.Encoding.UTF8);
        }

        public FileResult DownloadAnnouncement(string url)
        {
            //SystemProperty sysProp = (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(CommonConstant.SYSTEM_CD_UPLOAD_PATH_ANNOUNCEMENT, CommonConstant.SYSTEM_TYPE_UPLOAD_PATH);
            //string path = sysProp.SYSTEM_VALUE_TEXT;
            //string filePath = Path.Combine(path, url);

            if (System.IO.File.Exists(url))
            {
                Response.Clear();
                Response.ContentType = MimeExtensionHelper.GetMimeType(url);
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(url));
                Response.TransmitFile(url);
                Response.End();
                return File(url, MimeExtensionHelper.GetMimeType(url));
            }
            else
            {
                return null;
            }
        }
    }
}
