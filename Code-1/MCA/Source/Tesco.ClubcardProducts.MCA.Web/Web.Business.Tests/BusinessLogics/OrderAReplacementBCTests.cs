using NUnit.Framework;
using Rhino.Mocks;
using Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.OrderReplacement;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using  Tesco.ClubcardProducts.MCA.Web.Common.Logger;

namespace Web.Business.Tests
{
    [TestFixture]
    public class OrderAReplacementBCTests
    {
        private IServiceAdapter _clubcardServiceAdapter;
        private ILoggingService _logger;
        private OrderAReplacementBC orderAReplacementBC;


        [SetUp]
        public void SetUp()
        {
            _clubcardServiceAdapter = MockRepository.GenerateMock<IServiceAdapter>();
            _logger = MockRepository.GenerateMock<ILoggingService>();
            orderAReplacementBC = new OrderAReplacementBC(_clubcardServiceAdapter,_logger);
        }
        [TestCase]
        public void GetOrderReplacementModel_Execution_Successful()
        {
            OrderReplacementStatus status = new OrderReplacementStatus
            {
                OldOrderExists = false,
                NumDaysLeftToProcess = -99,
                NumOrdersPlacedInYear = 0,
                ClubcardTypeIndicator = "",
                StandardClubcardNumber = 1
            };

            //OrderReplacementModel orderReplacement = new OrderReplacementModel
            //{
            //    CustomerId=1,
            //    ClubcardNumber=1,
            //    Reason=null,
            //    RequestType = OrderReplacementTypeEnum.NewCardAndKeyFOB            
            //};

            MCARequest mcaRequestOrder = new MCARequest();
            MCAResponse mcaResponseOrder = new MCAResponse();
            mcaResponseOrder.Data = status;
            mcaResponseOrder.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<OrderReplacementStatus>(mcaRequestOrder)).IgnoreArguments().Return(mcaResponseOrder);

            MCARequest mcaRequestInProcess = new MCARequest();
            MCAResponse mcaResponseInProcess = new MCAResponse();
            mcaResponseInProcess.Data = status;
            mcaResponseInProcess.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<OrderReplacementModel>(mcaRequestInProcess)).IgnoreArguments().Return(mcaResponseInProcess);

            var replace = orderAReplacementBC.GetOrderAReplacementModel(1,"en-GB");
            Assert.AreEqual(false, replace.IsInProcess);
            Assert.AreEqual(5, replace.Reasons.Count);
            Assert.AreEqual("NewCardAndKeyFOB", replace.OrderReplacementModel.RequestType.ToString());

        }

    }

}
