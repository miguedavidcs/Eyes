using Back.Enum.Accounting;

namespace Back.Models
{
    public class AccountingPeriod : BaseEntity
    {
        // empresa company
        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public EAccountingPeriodStatus Status { get; set; } = EAccountingPeriodStatus.Open;
        public DateTime? ClosedAt { get; set; }

        public ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
    }
}