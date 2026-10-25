using Bradea_Pintiuta_Mariabela_Lab2.Models;
using Microsoft.EntityFrameworkCore;

public class Bradea_Pintiuta_Mariabela_Lab2Context(DbContextOptions<Bradea_Pintiuta_Mariabela_Lab2Context> options) : DbContext(options)
{
    public DbSet<Bradea_Pintiuta_Mariabela_Lab2.Models.Book> Book { get; set; } = default!;

    public DbSet<Bradea_Pintiuta_Mariabela_Lab2.Models.Publisher> Publisher { get; set; } = default!;

    public DbSet<Bradea_Pintiuta_Mariabela_Lab2.Models.Author> Author { get; set; } = default!;
    public DbSet<Category> Category { get; set; } = default!;
}