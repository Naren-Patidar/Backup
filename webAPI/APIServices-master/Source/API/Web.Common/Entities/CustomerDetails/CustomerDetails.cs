using System;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.CustomerDetails
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
}
