namespace GymManagement.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string PackageName { get; set; }
        public decimal PackagePrice { get; set; }
        public string Gender { get; set; } // Male/Female
    }
}
