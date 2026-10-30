using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bradea_Pintiuta_Mariabela_Lab2.Models;
using Bradea_Pintiuta_Mariabela_Lab2.Models.ViewModels;

namespace Bradea_Pintiuta_Mariabela_Lab2.Pages.CategoryPages;

public class IndexModel : PageModel
{
    private readonly Bradea_Pintiuta_Mariabela_Lab2Context _context;

    public IndexModel(Bradea_Pintiuta_Mariabela_Lab2Context context)
    {
        _context = context;
    }

    public IList<Category> Category { get; set; } = new List<Category>();

    public CategoryIndexData CategoryData { get; set; } = new CategoryIndexData();

    public int CategoryID { get; set; }

    public async Task OnGetAsync(int? id)
    {
        CategoryData = new CategoryIndexData();

        CategoryData.Categories = await _context.Category
            .Include(i => i.BookCategories)
            .ThenInclude(bc => bc.Book)
            .ThenInclude(b => b.Author)
            .OrderBy(i => i.CategoryName)
            .ToListAsync();

        Category = CategoryData.Categories.ToList();

        if (id != null)
        {
            CategoryID = id.Value;

            Category category = CategoryData.Categories
                .Where(i => i.ID == id.Value)
                .Single();

            CategoryData.Books = category.BookCategories
                .Select(s => s.Book);
        }
    }
}