// -----------------------------------------------------------------------
// <copyright file="IRestAsyncProxies.cs" company="Tesco HSC">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Tesco.ClubcardProducts.MCA.Web.RestClient.Contracts
{
    using Tesco.ClubcardProducts.MCA.Web.RestClient.Support;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IRestAsyncProxies
    {
        void RestGetAsync<TR>(string url, RestCallBack<TR> callback, string header);

        void RestGetNonQueryAsync(string url, RestCallBackNonQuery callback, string headerValue);

        void RestPostAsync<TR, TI>(string url, TI data, RestCallBack<TR> callback, string header);

        void RestPostNonQueryAsync<TI>(string url, TI data, RestCallBackNonQuery callback, string headerValue);

        void RestPutAsync<TR, TI>(string url, TI data, RestCallBack<TR> callback, string header);

        void RestPutNonQueryAsync<TI>(string url, TI data, RestCallBackNonQuery callback, string headerValue);
    }
}
