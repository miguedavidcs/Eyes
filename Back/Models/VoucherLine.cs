namespace Back.Models
{
    public class VoucherLine : BaseEntity
    {
        public Guid VoucherId { get; set; }
        public Voucher Voucher { get; set; } = null!;

        public Guid AccountId { get; set; }
        public Account Account { get; set; } = null!;

        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}