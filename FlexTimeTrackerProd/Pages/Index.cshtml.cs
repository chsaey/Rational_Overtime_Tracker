using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FlexTimeTrackerProd.Models;
using Microsoft.AspNetCore.Authorization;

namespace FlexTimeTrackerProd
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly FlexTimeTrackerProd.Data.ApplicationDbContext _context;

        public IndexModel(FlexTimeTrackerProd.Data.ApplicationDbContext context)
        {
            _context = context;
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
