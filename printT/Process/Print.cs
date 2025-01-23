using printT.Clases;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Razor.Tokenizer;
using System.Windows.Forms;
using System.Xml.Linq;
using ZXing;
using static System.Net.Mime.MediaTypeNames;

namespace printT.Process
{
    public class Print
    {
        public OPrintP OPrintParameters = new OPrintP();
        //public printParameters OPrintParameters = new printParameters();

        public List<OWithBySize> OWithBySize = new List<OWithBySize>();

        public Print() { }

        private PrintDocument doc = new PrintDocument();
        public Helpers.Helpers _helpers = new Helpers.Helpers();
        //private System.Drawing.Image companyIcon = System.Drawing.Image.FromFile("C:\\Github\\VioletaSystem\\Front\\src\\assets\\img\\icons\\VioletaIcon.png"); // Ruta del ícono de la empresa

        public OResponse PrintNow()
        {
            var oResponse = new OResponse();
            try
            {
                OWithBySize.Add(new OWithBySize() { size = 5, iWith = 65 });
                OWithBySize.Add(new OWithBySize() { size = 6, iWith = 53 });
                OWithBySize.Add(new OWithBySize() { size = 7, iWith = 45 });
                OWithBySize.Add(new OWithBySize() { size = 8, iWith = 45 });
                OWithBySize.Add(new OWithBySize() { size = 9, iWith = 45 });
                OWithBySize.Add(new OWithBySize() { size = 10, iWith = 45 });
                OWithBySize.Add(new OWithBySize() { size = 20, iWith = 45 });

                doc.PrinterSettings.PrinterName = OPrintParameters.oPrinter.printerName;
                OPrintParameters.oPrinter.maxWidth = doc.PrinterSettings.DefaultPageSettings.Bounds.Width;
                doc.PrintPage += new PrintPageEventHandler(printTicketNew);

                doc.Print();

                //_helpers.LogMessage("Print operation completed successfully.");
                oResponse.bOK = true;
                oResponse.message = "Print operation completed successfully.";
            }
            catch (Exception ex)
            {
                //_helpers.LogMessage("An error occurred during printing.", ex);
                //return false;
                oResponse.bOK = false;
                oResponse.message = ex.Message;
            }

            return oResponse;
            
        }

