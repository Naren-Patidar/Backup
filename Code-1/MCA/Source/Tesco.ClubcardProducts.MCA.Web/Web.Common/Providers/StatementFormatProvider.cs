using System.Configuration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using ClubcardOnline.PointsSummarySequencing;
using System.Web;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;

namespace Tesco.ClubcardProducts.MCA.Web.Common.StatementFormatProvider
{
    public interface IStatementFormatProvider
    {
        StatementFormat GetStatementFormat(int offerId, string path);
    }

    public class StatementFormatProvider : IStatementFormatProvider
    {
        public StatementFormat GetStatementFormat(int offerId, string path)
        {
            return XMLSerializer<StatementFormat>.Load(HttpContext.Current.Server.MapPath(path + offerId + ".xml"));
        }
    }
}