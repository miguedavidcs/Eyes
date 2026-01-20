using Back.Models;
using Back.Enum.Accounting;
using Microsoft.EntityFrameworkCore;

namespace Back.Data.Seed
{
    public static class AccountingSeed
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // =========================
            // 1. Empresa
            // =========================
            if (!await context.Companies.AnyAsync())
            {
                var company = new Company
                {
                    Nit = "900000000-1",
                    BusinessName = "Empresa Demo",
                    Address = "Dirección Demo",
                    Phone = "3000000000",
                    Email = "demo@empresa.com",
                    LogoUrl = ""
                };

                context.Companies.Add(company);
                await context.SaveChangesAsync();

                // =========================
                // 2. Período contable
                // =========================
                var period = new AccountingPeriod
                {
                    CompanyId = company.Id,
                    StartDate = new DateTime(DateTime.UtcNow.Year, 1, 1),
                    EndDate = new DateTime(DateTime.UtcNow.Year, 12, 31),
                    Status = EAccountingPeriodStatus.Open
                };

                context.AccountingPeriods.Add(period);

                // =========================
                // 3. Plan de cuentas básico
                // =========================
                var accounts = new List<Account>
                {
                    new Account { CompanyId = company.Id, Code = "1", Name = "ACTIVOS", Type = EAccountType.Asset, IsPostable = false },
                    new Account { CompanyId = company.Id, Code = "11", Name = "CAJA", Type = EAccountType.Asset, IsPostable = true },

                    new Account { CompanyId = company.Id, Code = "2", Name = "PASIVOS", Type = EAccountType.Liability, IsPostable = false },
                    new Account { CompanyId = company.Id, Code = "21", Name = "CUENTAS POR PAGAR", Type = EAccountType.Liability, IsPostable = true },

                    new Account { CompanyId = company.Id, Code = "3", Name = "PATRIMONIO", Type = EAccountType.Equity, IsPostable = false },
                    new Account { CompanyId = company.Id, Code = "31", Name = "CAPITAL", Type = EAccountType.Equity, IsPostable = true },

                    new Account { CompanyId = company.Id, Code = "4", Name = "INGRESOS", Type = EAccountType.Income, IsPostable = false },
                    new Account { CompanyId = company.Id, Code = "41", Name = "INGRESOS OPERACIONALES", Type = EAccountType.Income, IsPostable = true },

                    new Account { CompanyId = company.Id, Code = "5", Name = "GASTOS", Type = EAccountType.Expense, IsPostable = false },
                    new Account { CompanyId = company.Id, Code = "51", Name = "GASTOS OPERATIVOS", Type = EAccountType.Expense, IsPostable = true }
                };

                context.Accounts.AddRange(accounts);

                await context.SaveChangesAsync();
            }
        }
    }
}
