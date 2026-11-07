using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class LibraryIdentityContext(DbContextOptions<LibraryIdentityContext> options) : IdentityDbContext<Bradea_Pintiuta_Mariabela_Lab2.Data.ApplicationUser>(options)
{
}
