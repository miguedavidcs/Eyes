using Back.Enum.Voucher;

namespace Back.Models
{
    public class Voucher : BaseEntity
    {
        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        public Guid AccountingPeriodId  { get; set; }
        public AccountingPeriod AccountingPeriod  { get; set; } = null!;

        public EVoucherType VoucherType { get; set; }
        public int Number { get; set; }

        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;

        public EVoucherStatus Status { get; set; } = EVoucherStatus.Draft;

        // usuario creador / aprobador
        public Guid CreatedBy { get; set; }
        public Guid? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }

        public ICollection<VoucherLine> Lines { get; set; } = new List<VoucherLine>();
    }
}