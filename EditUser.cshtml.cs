using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibMan1.Models.DB;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace LibMan1
{
    public class EditUserModel : PageModel
    {
        private readonly LibMan1.Models.DB.LibMan_Context _context;
        public string Username { get; set; }
        [BindProperty]
        public IFormFile Image { set; get; }

        public EditUserModel(LibMan1.Models.DB.LibMan_Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Users Users { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Username = HttpContext.Session.GetString("username");
            if (Username == null)
            {
                return NotFound();
            }

            Users = await _context.Users.FirstOrDefaultAsync(m => m.Email == Username);

            if (Users == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (this.Image != null)
            {
                //Set Key Name
                string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                Users.Photo = ImageName;


                //Get url To Save
                string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Users", ImageName);

                using (var stream = new FileStream(SavePath, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(Users.Email))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./ListBooks");
        }

        private bool UsersExists(string id)
        {
            
            return _context.Users.Any(e => e.Email == Username);
        }
    }
}
