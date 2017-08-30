using System;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts
{
    internal class DummyDecryptCookieServiceClient : IDecryptCookieService
    {
        public DummyDecryptCookieServiceClient()
        {
        }

        public CustomerIdentity GetDecodedCookie(CustomerIdentity cust)
        {
            throw new NotImplementedException();
        }
    }
}
