using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using Task_Management.Domain;
using Task_Management_Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TaskDbContext>(options =>
                                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseSeeding((context, _) =>
        {
            var testBlog = context.Set<User>().FirstOrDefault(b => b.email == "mainmanger@gmail.com");
            if (testBlog == null)
            {
                context.Set<User>().Add(new User {
                    username = "Main Manager",
                   
                    email = "mainmanger@gmail.com",
                    password = "MainManager@3",
                    role = Enums.Manager,
                    mobile_number = "0777777777"


                });
                context.SaveChanges();
            }
        })
        .UseAsyncSeeding(async (context, _, cancellationToken) =>
        {
            var testBlog = await context.Set<User>().FirstOrDefaultAsync(b => b.email == "mainmanger@gmail.com", cancellationToken);
            if (testBlog == null)
            {
                context.Set<User>().Add(new User {
                    username = "Main Manager",
                  
                    email = "mainmanger@gmail.com",
                    password = "MainManager@3",
                    role = Enums.Manager,
                    mobile_number = "0777777777"
                });
                await context.SaveChangesAsync(cancellationToken);
            }
        }));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
