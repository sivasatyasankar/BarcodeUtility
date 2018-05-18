using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing.Imaging;

public partial class GenerateBarcodeImage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["d"] != null)
        {
            //Read in the parameters
            string strData = Request.QueryString["d"];
            int imageHeight = Convert.ToInt32(Request.QueryString["h"]);
            int imageWidth = Convert.ToInt32(Request.QueryString["w"]);
            string Forecolor = Request.QueryString["fc"];
            string Backcolor = Request.QueryString["bc"];
            bool bIncludeLabel = Request.QueryString["il"].ToLower().Trim() == "true";
            string strImageFormat = Request.QueryString["if"].ToLower().Trim();
            string strAlignment = Request.QueryString["align"].ToLower().Trim();

            BarcodeUtility.TYPE type = BarcodeUtility.TYPE.UNSPECIFIED;
            switch (Request.QueryString["t"].Trim())
            {
                case "UPC-A": type = BarcodeUtility.TYPE.UPCA; break;
                case "UPC-E": type = BarcodeUtility.TYPE.UPCE; break;
                case "UPC 2 Digit Ext": type = BarcodeUtility.TYPE.UPC_SUPPLEMENTAL_2DIGIT; break;
                case "UPC 5 Digit Ext": type = BarcodeUtility.TYPE.UPC_SUPPLEMENTAL_5DIGIT; break;
                case "EAN-13": type = BarcodeUtility.TYPE.EAN13; break;
                case "JAN-13": type = BarcodeUtility.TYPE.JAN13; break;
                case "EAN-8": type = BarcodeUtility.TYPE.EAN8; break;
                case "ITF-14": type = BarcodeUtility.TYPE.ITF14; break;
                case "Codabar": type = BarcodeUtility.TYPE.Codabar; break;
                case "PostNet": type = BarcodeUtility.TYPE.PostNet; break;
                case "Bookland-ISBN": type = BarcodeUtility.TYPE.BOOKLAND; break;
                case "Code 11": type = BarcodeUtility.TYPE.CODE11; break;
                case "Code 39": type = BarcodeUtility.TYPE.CODE39; break;
                case "Code 39 Extended": type = BarcodeUtility.TYPE.CODE39Extended; break;
                case "Code 93": type = BarcodeUtility.TYPE.CODE93; break;
                case "LOGMARS": type = BarcodeUtility.TYPE.LOGMARS; break;
                case "MSI": type = BarcodeUtility.TYPE.MSI_Mod10; break;
                case "Interleaved 2 of 5": type = BarcodeUtility.TYPE.Interleaved2of5; break;
                case "Standard 2 of 5": type = BarcodeUtility.TYPE.Standard2of5; break;
                case "Code 128": type = BarcodeUtility.TYPE.CODE128; break;
                case "Code 128-A": type = BarcodeUtility.TYPE.CODE128A; break;
                case "Code 128-B": type = BarcodeUtility.TYPE.CODE128B; break;
                case "Code 128-C": type = BarcodeUtility.TYPE.CODE128C; break;
                case "Telepen": type = BarcodeUtility.TYPE.TELEPEN; break;
                case "FIM (Facing Identification Mark)": type = BarcodeUtility.TYPE.FIM; break;
                case "Pharmacode": type = BarcodeUtility.TYPE.PHARMACODE; break;
                default: break;
            }//switch

            System.Drawing.Image barcodeImage = null;
            try
            {
                BarcodeUtility.Barcode b = new BarcodeUtility.Barcode();
                if (type != BarcodeUtility.TYPE.UNSPECIFIED)
                {
                    b.IncludeLabel = bIncludeLabel;

                    //alignment
                    switch (strAlignment.ToLower().Trim())
                    {
                        case "c": b.Alignment = BarcodeUtility.AlignmentPositions.CENTER;
                            break;
                        case "r": b.Alignment = BarcodeUtility.AlignmentPositions.RIGHT;
                            break;
                        case "l": b.Alignment = BarcodeUtility.AlignmentPositions.LEFT;
                            break;
                        default: b.Alignment = BarcodeUtility.AlignmentPositions.CENTER;
                            break;
                    }//switch

                    if (Forecolor.Trim() == "" && Forecolor.Trim().Length != 6)
                        Forecolor = "000000";
                    if (Backcolor.Trim() == "" && Backcolor.Trim().Length != 6)
                        Backcolor = "FFFFFF";
                    
                    //===== Encoding performed here =====
                    barcodeImage = b.Encode(type, strData.Trim(), System.Drawing.ColorTranslator.FromHtml("#" + Forecolor), System.Drawing.ColorTranslator.FromHtml("#" + Backcolor), imageWidth, imageHeight);
                    //===================================
                    
                    //===== Static Encoding performed here =====
                    //barcodeImage = BarcodeUtility.Barcode.DoEncode(type, this.txtData.Text.Trim(), this.chkGenerateLabel.Checked, this.btnForeColor.BackColor, this.btnBackColor.BackColor);
                    //==========================================

                    Response.ContentType = "image/" + strImageFormat;
                    System.IO.MemoryStream MemStream = new System.IO.MemoryStream(); 

                    switch(strImageFormat)
                    {
                        case "gif": barcodeImage.Save(MemStream, ImageFormat.Gif); break;
                        case "jpeg": barcodeImage.Save(MemStream, ImageFormat.Jpeg); break;
                        case "png": barcodeImage.Save(MemStream, ImageFormat.Png); break;
                        case "bmp": barcodeImage.Save(MemStream, ImageFormat.Bmp); break;
                        case "tiff": barcodeImage.Save(MemStream, ImageFormat.Tiff); break;
                        default: break;
                    }//switch
                    MemStream.WriteTo(Response.OutputStream);
                }//if
            }//try
            catch (Exception ex)
            {
                //TODO: find a way to return this to display the encoding error message
            }//catch
            finally
            {
                if (barcodeImage != null)
                {
                    //Clean up / Dispose...
                    barcodeImage.Dispose();
                }
            }//finally
        }//if
    }
}
