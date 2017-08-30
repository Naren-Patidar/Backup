using System;
using System.Collections;
using System.Runtime.Serialization;

namespace  Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation
{
    public class Household 
    {
        private int _totalPeople;
        private int[] _peopleAges;
        
        public int TotalPeople { get { return _totalPeople; } set { _totalPeople = value; } }

        public int[] PeopleAges { get { return _peopleAges; } set { _peopleAges = value; } }
    }
}
