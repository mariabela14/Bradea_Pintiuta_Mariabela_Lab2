using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bradea_Pintiuta_Mariabela_Lab2.Models;

namespace Bradea_Pintiuta_Mariabela_Lab2.Pages.AuthorPages;

public class DeleteModel : PageModel
{
    private readonly Bradea_Pintiuta_Mariabela_Lab2Context _context;

    public DeleteModel(Bradea_Pintiuta_Mariabela_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Author Author { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var author = await _context.Author.FirstOrDefaultAsync(m => m.ID == id);
        if (author is null)
        {
            return NotFound();
        }
        else
        {
            Author = author;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var author = await _context.Author.FindAsync(id);
        if (author != null)
        {
            Author = author;
            _context.Author.Remove(Author);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
