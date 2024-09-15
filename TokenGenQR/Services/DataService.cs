using System.Collections.Generic;
using System;
using QRCodeInASPNetCore.Models;
using static System.Net.WebRequestMethods;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using CsvHelper;

namespace TokenGenQR.Services
{
    public class DataService
    {

        public List<UserInfoModel> ReadPatients(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<UserInfoModel>();
                return new List<UserInfoModel>(records);
            }
        }

        public void AddPatient(UserInfoModel newPatient, string filePath)
        {
            var employees = ReadPatients(filePath);
            employees.Add(newPatient);

            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(employees);
            }
        }

        public void Update(UserInfoModel updatedEmployee, string filePath)
        {
            var employees = ReadPatients(filePath);

            var employee = employees.Find(e => e.Token == updatedEmployee.Token);
            if (employee != null)
            {
                employee.Name = updatedEmployee.Name;
                employee.Phone = updatedEmployee.Phone;
                employee.GroupName = updatedEmployee.GroupName;

                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(employees);
                }
            }
        }

        public void Remove(int token, string filePath)
        {
            var employees = ReadPatients(filePath);
            var employeeToDelete = employees.Find(e => e.Token == token);

            if (employeeToDelete != null)
            {
                employees.Remove(employeeToDelete);

                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(employees);
                }
            }
        }
    }
}
