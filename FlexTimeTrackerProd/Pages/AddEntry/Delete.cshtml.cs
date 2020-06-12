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
    public class DeleteModel : PageModel
    {
        private readonly FlexTimeTrackerProd.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteModel(FlexTimeTrackerProd.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;           
            _userManager = userManager;
        }

        [BindProperty]
        public AddEntry AddEntry { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AddEntry = await _context.AddEntry.FirstOrDefaultAsync(m => m.ID == id);

            if (AddEntry == null)
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

            AddEntry = await _context.AddEntry.FindAsync(id);

            //Updates minutes in UserMinutes
            var userMin = _context.UserMinutes.Find(AddEntry.UserID);
            userMin.Minutes -= AddEntry.Minutes;
            _context.UserMinutes.Update(userMin);

            //Generate Eventlog
            var user = await _userManager.GetUserAsync(User);
            var eventLog = "Removed " + AddEntry.Minutes + " Minutes";
            EventLog log = new EventLog { Date = DateTime.Now, UserName = user.UserName, Event = eventLog };
            _context.EventLog.Add(log);

            if (AddEntry != null)
            {
                _context.AddEntry.Remove(AddEntry);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Index");
        }
    }
}
