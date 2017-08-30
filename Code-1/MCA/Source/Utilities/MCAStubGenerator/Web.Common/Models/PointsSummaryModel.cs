using System;
using System.Collections.Generic;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class PointsSummaryModel
    {
        public PointsSummaryDetailsModel PointsDetails { get; set; }
        public List<TescoPointsModel> TescoPointsModelList { get; set; }
        public List<TescoPointsModel> TescoBankPointsModelList { get; set; }
        public bool IsHoldingPage { get; set; }
        public DateTime PointsSummaryCutOffDate { get; set; } 
    }
}
