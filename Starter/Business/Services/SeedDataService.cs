using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Core.Entities;
using Core.Enums;

namespace Business.Services
{
    public static class SeedDataService
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<AuthService>>();

                try
                {
                    // Rolleri oluştur
                    string[] roleNames = { "Admin", "Advisor", "Student" };
                    
                    foreach (var roleName in roleNames)
                    {
                        var roleExist = await roleManager.RoleExistsAsync(roleName);
                        if (!roleExist)
                        {
                            await roleManager.CreateAsync(new IdentityRole<int>(roleName));
                        }
                    }

                    // Default admin kullanıcısı oluştur
                    var adminEmail = "mert.shnm@gmail.com";
                    var adminUser = await userManager.FindByEmailAsync(adminEmail);

                    if (adminUser == null)
                    {
                        adminUser = new User
                        {
                            Email = adminEmail,
                            UserName = adminEmail,
                            FirstName = "Mert",
                            LastName = "Admin",
                            Role = UserRole.Admin,
                            IsApproved = true,
                            EmailConfirmed = true,
                            CreatedDate = DateTime.Now
                        };

                        var result = await userManager.CreateAsync(adminUser, "Merterdem2434.");
                        
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(adminUser, "Admin");
                            logger.LogInformation("Default admin kullanıcısı oluşturuldu: {Email}", adminEmail);
                        }
                        else
                        {
                            logger.LogError("Admin kullanıcısı oluşturulamadı: {Errors}", 
                                string.Join(", ", result.Errors.Select(e => e.Description)));
                        }
                    }
                    else
                    {
                        // Mevcut admin kullanıcısını güncelle
                        bool needUpdate = false;
                        
                        if (!adminUser.IsApproved)
                        {
                            adminUser.IsApproved = true;
                            needUpdate = true;
                        }
                        
                        if (adminUser.FirstName != "Mert" || adminUser.LastName != "Admin")
                        {
                            adminUser.FirstName = "Mert";
                            adminUser.LastName = "Admin";
                            needUpdate = true;
                        }
                        
                        if (needUpdate)
                        {
                            await userManager.UpdateAsync(adminUser);
                            logger.LogInformation("Admin kullanıcısı güncellendi: {Email}", adminEmail);
                        }
                        
                        // Şifre güncellemesi (isteğe bağlı)
                        var passwordToken = await userManager.GeneratePasswordResetTokenAsync(adminUser);
                        await userManager.ResetPasswordAsync(adminUser, passwordToken, "Merterdem2434.");
                        logger.LogInformation("Admin şifresi güncellendi: {Email}", adminEmail);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Seed data oluşturulurken hata oluştu");
                }
            }
        }
    }
} 