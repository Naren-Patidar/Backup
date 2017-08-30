using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder
{
    public class Recorder
    {
        private long _customerID = 0;
        private string _rootDir = String.Empty;

        public string OutputDir { get; private set; }

        public Recorder(long customerID)
        {
            this._customerID = customerID;
            this._rootDir = ConfigurationManager.AppSettings["DataRoot"];
        }

        public virtual void RecordResponse(RecordLog rLog, string serviceName, string methodName, ResponseType rType)
        {
            if (this._customerID > 0)
            {
                string dirPath = Path.Combine(this._rootDir, serviceName, "DataSource", methodName, this._customerID.ToString());

                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                if (Char.IsLetter(rLog.Activated))
                {
                    string filePath = Path.Combine(dirPath, "activated.txt");
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    using (StreamWriter file = new StreamWriter(filePath, false))
                    {
                        file.Write(rLog.Activated);
                    }
                }

                if (rLog.CustomerID > 0)
                {
                    string filePath = Path.Combine(dirPath, "customerID.txt");
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    using (StreamWriter file = new StreamWriter(filePath, false))
                    {
                        file.Write(rLog.CustomerID);
                    }
                }

                if (!String.IsNullOrWhiteSpace(rLog.Result))
                {
                    string filePath = Path.Combine(dirPath, "data." + rType.ToString());
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    using (StreamWriter file = new StreamWriter(filePath, false))
                    {
                        file.Write(rLog.Result);
                    }
                }

                if (!String.IsNullOrWhiteSpace(rLog.Error))
                {
                    string errorfilePath = Path.Combine(dirPath, "error." + rType.ToString());
                    if (File.Exists(errorfilePath))
                    {
                        File.Delete(errorfilePath);
                    }

                    using (StreamWriter file = new StreamWriter(errorfilePath, false))
                    {
                        file.Write(rLog.Error);
                    }
                }

                if (rLog.RowCount > 0)
                {
                    string rowCountfilePath = Path.Combine(dirPath, "rowCount.txt");
                    if (File.Exists(rowCountfilePath))
                    {
                        File.Delete(rowCountfilePath);
                    }

                    using (StreamWriter file = new StreamWriter(rowCountfilePath, false))
                    {
                        file.Write(rLog.RowCount);
                    }
                }

                this.OutputDir = dirPath;
            }
        }
    }
    
    public class RecordLog
    {
        public string Error { get; set; }
        public string Result { get; set; }
        public int RowCount { get; set; }
        public long CustomerID { get; set; }
        public char Activated { get; set; }
        public int OfferID { get; set; }
        public int TotalCoupons { get; set; }
    }

    public enum ResponseType
    {
        js,
        Xml
    }

    public enum Services
    {
        RewardServices,
        ClubCardCouponService,
        ClubCardService,
        CustomerActivationService,
        CustomerService,
        JoinLoyaltyService,
        PreferenceService,
        SmartVoucherServices,
        UtilityService
    }
}
