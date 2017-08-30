using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstoreClubcardReward.Business
{
    class cClubcard
    {


        /// <summary>
        /// Determines whether [has valid clubcard].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [has valid clubcard]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasValidClubcard(string clubcard)
        {
            return HasValidCheckDigit(clubcard);
        }




        /// <summary>
        /// Determines whether the Clubcard number passed in has a valid check digit
        /// </summary>
        /// <param name="clubcardNumber">The Clubcard number.</param>
        /// <returns>
        /// 	<c>true</c> if the Clubcard has a valid check digit; otherwise, <c>false</c>.
        /// </returns>
        private static Boolean HasValidCheckDigit(String clubcardNumber)
        {
            // Code lifted from VerifyValidCard.cls (Original Matthew code)
            // was based on variants so added object and added necessary conversions between strings and integers
            //
            // This function verifies the card check digit is correct
            int Loop_Count;
            int Times_2_Units;
            int Times_1_Units;
            double Times_2_Tens;
            int Running_Total;
            int Current_Check_Digit;
            int Calculated_Check_Digit;
            Boolean bFlag;
            Running_Total = 0;
            bFlag = true;
            int clubcard;

            if (!int.TryParse(clubcardNumber, out clubcard))
            {
                // Remove Spaces
                for (Loop_Count = clubcardNumber.Length; Loop_Count >= 1; Loop_Count--)
                {
                    if (Mid(clubcardNumber, Loop_Count, 1) == " ")
                    {
                        switch (Loop_Count)
                        {
                            case 1:
                                clubcardNumber = Right(clubcardNumber, clubcardNumber.Length - 1);
                                break;
                            case 2:
                                clubcardNumber = Left(clubcardNumber, Loop_Count - 1);
                                break;
                            default:
                                clubcardNumber = string.Format("{0}{1}", Left(clubcardNumber, Loop_Count - 1), Right(clubcardNumber, clubcardNumber.Length - Loop_Count));
                                break;
                        }
                    }
                }
                //If the string length is too short, go no further
                if (clubcardNumber.Length > 15)
                {
                    //Check For Numerics
                    for (Loop_Count = clubcardNumber.Length; Loop_Count >= 1; Loop_Count--)
                    {
                        if (!int.TryParse(Mid(clubcardNumber, Loop_Count, 1), out clubcard))
                        {
                            bFlag = false;

                        }
                    }
                }
            }

            if (bFlag & (clubcardNumber.Length == 16 || clubcardNumber.Length == 18))
            {
                //Calculate Check Digit
                Current_Check_Digit = int.Parse(clubcardNumber.Substring(clubcardNumber.Length - 1, 1));
                for (Loop_Count = clubcardNumber.Length - 1; Loop_Count >= 1; Loop_Count--)
                {

                    Times_2_Units = int.Parse(Mid(clubcardNumber, Loop_Count, 1));
                    Times_2_Units = (int)(Times_2_Units) * 2;
                    if (Times_2_Units.ToString().Length > 1)
                    {
                        Times_2_Tens = (int)(Times_2_Units * Math.Pow(10, -1));
                        Times_2_Units = Times_2_Units - ((int)Times_2_Tens * 10);
                    }
                    else
                    {
                        Times_2_Tens = 0;
                    }
                    if (Loop_Count > 1)
                    {
                        Times_1_Units = int.Parse(Mid(clubcardNumber, Loop_Count - 1, 1));
                    }
                    else
                    {
                        Times_1_Units = 0;
                    }
                    Running_Total = Running_Total + Times_2_Units + Times_1_Units + (int)Times_2_Tens;

                    // VB.Net code uses step -2 to decrement this twice
                    Loop_Count--;
                }
                Calculated_Check_Digit = 10 - int.Parse(Right(Running_Total.ToString(), 1));
                if (Running_Total != 0)
                {
                    if (Calculated_Check_Digit > 9)
                    {
                        Calculated_Check_Digit = 0;
                    }
                    if (Current_Check_Digit == Calculated_Check_Digit)
                    {
                        return true;
                    }
                }
            }
            return false;
            //App.LogEvent ("bCheckDigit : " & Err.Number & ", " & Err.Description & " sClubCardNumber=>" & sClubCardNumber & "<"), 1
        }

        private static string Left(string param, int length)
        {
            string result = param.Substring(0, length);
            return result;
        }
        private static string Right(string param, int length)
        {
            string result = param.Substring(param.Length - length, length);
            return result;
        }

        private static string Mid(string param, int startIndex, int length)
        {
            string result = param.Substring(startIndex - 1, length);
            return result;
        }

        private static string Mid(string param, int startIndex)
        {
            string result = param.Substring(startIndex - 1);
            return result;
        }





        /// a futher check that a clubcard is not in 
        /// a range of TPF that are not clubcard
        /// put in routine for identifying tpf cards that are not clubcards
        /// ########## NOT TESTED / NOT USED 18/8/10
        /// false is a positive output
        /// true means not in the range... include parsing error
        public static Boolean isValidClubcardTPF(string clubcard)
        {
            // ranges provide by Tim Wiles 16/08/10
            /*
            4305674000000000 through to 430567899999999
            4029395000000000 through to 4029398999999999
            4655905000000000 through to 4655908999999999
            5186451000000000 through to 5186458999999999
            5521881000000000 through to 5521888999999999
            5206414000000000 through to 5206418999999999
            4013434000000000 through to 4013438999999999
            5186524000000000 through to 5186528999999999
             */
            // convert to long number... can cope with 18 digits
            // Int64..::.MaxValue Field, The value of this constant is 9,223,372,036,854,775,807; that is, hexadecimal 0x7FFFFFFFFFFFFFFF
            try
            {
                long cc = long.Parse(clubcard);
                if (cc >= 4305674000000000 && cc <= 4305678999999999) return false;
                if (cc >= 4029395000000000 && cc <= 4029398999999999) return false;
                if (cc >= 4655905000000000 && cc <= 4655908999999999) return false;
                if (cc >= 5186451000000000 && cc <= 5186458999999999) return false;
                if (cc >= 5521881000000000 && cc <= 5521888999999999) return false;
                if (cc >= 5206414000000000 && cc <= 5206418999999999) return false;
                if (cc >= 4013434000000000 && cc <= 4013438999999999) return false;
                if (cc >= 5186524000000000 && cc <= 5186528999999999) return false;

            }
            catch (Exception ex)
            {
                return true;    //any error means not going to match
            }
            
            return true;

        }


    }
}
