using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;
using WebSupergoo.ABCpdf11;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PDF.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDFController : ControllerBase
    {
        // GET: api/<PDFController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API is running.....");
        }

        // GET: api/<PDFController>
        [HttpGet("GetPDF")]
        public IActionResult GetPdf(string url)
        {     
            SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
            converter.Options.CustomCSS = "";
            converter.Options.PdfPageSize = SelectPdf.PdfPageSize.A4;

            converter.Options.CssMediaType = SelectPdf.HtmlToPdfCssMediaType.Screen;
            converter.Options.EmbedFonts = true;
            converter.Options.MarginLeft = 1;
            converter.Options.MarginRight = 1;
            converter.Options.MarginTop = 10;

            //PdfTextSection pdfTextSection = new PdfTextSection(0,0,"", new System.Drawing.Font("Arial", 8));
            //pdfTextSection.RightToLeft = true;
            converter.Options.KeepTextsTogether = true;
            converter.Options.PdfPageOrientation = SelectPdf.PdfPageOrientation.Portrait;
            //converter.
            converter.Options.DisplayCutText = true;
            converter.Options.DisplayFooter = false;
            converter.Options.DisplayHeader = false;
            // converter.Options.WebPageHeight = 0;
            converter.Options.WebPageWidth = 1300;
            converter.Options.AutoFitHeight = SelectPdf.HtmlToPdfPageFitMode.NoAdjustment;
            SelectPdf.PdfDocument doc = converter.ConvertUrl(url);
            byte[] bytearray = doc.Save(); 
            Stream stream = new MemoryStream(bytearray);
            string mimeType = "application/pdf";
            return new FileStreamResult(stream, mimeType)
            {
                FileDownloadName = "htmltoPdf" + System.DateTime.Now.Ticks.ToString() + ".pdf"
            };
        }


        [HttpPost]
        public IActionResult Post([FromForm]PDFDTO pDFDTO)
        {         

            StreamReader Sr = new StreamReader(pDFDTO.HtmlString.OpenReadStream());
            string str = Sr.ReadToEnd();
            Sr.Close();
            Sr.Dispose();

            SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
            converter.Options.CustomCSS = "";
            converter.Options.PdfPageSize = SelectPdf.PdfPageSize.A4;
            
            converter.Options.CssMediaType = SelectPdf.HtmlToPdfCssMediaType.Screen;
            converter.Options.EmbedFonts = true;
            converter.Options.MarginLeft = 1;
            converter.Options.MarginRight = 1;
            converter.Options.MarginTop = 10;

            //PdfTextSection pdfTextSection = new PdfTextSection(0,0,"", new System.Drawing.Font("Arial", 8));
            //pdfTextSection.RightToLeft = true;
            converter.Options.KeepTextsTogether = true;
            converter.Options.PdfPageOrientation = SelectPdf.PdfPageOrientation.Portrait;
            //converter.
            converter.Options.DisplayCutText = true;
            converter.Options.DisplayFooter = false;
            converter.Options.DisplayHeader = false;
           // converter.Options.WebPageHeight = 0;
            converter.Options.WebPageWidth = 1300;          
            converter.Options.AutoFitHeight = SelectPdf.HtmlToPdfPageFitMode.NoAdjustment;

            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(str);
            doc.Save(@"C:\pdf\testHtmltoPDF" + System.DateTime.Now.Ticks.ToString() + ".pdf");
            doc.Close();          
            
            return Ok();
        }       
    }

    // XSettings.Register();
    // XSettings.InstallTrialLicense("");
    // Doc d = new Doc();
    //d.HtmlOptions.Media = MediaType.Print;
    //d.AddImageHtml(str);
    ////d.Font = d.EmbedFont("Trado", LanguageType.Unicode, true,true);
    //d.Save(@"C:\pdf\ABCPdf" + System.DateTime.Now.Ticks.ToString() + ".pdf");
    //Response.Headers.Clear();
    //Response.Headers.Add("content-disposition", $"attachment;filename={pDFDTO.FileName}.pdf");
    //return new FileStreamResult(d.GetStream(), "application/pdf");

    public class PDFDTO
    {
        public IFormFile HtmlString { get; set; }
        public string FileName { get; set; }
    }
}
