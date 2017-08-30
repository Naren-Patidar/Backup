using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings
{
    public enum SecureConfigEnum
    {
        /// <summary>
        /// Password to get MCA servie token from identity.
        /// </summary>
        MCAServiceTokenPassword,

        /// <summary>
        /// User name to get MCA servie token from identity.
        /// </summary>
        MCAServiceTokenUsername,
        
        /// <summary>
        /// MCA specific client ID provided by identity.
        /// </summary>
        MCAClientID
    }
}
