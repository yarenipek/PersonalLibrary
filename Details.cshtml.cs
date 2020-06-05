using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LibMan1.Models.DB;

namespace LibMan1
{
    public class DetailsModel : PageModel
    {
        private readonly LibMan1.Models.DB.LibMan_Context _context;

        public DetailsModel(LibMan1.Models.DB.LibMan_Context context)
        {
            _context = context;
        }

        public Books Books { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Books = await _context.Books.FirstOrDefaultAsync(m => m.BookId == id);

            if (Books == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
