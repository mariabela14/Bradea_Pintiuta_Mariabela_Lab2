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

    public string TitleSort { get; set; } = "";

    public string AuthorSort { get; set; } = "";

    public string CurrentFilter { get; set; } = "";

    public SelectList Authors { get; set; } = default!;

    public async Task OnGetAsync(
        int? id,
        int? categoryID,
        string sortOrder,
        string searchString)
    {
        // Lista autorilor pentru filtrare
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

        // Codul pentru sortare - conform laboratorului
        TitleSort = String.IsNullOrEmpty(sortOrder)
            ? "title_desc"
            : "";

        AuthorSort = sortOrder == "author"
            ? "author_desc"
            : "author";

        // Retinem textul cautat
        CurrentFilter = searchString;

        // Afisam cartile impreuna cu Autorul, Publisher-ul si Categoriile
        BookD.Books = await _context.Book
            .Include(b => b.Author)
            .Include(b => b.Publisher)
            .Include(b => b.BookCategories)
            .ThenInclude(b => b.Category)
            .AsNoTracking()
            .OrderBy(b => b.Title)
            .ToListAsync();

        // Filtrare dupa autorul selectat
        if (AuthorID.HasValue)
        {
            BookD.Books = BookD.Books
                .Where(b => b.AuthorID == AuthorID.Value);
        }

        // Cautare dupa titlul cartii sau numele autorului
        if (!String.IsNullOrEmpty(searchString))
        {
            BookD.Books = BookD.Books
                .Where(s => s.Author.FirstName.Contains(searchString)
                    || s.Author.LastName.Contains(searchString)
                    || s.Title.Contains(searchString));
        }

        // Selectarea unei carti
        if (id != null)
        {
            BookID = id.Value;

            Book book = BookD.Books
                .Where(i => i.ID == id.Value)
                .Single();

            BookD.Categories = book.BookCategories
                .Select(s => s.Category);
        }

        // Sortarea cartilor
        switch (sortOrder)
        {
            case "title_desc":
                BookD.Books = BookD.Books
                    .OrderByDescending(s => s.Title);
                break;

            case "author_desc":
                BookD.Books = BookD.Books
                    .OrderByDescending(s => s.Author.FullName);
                break;

            case "author":
                BookD.Books = BookD.Books
                    .OrderBy(s => s.Author.FullName);
                break;

            default:
                BookD.Books = BookD.Books
                    .OrderBy(s => s.Title);
                break;
        }

        Book = BookD.Books.ToList();
    }
}