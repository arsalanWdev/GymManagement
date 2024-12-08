namespace GymManagement.Models
{
    public class Trainer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string TrainerType { get; set; } // Personal Trainer/Gym Trainer
        public string ContactNo { get; set; }
        public string Status { get; set; } // Active/Deactive
    }
}
