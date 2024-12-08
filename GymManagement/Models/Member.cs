using System.ComponentModel.DataAnnotations;

namespace GymManagement.Models
{
    public class Member
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; } // Male/Female
        public string Contact { get; set; }
        public string MemberStatus { get; set; } // Active/Deactive
        public string DayTiming { get; set; } // Morning/Evening
        public DateTime AccountOpenDate { get; set; }
        public int PackageId { get; set; } // Foreign Key
        public Package Package { get; set; }
        public DateTime? PackageExpiryDate { get; set; }
        public int? TrainerId { get; set; } // Foreign Key
        public Trainer Trainer { get; set; }
        public decimal AdmissionFee { get; set; }
        public decimal MonthlyFee { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Account { get; set; } // Payment Method
        public string MemberImage { get; set; }
    }
}
