using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using System.Linq;
using System.Globalization;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.CustomerDetails
{
    public class CustomerDetails
    {
        public string FirstName {get;set;}
        public string Initial { get; set; }
        public string SurName { get; set; }
        public string MailingAddressLine1 { get; set; }
        public string MailingAddressLine2 { get; set; }
        public string MailingAddressLine3 { get; set; }
        public string MailingAddressLine4 { get; set; }
        public string MailingAddressLine5 { get; set; }
        public string MailingAddressLine6 { get; set; }
        public string MailingAddressPostCode { get; set; }
        public string DaytimePhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string PrimaryId { get; set; }
        public string SecondaryId { get; set; }
        public string Race { get; set; }
        public string Language { get; set; }

        private event EventHandler _invalidFirstName = null;
        private event EventHandler _raceMustBeMandatory = null;
        private event EventHandler _languageMustBeMandatory = null;
        private event EventHandler _primaryIdMustBeMandatory = null;
        private event EventHandler _secondaryIdMustBeMandatory = null;

        public event EventHandler InvalidFirstName
        {
            add { _invalidFirstName += value; }
            remove { _invalidFirstName -= value; }
        }
        public event EventHandler RaceMustBeMandatory
        {
            add { _raceMustBeMandatory += value; }
            remove { _raceMustBeMandatory -= value; }
        }
        public event EventHandler LanguageMustBeMandatory
        {
            add { _languageMustBeMandatory += value; }
            remove { _languageMustBeMandatory -= value; }
        }
        public event EventHandler PrimaryIdMustBeMandatory
        {
            add { _primaryIdMustBeMandatory += value; }
            remove { _primaryIdMustBeMandatory -= value; }
        }
        public event EventHandler SecondaryIdMustBeMandatory
        {
            add { _secondaryIdMustBeMandatory += value; }
            remove { _secondaryIdMustBeMandatory -= value; }
        }

        public void RaiseInvalidFirstName()
        {
            if (_invalidFirstName != null)
                _invalidFirstName(this, new EventArgs());
        }
        public void RaiseRaceMustBeMandatory()
        {
            if (_raceMustBeMandatory != null)
                _raceMustBeMandatory(this, new EventArgs());
        }
        public void RaiseLanguageMustBeMandatory()
        {
            if (_languageMustBeMandatory != null)
                _languageMustBeMandatory(this, new EventArgs());
        }
        public void RaisePrimaryIdMustBeMandatory()
        {
            if (_primaryIdMustBeMandatory != null)
                _primaryIdMustBeMandatory(this, new EventArgs());
        }
        public void RaiseSecondaryIdMustBeMandatory()
        {
            if (_secondaryIdMustBeMandatory != null)
                _secondaryIdMustBeMandatory(this, new EventArgs());
        }
    }

    public class DotcomCustomerDetails : BaseEntity<DotcomCustomerDetails>
    {
        public string LastName { get; set; }
        public string PostCode { get; set; }
        public string WebCustomerID { get; set; }
        public string ClubcardNumber { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public string Forename { get; set; }
        public string Title { get; set; }

        public override void ConvertFromXml(string xml)
        {
            LogData logData = new LogData();
            XDocument xDoc = XDocument.Parse(xml);
            try
            {
                string rootElement = "PersonalDetailsEntity";

                this.LastName = xDoc.Element(rootElement).Element(DotcomCustomerDetailsEnum.Surname).GetValue<string>();
                this.PostCode = xDoc.Element(rootElement).Element(DotcomCustomerDetailsEnum.ClubcardPostcode).GetValue<string>();
                this.WebCustomerID = xDoc.Element(rootElement).Element(DotcomCustomerDetailsEnum.WebCustomerID).GetValue<string>();
                this.ClubcardNumber = xDoc.Element(rootElement).Element(DotcomCustomerDetailsEnum.ClubcardNumber).GetValue<string>();
                this.DisplayName = xDoc.Element(rootElement).Element(DotcomCustomerDetailsEnum.DisplayName).GetValue<string>();
                this.EmailAddress = xDoc.Element(rootElement).Element(DotcomCustomerDetailsEnum.EmailAddress).GetValue<string>();
                this.Forename = xDoc.Element(rootElement).Element(DotcomCustomerDetailsEnum.Forename).GetValue<string>();
                this.Title = xDoc.Element(rootElement).Element(DotcomCustomerDetailsEnum.Title).GetValue<string>();
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException(string.Format("{0}{1}", "Failed in data extraction-Root node missing(GetPersonalDetails) : ", xml), ex,
                            new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }
    }

    public class DotcomCustomerAddressDetails : BaseEntity<DotcomCustomerAddressDetails>
    {
       
        public string PostCode { get; set; }
        public string WebCustomerID { get; set; }   

        public override void ConvertFromXml(string xml)
        {
            LogData logData = new LogData();
            XDocument xDoc = XDocument.Parse(xml);
            string rootElement = "AddressEntity";
            try
            {
                this.PostCode = xDoc.Element(rootElement).Element(DotcomCustomerDetailsEnum.HomePostCode).GetValue<string>();
                this.WebCustomerID = xDoc.Element(rootElement).Element(DotcomCustomerDetailsEnum.WebCustomerID).GetValue<string>();
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException(string.Format("{0}{1}", "Failed in data extraction-Root node missing(GetHomeAddress) : ", xml), ex,
                            new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
          
        }
    }

    public static class DotcomCustomerDetailsEnum
    {
        public const string WebCustomerID       = "WebCustomerID";
        public const string ClubcardNumber      = "ClubcardNumber";
        public const string DisplayName         = "DisplayName";
        public const string EmailAddress        = "EmailAddress";
        public const string Title               = "Title";
        public const string Surname             = "Surname";
        public const string Forename            = "Forename";
        public const string ClubcardPostcode    = "ClubcardPostcode";
        public const string HomePostCode        = "Postcode";
    }
   
}
