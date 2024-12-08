using GymManagement.Areas.Identity.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace GymManagement.Models.ViewModels
{
    public class MemberViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [Phone]
        public string Contact { get; set; }

        [Required(ErrorMessage = "Member status is required")]
        public string MemberStatus { get; set; }

        public string DayTiming { get; set; }

        [Required(ErrorMessage = "Admission fee is required")]
        [DataType(DataType.Currency)]
        public decimal AdmissionFee { get; set; }

        [Required(ErrorMessage = "Monthly fee is required")]
        [DataType(DataType.Currency)]
        public decimal MonthlyFee { get; set; }

        public decimal Discount { get; set; }

        [Required(ErrorMessage = "Total amount is required")]
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }

        public string Account { get; set; }

        public DateTime? PackageExpiryDate { get; set; }

        public int PackageId { get; set; }
        public int TrainerId { get; set; }

        public IFormFile? photo { get; set; }
    }
}
