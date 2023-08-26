using MudBlazor.Services;
using tcgct_mtg.Services;
using tcgct_services_interfaces.MTG;

namespace tcgct_mud
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddMudServices();
            //builder.Services.AddScoped<IMTGService, MTGSqlService>();
            builder.Services.AddScoped<IMTGService>(di => new MTGSqlService(builder.Configuration.GetSection("ConnectionStrings")["MainDB"]));
            //builder.Configuration
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}