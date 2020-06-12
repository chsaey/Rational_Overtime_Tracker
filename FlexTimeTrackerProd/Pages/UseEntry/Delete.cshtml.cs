using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FlexTimeTrackerProd.Data;
using FlexTimeTrackerProd.Models;
using Microsoft.AspNetCore.Identity;

namespace FlexTimeTrackerProd
{
    public class DeleteUseModel : PageModel
    {
        private readonly FlexTimeTrackerProd.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteUseModel(FlexTimeTrackerProd.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)

        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public UseEntry UseEntry { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UseEntry = await _context.UseEntry.FirstOrDefaultAsync(m => m.ID == id);

            if (UseEntry == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UseEntry = await _context.UseEntry.FindAsync(id);

            //Updates minutes in UserMinutes
            var userMin = _context.UserMinutes.Find(UseEntry.UserID);
            userMin.Minutes += UseEntry.Minutes;
            _context.UserMinutes.Update(userMin);

            //Generate Eventlog
            var user = await _userManager.GetUserAsync(User);
            var eventLog = "Unused " + UseEntry.Minutes + " Minutes";
            EventLog log = new EventLog { Date = DateTime.Now, UserName = user.UserName, Event = eventLog };
            _context.EventLog.Add(log);

            if (UseEntry != null)
            {
                _context.UseEntry.Remove(UseEntry);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Index");
        }
    }
}
