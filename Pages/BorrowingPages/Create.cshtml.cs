using Bradea_Pintiuta_Mariabela_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bradea_Pintiuta_Mariabela_Lab2.Pages.BorrowingPages;

public class CreateModel : PageModel
{
    private readonly Bradea_Pintiuta_Mariabela_Lab2Context _context;

    public CreateModel(Bradea_Pintiuta_Mariabela_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Borrowing Borrowing { get; set; } = new();

    public async Task OnGetAsync()
    {
        await LoadListsAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadListsAsync();
            return Page();
        }

        _context.Borrowing.Add(Borrowing);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    private async Task LoadListsAsync()
    {
        var bookList = await _context.Book
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
            bookList, "ID", "BookFullName");

        var memberList = await _context.Member
            .Select(member => new
            {
                member.ID,
                member.FullName
            })
            .ToListAsync();

        ViewData["MemberID"] = new SelectList(
            memberList, "ID", "FullName");
    }
}