using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstoreClubcardReward.Business
{
    [Serializable]
    public class PrintReasonCollection : BaseCollection<PrintReason>
    {

        public static PrintReasonCollection GetPrintReasons()
        {

            PrintReasonCollection printReasons = new PrintReasonCollection();

            System.Collections.ObjectModel.Collection<Data.SelectPrintReasonsAllRow> printReasonRows;
           
                if (System.Web.HttpContext.Current.Application["PrintReasons"] != null)
                {
                    printReasonRows = (System.Collections.ObjectModel.Collection<Data.SelectPrintReasonsAllRow>)System.Web.HttpContext.Current.Application["PrintReasons"];
                }
                else
                {
                    Data.SelectPrintReasonsAll selectPrintReasonsAll = new Data.SelectPrintReasonsAll(ConnectionString);

                    printReasonRows = selectPrintReasonsAll.Execute();

                    // Store the PrintReasons rows in application
                    if (printReasonRows.Count > 0)
                    {
                        System.Web.HttpContext.Current.Application.Add("PrintReasons", printReasonRows);
                    }

                }
            

            if (printReasonRows.Count > 0)
            {
                // database rows into printreasons collection
                foreach (Data.SelectPrintReasonsAllRow row in printReasonRows)
                {
                    printReasons.Items.Add(
                        new PrintReason(
                            row.PrintReasonId,
                            row.DisplayOrder,
                            row.PrintReason,
                            row.Enabled
                   ));
                }
            }

            return printReasons;

        }


        /// Added by Seema to add default parameter isWcfCall
        public static PrintReasonCollection GetPrintReasonsWCF()
        {

            PrintReasonCollection printReasons = new PrintReasonCollection();

            System.Collections.ObjectModel.Collection<Data.SelectPrintReasonsAllRow> printReasonRows;

            
                Data.SelectPrintReasonsAll selectPrintReasonsAll = new Data.SelectPrintReasonsAll(ConnectionString);

                printReasonRows = selectPrintReasonsAll.Execute();

            if (printReasonRows.Count > 0)
            {
                // database rows into printreasons collection
                foreach (Data.SelectPrintReasonsAllRow row in printReasonRows)
                {
                    printReasons.Items.Add(
                        new PrintReason(
                            row.PrintReasonId,
                            row.DisplayOrder,
                            row.PrintReason,
                            row.Enabled
                   ));
                }
            }

            return printReasons;

        }


    }
}
