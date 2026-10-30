using Bradea_Pintiuta_Mariabela_Lab2.Models;

namespace Bradea_Pintiuta_Mariabela_Lab2.Models.ViewModels;

public class CategoryIndexData
{
    public IEnumerable<Category> Categories { get; set; } = new List<Category>();

    public IEnumerable<Book> Books { get; set; } = new List<Book>();
}