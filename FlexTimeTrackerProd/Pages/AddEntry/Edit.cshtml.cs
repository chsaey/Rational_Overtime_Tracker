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
    public class EditModel : PageModel
    {
        private readonly FlexTimeTrackerProd.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
     
    
       


        public EditModel(FlexTimeTrackerProd.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            
        }
        public UseEntry UseEntry { get; set; }

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

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //Gets Current AddEntry post minutes before update -> need AsNoTracking to resolve tracking concurrency conflict
            var currentAddEntry = _context.AddEntry.AsNoTracking().Where(n => n.ID == AddEntry.ID);       
            //Get the users current users UserMinutes Entry
            var userMin = _context.UserMinutes.Find(currentAddEntry.FirstOrDefault().UserID);
            //Set the UserID for update
            AddEntry.UserID = userMin.ApplicationUserID;
            //Get the minutes
            var currentMinutes = currentAddEntry.FirstOrDefault().Minutes;         
            //Find the new total minutes
            userMin.Minutes = (userMin.Minutes - currentMinutes ) + AddEntry.Minutes;
            //Update minutes
            _context.UserMinutes.Update(userMin);

            //Generate Eventlog
            var user = await _userManager.GetUserAsync(User);
            var eventLog = "Changed an Add Minutes entry from" + currentMinutes +" Minutes to "+ AddEntry.Minutes+ " Minutes";
            EventLog log = new EventLog { Date = DateTime.Now, UserName = user.UserName, Event = eventLog };
            _context.EventLog.Add(log);


            if (!ModelState.IsValid)
            {
                return Page();
            }           

            _context.Attach(AddEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddEntryExists(AddEntry.ID))
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

        private bool AddEntryExists(int id)
        {
            return _context.AddEntry.Any(e => e.ID == id);
        }
    }
}
