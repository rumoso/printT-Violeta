using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace printT.Helpers
{
    public class Helpers
    {
        public Helpers() { }

        public FontStyle getFontStyle(string style)
        {
            switch (style)
            {
                case "Regular":
                    return FontStyle.Regular;
                case "Bold":
                    return FontStyle.Bold;
                case "Italic":
                    return FontStyle.Italic;
                case "Underline":
                    return FontStyle.Underline;
                case "Strikeout":
                    return FontStyle.Strikeout;
                default:
                    return FontStyle.Regular;
            }
        }

        public StringFormat getAling(string aling)
        {
            StringFormat stringFormat = new StringFormat();
            try
            {
                switch (aling)
                {
                    case "Right":
                        stringFormat.Alignment = StringAlignment.Far;
                        stringFormat.LineAlignment = StringAlignment.Near;
                        break;
                    case "left":
                        stringFormat.Alignment = StringAlignment.Near;
                        stringFormat.LineAlignment = StringAlignment.Near;
                        break;
                    case "Center":
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Near;
                        break;
                    default:
                        stringFormat.Alignment = StringAlignment.Near;
                        stringFormat.LineAlignment = StringAlignment.Near;
                        break;

                }
            }
            catch
            {
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;
            }

            return stringFormat;
        }

        public void LogMessage(string message, Exception ex = null)
        {
            string filePath = ConfigurationManager.AppSettings["LogFilePath"];
            filePath = filePath.Replace("{0}", DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString());
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Date : " + DateTime.Now.ToString());
                writer.WriteLine("Message : " + message);

                if (ex != null)
                {
                    writer.WriteLine("Error Message : " + ex.Message);
                    writer.WriteLine("Stack Trace : " + ex.StackTrace);
                }

                writer.WriteLine("--------------------------------------------------");
            }
        }
    }
}