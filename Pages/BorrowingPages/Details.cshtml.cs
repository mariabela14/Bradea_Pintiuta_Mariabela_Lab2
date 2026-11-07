using Bradea_Pintiuta_Mariabela_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bradea_Pintiuta_Mariabela_Lab2.Pages.BorrowingPages;

public class DetailsModel : PageModel
{
    private readonly Bradea_Pintiuta_Mariabela_Lab2Context _context;

    public DetailsModel(Bradea_Pintiuta_Mariabela_Lab2Context context)
    {
        _context = context;
    }

    public Borrowing Borrowing { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
            return NotFound();

        var borrowing = await _context.Borrowing
            .Include(item => item.Member)
            .Include(item => item.Book)
                .ThenInclude(book => book!.Author)
            .FirstOrDefaultAsync(item => item.ID == id);

        if (borrowing is null)
            return NotFound();

        Borrowing = borrowing;
        return Page();
    }
}