using Bradea_Pintiuta_Mariabela_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bradea_Pintiuta_Mariabela_Lab2.Pages.BorrowingPages;

public class EditModel : PageModel
{
    private readonly Bradea_Pintiuta_Mariabela_Lab2Context _context;

    public EditModel(Bradea_Pintiuta_Mariabela_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Borrowing Borrowing { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
            return NotFound();

        var borrowing = await _context.Borrowing
            .FirstOrDefaultAsync(item => item.ID == id);

        if (borrowing is null)
            return NotFound();

        Borrowing = borrowing;
        await LoadListsAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadListsAsync();
            return Page();
        }

        _context.Attach(Borrowing).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Borrowing.AnyAsync(item => item.ID == Borrowing.ID))
                return NotFound();

            throw;
        }

        return RedirectToPage("./Index");
    }

    private async Task LoadListsAsync()
    {
        var members = await _context.Member
            .Select(member => new
            {
                member.ID,
                member.FullName
            })
            .ToListAsync();

        ViewData["MemberID"] = new SelectList(
            members, "ID", "FullName", Borrowing.MemberID);

        var books = await _context.Book
            .Include(book => book.Author)
            .Select(book => new
            {
                book.ID,
                BookFullName = book.Title + " - " +
                    (book.Author == null
                        ? "Autor necunoscut"
                        : book.Author.LastName + " " + book.Author.FirstName)
            })
            .ToListAsync();

        ViewData["BookID"] = new SelectList(
            books, "ID", "BookFullName", Borrowing.BookID);
    }
}