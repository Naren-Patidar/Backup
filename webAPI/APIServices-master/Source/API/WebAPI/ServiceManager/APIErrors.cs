using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Tesco.ClubcardProducts.MCA.API.ServiceManager
{
    public enum APIErrors
    {
        #region E_400

        /// <summary>
        /// Request body is malformed. Request body could not be deserialized into APIRequest object.
        /// </summary>
        E_400_1,

        /// <summary>
        /// Service name is mandatory
        /// </summary>
        E_400_2,

        /// <summary>
        /// Operation name is mandatory
        /// </summary>
        E_400_3,

        /// <summary>
        /// Cannot locate service by name
        /// </summary>
        E_400_4,

        /// <summary>
        /// Service hasn't been accepted yet
        /// </summary>
        E_400_5,

        /// <summary>
        /// Cannot locate operation in assembly
        /// </summary>
        E_400_6,

        /// <summary>
        /// Parameter must be of type
        /// </summary>
        E_400_7,

        /// <summary>
        /// Parameter missing
        /// </summary>
        E_400_8,

        #endregion E_400

        #region E_401

        /// <summary>
        /// Missing publickey.
        /// </summary>
        E_401_1,

        /// <summary>
        /// Incorrect application key.
        /// </summary>
        E_401_2,

        /// <summary>
        /// Missing nonce
        /// </summary>
        E_401_3,

        /// <summary>
        /// Incorrect nonce format.
        /// </summary>
        E_401_4,

        /// <summary>
        /// Missing timestamp
        /// </summary>
        E_401_5,

        /// <summary>
        /// Signature missing
        /// </summary>
        E_401_6,

        /// <summary>
        /// Unable to locate a registered client
        /// </summary>
        E_401_7,

        /// <summary>
        /// Signature mismatch
        /// </summary>
        E_401_8,

        /// <summary>
        /// Current Request signature is older than threshold
        /// </summary>
        E_401_9,

        /// <summary>
        /// Missing identity access token
        /// </summary>
        E_401_10,

        /// <summary>
        /// Missing uuid
        /// </summary>
        E_401_11,

        /// <summary>
        /// Enterprise validation failed
        /// </summary>
        E_401_12,

        /// <summary>
        /// Activation validation failed
        /// </summary>
        E_401_13,

        /// <summary>
        /// Validation details deserialization failed
        /// </summary>
        E_401_14,

        /// <summary>
        /// Validation status is invalid
        /// </summary>
        E_401_15,

        /// <summary>
        /// UUID Mismatch
        /// </summary>
        E_401_16,

        /// <summary>
        /// Empty or null resoponse received from Validate Token
        /// </summary>
        E_401_17,

        /// <summary>
        /// Validation Claims unavailable
        /// </summary>
        E_401_18,
        
        /// <summary>
        /// DotComID unavailable.
        /// </summary>
        E_401_19,

        #endregion E_401

        #region E_403

        /// <summary>
        /// Operation not supported
        /// </summary>
        E_403_1,
        
        #endregion E_403
    }
}