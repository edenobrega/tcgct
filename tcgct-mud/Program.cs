using MudBlazor.Services;
using tcgct_mtg.Services;
using tcgct_services_framework.MTG;
using Microsoft.AspNetCore.Components.Authorization;
using tcgct_mud.Data.Identity;
using tcgct_mud.Areas.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace tcgct_mud
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetSection("ConnectionStrings")["MainDB"];

            builder.Services.AddDefaultIdentity<CustomIdentityUser>();

            builder.Services.AddTransient<IUserStore<CustomIdentityUser>, CustomUserStore>();
            builder.Services.AddTransient<IRoleStore<CustomRole>, CustomRoleStore>();

            builder.Services.AddTransient<SqlConnection>(conn => new SqlConnection(connectionString));
            builder.Services.AddTransient<CustomDataAccess>();
            builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<CustomIdentityUser>>();

            // todo: make this use the above sqlconnection service
            builder.Services.AddScoped<IMTGService>(di => new MTGSqlService(connectionString));

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddMudServices();

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
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}