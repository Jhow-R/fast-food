using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFood.Data
{
    public static class SeedData
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider,
           IConfiguration Configuration)
        {
            // Inclusão de perfis customizados
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Perfis catalogados em um array de strings
            string[] roleNames = { "Admin", "Member" };
            foreach (var roleName in roleNames)
            {
                IdentityResult roleResult;

                // Cria os perfis caso não existam no banco
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // Cria o super usuário que pode manter a aplicação web
            var poweruser = new IdentityUser
            {
                // Obtêm os dados de appsettings.json
                UserName = Configuration.GetSection("UserSettings")["UserName"],
                Email = Configuration.GetSection("UserSettings")["UserEmail"]
            };

            // Obtém a senha do arquivo de configuração
            string userPassword = Configuration.GetSection("UserSettings")["UserPassword"];

            // Verifica se existe um usuário com o email informado
            var user = await userManager.FindByEmailAsync(poweruser.Email);

            if (user is null)
            {
                // Cria o super usuário com os dados informados
                var createPowerUser = await userManager.CreateAsync(poweruser, userPassword);
                if (createPowerUser.Succeeded)
                    // Atribui o perfil Admin ao usuário 
                    await userManager.AddToRoleAsync(poweruser, roleNames[0]);
            }
        }
    }
}
