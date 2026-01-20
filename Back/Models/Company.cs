using Back.Enum.Accounting;

namespace Back.Models
{
    public class Company : BaseEntity
    {
        public string Nit { get; set; } = null!;
        public string BusinessName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string LogoUrl { get; set; } = null!;
        public ICollection<AccountingPeriod> Periods { get; set; } = new List<AccountingPeriod>();
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
        public ICollection<CompanyUser> CompanyUsers { get; set; } = new List<CompanyUser>();
    }
}