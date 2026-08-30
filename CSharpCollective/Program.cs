

using DataBase.DataContext;
using Services;
using CSharpCollective.Services;
using Services.Interfaces;


namespace CSharpCollective
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);




            builder.Services.AddDbContext<CollectiveContext>();



            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
         
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<IRegisterService,RegisterService>();
            builder.Services.AddTransient<CommentService>();
            builder.Services.AddTransient<PostService>();
            builder.Services.AddTransient<LogCheck>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();



            builder.Services.AddRazorPages();
            builder.Services.AddControllersWithViews();


            var app = builder.Build();
            app.UseSession();
            var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CollectiveContext>();
            db.Database.EnsureCreated();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
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
                pattern: "{controller=MainPage}/{action=MainPage}")
                .WithStaticAssets();

            app.MapControllerRoute(
               name: "Login",
               pattern: "{controller=Login}/{action=Login}");
            app.MapControllerRoute(
              name: "Register",
              pattern: "{controller=Register}/{action=Register}");
            app.MapControllerRoute(
              name: "Post",
              pattern: "{controller=Post}/{action=Post}");
            app.MapControllerRoute(
              name: "Comment",
              pattern: "{controller=Comment}/{action=Comment}");


            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
