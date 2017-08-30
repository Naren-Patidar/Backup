namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.CustomerDetails
{
   public class BTClubPreference
    {
       public short PreferenceID { get; set; }
       public short PreferenceType { get; set; }
       public string PreferenceDescLocal { get; set; }
       public string PreferenceDescEnglish { get; set; }
       public bool OptedStatus { get; set; }        
    }
}
