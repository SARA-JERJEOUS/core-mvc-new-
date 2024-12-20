using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace towing_services.Models
{
    public class DriverViewModel
    {
        public int DriverId { get; set; }
        public string Name { get; set; }
        public string VehicleType { get; set; }

        public string Status { get; set; }
        public bool  isAvailable{ get; set; }

        public string ProfilePicture { get; set; } // مسار صورة الملف الشخصي
        public string LicensePicture { get; set; } // مسار صورة رخصة القيادة
        public string Email { get; set; }
        public string phone { get; set; }

        public string Password { get; set; } // لتعديل كلمة المرور
      
        public string TrackingDriver { get; set; }

        public string CurrentLocation { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
