using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace BigExchange
{
    /// <summary>
    /// Created Date : 16/02/2012
    /// Created By: Dimple Kandoliya
    /// Enum Name: 
    /// </summary>

    [DataContract]
    public enum LoggingOperations
    {
        [EnumMember]
        SaveToTransError = 1,
        [EnumMember]
        UpdateTranDetailsStatus = 2,
        [EnumMember]
        UpdateTranDetailsVerifiedStartTime = 3,
        [EnumMember]
        UpdateTranDetailsVerifiedTime = 4,
        [EnumMember]
        InsertLoginAttempts = 5,
        [EnumMember]
        UpdateTranDetailsActiveVoucher = 6,
        [EnumMember]
        UpdateTranDetailsPrintDate = 7,
        [EnumMember]
        SaveUnusedVouchers = 8,
         [EnumMember]
        SaveUnusedCoupons = 9
        
    }
}
    
