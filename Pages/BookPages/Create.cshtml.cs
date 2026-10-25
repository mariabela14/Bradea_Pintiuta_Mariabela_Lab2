using Bradea_Pintiuta_Mariabela_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bradea_Pintiuta_Mariabela_Lab2.Pages.BookPages;

public class CreateModel : BookCategoriesPageModel
{
    private readonly Bradea_Pintiuta_Mariabela_Lab2Context _context;

    public CreateModel(Bradea_Pintiuta_Mariabela_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public IActionResult OnGet()
    {
        var authorList = _context.Author.Select(x => new
        {
            x.ID,
            FullName = x.LastName + " " + x.FirstName
        });

        ViewData["AuthorID"] = new SelectList(
            authorList,
            "ID",
            "FullName");

        ViewData["PublisherID"] = new SelectList(
            _context.Publisher,
            "ID",
            "PublisherName");

        var book = new Book();
        book.BookCategories = new List<BookCategory>();

        PopulateAssignedCategoryData(_context, book);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(
        string[] selectedCategories)
    {
        var newBook = new Book();

        if (selectedCategories != null)
        {
            newBook.BookCategories = new List<BookCategory>();

            foreach (var cat in selectedCategories)
            {
                var catToAdd = new BookCategory
                {
                    CategoryID = int.Parse(cat)
                };

                newBook.BookCategories.Add(catToAdd);
            }
        }

        Book.BookCategories = newBook.BookCategories;

        if (!ModelState.IsValid)
        {
            ViewData["AuthorID"] = new SelectList(
                _context.Author,
                "ID",
                "FirstName");

            ViewData["PublisherID"] = new SelectList(
                _context.Publisher,
                "ID",
                "PublisherName");

            PopulateAssignedCategoryData(_context, Book);

            return Page();
        }

        _context.Book.Add(Book);

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}