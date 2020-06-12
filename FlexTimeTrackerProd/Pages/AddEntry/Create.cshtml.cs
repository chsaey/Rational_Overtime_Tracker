using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FlexTimeTrackerProd.Data;
using FlexTimeTrackerProd.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FlexTimeTrackerProd
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly FlexTimeTrackerProd.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public CreateModel(FlexTimeTrackerProd.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public AddEntry AddEntry { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }            
            AddEntry.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //Updates minutes in UserMinutes
            var userMin = _context.UserMinutes.Find(AddEntry.UserID);
            userMin.Minutes += AddEntry.Minutes;
            _context.UserMinutes.Update(userMin);
            _context.AddEntry.Add(AddEntry);

            //Generate Eventlog
            var user = await _userManager.GetUserAsync(User);            
            var eventLog = "Added " + AddEntry.Minutes + " Minutes";
            EventLog log  = new EventLog {Date = DateTime.Now, UserName = user.UserName, Event = eventLog };
            _context.EventLog.Add(log);


            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }

    }
}