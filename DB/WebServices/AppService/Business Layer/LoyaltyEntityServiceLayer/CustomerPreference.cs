/*
 * File   : CustomerPreference.cs
 * Author : Harshal VP (HSC) 
 * email  :
 * File   : This file contains methods/properties related to Customer Alternate ID
 * Date   : 06/Aug/2008
 * 
 */

#region using

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Tesco.NGC.DataAccessLayer;
using Tesco.NGC.Utils;


#endregion

namespace Tesco.NGC.Loyalty.EntityServiceLayer
{
    /// <summary>
    /// Tesco Customer Preference Details
    /// </summary>
    public class CustomerPreference
    {
        #region Fields

        /// <summary>
        /// CustomerID
        /// </summary>
        private Int64 customerID;

        /// <summary>
        /// PreferenceID
        /// </summary>
        private Int16 preferenceID;

        /// <summary>
        /// PrimaryPreferenceOfTypeInd
        /// </summary>
        private string primaryPreferenceOfTypeInd;

        /// <summary>
        /// PreferenceOfTypeSeqNo
        /// </summary>
        private Int16 preferenceOfTypeSeqNo;


        #endregion

        #region Properties

        /// <summary>
        ///  CustomerID
        /// </summary>
        public Int64 CustomerID
        {
            get{return this.customerID;}
            set{this.customerID = value;
            }
        }

        /// <summary>
        ///  PreferenceID
        /// </summary>
        public Int16 PreferenceID
        {
            get{return this.preferenceID;}
            set{this.preferenceID = value;}
        }

        /// <summary>
        ///  PrimaryPreferenceOfTypeInd
        /// </summary>
        public string PrimaryPreferenceOfTypeInd
        {
            get{return this.primaryPreferenceOfTypeInd;}
            set{this.primaryPreferenceOfTypeInd = value;}
        }

        /// <summary>
        ///  PreferenceOfTypeSeqNo
        /// </summary>
        public Int16 PreferenceOfTypeSeqNo
        {
            get{return this.preferenceOfTypeSeqNo;}
            set{this.preferenceOfTypeSeqNo = value;}
        }

        #endregion

    }
}
