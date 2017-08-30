using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.Settings
{
    public class DbConfiguration : BaseEntity<DbConfiguration>
    {
        public List<DbConfigurationItem> ConfigurationItems { get; set; }
        public List<Race> Races { get; set; }
        public List<ISOLanguage> Languages { get; set; }
        public List<Province> Province { get; set; }

        public override void ConvertFromXml(string xml)
        {
            DateTime dtTemp = DateTime.Now;
            XDocument xDoc = XDocument.Parse(xml);
            ConfigurationItems = (from t in xDoc.Descendants(ParameterNames.CONFIGURATION_TABLE_NAME)
                                  select new DbConfigurationItem
                                  {
                                      ConfigurationType = (DbConfigurationTypeEnum)t.Element(DbConfigurationItemEnum.ConfigurationType.ToString()).GetValue<Int64>(),
                                      ConfigurationName = t.Element(DbConfigurationItemEnum.ConfigurationName.ToString()).GetValue<string>(),
                                      ConfigurationValue1 = ((DbConfigurationTypeEnum)t.Element(DbConfigurationItemEnum.ConfigurationType.ToString()).GetValue<Int64>() 
                                                                == DbConfigurationTypeEnum.Holding_dates) ? 
                                                            t.Element(DbConfigurationItemEnum.ConfigurationValue1.ToString()).GetValue<string>().TryParseDate(out dtTemp) 
                                                            ? dtTemp.ToString("o") : String.Empty
                                                            : t.Element(DbConfigurationItemEnum.ConfigurationValue1.ToString()).GetValue<string>(),
                                      ConfigurationValue2 = ((DbConfigurationTypeEnum)t.Element(DbConfigurationItemEnum.ConfigurationType.ToString()).GetValue<Int64>() 
                                                                == DbConfigurationTypeEnum.Holding_dates) ? 
                                                            t.Element(DbConfigurationItemEnum.ConfigurationValue2.ToString()).GetValue<string>().TryParseDate(out dtTemp) 
                                                            ? dtTemp.ToString("o") : String.Empty
                                                            : t.Element(DbConfigurationItemEnum.ConfigurationValue2.ToString()).GetValue<string>(),
                                      IsDeleted = false
                                  }).ToList();
            Races = (from t in xDoc.Descendants(ParameterNames.RACE_TABLE_NAME)
                     select new Race
                     {
                         Racedescenglish = t.Element(RaceEnum.racedescenglish.ToString()).GetValue<string>(),
                         Racedesclocal = t.Element(RaceEnum.RaceDescLocal.ToString()).GetValue<string>(),
                         RaceID = t.Element(RaceEnum.raceid.ToString()).GetValue<Int32>()
                     }).ToList();
            Languages = (from t in xDoc.Descendants(ParameterNames.LANGUAGE_TABLE_NAME)
                         select new ISOLanguage
                         {
                             ISOLanguageCode = t.Element(ISOLanguageEnum.ISOLanguageCode.ToString()).GetValue<string>(),
                             ISOLanguageDescEnglish = t.Element(ISOLanguageEnum.ISOLanguageDescEnglish.ToString()).GetValue<string>()
                         }).ToList();
            Province = (from t in xDoc.Descendants(ParameterNames.PROVINCE_TABLE_NAME)
                        select new Province
                        {
                            ProvinceID = t.Element(ProvinceEnum.ProvinceID.ToString()).GetValue<Int32>(),
                            ProvinceNameEnglish = t.Element(ProvinceEnum.ProvinceNameEnglish.ToString()).GetValue<string>(),
                            ProvinceNameLocal = t.Element(ProvinceEnum.ProvinceNameLocal.ToString()).GetValue<string>()
                        }).ToList();
        }
    }
}
