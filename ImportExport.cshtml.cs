using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibMan1.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace LibMan1
{
    public class ImportExportModel : PageModel
    {

        private readonly LibMan1.Models.DB.LibMan_Context _context;
        public string Username { get; set; }
        public ImportExportModel(LibMan1.Models.DB.LibMan_Context context)
        {
            _context = context;

        }
        public IFormFile JsonFile { set; get; }
        public string SavePath { set; get; }
        public IList <Books> ImportBooks  { get; set; }
        public IList<Books> Books { get; set; }


        public async Task OnGetAsync()
        {
            
           
        }

        public IActionResult OnPostImport()
        {
            if (JsonFile != null) {
                string JsonName = Guid.NewGuid().ToString() + Path.GetExtension(JsonFile.FileName);
                SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImportJson", JsonFile.FileName);

                using (var stream = new FileStream(SavePath, FileMode.Create))
                {
                    JsonFile.CopyTo(stream);
                }

           
            }
            using (StreamReader r = new StreamReader(SavePath))
            {
                string json = r.ReadToEnd();
                JObject o = JObject.Parse(json);
                JArray a = (JArray)o["d"];
                ImportBooks = a.ToObject<IList<Books>>();
            }

            Books AddedBook = new Books();

            foreach (var item in ImportBooks)
            {
                AddedBook.BookTitle = item.BookTitle;
                AddedBook.BookAuthor = item.BookAuthor;
                AddedBook.BookTranslator = item.BookTranslator;
                AddedBook.BookPublisher = item.BookPublisher;
                AddedBook.BookDescription = item.BookDescription;
                AddedBook.BookCategory = item.BookCategory;
                AddedBook.BookCover = item.BookCover;
                AddedBook.ReadingStatus = item.ReadingStatus;
                AddedBook.UserMail = item.UserMail;
                _context.Books.Add(AddedBook);
                _context.SaveChanges();
            }

            return RedirectToPage("./ListBooks");
        }

        public async Task <IActionResult> OnPostExport()
        {
            var books = from b in _context.Books
                        select b;
            Username = HttpContext.Session.GetString("username");

            Books = await books.Where(a => a.UserMail == Username).ToListAsync();
            Books AddedBook = new Books();
            JsonSerializer serializer = new JsonSerializer();
            int count = Books.Count();
            Books[] JsonBooks = new Books[count];
            int i = 0;
            foreach (var item in Books)
            {
                AddedBook = item;
              
                JsonBooks[i] = AddedBook;
                i++;
            }
            string JsonName = Guid.NewGuid().ToString() + Path.GetExtension(JsonFile.FileName);
            SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ExportJson", Username + ".json");

            using (var stream = new FileStream(SavePath, FileMode.Create))
            {
                JsonFile.CopyTo(stream);
            }

           // string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ExportJson", "export.json");

            using (StreamWriter sw = new StreamWriter(SavePath))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    for ( i = 0; i < JsonBooks.Length; i++)
                    {
                        serializer.Serialize(writer, JsonBooks[i]);
                    }

                }
            }
            return Page();
        }

    }
    
}