using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LibMan1.Models.DB;
using Microsoft.AspNetCore.Http;

namespace LibMan1
{
    public class IndexModel : PageModel
    {
        private readonly LibMan1.Models.DB.LibMan_Context _context;

        public IndexModel(LibMan1.Models.DB.LibMan_Context context)
        {
            _context = context;
        }
        [BindProperty] public string Username { get; set; }
        [BindProperty] public string Password { get; set; }
        public string Msg { get; set; }
        public IList<Users> Users { get; set; }

        public async Task OnGetAsync()
        {
            Users = await _context.Users.ToListAsync();
        }

        private bool CustomersExists(string username, string password)
        {
            bool usern = false, pass = false;
            usern = _context.Users.Any(e => e.Email == username);
            pass = _context.Users.Any(e => e.Password == password);
            if (usern == true && pass == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public IActionResult OnPost()
        {
            if (CustomersExists(Username, Password))
            { //HttpContext.Session.SetString("username", Username); 
                var cust = _context.Users.Single(a => a.Email == Username);
                HttpContext.Session.SetString("username", cust.Email);
                // return RedirectToPage("Welcome"); 
                return RedirectToPage("ListBooks");
            }
        

            else 
            { 
              Msg = "Invalid"; 
              return Page();
             }
        }
    }
}
