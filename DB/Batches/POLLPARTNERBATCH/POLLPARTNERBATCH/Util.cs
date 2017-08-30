#region Using
using System;
#endregion

namespace PollPartnerBatchService
{
	/// <summary>
	/// Summary description for Util.
	/// </summary>
	public class Util
	{
		public Util()
		{
			
		}

		/// <summary>
		/// converts the string to datetimeformat
		/// </summary>
		/// <param name="date">date in yyyyMMdd format</param>
		/// <param name="time">time in hhmmss format</param>
		/// <returns>Datetime</returns>
		protected internal DateTime ConvertToDateTime(string date,string time)
		{
			DateTime dt=DateTime.Now;
			try
			{
				int year=0;
				int month=0;
				int day=0;
				int hours=0;
				int minutes=0;
				int seconds=0;
				if(date!=null && date!="")
				{
					year=int.Parse(date.Substring(0,4));
					month=int.Parse(date.Substring(4,2));
					day=int.Parse(date.Substring(6,2));
					if(time !=null && time !="")
					{
						hours=int.Parse(time.Substring(0,2));
						minutes=int.Parse(time.Substring(2,2));
						seconds=int.Parse(time.Substring(4,2));
					}
					dt=new DateTime(year,month,day,hours,minutes,seconds);
				}
			}
			catch(Exception)
			{
				
			}
			return dt;
		}

		protected internal bool CalculateCheckDigit(string cardNumber) 
		{

			try 
			{

				int cardNumberBound = cardNumber.Length-1;
				// Card Number must contain at least 2 digits, including the check digit
				if (cardNumberBound < 1) 
				{
					return false;
				}

				int sum = 0;
				// ignore the last digit, as this is the check digit
				for (int i=0; i<cardNumberBound; i++) 
				{
					int weight = 2 - (i%2);
					int digitTimesWeight = int.Parse(cardNumber[i].ToString()) * weight;
					sum += (digitTimesWeight%10) + (digitTimesWeight/10);
				}

				int correctCheckDigit = 10-(sum%10);
				// If the check digit is 10, the check digit should be zero
				if (correctCheckDigit == 10) 
				{
					correctCheckDigit = 0;
				}

				int cardCheckDigit = int.Parse(cardNumber[cardNumberBound].ToString());
				return (correctCheckDigit == cardCheckDigit);

			} 
			catch 
			{
				return false;
			}
		}
	}
}
