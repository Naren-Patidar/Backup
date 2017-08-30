using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Utilities
{
    public class MCASecurityException : Exception
    {
        private SecurityErrors _Error = SecurityErrors.E_401_1;

        public SecurityErrors Error
        {
            get
            {
                return this._Error;
            }
        }

        public MCASecurityException(SecurityErrors err)
        {
            this._Error = err;
        }
    }

    public enum SecurityErrors
    {
        /// <summary>
        /// UUID cookie not available
        /// </summary>
        E_401_1,

        /// <summary>
        /// Access Token not available
        /// </summary>
        E_401_2,

        /// <summary>
        /// Refresh Token not available
        /// </summary>
        E_401_3,

        /// <summary>
        /// Identity Web Exception
        /// </summary>
        E_401_4,

        /// <summary>
        /// Identity Validate Token says Invalid or blank
        /// </summary>
        E_401_5,

        /// <summary>
        /// Validate token uuid doesn't matche browser cookie uuid
        /// </summary>
        E_401_6,

        /// <summary>
        /// Expiration data not available when trying to create new JWT.
        /// </summary>
        E_401_7,

        /// <summary>
        /// Dotcomid not available when trying to create new JWT.
        /// </summary>
        E_401_8,

        /// <summary>
        /// Activation Status object not available when trying to create new JWT.
        /// </summary>
        E_401_9,

        /// <summary>
        /// JWT access token does not match browser cookie access token.
        /// </summary>
        E_401_10,

        /// <summary>
        /// JWT uuid does not match browser cookie uuid.
        /// </summary>
        E_401_11,

        /// <summary>
        /// JWT doesn't have dotcomid in it.
        /// </summary>
        E_401_12,

        /// <summary>
        /// JWT payload doesn't have the desired property in it.
        /// </summary>
        E_401_13

    }
}