        public void printTicketNew(object sender, PrintPageEventArgs e)
        {
            try
            {
                var oLinesP = OPrintParameters.oLinesP;
                int posX = 0, posY = 0;
                int margin = 10; // Espacio entre los bloques
                List<int> iTextCount = new List<int>();

                //// Función para ajustar el texto basado en el tipo de caracteres
                //int AjustarTexto(string texto)
                //{
                //    int minuscCount = texto.Count(char.IsLower);
                //    int mayuscCount = texto.Count(char.IsUpper);
                //    int numCount = texto.Count(char.IsDigit);
                //    int otherCount = texto.Length - (minuscCount + mayuscCount + numCount);

                //    int totalMinusc = (minuscCount * 53) / 53;
                //    int totalMayusc = (mayuscCount * 40) / 53;
                //    int totalNum = (numCount * 50) / 53;
                //    int totalOther = (otherCount * 50) / 53; // Asumiendo que otros caracteres (espacios, signos) se ajustan igual que números

                //    return totalMinusc + totalMayusc + totalNum + totalOther;
                //}

                // Bloque 1: Detalles del producto
                for (var i = 0; i < oLinesP.Count; i++)
                {
                    posX = 0;
                    iTextCount = new List<int>();

                    for (var j = 0; j < oLinesP[i].oLines.Count; j++)
                    {
                        var text = oLinesP[i].oLines[j].text ?? " ";
                        var aling = oLinesP[i].oLines[j].aling;
                        var size = oLinesP[i].oLines[j].size;
                        var type = oLinesP[i].oLines[j].type;
                        var style = oLinesP[i].oLines[j].style;
                        var iWith = oLinesP[i].oLines[j].iWith;

                        var oFontStyle = _helpers.getFontStyle(style);

                        int iWithNow = (int)(((decimal)OPrintParameters.oPrinter.maxWidth / (decimal)OPrintParameters.oPrinter.maxMargen) * (decimal)iWith);

                        Font oFont = new Font(type, size, oFontStyle);

                        Rectangle rect = new Rectangle(posX, posY, iWithNow, 10000);
                        posX += iWithNow;

                        var iWithByLine = OWithBySize.FirstOrDefault(x => x.size == size);
                        if (iWithByLine == null)
                        {
                            // Si el tamaño no está definido, usa un valor predeterminado (por ejemplo, 50)
                            iWithByLine = new OWithBySize() { size = size, iWith = 50 };
                        }

                        var iWithByLineN = iWithByLine.iWith - oLinesP[i].oLines.Count - 1;
                        var porcent = (decimal)iWith / (decimal)100;
                        var porcentWith = (decimal)iWithByLineN * porcent;
                        var porcentWithRound = Math.Round(porcentWith);
                        var div = text.Length / porcentWithRound;
                        var iTextC = (int)(div + 1) * 13;
                        iTextCount.Add(iTextC);

                        var stringFormat = _helpers.getAling(aling);

                        e.Graphics.DrawString(text, oFont, Brushes.Black, rect, stringFormat);
                    }
                    var maxLength = iTextCount.Max();

                    posY += maxLength;
                }

                // Código de barras en la misma línea que el texto
                if (OPrintParameters?.oPrinter?.sBarCode?.Length > 0)
                {
                    // PARA QUE QUEDE ALINEADO AL NOMBRE
                    posY = 0;

                    // Bloque 2: Código de barras
                    int barcodeWidth = 150;
                    int barcodeHeight = 30;

                    // Calcular posX para centrar el código de barras
                    posX = (OPrintParameters.oPrinter.maxWidth - barcodeWidth) / 2;

                    // Generar e imprimir el código de barras
                    string barcodeContent = OPrintParameters.oPrinter.sBarCode; // El contenido del código de barras
                    var writer = new BarcodeWriter
                    {
                        Format = BarcodeFormat.CODE_128,
                        Options = new ZXing.Common.EncodingOptions
                        {
                            Width = barcodeWidth,
                            Height = barcodeHeight
                        }
                    };

                    Bitmap barcodeBitmap = writer.Write(barcodeContent);

                    // Imprimir el código de barras al mismo nivel que el texto
                    e.Graphics.DrawImage(barcodeBitmap, posX, posY);

                    // Liberar los recursos de la imagen
                    barcodeBitmap.Dispose();
                }


                //if (OPrintParameters.oPrinter.sBarCode?.Length > 0)
                //{
                //    // Guardar la posición inicial de Y
                //    int initialPosY = posY;

                //    // Espacio entre bloques
                //    posY += margin;

                //    // Bloque 2: Ícono de la empresa y código de barras
                //    int iconWidth = 50;
                //    int iconHeight = 50;
                //    int barcodeWidth = 150;
                //    int barcodeHeight = 30;

                //    // Calcular posX para centrar el icono y el código de barras
                //    posX = (OPrintParameters.oPrinter.maxWidth - Math.Max(iconWidth, barcodeWidth)) / 2;

                //    // Restablecer posY para el ícono de la empresa
                //    posY = initialPosY - iconHeight - margin; // Ajustar para que empiece a la misma altura que el primer bloque

                //    // Imprimir el ícono de la empresa
                //    e.Graphics.DrawImage(companyIcon, posX + (barcodeWidth - iconWidth) / 2, posY, iconWidth, iconHeight);

                //    // Generar e imprimir el código de barras debajo del ícono de la empresa
                //    string barcodeContent = OPrintParameters.oPrinter.sBarCode; // El contenido del código de barras
                //    var writer = new BarcodeWriter
                //    {
                //        Format = BarcodeFormat.CODE_128,
                //        Options = new ZXing.Common.EncodingOptions
                //        {
                //            Width = barcodeWidth,
                //            Height = barcodeHeight
                //        }
                //    };

                //    Bitmap barcodeBitmap = writer.Write(barcodeContent);

                //    // Imprimir el código de barras
                //    posY += iconHeight + margin;
                //    e.Graphics.DrawImage(barcodeBitmap, posX, posY);

                //    // Liberar los recursos de la imagen
                //    barcodeBitmap.Dispose();
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}