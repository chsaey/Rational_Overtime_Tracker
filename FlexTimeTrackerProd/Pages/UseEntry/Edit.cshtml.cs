using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlexTimeTrackerProd.Data;
using FlexTimeTrackerProd.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace FlexTimeTrackerProd
{
    public class EditUseModel : PageModel
    {
        private readonly FlexTimeTrackerProd.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditUseModel(FlexTimeTrackerProd.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            //Gets Current AddEntry post minutes before update -> need AsNoTracking to resolve tracking concurrency conflict
            var currentUseEntry = _context.UseEntry.AsNoTracking().Where(n => n.ID == UseEntry.ID);
            //Get the users current users UserMinutes Entry
            var userMin = _context.UserMinutes.Find(currentUseEntry.FirstOrDefault().UserID);
            //Set the UserID for update
            UseEntry.UserID = userMin.ApplicationUserID;
            //Get the minutes
            var currentMinutes = currentUseEntry.FirstOrDefault().Minutes;
            //Find the new total minutes
            userMin.Minutes = (userMin.Minutes + currentMinutes) - UseEntry.Minutes;
            //Update minutes
            _context.UserMinutes.Update(userMin);

            //Generate Eventlog
            var user = await _userManager.GetUserAsync(User);
            var eventLog = "Edited a Use Minutes Entry " + currentMinutes + " Minutes to " + UseEntry.Minutes + " Minutes";
            EventLog log = new EventLog { Date = DateTime.Now, UserName = user.UserName, Event = eventLog };
            _context.EventLog.Add(log);



            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(UseEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UseEntryExists(UseEntry.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Index");
        }

        private bool UseEntryExists(int id)
        {
            return _context.UseEntry.Any(e => e.ID == id);
        }
    }
}
