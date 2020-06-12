using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FlexTimeTrackerProd.Models;
using Microsoft.AspNetCore.Authorization;
using FlexTimeTrackerProd.Data;
using Microsoft.AspNetCore.Identity;
using System.Web;



namespace FlexTimeTrackerProd
{
    
    [Authorize(Roles = "Admin")]
    public class OverviewModel : PageModel
    {
        private readonly FlexTimeTrackerProd.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OverviewModel(FlexTimeTrackerProd.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<AddEntry> AddEntry { get; set; }
        public IList<UseEntry> UseEntry { get; set; }
        public IList<UserMinutes> UserMinutes { get; set; }        
      

        public async Task OnGetAsync()
        {
            
            AddEntry = await _context.AddEntry.ToListAsync();
            UseEntry = await _context.UseEntry.ToListAsync();
            UserMinutes = await _context.UserMinutes.ToListAsync();
           
           




        }
       
        


    }
}
