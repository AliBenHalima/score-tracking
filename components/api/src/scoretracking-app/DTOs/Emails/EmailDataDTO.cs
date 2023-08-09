using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.DTOs.Emails
{
    public class EmailDataDTO
    {
        public string ReceiverName { get; set; }
        public string ReceiverAddress { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
    }
}
