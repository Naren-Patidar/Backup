using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Data;


namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Boost
{
    public class RewardAndPoints : BaseEntity<RewardAndPoints>
    {
        public int Reward_Points { get; set; }
        public string StatementDate { get; set; }
        public int VoucherValue { get; set; }
    }

    public class RewardAndPointsLst : BaseEntity<RewardAndPointsLst>
    {
        List<RewardAndPoints> _rewardAndPointsLstInstance = new List<RewardAndPoints>();
        public List<RewardAndPoints> RewardAndPointsLstInstance
        {
            get { return _rewardAndPointsLstInstance; }
            set { _rewardAndPointsLstInstance = value; }
        }

        public override void ConvertFromDataset(DataSet ds)
        {
            XDocument xDoc = XDocument.Parse(ds.GetXml());
            this.ConvertFromXml(xDoc.ToString());
        }

        public override void ConvertFromXml(string xml)
        {
            XDocument xDoc = XDocument.Parse(xml);
            _rewardAndPointsLstInstance = (from t in xDoc.Descendants("Table")
                                           select new RewardAndPoints
                                           {
                                               Reward_Points = t.Element(RewardAndPointsEnum.Reward_Points.ToString()).GetValue<int>(),
                                               StatementDate = t.Element(RewardAndPointsEnum.Statement_Date.ToString()).GetValue<string>(),
                                               VoucherValue = t.Element(RewardAndPointsEnum.VoucherValue.ToString()).GetValue<int>()
                                           }).ToList();
        }
    }
}
