using Dapper;
using QRCodeInASPNetCore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TokenGenQR.Services
{
    public class DataService
    {
        private readonly IDbConnection _db;
        public DataService(IDbConnection db)
        {
            _db = db;
        }

        public IEnumerable<UserInfoModel> ReadPatients(int? patientId = null)
        {
            var patients = _db.Query<UserInfoModel>(
                "sp_GetPatient",
                new { ID = patientId },
                commandType: CommandType.StoredProcedure);

            return patients;

        }

        public UserInfoModel AddPatient(UserInfoModel newPatient)
        {
            var addedPatient = new UserInfoModel();
            var patientId = _db.Query<int>(
                    "sp_AddOrEditPatient",
                    new { Name = newPatient.Name,
                        Phone = newPatient.Phone,
                        PatientType = newPatient.PatientType,  
                        GroupName = newPatient.GroupName,
                        CreatedDate = DateTime.UtcNow.AddHours(5.5),
                        UpdatedDate = DateTime.UtcNow.AddHours(5.5)
                    },
                    commandType: CommandType.StoredProcedure);

            addedPatient = ReadPatients(patientId.First()).First();
            return addedPatient;
        }
    }

   

    //public void Update(UserInfoModel updatedEmployee, string filePath)
    //{
    //    var employees = ReadPatients(filePath);

    //    var employee = employees.Find(e => e.Token == updatedEmployee.Token);
    //    if (employee != null)
    //    {
    //        employee.Name = updatedEmployee.Name;
    //        employee.Phone = updatedEmployee.Phone;
    //        employee.GroupName = updatedEmployee.GroupName;

    //        using (var writer = new StreamWriter(filePath))
    //        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
    //        {
    //            csv.WriteRecords(employees);
    //        }
    //    }
    //}

    //public void Remove(int token, string filePath)
    //{
    //    var employees = ReadPatients(filePath);
    //    var employeeToDelete = employees.Find(e => e.Token == token);

    //    if (employeeToDelete != null)
    //    {
    //        employees.Remove(employeeToDelete);

    //        using (var writer = new StreamWriter(filePath))
    //        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
    //        {
    //            csv.WriteRecords(employees);
    //        }
    //    }
    //}
}
