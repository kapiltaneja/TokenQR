using Microsoft.AspNetCore.Mvc;
using QRCodeInASPNetCore.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using TokenGenQR.Services;

namespace QRCodeInASPNetCore.Controllers
{

    public class TokenGenController : Controller
    {
        private DataService _dataService;
        private string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CSVFiles");
        private string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CSVFiles\\UserData" + DateTime.Today.ToShortDateString() + ".csv");

        public TokenGenController()
        {
            _dataService = new DataService();
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            if (!System.IO.File.Exists(filePath))
            {
                System.IO.File.Create(filePath).Close();
                string header = ("Token" + "," + "Name" + "," + "Phone" + "," + "GroupName" + "," + "PatientType") + Environment.NewLine;
                System.IO.File.AppendAllText(filePath, header);
            }

        }

        public IActionResult Index(string encpass)
        {
            ViewBag.ShowNavBar = false;
            var model = new UserInfoModel();
            var date = DecodeBase64(encpass);
            if (date == DateTime.Today.ToString())
            {
                return View(model);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Index(bool isNewPatient)
        {
            var patientType = isNewPatient ? "new" : "old";

            return RedirectToAction("AddPatientInfo", new { patientType });
        }

        public IActionResult AddPatientInfo(string patientType)
        {
            ViewBag.ShowNavBar = false;
            var model = new UserInfoModel();
            model.PatientType = patientType;
            return View(model);
        }

        [HttpPost]
        public IActionResult AddPatientInfo(UserInfoModel model)
        {
            ViewBag.ShowNavBar = false;
            var oldRecords = _dataService.ReadPatients(filePath);

            if (oldRecords.Count > 0 && oldRecords.Where(x => x.PatientType == model.PatientType).ToList().Count > 0)
                model.Token = oldRecords.Where(x => x.PatientType == model.PatientType).Max(x => x.Token) + 1;
            else
                model.Token = 1;

            _dataService.AddPatient(model, filePath);

            return View("Token", model);
        }

        public ActionResult Patients()
        {
            ViewBag.ShowNavBar = true;
            var model = _dataService.ReadPatients(filePath);
            return View(model);
        }


        private static string DecodeBase64(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            var valueBytes = Convert.FromBase64String(value);
            return System.Text.Encoding.UTF8.GetString(valueBytes);
        }
    }
}
