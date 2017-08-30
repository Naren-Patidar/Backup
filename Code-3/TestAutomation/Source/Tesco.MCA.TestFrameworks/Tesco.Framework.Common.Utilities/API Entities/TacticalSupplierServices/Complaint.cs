using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.Common.Utilities.API_Enums;

namespace Tesco.Framework.Common.Utilities.API_Entities.TacticalSupplierServices
{
    public class Complaint
    {
        #region PRIVATE MEMBERS

        private string _type;
        private int _accountDivisionNumber;
        private string _batchCode;
        private string _batchCodeAvailability;
        private string _batchCodeUnavailableReason;
        private PackagingDate _packagingDate;
        private string _packagingDateType;
        private int? _gestureOfGoodwillAmount;
        private int? _refundAmount;
        private Boolean _customerIsAnonymous;
        private Customer _customer;
        private Boolean _contactCustomer;
        private string _summary;
        private string _gtin;
        private int _storeid;

        #endregion

        #region PROPERTIES

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public int AccountDivisionNumber
        {
            get { return _accountDivisionNumber; }
            set { _accountDivisionNumber = value; }
        }

        public string BatchCode
        {
            get { return _batchCode; }
            set { _batchCode = value; }
        }

        public string BatchCodeAvailability
        {
            get { return _batchCodeAvailability; }
            set { _batchCodeAvailability = value; }
        }

        public string BatchCodeUnavailableReason
        {
            get { return _batchCodeUnavailableReason; }
            set { _batchCodeUnavailableReason = value; }
        }

        public PackagingDate PackagingDate
        {
            get { return _packagingDate; }
            set { _packagingDate = value; }
        }

        public string PackagingDateType
        {
            get { return _packagingDateType; }
            set { _packagingDateType = value; }
        }

        public int? GestureOfGoodwillAmount
        {
            get { return _gestureOfGoodwillAmount; }
            set { _gestureOfGoodwillAmount = value; }
        }

        public int? RefundAmount 
        {
            get { return _refundAmount; }
            set { _refundAmount = value; }
        }

        public Boolean CustomerIsAnonymous
        {
            get { return _customerIsAnonymous; }
            set { _customerIsAnonymous = value; }
        }

        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; }
        }

        public Boolean ContactCustomer
        {
            get { return _contactCustomer; }
            set { _contactCustomer = value; }
        }

        public string Summary
        {
            get { return _summary; }
            set { _summary = value; }
        }

        public string GTIN
        {
            get { return _gtin; }
            set { _gtin = value; }
        }

        public int StoreId
        {
            get { return _storeid; }
            set { _storeid = value; }
        }

        #endregion
    }
}
