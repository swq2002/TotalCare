namespace Totalcare.Models
{
    public class AdminViewModel
    {
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public ICollection<Beneficiary> BeneficiariesWaiting { get; set; } = new List<Beneficiary>();
        public ICollection<Beneficiary> BeneficiariesRejected { get; set; } = new List<Beneficiary>();
        public ICollection<Beneficiary> BeneficiariesAccepted { get; set; } = new List<Beneficiary>();
        public ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
        public ICollection<Testimonial> TestimonialsWaiting { get; set; } = new List<Testimonial>();
        public ICollection<Testimonial> TestimonialsRejected { get; set; } = new List<Testimonial>();
        public ICollection<Testimonial> TestimonialsAccepted { get; set; } = new List<Testimonial>();

    }
}
