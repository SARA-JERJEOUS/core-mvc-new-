using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace towing_services.Models
{
    public class OtpViewModel
    {
        public int RemainingAttempts { get; set; }
        public double RemainingTime { get; set; }
        public bool CanResend { get; set; }
    }

}
