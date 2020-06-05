using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LibMan1.Models.DB;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace LibMan1
{
    public class CreateModel : PageModel
    {
        private readonly LibMan1.Models.DB.LibMan_Context _context;
        public string Username { get; set; }

        [BindProperty]
        public IFormFile Image { set; get; }
      
        public CreateModel(LibMan1.Models.DB.LibMan_Context context)
        {
            _context = context;

        }

        public IActionResult OnGet()
        {
            Username = HttpContext.Session.GetString("username");
            return Page();
        }

        [BindProperty]
        public Books Books { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        private string GetUniqueName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_" + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (this.Image != null)
            {
                //Set Key Name
                string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                Books.BookCover = ImageName;


                //Get url To Save
                string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Books", ImageName);

                using (var stream = new FileStream(SavePath, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }
            }


            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Books.Add(Books);
            
            await _context.SaveChangesAsync();



            

            return RedirectToPage("./ListBooks");
        }

       
    }
}
