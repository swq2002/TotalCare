namespace Totalcare.Models
{
    public class UserViewModel
    {
        public User User { get; set; }
        public IEnumerable<Subscriptiontype> SubscriptionTypes { get; set; }
        public Subscription Subscription { get; set; }
        public ICollection<Beneficiary> Beneficiaries { get; set; } = new List<Beneficiary>();
        public Beneficiary Beneficiary { get; set; }
        public Testimonial Testimonial { get; set; }


    }

}
