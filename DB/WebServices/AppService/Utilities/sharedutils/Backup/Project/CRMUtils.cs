using System;

namespace Fujitsu.eCrm.Generic.SharedUtils {

	/// <summary>
	/// Summary description for CrmUtils.
	/// </summary>
	public class CrmUtils {


		/// <summary>
		/// Calculates a card's check digit from its number. Based on an email from Jack Hanison
		/// (Tesco) to Stuart Forbes dated 17/2/03.
		/// </summary>
		/// <param name="cardNumber"></param>
		/// <returns></returns>
		public static bool CalculateCheckDigit(string cardNumber) {

			try {

				int cardNumberBound = cardNumber.Length-1;
				// Card Number must contain at least 2 digits, including the check digit
				if (cardNumberBound < 1) {
					return false;
				}

				int sum = 0;
				// ignore the last digit, as this is the check digit
				for (int i=0; i<cardNumberBound; i++) {
					int weight = 2 - (i%2);
					int digitTimesWeight = int.Parse(cardNumber[i].ToString()) * weight;
					sum += (digitTimesWeight%10) + (digitTimesWeight/10);
				}

				int correctCheckDigit = 10-(sum%10);
				// If the check digit is 10, the check digit should be zero
				if (correctCheckDigit == 10) {
					correctCheckDigit = 0;
				}

				int cardCheckDigit = int.Parse(cardNumber[cardNumberBound].ToString());
				return (correctCheckDigit == cardCheckDigit);

			} catch {
				return false;
			}
		}

		/// <summary>
		/// Test if a Korean Customer's official ID (SSN) is valid
		/// </summary>
		/// <param name="ssn">The Korean SSN to validate</param>
		/// <param name="foreignOnly">If TRUE, validation will fail if the SSN is not for a Foreigner</param>
		/// <param name="localOnly">If TRUE, validation will fail if the SSN is not for a Local citizen</param>
		/// <param name="isForeign">Returns TRUE if the 7th character of the SSN indicates it is for a Foreigner.</param>
		/// <returns>True if SSN is valid, false otherwise</returns>
		public static bool ValidateKoreanSsn(string ssn, 
											 bool foreignOnly, 
										     bool localOnly, 
											 out bool isForeign) {
			

			// Default value for foreign flag
			isForeign = false;
			int century = 1900;

			#region Check string length & contents

			// Check Length
			if (ssn.Length != 13) {
				return false;
			}

			// Split string into a list of characters
			char[] ssnCharArray = ssn.ToCharArray();

			// Check all characters are numeric
			for (int i=0; i<13; i++) {
				if (!Char.IsDigit(ssnCharArray[i])) {
					return false;
				}
			}
			#endregion

			#region Get foreign flag and century from Character #7

			// Check Seventh Character is between 1 and 4 for Korean SSNs and
			// 5 and 9 or 0 for Foreign SSNs.
			switch (ssnCharArray[6]) {
				case '1':
				case '2':
					break;
				case '3':
				case '4':
					century = 2000;
					break;
				case '5':
				case '6':
					isForeign = true;
					break;
				case '7':
				case '8':
					century = 2000;
					isForeign = true;
					break;
				case '9':
				case '0':
					century = 1800;
					isForeign = true;
					break;
				default: 
					return false;
			}
			#endregion

			#region Validate the day, month and year

			// Extract the date part of the SSN.
			int month = (int)( (Char.GetNumericValue(ssnCharArray[2]) * 10) + 
							    Char.GetNumericValue(ssnCharArray[3]) );

			int day = (int)( (Char.GetNumericValue(ssnCharArray[4]) * 10) + 
							  Char.GetNumericValue(ssnCharArray[5]) );

			int year = century + (int)( (Char.GetNumericValue(ssnCharArray[0]) * 10) + 
							             Char.GetNumericValue(ssnCharArray[1]));

			// Check that the month is valid
			if ((month < 1) || (month > 12)) {
				return false;
			}

			// Check that the day is valid for the month 
			int[] daysInMonth ; 
			if (((year % 4)==0) && (((year % 100) != 0) || ((year % 400) == 0))){
				// Leap year
				daysInMonth = new int[] {31,29,31,30,31,30,31,31,30,31,30,31};
			}
			else {
				// Non leap year
				daysInMonth = new int[] {31,28,31,30,31,30,31,31,30,31,30,31};
			}

			if ((day < 1) || (day > daysInMonth[month-1])) {
				return false;
			}

			#endregion

			#region Calculate Expected Check Digit
			double runningCheckDigit = 0;
			for (int i=0; i<12 ; i++) {
				double digit = Char.GetNumericValue(ssnCharArray[i]);
				runningCheckDigit = runningCheckDigit + (((i%8) + 2) * digit);
			}
			runningCheckDigit = 11 - (runningCheckDigit%11);
			runningCheckDigit = runningCheckDigit%10;
			#endregion

			#region Extra Checks for Foreign SSNs
			if (isForeign) {
				if (localOnly) {
					return false;
				}

				int iChk = ((int)Char.GetNumericValue(ssnCharArray[7]) * 10) + 
							(int)Char.GetNumericValue(ssnCharArray[8]);
				if ((iChk % 2) != 0) {
					return false;
				}
				
				iChk = (int)Char.GetNumericValue(ssnCharArray[11]);
				if ((iChk < 6) || (iChk > 9)) {
					return false;
				}

				runningCheckDigit += 2;
				if (runningCheckDigit >= 10){
					runningCheckDigit -= 10;
				}

			}
			else {
				if (foreignOnly) {
					return false;
				}
			}
			#endregion

			#region Check Provided Check Digit matches Expected
			double checkDigit = Char.GetNumericValue(ssnCharArray[12]);
			if (runningCheckDigit != checkDigit) {
				return false;
			}
			#endregion

			// Passed all tests
			return true;
		}

		/// <summary>
		/// Batch version of ValidateKoreanSsn to comply with batch's 
		/// interface requirements
		/// </summary>
		/// <param name="parameters">Parameter 0 : The SSN to validate</param>
		/// <returns>"local", "foreign" or "false"</returns>
		public static string BatchValidateKoreanSsn(string[] parameters) {

			if (parameters.Length < 2) {
				return false.ToString();
			}

			string foreignFlag = parameters[1].Trim();
			bool result;
			bool isForeign;

			if (foreignFlag == "1")
			{
				result = ValidateKoreanSsn(parameters[0], true, false, out isForeign);
			}
			else if (foreignFlag == "0")
			{
				result = ValidateKoreanSsn(parameters[0], false, true, out isForeign);
			}
			else
			{
				result = false;
			}

			return result.ToString();

		}

	}
}
