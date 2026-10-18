using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bradea_Pintiuta_Mariabela_Lab2.Models;

namespace Bradea_Pintiuta_Mariabela_Lab2.Pages.BookPages;

public class IndexModel : PageModel
{
    private readonly Bradea_Pintiuta_Mariabela_Lab2Context _context;

    public IndexModel(Bradea_Pintiuta_Mariabela_Lab2Context context)
    {
        _context = context;
    }

    public IList<Book> Book { get; set; } = default!;

    public SelectList Authors { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public int? AuthorID { get; set; }

    public async Task OnGetAsync()
    {
        Authors = new SelectList(
            await _context.Author.ToListAsync(),
            "ID",
            "FirstName",
            AuthorID);

        var books = _context.Book
            .Include(b => b.Publisher)
            .Include(b => b.Author)
            .AsQueryable();

        if (AuthorID.HasValue)
        {
            books = books.Where(b => b.AuthorID == AuthorID);
        }

        Book = await books.ToListAsync();
    }
}