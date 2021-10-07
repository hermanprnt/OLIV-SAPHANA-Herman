using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;
using consumable.Models.PRInquiry;

namespace consumable.Controllers
{
    public class DatagridLazyController : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "Lazy Datagrid";
            ViewData["PRInquiry"] = PRInquiryRepository.Instance.GetPRInquiryLazy();
        }


        [HttpGet]
        public ContentResult GetPRInquiry(int lastNum)
        {
            List<String> result = new List<String>();
            PRInquiry item = null;
            Random numGen = new Random();

            for (int count = lastNum; count <= (lastNum + 15); count++)
            {
                item = new PRInquiry();
                item.ROW_NUM = count;
                item.PR_NO = "PR" + Convert.ToString(count).PadLeft(5, '0');
                item.PR_DESC = "PR" + Convert.ToString(count).PadLeft(5, '0') + " Desc";
                item.PR_DATE = DateTime.Now;
                item.PR_STATUS = "Status " + Convert.ToString(numGen.Next(1, 5));
                item.PLANT = "Plant " + Convert.ToString(numGen.Next(1, 10));
                item.STORAGE = "Storage " + Convert.ToString(numGen.Next(1, 3));
                item.DIVISION = "Div " + Convert.ToString(numGen.Next(1, 6));
                item.PROJECT_NO = "PJ" + Convert.ToString(count).PadLeft(5, '0');
                item.VENDOR_CD = "VN" + Convert.ToString(count).PadLeft(5, '0');
                item.VENDOR_NAME = "Vendor " + Convert.ToString(count);

                result.Add("<tr>" +
                    "<td class=\"text-center\" width=\"20px\"><input type=\"checkbox\"  class=\"grid-checkbox grid-checkbox-body\" /></td>" +
                    "<td class=\"text-center\" width=\"30px\">" + item.ROW_NUM + "</td>" +
                    "<td class=\"text-left\" width=\"120px\">" + item.PR_NO + "</td>" +
                    "<td class=\"text-left ellipsis\" width=\"200px\" title=\"" + item.PR_DESC + "\">" + item.PR_DESC + "</td>" +
                    "<td class=\"text-center\" width=\"120px\">" + (item.PR_DATE != DateTime.MinValue ? item.PR_DATE.ToString("dd.MM.yyyy") : "") + "</td>" +
                    "<td class=\"text-center\" width=\"100px\">" + item.PR_STATUS + "</td>" +
                    "<td class=\"text-center\" width=\"80px\">" + item.PLANT + "</td>" +
                    "<td class=\"text-center\" width=\"80px\">" + item.STORAGE + "</td>" +
                    "<td class=\"text-center\" width=\"170px\">" + item.DIVISION + "</td>" +
                    "<td class=\"text-center\" width=\"100px\">" + item.PROJECT_NO + "</td>" +
                    "<td class=\"text-center\" width=\"100px\">" + item.VENDOR_CD + "</td>" +
                    "<td class=\"text-left\" width=\"120px\">" + item.VENDOR_NAME + "</td>" +
                    "<td class=\"_toggle-detail\" width=\"120px\" class=\"text-left\">System</td>" +
                    "<td class=\"_toggle-detail\" width=\"90px\" class=\"text-center\">12.05.2015</td>" +
                    "<td class=\"_toggle-detail\" width=\"120px\" class=\"text-left\">&nbsp;</td>" +
                    "<td class=\"_toggle-detail\" width=\"90px\" class=\"text-center\">&nbsp;</td>" +
                "</tr>");
            }

            return Content(String.Join("", result.ToArray()));
        }

        [HttpGet]
        public ContentResult GetPRInquirySort(string field, string sort, int lastNum, string method)
        {
            List<String> result = new List<String>();
            List<PRInquiry> resultItem = new List<PRInquiry>();
            PRInquiry item = null;
            Random numGen = new Random();

            int endVal = ((method == "sort") ? lastNum : (lastNum + 15));

            for (int count = 1; count <= endVal; count++)
            {
                item = new PRInquiry();
                item.ROW_NUM = count;
                item.PR_NO = "PR" + Convert.ToString(count).PadLeft(5, '0');
                item.PR_DESC = "PR" + Convert.ToString(count).PadLeft(5, '0') + " Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua";
                item.PR_DATE = DateTime.Now;
                item.PR_STATUS = "Status " + Convert.ToString(numGen.Next(1, 5));
                item.PLANT = "Plant " + Convert.ToString(numGen.Next(1, 10));
                item.STORAGE = "Storage " + Convert.ToString(numGen.Next(1, 3));
                item.DIVISION = "Div " + Convert.ToString(numGen.Next(1, 6));
                item.PROJECT_NO = "PJ" + Convert.ToString(count).PadLeft(5, '0');
                item.VENDOR_CD = "VN" + Convert.ToString(count).PadLeft(5, '0');
                item.VENDOR_NAME = "Vendor " + Convert.ToString(count);
                resultItem.Add(item);
            }

            List<PRInquiry> returnResult = new List<PRInquiry>();
            switch (field)
            {
                case "PR_NO":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PR_NO).ToList() : resultItem.OrderByDescending(o => o.PR_NO).ToList());
                    break;
                case "PR_DESC":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PR_DESC).ToList() : resultItem.OrderByDescending(o => o.PR_DESC).ToList());
                    break;
                case "PR_DATE":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PR_DATE).ToList() : resultItem.OrderByDescending(o => o.PR_DATE).ToList());
                    break;
                case "PR_STATUS":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PR_STATUS).ToList() : resultItem.OrderByDescending(o => o.PR_STATUS).ToList());
                    break;
                default:
                    returnResult = resultItem;
                    break;
            }

            foreach (PRInquiry pr in returnResult)
            {
                result.Add("<tr>" +
                    "<td class=\"text-center\" width=\"20px\"><input type=\"checkbox\"  class=\"grid-checkbox grid-checkbox-body\" /></td>" +
                    "<td class=\"text-center\" width=\"30px\">" + pr.ROW_NUM + "</td>" +
                    "<td class=\"text-left\" width=\"120px\">" + pr.PR_NO + "</td>" +
                    "<td class=\"text-left ellipsis\" width=\"200px\" style=\"max-width: 200px;\" title=\"" + pr.PR_DESC + "\">" + pr.PR_DESC + "</td>" +
                    "<td class=\"text-center\" width=\"120px\">" + (pr.PR_DATE != DateTime.MinValue ? pr.PR_DATE.ToString("dd.MM.yyyy") : "") + "</td>" +
                    "<td class=\"text-center\" width=\"100px\">" + pr.PR_STATUS + "</td>" +
                    "<td class=\"text-center\" width=\"80px\">" + pr.PLANT + "</td>" +
                    "<td class=\"text-center\" width=\"80px\">" + pr.STORAGE + "</td>" +
                    "<td class=\"text-center\" width=\"170px\">" + pr.DIVISION + "</td>" +
                    "<td class=\"text-center\" width=\"100px\">" + pr.PROJECT_NO + "</td>" +
                    "<td class=\"text-center\" width=\"100px\">" + pr.VENDOR_CD + "</td>" +
                    "<td class=\"text-left\" width=\"120px\">" + pr.VENDOR_NAME + "</td>" +
                    "<td class=\"_toggle-detail\" width=\"120px\" class=\"text-left\">System</td>" +
                    "<td class=\"_toggle-detail\" width=\"90px\" class=\"text-center\">12.05.2015</td>" +
                    "<td class=\"_toggle-detail\" width=\"120px\" class=\"text-left\">&nbsp;</td>" +
                    "<td class=\"_toggle-detail\" width=\"90px\" class=\"text-center\">&nbsp;</td>" +
                "</tr>");
            }

            return Content(String.Join("", result.ToArray()));
        }

    }
}
