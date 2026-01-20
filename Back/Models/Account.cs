using Back.Enum.Accounting;
using Back.Models;
namespace Back.Models
{
    public class Account:BaseEntity
    {
        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        public string Code { get; set; } = null!;
        
        public EAccountType Type { get; set; }

        public Guid? ParentId { get; set; }
        public Account? Parent { get; set; }

        public bool IsPostable { get; set; }
    }
}