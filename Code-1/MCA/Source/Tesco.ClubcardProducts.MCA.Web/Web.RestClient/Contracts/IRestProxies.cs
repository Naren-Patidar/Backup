namespace Tesco.ClubcardProducts.MCA.Web.RestClient.Contracts
{
    using System;

    /// <summary>
    /// Interface IRestProxies
    /// </summary>
    public interface IRestProxies
    {
        RestAPIResponse<TR> RestGet<TR>(string url, string headerValue);

        void RestGetNonQuery(string url, string headerValue);

        RestAPIResponse<TR> RestPost<TR, TI>(string url, TI data, string headerValue);

        void RestPostNonQuery<TI>(string url, TI data, string headerValue);

        RestAPIResponse<TR> RestPut<TR, TI>(string url, TI data, string headerValue);

        void RestPutNonQuery<TI>(string url, TI data, string headerValue);
    }
}
