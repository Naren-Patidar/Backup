using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.CustomerDetails
{
    public class AccountContext : BaseEntity<AccountContext>
    {
        #region private Fileds

        private bool _promotionCodeExist;
        private bool _isAlternateAccountUnique;
        private bool _isMainAccountUnique;

        #endregion

        #region Public Properties

        public bool IsMainAccountUnique
        {
            get { return _isMainAccountUnique; }
            set { _isMainAccountUnique = value; }
        }
        
        public bool PromotionCodeExist
        {
            get { return _promotionCodeExist; }
            set { _promotionCodeExist = value; }
        }
        public bool IsAlternateAccountUnique
        {
            get { return _isAlternateAccountUnique; }
            set { _isAlternateAccountUnique = value; }
        }
        #endregion

        public override void ConvertFromXml(string xml)
        {
                XDocument xDoc = XDocument.Parse(xml);
                List<AccountContext> _accountContexts = (from t in xDoc.Descendants("Duplicate")
                                                        select new AccountContext
                                   {
                                       IsMainAccountUnique = t.Element(AccountContextEnum.ISDuplicate.ToString()).GetValue<string>().Equals("0"),
                                       IsAlternateAccountUnique = t.Element(AccountContextEnum.IsAlternateIDDuplicate.ToString()).GetValue<string>().Equals("0"),
                                       PromotionCodeExist = t.Element(AccountContextEnum.PromotionCodeExist.ToString()).GetValue<string>().Equals("0")
                                   }).ToList();
                if (_accountContexts.Count > 0)
                {
                    this.CopyFrom(_accountContexts[0]);
                }
        }

        public override void ConvertFromDataset(System.Data.DataSet ds)
        {
            XDocument xDoc = XDocument.Parse(ds.GetXml());
            this.ConvertFromXml(xDoc.ToString());
        }

        public void CopyFrom(AccountContext obj)
        {
            IsMainAccountUnique = obj.IsMainAccountUnique;
            IsAlternateAccountUnique = obj.IsAlternateAccountUnique;
            PromotionCodeExist = obj.PromotionCodeExist;
        }
    }
}
