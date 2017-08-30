using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using InstoreClubcardReward.Data;
using System.Diagnostics;

namespace InstoreClubcardReward.Business
{
    [Serializable]
    public class Store : BaseClass
    {
        public int StoreNo { get; set; }
        public string StoreName { get; set; }
        public bool Enabled { get; set; }

        public Store()
        {
        }

        // read from the database if the store number is enabled
        public Store(int storeno)
        {
            this.StoreNo = storeno;

            // collection for output from stored procedure
            try
            {
                System.Collections.ObjectModel.Collection<SelectStoreRow> StoreRow;
                StoreRow = SelectStore.Execute(ConnectionString, storeno);

                // if there is a record for store
                if (StoreRow.Count == 1)
                {
                    // select on key so only one entry
                    foreach (SelectStoreRow row in StoreRow)
                    {
                        this.StoreName = (string)row.StoreName;
                        this.Enabled = (bool)row.Enabled;
                        break;
                    }

                }
                else
                {
                    // a record is created for the store 
                    // bool defaults to false - but be explicit
                    this.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                // write any error to event log
                using (EventLog objEventLog = new EventLog("Application"))
                {
                    objEventLog.Source = "InstoreCCVPrint";
                    objEventLog.WriteEntry(string.Format("InstoreCCVPrint Store. Error: {1}", ex.Message));
                }

            }

        }

    }
}

