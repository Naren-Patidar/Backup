using System.Xml.Linq;
using System.Data;
using System.Linq;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;
using System.Collections.Generic;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.OrderReplacement
{
    public class OrderReplacementStatus : BaseEntity<OrderReplacementStatus>
    {
        public bool OldOrderExists { get; set; }
        public string ClubcardTypeIndicator { get; set; }
        public int NumDaysLeftToProcess { get; set; }
        public int NumOrdersPlacedInYear { get; set; }
        public string StandardClubcardNumber { get; set; }

    }

    public class OrderReplacementStatusList : BaseEntity<OrderReplacementStatusList>
    {
        List<OrderReplacementStatus> _orderreplacementstatus;
        public List<OrderReplacementStatus> OrderReplacementStatusEntity
        {
            get { return _orderreplacementstatus; }
        }
        public override void ConvertFromDataset(DataSet ds)
        {

            DataSet newDataSet = new DataSet();
            DataTable orderReplacementTable = ds.Tables[0];
            //--Add missing columns in table
            orderReplacementTable.AddMissingColumns(typeof(OrderReplacementStatusEnum));
            ds.Tables.Remove(orderReplacementTable);
            newDataSet.Tables.Add(orderReplacementTable);
            XDocument xDoc = XDocument.Parse(newDataSet.GetXml());
            this.ConvertFromXml(xDoc.ToString());
        }

        public override void ConvertFromXml(string xml)
        {
            XDocument xDoc = XDocument.Parse(xml);
            _orderreplacementstatus = (from r in xDoc.Descendants("OrderReplacement")
                                       select new OrderReplacementStatus
                                       {
                                           OldOrderExists = r.Element(OrderReplacementStatusEnum.oldOrderExists.ToString()).GetValue<string>()!="0",
                                           ClubcardTypeIndicator = r.Element(OrderReplacementStatusEnum.ClubcardTypeIndicatior.ToString()).GetValue<string>(),
                                           NumDaysLeftToProcess = r.Element(OrderReplacementStatusEnum.noOfDaysLeftToProcess.ToString()).GetValue<int>(),
                                           NumOrdersPlacedInYear = r.Element(OrderReplacementStatusEnum.countOrdersPlacedInYear.ToString()).GetValue<int>(),
                                           StandardClubcardNumber = r.Element(OrderReplacementStatusEnum.standardCardNumber.ToString()).GetValue<string>()
                                       }).ToList();
        }
    }
}

