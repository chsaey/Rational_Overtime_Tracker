
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlexTimeTrackerProd.Models
{
    public class UserMinutes
    {
        [Key]
        public string ApplicationUserID { get; set; }
        public int Minutes { get; set; }

       
     
        
    }
}
