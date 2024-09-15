using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QRCodeInASPNetCore.Models
{
    public class UserInfoModel
    {
        
        [DisplayName("Patient Name")]
        public string Name { get; set; }
        
        [DisplayName("Phone Number")]
        public string Phone { get; set; }
        
        [DisplayName("Group Name")]
        public string GroupName { get; set; }
        public int Token { get; set; }
        [DisplayName("Patient Type")]
        public string PatientType { get; set; }
    }
}
