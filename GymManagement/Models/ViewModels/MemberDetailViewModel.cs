namespace GymManagement.Models.ViewModels
{
    public class MemberDetailViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Contact { get; set; }
        public string MemberStatus { get; set; }
        public string DayTiming { get; set; }
        public DateTime? AccountOpenDate { get; set; }
        public DateTime? PackageExpiryDate { get; set; }
        public decimal AdmissionFee { get; set; }
        public decimal MonthlyFee { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Account { get; set; }
        public string MemberImage { get; set; }
        public string PackageName { get; set; }
        public string TrainerName { get; set; }
    }

}
