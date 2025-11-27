using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCMovie.Data;
using MVCMovie.Models;

namespace MVCMovie.Controllers
{
    public class BooksController : Controller
    {
        private readonly MVCMovieContext _context;
        private readonly IWebHostEnvironment _environment;

        public BooksController(MVCMovieContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create importacion de imagen -------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, IFormFile? ImagenUpload)
        {
            if (ImagenUpload != null)
            {
                string folder = "uploads/books/";
                string filePath = Path.Combine(_environment.WebRootPath, folder);

                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImagenUpload.FileName);
                string fullPath = Path.Combine(filePath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await ImagenUpload.CopyToAsync(stream);
                }

                book.Portada = "/" + folder + fileName;
            }

            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // -------------------------
        // PRESTAR – marca el libro como prestado
        // -------------------------
        public async Task<IActionResult> Prestar(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            book.Prestado = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // -------------------------
        // DEVOLVER – marca como disponible
        // -------------------------
        public async Task<IActionResult> Devolver(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            book.Prestado = false;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
