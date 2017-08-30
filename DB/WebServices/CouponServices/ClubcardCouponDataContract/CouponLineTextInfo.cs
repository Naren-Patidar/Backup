using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tesco.Marketing.IT.ClubcardCoupon.DataContract
{
    /// <summary>
    /// ClubcardCoupon CouponLineTextInfo Entity
    /// </summary>
    /// <remarks>This is basically used to print the coupon at Till</remarks>
    [DataContract]
    public class CouponLineTextInfo
    {
        /// <summary>
        /// Represents the LineNumber of Till Template
        /// </summary>
        /// <example>Line number can be from 1 to 10</example>
        [DataMember]
        public string LineNumber { get; set; }

        /// <summary>
        /// Represents the LineText that is to be printed on the Coupon 
        /// </summary>
        /// <example>2 pounds off when you buy bread</example>
        [DataMember]
        public string LineText { get; set; }

        /// <summary>
        /// Represents whether the line is used to be printed at Till
        /// </summary>
        /// <example>Y/N, Y for yes, N for no</example>
        [DataMember]
        public char LineUsed { get; set; }

        /// <summary>
        /// Represents whether the line is underlined
        /// </summary>
        /// <example>Y/N, Y for yes, N for no</example>
        [DataMember]
        public char UnderLine { get; set; }

        /// <summary>
        /// Represents whether the lineText is Italic
        /// </summary>
        /// <example>Y/N, Y for yes, N for no</example>
        [DataMember]
        public char Italic { get; set; }

        /// <summary>
        /// Represents whether the WhiteOnBlack text formatting is used
        /// </summary>
        /// <example>Y/N, Y for yes, N for no</example>
        [DataMember]
        public char WhiteOnBlack { get; set; }

        /// <summary>
        /// Represents whether the lineText is Centered
        /// </summary>
        /// <example>Y/N, Y for yes, N for no</example>
        [DataMember]
        public char Center { get; set; }

        /// <summary>
        /// Represents if the barcode should be printed on the coupon
        /// </summary>
        /// <example>Y/N, Y for yes, N for no</example>
        [DataMember]
        public char Barcode { get; set; }

        /// <summary>
        /// Represents the LineText CharacterWidth 
        /// </summary>
        /// <example>Y/N, Y for yes, N for no</example>
        [DataMember]
        public byte CharacterWidth { get; set; }

        /// <summary>
        /// Specifies the LineText CharacterWeigth 
        /// </summary>
        /// <example>Y/N, Y for yes, N for no</example>
        [DataMember]
        public byte CharacterWeigth { get; set; }
    }
}
