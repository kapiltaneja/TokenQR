using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QRCodeInASPNetCore.Models
{
    public class UserInfoModel
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [DisplayName("Patient Name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Phone Number")]
        public string Phone { get; set; }

        [Required]
        [DisplayName("Group Name")]
        public string GroupName { get; set; }
        public int Token { get; set; }
        [DisplayName("Patient Type")]
        public string PatientType { get; set; }
    }
}
