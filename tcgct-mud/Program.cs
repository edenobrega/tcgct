using MudBlazor.Services;
using tcgct_mtg.Services;
using tcgct_services_framework.MTG;
using Microsoft.AspNetCore.Components.Authorization;
using tcgct_mud.Areas.Identity;
using Microsoft.AspNetCore.Identity;
using tcgct_services_framework.Identity;
using tcgct_services_framework.Generic;

namespace tcgct_mud
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetSection("ConnectionStrings")["MainDB"];

            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("No connection string found.");
            }

            var backendTech = builder.Configuration["BackendTech"];

            if(string.IsNullOrEmpty(backendTech) || string.IsNullOrWhiteSpace(backendTech))
            {
                throw new Exception("No backend technology defined.");
            }

            var validImps = IdentityHelper.GetValidImplementations();

            if (!validImps.Contains(backendTech))
            {
                throw new Exception($"Chosen implementation is not valid: {backendTech}");
            }

            builder.Services.AddScoped(di => new SettingsService(connectionString));

            var identityClasses = IdentityHelper.GetClasses(backendTech);

            builder.Services.AddDefaultIdentity<TCGCTUser>();

            builder.Services.AddTransient(typeof(IUserStore<TCGCTUser>), identityClasses.UserStore);
            builder.Services.AddTransient(typeof(IRoleStore<TCGCTRole>), identityClasses.RoleStore);

            builder.Services.AddTransient(identityClasses.DataAccess);

            builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<TCGCTUser>>();

            // End of identity services
            // todo: do above to here.
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