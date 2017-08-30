namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.Activation
{
    public class CustomerActivationDetails
    {
        public long CustomerId { get; set; }
        public char Activated { get; set; }
        public int CustomerMailStatus { get; set; }
        public int CustomerUseStatus { get; set; }


        
    }

    public class CustomerVerificationDetails
    {
        public string IsBlocked { get; set; }
    }

    //public enum ActivationStatusEnum
    //{
    //    Activated = 0,
    //    ActivationPending =1,
    //    Inactive = 2
    //}
}
