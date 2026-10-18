using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bradea_Pintiuta_Mariabela_Lab2.Models;

namespace Bradea_Pintiuta_Mariabela_Lab2.Pages.AuthorPages;

public class CreateModel : PageModel
{
    private readonly Bradea_Pintiuta_Mariabela_Lab2Context _context;

    public CreateModel(Bradea_Pintiuta_Mariabela_Lab2Context context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Author Author { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Author.Add(Author);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
