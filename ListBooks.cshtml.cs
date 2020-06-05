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
    public class ListBooksModel : PageModel
    {
       
            private readonly LibMan1.Models.DB.LibMan_Context _context;
             public string Username { get; set; }
             public ListBooksModel(LibMan1.Models.DB.LibMan_Context context)
            {
                _context = context;
            }

           [BindProperty(SupportsGet = true)]
            public string SearchString { get; set; }
            public IList<Books> Books { get; set; }
            public IList<Books> Paths { get; set; }
            public async Task OnGetAsync()
            {
            Username = HttpContext.Session.GetString("username");
            var books = from b in _context.Books
                         select b;
            if (!string.IsNullOrEmpty(SearchString))
            {
                books = books.Where(s => s.BookTitle.Contains(SearchString));
            }
            Books = await books.Where(a => a.UserMail == Username).ToListAsync();
                

        }
        
    }
}