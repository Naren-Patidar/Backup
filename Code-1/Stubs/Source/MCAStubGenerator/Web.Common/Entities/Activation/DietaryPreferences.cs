using System;
using System.Runtime.Serialization;

namespace  Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation
{
   
    public class DietaryPreferences
    {
        private string _diabetic;
        private string _vegiterian;
        private string _halal;
        private string _koshar;
        private string _teetotal;
        
        public string IsDiabetic
        {
            get { return _diabetic; }
            set { _diabetic = value; }
        }

        
        public string IsVegiterian
        {
            get { return _vegiterian; }
            set { _vegiterian = value; }
        }

        
        public string IsHalal
        {
            get { return _halal; }
            set { _halal = value; }
        }

        
        public string IsKoshar
        {
            get { return _koshar; }
            set { _koshar = value; }
        }

        
        public string IsTeetotal
        {
            get { return _teetotal; }
            set { _teetotal = value; }
        }
    }
}
