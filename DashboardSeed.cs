using Microsoft.AspNetCore.Identity;

namespace IdentityApp;

public static class DashboardSeed
{
    public static void SeedUserStoreForDashboard(this IApplicationBuilder app)
    {
        SeedStore(app).GetAwaiter().GetResult();
    }

    private async static Task SeedStore(IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var config = scope.ServiceProvider.GetService<IConfiguration>();
            var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

            var roleName = config["Dashboard:Role"] ?? "Dashboard";
            var userName = config["Dashboard:User"] ?? "mail@josealonso.dev";
            var password = config["Dashboard:Password"] ?? "mysecret";

            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            var dashboardUser = await userManager.FindByEmailAsync(userName);
            if (dashboardUser == null)
            {
                dashboardUser = new IdentityUser
                {
                    UserName = userName,
                    Email = userName,
                    EmailConfirmed = true,
                };
                await userManager.CreateAsync(dashboardUser);
                dashboardUser = await userManager.FindByEmailAsync(userName);
                await userManager.AddPasswordAsync(dashboardUser, password);
            }
            if (!await userManager.IsInRoleAsync(dashboardUser, roleName))
            {
                await userManager.AddToRoleAsync(dashboardUser, roleName);
            }
        }
    }
}
