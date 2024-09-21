using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeInASPNetCore.Models
{
    public class QRCodeModel
    {
        public string QRImageURL { get; set; }
        //for website
        public string WebsiteURL { get; set; }

        public string QrGenDate { get; set; } 
    }


}
