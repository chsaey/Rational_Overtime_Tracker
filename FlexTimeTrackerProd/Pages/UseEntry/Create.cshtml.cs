using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FlexTimeTrackerProd.Data;
using FlexTimeTrackerProd.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace FlexTimeTrackerProd
{
    public class CreateUseModel : PageModel
    {
        private readonly FlexTimeTrackerProd.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public CreateUseModel(FlexTimeTrackerProd.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public UseEntry UseEntry { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            UseEntry.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //Updates minutes in UserMinutes
            var userMin = _context.UserMinutes.Find(UseEntry.UserID);
            userMin.Minutes -= UseEntry.Minutes;
            _context.UserMinutes.Update(userMin);

            _context.UseEntry.Add(UseEntry);

            //Generate Eventlog
            var user = await _userManager.GetUserAsync(User);
            var eventLog = "Used " + UseEntry.Minutes + " Minutes";
            EventLog log = new EventLog { Date = DateTime.Now, UserName = user.UserName, Event = eventLog };
            _context.EventLog.Add(log);
            await _context.SaveChangesAsync();

         

            return RedirectToPage("/Index");
        }
    }
}
