﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace printT.Clases
{
    public class OLineP
    {
        public int iWith { get; set; } = 100;
        public string text { get; set; }
        public string aling { get; set; }
        public int size { get; set; } = 7;
        public string type { get; set; } = "consola";
        public string style { get; set; } = "Regular";
        public bool bImage { get; set; }
        public string base64Image { get; set; }
        public int iHeight { get; set; }
        public int ticketWidth { get; set; }
    }
}