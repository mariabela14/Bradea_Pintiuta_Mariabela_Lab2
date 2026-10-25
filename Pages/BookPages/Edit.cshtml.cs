using Bradea_Pintiuta_Mariabela_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bradea_Pintiuta_Mariabela_Lab2.Pages.BookPages;

public class EditModel : BookCategoriesPageModel
{
    private readonly Bradea_Pintiuta_Mariabela_Lab2Context _context;

    public EditModel(Bradea_Pintiuta_Mariabela_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // se va include Author conform cu sarcina de la lab 2
        Book = await _context.Book
            .Include(b => b.Publisher)
            .Include(b => b.BookCategories)
                .ThenInclude(b => b.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);

        if (Book == null)
        {
            return NotFound();
        }

        // apelam PopulateAssignedCategoryData pentru a obtine
        // informatiile necesare checkbox-urilor
        PopulateAssignedCategoryData(_context, Book);

        var authorList = _context.Author.Select(x => new
        {
            x.ID,
            FullName = x.LastName + " " + x.FirstName
        });

        ViewData["AuthorID"] = new SelectList(
            authorList,
            "ID",
            "FullName",
            Book.AuthorID);

        ViewData["PublisherID"] = new SelectList(
            _context.Publisher,
            "ID",
            "PublisherName",
            Book.PublisherID);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(
        int? id,
        string[] selectedCategories)
    {
        if (id == null)
        {
            return NotFound();
        }

        // se va include Author conform cu sarcina de la lab 2
        var bookToUpdate = await _context.Book
            .Include(i => i.Publisher)
            .Include(i => i.BookCategories)
                .ThenInclude(i => i.Category)
            .FirstOrDefaultAsync(s => s.ID == id);

        if (bookToUpdate == null)
        {
            return NotFound();
        }

        // se va modifica AuthorID conform cu sarcina de la lab 2
        if (await TryUpdateModelAsync<Book>(
            bookToUpdate,
            "Book",
            i => i.Title,
            i => i.AuthorID,
            i => i.Price,
            i => i.PublishingDate,
            i => i.PublisherID))
        {
            UpdateBookCategories(
                _context,
                selectedCategories,
                bookToUpdate);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        // aplicam informatiile din checkboxuri
        UpdateBookCategories(
            _context,
            selectedCategories,
            bookToUpdate);

        PopulateAssignedCategoryData(
            _context,
            bookToUpdate);

        return Page();
    }
}