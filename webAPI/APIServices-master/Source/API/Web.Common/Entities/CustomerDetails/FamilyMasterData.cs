using System;
using System.Collections.Generic;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities
{
    public class FamilyMasterData
    {
        public int SeqNo { get; set; }
        public string DateOfBirth { get; set; }
        public int NumberOfHouseholdMembers { get; set; }

        public static DateTime GetDateOfBirth(short age)
        {
            int presentYear = DateTime.Now.Year;

            //Considering employee has born on 1 January
            DateTime dob = DateTime.Parse((presentYear - age) + "/1/1");

            return dob;
        }
    }
}
