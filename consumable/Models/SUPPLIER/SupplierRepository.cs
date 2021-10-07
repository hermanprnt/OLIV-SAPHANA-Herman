﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models.SUPPLIER
{
    public class SupplierRepository
    {
        private SupplierRepository() { }
        private static SupplierRepository instance = null;

        public static SupplierRepository Instance
        {
            get
            {
                if(instance==null)
                {
                    instance = new SupplierRepository();
                }
                return instance;
            }
        }

        public Supplier GetDataByID(string TAX_INVOICE_NO, string REPLACEMENT_FG)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TAX_INVOICE_NO = TAX_INVOICE_NO,
                REPLACEMENT_FG = REPLACEMENT_FG
            };

            Supplier result = db.SingleOrDefault<Supplier>("SUPPLIER_GetByID", args);
            db.Close();

            //result.NPWP = HelperRepository.Instance.GenNPWPFormat(result.NPWP);
            return result;
        }

        public bool InsertData(VAT_IN_ScanBarcode data, string Username, string SuppCode)
        {
            if(SuppCode=="1000")
            {
                SuppCode = "";
            }
            bool status = false;
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TAX_INVOICE_NO = data.noFaktur,
                REPLACEMENT_FG = data.fgPengganti,
                SUPPLIER_CODE = SuppCode,
                NAME = data.NamaPenjual,
                NPWP = data.NPWPPenjual,
                ADDRESS = data.AlamatPenjual,
                CREATED_DT = DateTime.Now,
                CHANGED_DT = DateTime.Now,
                CREATED_BY = Username,
                CHANGED_BY = Username
            };

            try
            {
                int result = db.Execute("Supplier_InsertData", args);
                db.Close();
                status = true;
                return status;
            }
            catch (Exception)
            {
                db.Close();
                status = false;
                return status;
            }
        }

        public List<string> GetAllSupplier()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {

            };

            List<string> result = db.Fetch<string>("Supplier_GetAll", args);
            db.Close();
            return result;
        }

        public int countSupplier(string parameter)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                Parameter = parameter
            };
            return db.SingleOrDefault<int>("CountSupplier", args);
        }

        public List<Supplier> GetSupplier(string parameter, int fromnumber, int tonumber)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                Parameter = parameter,
                NumberFrom = fromnumber,
                NumberTo = tonumber
            };

            List<Supplier> result = db.Fetch<Supplier>("GetSupplier", args);
            db.Close();
            return result;
        }

        public int countSupplier(string param1, string param2)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                Parameter1 = param1,
                Parameter2 = param2
            };
            return db.SingleOrDefault<int>("CountSupplierParam2", args);
        }

        public List<Supplier> GetSupplier(string param1, string param2, int fromnumber, int tonumber)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                Parameter1 = param1,
                Parameter2 = param2,
                NumberFrom = fromnumber,
                NumberTo = tonumber
            };

            List<Supplier> result = db.Fetch<Supplier>("GetSupplierParam2", args);
            db.Close();
            return result;
        }

        //public AjaxResult SaveData(IDBContext db, Supplier supplier, string NoReg)
        //{
        //    //IDBContext db = DatabaseManager.Instance.GetContext();
        //    AjaxResult ajaxResult = new AjaxResult();

        //    try
        //    {
        //        dynamic args = new
        //        {
        //            SUPPLIER_CD = supplier.SUPPLIER_CD,
        //            SUPPLIER_NAME = supplier.SUPPLIER_NAME,
        //            PKP_FLAG = supplier.PKP_FLAG,
        //            EDIT_AMOUNT_FLAG = supplier.EDIT_AMOUNT_FLAG,
        //            PPN_RATE = supplier.PPN_RATE,
        //            PAY_METHOD = supplier.PAY_METHOD,
        //            TERM_PAY = supplier.TERM_PAY,
        //            //PARTNER_BANK = supplier.PARTNER_BANK,
        //            CREATED_BY = NoReg,
        //        };

        //        db.Execute("InsertSupplier", args);

        //        ajaxResult.Result = AjaxResult.VALUE_SUCCESS;

        //    }
        //    catch (Exception ex)
        //    {
        //        ajaxResult.Result = AjaxResult.VALUE_ERROR;
        //        ajaxResult.ErrMesgs = new String[] { 
        //                            string.Format("{0} = {1}", ex.GetType().FullName, ex.Message), 
        //        };
        //    }
        //    finally
        //    {
        //        db.Close();
        //    }

        //    return ajaxResult;
        //}

        public Supplier GetBySupplierId(string supplierId)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                SupplierId = supplierId
            };

            Supplier result =
                db.SingleOrDefault<Supplier>("GetSupplierById", args);

            db.Close();

            return result;
        }

        public Supplier GetBySupplierCd(string supplierCd)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                SupplierCd = supplierCd
            };

            Supplier result =
                db.SingleOrDefault<Supplier>("GetSupplierByCd", args);

            db.Close();

            return result;
        }

        public AjaxResult EditData(IDBContext db, Supplier supplier, string NoReg)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    SUPPLIER_ID = supplier.SUPPLIER_ID,
                    //SUPPLIER_CD = supplier.SUPPLIER_CD,
                    //SUPPLIER_NAME = supplier.SUPPLIER_NAME,
                    PKP_FLAG = supplier.PKP_FLAG,
                    EDIT_AMOUNT_FLAG = supplier.EDIT_AMOUNT_FLAG,
                    PPN_RATE = supplier.PPN_RATE,
                    //PAY_METHOD = supplier.PAY_METHOD,
                    //TERM_PAY = supplier.TERM_PAY,
                    //PARTNER_BANK = supplier.PARTNER_BANK,
                    EMAIL = supplier.EMAIL,
                    AUTO_POSTING_FLAG = supplier.AUTO_POSTING_FLAG,
                    WITHOLDING_CODE = supplier.WITHOLDING_CODE,

                    //20200706 
                    SKB_FLAG = supplier.SKB_FLAG,
                    PLAT_KUNING_FLAG = supplier.PLAT_KUNING_FLAG,
                    SKB_VALID_FROM = supplier.SKB_VALID_FROM_STR.FormatSQLDate(),
                    SKB_VALID_TO = supplier.SKB_VALID_TO_STR.FormatSQLDate(),

                    //20200706

                    CHANGED_BY = NoReg
                };

                db.Execute("UpdateSupplier", args);

                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;

            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new String[] { 
                                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message), 
                };
            }
            //finally
            //{
            //    db.Close();
            //}

            return ajaxResult;
        }

        public AjaxResult DeleteData(IDBContext db, List<Supplier> supplierList)
        {
            Console.Write("Delete Supplier Repo");
            int result = 1;
            //IDBContext db = DatabaseManager.Instance.GetContext();

            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                foreach (Supplier item in supplierList)
                {
                    dynamic args = new
                    {
                        SUPPLIER_CD = item.SUPPLIER_CD
                    };

                    result = db.Execute("DeleteSupplier", args);

                    ajaxResult.Result = AjaxResult.VALUE_SUCCESS;

                }
            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new String[] { 
                                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message), 
                };
            }
            //finally
            //{
            //    db.Close();
            //}

            return ajaxResult;
        }

        public List<Supplier> GetByWithHoldingTax(string withHoldingTax)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                WITHHOLDING_TAX = withHoldingTax
            };

            List<Supplier> result =
                db.Fetch<Supplier>("GetWithHoldingTax", args);

            db.Close();

            return result;
        }

    }
}