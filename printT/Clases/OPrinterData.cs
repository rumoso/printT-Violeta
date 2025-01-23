using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace printT.Clases
{
    public class OPrinterData
    {
        public string printerName { get; set; }
        public int maxWidth { get; set; } = 0;
        public int maxMargen { get; set; } = 0;
        public string sBarCode { get; set; } = "";
    }
}