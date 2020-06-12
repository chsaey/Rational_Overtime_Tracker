using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlexTimeTrackerProd.Models
{
    public class UseEntry
    {
        public int ID { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public int Minutes { get; set; }
        public string Notes { get; set; }
        public string UserID { get; set; }
    }

}

