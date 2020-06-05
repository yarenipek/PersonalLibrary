using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibMan1.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LibMan1
{
    public class BookIndexModel : PageModel
    {
        private readonly LibMan1.Models.DB.LibMan_Context _context;
        public string Username { get; set; }
        public BookIndexModel(LibMan1.Models.DB.LibMan_Context context)
        {
            _context = context;
        }

        public IList<Books> Books { get; set; }

        
        public async Task OnGetAsync()
        {
            Books = await _context.Books.ToListAsync();

            Username = HttpContext.Session.GetString("username");
        }

        
    }
}