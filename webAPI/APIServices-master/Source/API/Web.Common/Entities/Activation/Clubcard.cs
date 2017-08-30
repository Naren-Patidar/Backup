using System;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.Activation
{
    /// <summary>
    /// <para>Clubcard Entity</para>
    /// <para>Purpose: To store clubcard information</para>
    /// <para>Author: Akash Jain</para>
    /// <para>Date: 15/04/2010</para>
    /// <para>Copyrights (C) 2010, Tesco HSC Pte Ltd,81-82, EPIP Area, WhiteFiled, Bangalore-66</para>
    /// </summary>
   
    public class Clubcard 
    {        
        private string _clubcardNumber;
        private DateTime _startDate;
        private DateTime _expiryDate;
        private bool _primary;

        [DisplayName("Activation_ClubcardNumber")]
        public string ClubcardNumber { get { return _clubcardNumber; } set { _clubcardNumber = value; } }
        
        public DateTime StartDate { get { return _startDate; } set { _startDate = value; } }
        
        public DateTime ExpiryDate { get { return _expiryDate; } set { _expiryDate = value; } }

        public bool Primary { get { return _primary; } set { _primary = value; } }
    }
}
