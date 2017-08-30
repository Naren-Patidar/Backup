#region using

using System;
using System.Collections.Generic;
using System.Text;
using Tesco.NGC.DataAccessLayer;
using Tesco.NGC.Utils;
using System.Data;
using System.Collections;

#endregion


namespace Tesco.NGC.Loyalty.EntityServiceLayer
{
    #region Header
    ///
    /// <summary>
    /// StoreIPAddress Details
    /// </summary>
    /// <development>
    ///		<version number="1.00" date="13/Aug/2008">
    ///			<developer>Netra</developer>
    ///			<Reviewer></Reviewer>
    ///			<description>Initial Implementation</description>
    ///		</version>
    ///	<development>
    ///	
    #endregion
    class StoreIPAddress
    {
        #region Fields

        private Int32 tescoStoreID;
        public string ipAddress;
        private string ipAddressFormat;
        private string serverType;
        public Int32 port;

        #endregion

        #region Properties
        //public Int32 TescoStoreID { get { return this.tescoStoreID; } set { this.tescoStoreID = value; } }
        public string IPAddress { get { return this.ipAddress; } set { this.ipAddress = value; } }
        public string IPAddressFormat { get { return this.ipAddressFormat; } set { this.ipAddressFormat = value; } }
        public string ServerType { get { return this.serverType; } set { this.serverType = value; } }
        public int Port { get { return this.port; } set { this.port = value; } }

        #endregion
    }
}
