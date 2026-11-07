using Bradea_Pintiuta_Mariabela_Lab2.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString(
    "Bradea_Pintiuta_Mariabela_Lab2Context")
    ?? throw new InvalidOperationException(
        "Connection string 'Bradea_Pintiuta_Mariabela_Lab2Context' not found.");

builder.Services.AddDbContext<Bradea_Pintiuta_Mariabela_Lab2Context>(options =>
    options.UseSqlServer(connectionString));

var identityConnectionString = builder.Configuration.GetConnectionString(
    "LibraryIdentityContext")
    ?? throw new InvalidOperationException(
        "Connection string 'LibraryIdentityContext' not found.");

builder.Services.AddDbContext<LibraryIdentityContext>(options =>
    options.UseSqlServer(identityConnectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<LibraryIdentityContext>();

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();