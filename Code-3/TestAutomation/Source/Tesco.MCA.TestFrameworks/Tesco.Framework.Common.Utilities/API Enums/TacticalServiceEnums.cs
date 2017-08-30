using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.Framework.Common.Utilities.API_Enums
{
    public enum ComplaintType
    {
        FAULTY = 1,         // Standard fault with refund and no customer response	
        NORMAL,         // Customer requests investigation and response	
        SERIOUS         // Represents accident, health issue, damage and foreign body
    }

    public enum BatchCodeAvailability
    {
        NOT_APPLICABLE = 1, // Batch code doesn’t exist, e.g., for non-Tesco branded products, batch codes are not applicable	
        AVAILABLE,      // Batch code is available	
        UNAVAILABLE     // Batch code was unavailable
    }

    public enum PackagingDateType
    {
        BEST_BEFORE = 1,    // Typically on fresh fruit and vegetables and frozen food
        DISPLAY_UNTIL =2,  // Sometimes used on fresh fruit and vegetables and chilled ready meals and dairy
        USE_BY =3          // Fresh meat and poultry, sometimes on dairy, chilled ready meals and diary
    }

}
