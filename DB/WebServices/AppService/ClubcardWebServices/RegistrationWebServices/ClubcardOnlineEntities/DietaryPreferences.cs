using System;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.Entities
{
    /// <summary>
    /// <para>Dietary Information Entity, Associative Entity for Customer</para>
    /// <para>Author: Akash Jainr</para>
    /// <para>Date: 15/02/2010</para>
    /// <para>Copyrights (C) 2010, Tesco HSC ,81-82, EPIP Area, WhiteFiled, Bangalore-66</para>
    /// </summary>
    [Serializable]
    [DataContract]
    public class DietaryPreferences : EntityBase
    {
        private string _diabetic;
        private string _vegiterian;
        private string _halal;
        private string _koshar;
        private string _teetotal;
        
        public static readonly DietaryPreferences Empty = new DietaryPreferences(true);

        private DietaryPreferences(bool isEmpty)
            : base(isEmpty)
        {
        }

        [DataMember]
        public string IsDiabetic
        {
            get { return _diabetic; }
            set { _diabetic = value; }
        }

        [DataMember]
        public string IsVegiterian
        {
            get { return _vegiterian; }
            set { _vegiterian = value; }
        }

        [DataMember]
        public string IsHalal
        {
            get { return _halal; }
            set { _halal = value; }
        }

        [DataMember]
        public string IsKoshar
        {
            get { return _koshar; }
            set { _koshar = value; }
        }

        [DataMember]
        public string IsTeetotal
        {
            get { return _teetotal; }
            set { _teetotal = value; }
        }
    }
}
