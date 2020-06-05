using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibMan1.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LibMan1
{
    public class SignUpModel : PageModel
    {
        private readonly LibMan1.Models.DB.LibMan_Context _context;
        [BindProperty]
        public IFormFile Image { set; get; }
        public SignUpModel(LibMan1.Models.DB.LibMan_Context context)
        {
            _context = context;
        }

        public string Msg { get; set; }
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Users Users { get; set; }

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

            try
            {
                _context.Users.Add(Users);
                await _context.SaveChangesAsync();
            }

            catch(DbUpdateException ex)
                  when ((ex.InnerException as SqlException)?.Number == 2627)
            {
                return RedirectToPage("./SignUp");
            }



            
            return RedirectToPage("./Index");
        }
    }
}
