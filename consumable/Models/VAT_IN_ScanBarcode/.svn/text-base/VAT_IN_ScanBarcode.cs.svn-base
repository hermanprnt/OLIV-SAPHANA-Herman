using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models
{
    public class VAT_IN_ScanBarcode
    {
        public String kdJenisTransaksi { get; set; }
        public String fgPengganti { get; set; }
        public String noFaktur { get; set; }
        public DateTime TanggalFaktur { get; set; }
        public String NPWPPenjual { get; set; }
        public String NamaPenjual { get; set; }
        public String AlamatPenjual { get; set; }
        public String NPWPLawanTrans { get; set; }
        public String NamaLawanTrans { get; set; }
        public String AlamatLawanTrans { get; set; }
        public String JmlDPP { get; set; }
        public String JmlPPN { get; set; }
        public String JmlPPnBM { get; set; }
        public String StatusApproval { get; set; }
        public String StatusFaktur { get; set; }
        public List<detailTransaksi> DetailTransaksi { get; set; }
        public string ErrorMessage { get; set; }
        public string Url { get; set; }
        public string QRPath { get; set; }
        public int IsOriginalTaxExist { get; set; } // 1 = True; 0 = False;
    }

    public class detailTransaksi
    {
        public String fgPengganti { get; set; }
        public String noFaktur { get; set; }
        public String Nama { get; set; }
        public String HargaSatuan { get; set; }
        public String JmlBarang { get; set; }
        public String HargaTotal { get; set; }
        public String Diskon { get; set; }
        public String DPP { get; set; }
        public String PPN { get; set; }
        public String TarifPPnBM { get; set; }
        public String PPnBM { get; set; }
    }
}