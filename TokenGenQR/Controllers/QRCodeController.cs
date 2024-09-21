using Microsoft.AspNetCore.Mvc;
using QRCodeInASPNetCore.Models;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using static QRCoder.PayloadGenerator;

namespace QRCodeInASPNetCore.Controllers
{
    public class QRCodeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.ShowNavBar = true;
            QRCodeModel model = new QRCodeModel();
            Payload payload = null;
            var time3pm = new TimeSpan(15, 0, 0);
            var currentTime = DateTime.Now.TimeOfDay;
            var qrDate = time3pm < currentTime ? DateTime.Today.AddDays(1).ToString() : DateTime.Today.ToString();
            model.QrGenDate = qrDate;
            model.WebsiteURL = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/TokenGen?encpass=" +Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(qrDate));
            payload = new Url(model.WebsiteURL);
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload);
            QRCode qrCode = new QRCode(qrCodeData);
            //var qrCodeAsBitmap = qrCode.GetGraphic(20);

            // use this when you want to show your logo in middle of QR Code and change color of qr code
            Bitmap logoImage = new Bitmap(@"wwwroot/img/gfri.jpg");
            var qrCodeAsBitmap = qrCode.GetGraphic(20, Color.Black, Color.White, logoImage);

            string base64String = Convert.ToBase64String(BitmapToByteArray(qrCodeAsBitmap));
            model.QRImageURL = "data:image/png;base64," + base64String;
            return View("Index", model);
        }

        private byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
