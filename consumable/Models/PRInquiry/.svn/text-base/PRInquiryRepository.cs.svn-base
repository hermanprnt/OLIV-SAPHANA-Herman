using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models.PRInquiry
{
    public class PRInquiryRepository
    {
        private PRInquiryRepository() { }

        #region Singleton
        private static PRInquiryRepository instance = null;
        public static PRInquiryRepository Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new PRInquiryRepository();
                }
                return instance;
            }
        }
        #endregion

        public IEnumerable<PRInquiry> GetPRInquiry()
        {
            List<PRInquiry> result = new List<PRInquiry>();
            PRInquiry item = null;
            Random numGen = new Random();

            for (int count = 1; count <= 20; count++)
            {
                item = new PRInquiry();
                item.ROW_NUM = count;
                item.PR_NO = "PR" + Convert.ToString(count).PadLeft(5, '0');
                item.PR_DESC = "PR" + Convert.ToString(count).PadLeft(5, '0') +" Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua";
                item.PR_DATE = DateTime.Now;
                item.PR_STATUS = "Status " + Convert.ToString(numGen.Next(1, 5));
                item.PLANT = "Plant " + Convert.ToString(numGen.Next(1, 10));
                item.STORAGE = "Storage " + Convert.ToString(numGen.Next(1, 3));
                item.DIVISION = "Div " + Convert.ToString(numGen.Next(1, 6));
                item.PROJECT_NO = "PJ" + Convert.ToString(count).PadLeft(5, '0');
                item.VENDOR_CD = "VN" + Convert.ToString(count).PadLeft(5, '0');
                item.VENDOR_NAME = "Vendor " + Convert.ToString(count);
                item.CREATED_BY = "Dummy User";
                item.CREATED_DT = "01.06.2015";

                if (count == 5) 
                {
                    item.CHANGED_BY = "Dummy User";
                    item.CHANGED_DT = "01.06.2015";
                }

                result.Add(item);
            }

            return result;
        }

        public List<String> GetPRInquirySort(string field, string sort)
        {
            List<String> result = new List<String>();
            List<PRInquiry> resultItem = new List<PRInquiry>();
            PRInquiry item = null;
            Random numGen = new Random();

            for (int count = 1; count <= 20; count++)
            {
                item = new PRInquiry();
                item.ROW_NUM = count;
                item.PR_NO = "PR" + Convert.ToString(count).PadLeft(5, '0');
                item.PR_DESC = "PR" + Convert.ToString(count).PadLeft(5, '0') +" Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua";
                item.PR_DATE = DateTime.Now;
                item.PR_STATUS = "Status " + Convert.ToString(numGen.Next(1, 5));
                item.PLANT = "Plant " + Convert.ToString(numGen.Next(1, 10));
                item.STORAGE = "Storage " + Convert.ToString(numGen.Next(1, 3));
                item.DIVISION = "Div " + Convert.ToString(numGen.Next(1, 6));
                item.PROJECT_NO = "PJ" + Convert.ToString(count).PadLeft(5, '0');
                item.VENDOR_CD = "VN" + Convert.ToString(count).PadLeft(5, '0');
                item.VENDOR_NAME = "Vendor " + Convert.ToString(count);
                item.CREATED_BY = "Dummy User";
                item.CREATED_DT = "01.06.2015";

                if (count == 5) 
                {
                    item.CHANGED_BY = "Dummy User";
                    item.CHANGED_DT = "01.06.2015";
                }

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
            }

            foreach (PRInquiry pr in returnResult)
            {
                result.Add("<tr>" +
                        "<td width=\"20px\" class=\"text-center grid-checkbox-col\">" +
                            "<input type=\"checkbox\" class=\"grid-checkbox grid-checkbox-body\" /> " +
                        "</td>" +
                        "<td width=\"30px\" class=\"text-left\">" + pr.ROW_NUM + "</td>" +
                        "<td width=\"120px\" class=\"text-left\">" + pr.PR_NO + "</td>" +
                        "<td width=\"200px\" style=\"max-width: 200px;\" class=\"text-left ellipsis\">" + pr.PR_DESC + "</td>" +
                        "<td width=\"120px\" class=\"text-center\">" + (pr.PR_DATE != DateTime.MinValue ? pr.PR_DATE.ToString("dd.MM.yyyy") : "") + "</td>" +
                        "<td width=\"100px\" class=\"text-center\">" + pr.PR_STATUS + "</td>" +
                        "<td width=\"80px\" class=\"text-center\">" + pr.PLANT + "</td>" +
                        "<td width=\"80px\" class=\"text-center\">" + pr.STORAGE + "</td>" +
                        "<td width=\"170px\" class=\"text-center\">" + pr.DIVISION + "</td>" +
                        "<td width=\"100px\" class=\"text-center\">" + pr.PROJECT_NO + "</td>" +
                        "<td width=\"100px\" class=\"text-center\">" + pr.VENDOR_CD + "</td>" +
                        "<td width=\"120px\" class=\"text-left\">" + pr.VENDOR_NAME + "</td>" +
                        "<td width=\"120px\" class=\"_toggle-detail text-left\">" + pr.CREATED_BY + "</td>" +
                        "<td width=\"90px\" class=\"_toggle-detail text-center\">" + pr.CREATED_DT + "</td>" +
                        "<td width=\"120px\" class=\"_toggle-detail text-left\">" + pr.CHANGED_BY + "</td>" +
                        "<td width=\"90px\" class=\"_toggle-detail text-center\">" + pr.CHANGED_DT + "</td>" +
                    "</tr>");
            }

            return result;
        }

        public IEnumerable<PRInquiry> GetPRInquiryLazy()
        {
            List<PRInquiry> result = new List<PRInquiry>();
            PRInquiry item = null;
            Random numGen = new Random();

            for (int count = 1; count <= 20; count++)
            {
                item = new PRInquiry();
                item.ROW_NUM = count;
                item.PR_NO = "PR" + Convert.ToString(count).PadLeft(5, '0');
                item.PR_DESC = "PR" + Convert.ToString(count).PadLeft(5, '0') +" Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua";
                item.PR_DATE = DateTime.Now;
                item.PR_STATUS = "Status " + Convert.ToString(numGen.Next(1, 5));
                item.PLANT = "Plant " + Convert.ToString(numGen.Next(1, 10));
                item.STORAGE = "Storage " + Convert.ToString(numGen.Next(1, 3));
                item.DIVISION = "Div " + Convert.ToString(numGen.Next(1, 6));
                item.PROJECT_NO = "PJ" + Convert.ToString(count).PadLeft(5, '0');
                item.VENDOR_CD = "VN" + Convert.ToString(count).PadLeft(5, '0');
                item.VENDOR_NAME = "Vendor " + Convert.ToString(count);

                result.Add(item);
            }

            return result;
        }
    }
}