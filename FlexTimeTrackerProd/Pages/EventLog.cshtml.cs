using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FlexTimeTrackerProd.Data;
using FlexTimeTrackerProd.Models;
using Microsoft.AspNetCore.Authorization;

namespace FlexTimeTrackerProd
{
    [Authorize(Roles = "Admin")]
    public class EventLogModel : PageModel
    {
        private readonly FlexTimeTrackerProd.Data.ApplicationDbContext _context;

        public EventLogModel(FlexTimeTrackerProd.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<EventLog> EventLog { get;set; }

        public async Task OnGetAsync()
        {
            EventLog = await _context.EventLog.ToListAsync();
        }
    }
}
