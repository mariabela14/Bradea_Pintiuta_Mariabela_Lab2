using Bradea_Pintiuta_Mariabela_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bradea_Pintiuta_Mariabela_Lab2.Pages.BookPages;

public class IndexModel : PageModel
{
    private readonly Bradea_Pintiuta_Mariabela_Lab2Context _context;

    public IndexModel(Bradea_Pintiuta_Mariabela_Lab2Context context)
    {
        _context = context;
    }

    public IList<Book> Book { get; set; } = new List<Book>();

    public BookData BookD { get; set; } = new BookData();

    public int BookID { get; set; }

    public int CategoryID { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? AuthorID { get; set; }

    public SelectList Authors { get; set; } = default!;

    public async Task OnGetAsync(int? id, int? categoryID)
    {
        var authorList = _context.Author.Select(x => new
        {
            x.ID,
            FullName = x.LastName + " " + x.FirstName
        });

        Authors = new SelectList(
            authorList,
            "ID",
            "FullName",
            AuthorID);

        BookD = new BookData();

        var booksQuery = _context.Book
            .Include(b => b.Author)
            .Include(b => b.Publisher)
            .Include(b => b.BookCategories)
                .ThenInclude(b => b.Category)
            .AsNoTracking()
            .OrderBy(b => b.Title)
            .AsQueryable();

        if (AuthorID.HasValue)
        {
            booksQuery = booksQuery
                .Where(b => b.AuthorID == AuthorID.Value)
                .OrderBy(b => b.Title);
        }

        BookD.Books = await booksQuery.ToListAsync();

        Book = BookD.Books.ToList();

        if (id != null)
        {
            BookID = id.Value;

            Book book = BookD.Books
                .Where(i => i.ID == id.Value)
                .Single();

            BookD.Categories = book.BookCategories
                .Select(s => s.Category);
        }
    }
}