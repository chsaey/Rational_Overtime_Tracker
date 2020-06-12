using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlexTimeTrackerProd.Models
{
    public class EventLog
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        [Display(Name = "Username")]
        public string UserName { get; set; }
        public string Event { get;set; }
        


    }
}
