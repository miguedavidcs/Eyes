using back.Models;
using Microsoft.EntityFrameworkCore;

namespace back.Data.Seed
{
    public static class CompanyUserSeed
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            var company = await context.Company.FirstOrDefaultAsync();
            if (company == null)
                return;
            var user = await context.Users.FirstOrDefaultAsync();
            if (user == null)
             return;
            var exists_relation = await context.CompanyUsers
                .AnyAsync(cu => cu.CompanyId == company.Id &&
                 cu.UserId == user.Id);
            if (!exists_relation)
            {
                var companyUser = new CompanyUser
                {
                    CompanyId = company.Id,
                    UserId = user.Id,
                    Role = ECompanyUserRole.Admin
                };
                context.CompanyUsers.Add(companyUser);
                await context.SaveChangesAsync();
            }
        }
    }
}