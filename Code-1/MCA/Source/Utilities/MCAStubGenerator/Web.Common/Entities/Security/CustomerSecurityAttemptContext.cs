using System;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Security
{
    /// <summary>
    /// Change Details  : CustomerSecurityAttemptContext, this is used to get the security check details for the verification.
    /// Changed By      : Swaraj Kumar Patra
    /// Reviewed By     : Srinivasa RaoPelluri (On 25/09/2014)
    /// Code Checked In : By Swaraj Patra (On 26/09/2014)
    /// Team Name       : 41_Digital_Clubcard_MCA (Vikings)
    /// </summary>
    
    [Serializable]
    public class CustomerSecurityAttemptContext
    {
        private long _customerId;
        private short _spnFirstDigit;
        private short _spnSecondDigit;
        private short _spnThirdDigit;
        private short _firstCheckDigit;
        private short _secondCheckDigit;
        private short _thirdCheckDigit;

        public long CustomerId
        {
            get { return _customerId; }
            set { _customerId = value; }
        }

        public short CardDigitPosition1
        {
            get { return _spnFirstDigit; }
            set { _spnFirstDigit = value; }
        }


        public short CardDigitPosition2
        {
            get { return _spnSecondDigit; }
            set { _spnSecondDigit = value; }
        }


        public short CardDigitPosition3
        {
            get { return _spnThirdDigit; }
            set { _spnThirdDigit = value; }
        }


        public short CardDigitPosition1InputValue
        {
            get { return _firstCheckDigit; }
            set { _firstCheckDigit = value; }
        }


        public short CardDigitPosition2InputValue
        {
            get { return _secondCheckDigit; }
            set { _secondCheckDigit = value; }
        }


        public short CardDigitPosition3InputValue
        {
            get { return _thirdCheckDigit; }
            set { _thirdCheckDigit = value; }
        }
    }
}
