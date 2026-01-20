namespace Back.Models
{
    public class CompanyUser : BaseEntity
    {
        public Guid CompanyId { get; set; }
        public Company? Company { get; set; } = null!;

        //Relacion con User
        public Guid UserId { get; set; }
        
    }
}