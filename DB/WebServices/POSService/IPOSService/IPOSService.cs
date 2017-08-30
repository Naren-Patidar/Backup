using System;
using Fujitsu.eCrm.Generic.SharedUtils;

namespace Fujitsu.eCrm.Seoul.CrmService 
{

	#region Header
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// <development> 
		///    <version number="1.10" day="16" month="01" year="2003">
		///			<developer>Tom Bedwell</developer>
		///			<checker>Steve Lang</checker>
		///			<work_packet>WP/Barcelona/046</work_packet>
		///			<description>Namespaces conform to standards</description>
	///	</version>
	/// 	<version number="1.00" day="07" month="01" year="2003">
	///			<developer>Tom Bedwell</developer>
	///			<checker></checker>
	///			<work_packet>WP/Barcelona/042</work_packet>
	///			<description>POS Interface definition </description>
	///		</version>
	/// </development>
	/// <summary>
	///	POS Interface definition
	/// </summary>
	#endregion	
	public interface IPOSService
	{
		#region POS
		
		/// <summary>
		/// PosGet receives card and store details, and responds with customer details
		/// </summary>
		/// <param name="trace">Instance of the trace file</param>
		/// <param name="cardAccountNumber">The number of the card that has been swiped</param>
		/// <param name="storeCode">The store where the card is used</param>
		/// <param name="storeDate">The date at the store where the card is used</param>
		/// <param name="informConversionTxnThreshold">The number of transactions for send points conversion messages</param>
		/// <param name="customerName">The name of the card's primary customer</param>
		/// <param name="pointsBalance">The points balance of the card's household</param>
		/// <param name="pointsConvertedBalance">The number of points converted since previous visit</param>
		/// <param name="welcomedPoints">If this first time the card is used then specify x points to be added to the card account</param>
		/// <param name="primaryCardAccountNumber">The primary card of the householder</param>
		/// <param name="message">Error Message if PosGet fails</param>
		/// <param name="returnCode">The Status of PosGet, 0=success, otherwise an error</param>
		/// <param name="resultXml">Detail description of errors</param>
		/// <returns>True if PosGet is successfully responds to request</returns>
		  bool PosGet(
			ITrace trace,
			string cardAccountNumber,
			string storeCode,
			string storeDate,
			string informConversionTxnThreshold,
			out string customerName,
			out decimal pointsBalance,
			out decimal pointsConvertedBalance,
			out decimal welcomedPoints,
			out string primaryCardAccountNumber,
			out string message,
			out int returnCode,
			out string resultXml) ;

	
		/// <summary>
		/// PosSet receives card and txn details, it updates card and household accounts
		/// </summary>
		/// <param name="trace">Instance of the trace file</param>
		/// <param name="cardAccountNumber">The number of the card that has been swiped</param>
		/// <param name="customerName">The default nameto use when creatinga skeleton customer</param>
		/// <param name="posType">The type of POS</param>
		/// <param name="txnType">The type of Transaction (normal, cancelled, returned)</param>
		/// <param name="storeCode">The store where the card is used</param>
		/// <param name="storeDate">The date & time of the transaction</param>
		/// <param name="futureTime">The number of hours that a transaction can be ahead & be allowed</param>
		/// <param name="posId">The number of the POS terminal</param>
		/// <param name="txnNbr">The transaction number of the POS terminal</param>
		/// <param name="cashierId">The operator behind the POS</param>
		/// <param name="amountSpent">The amount spent by the customer</param>
		/// <param name="points">The number of standard points to award the customer</param>
		/// <param name="bonusPoints">The number of bonus points to award the customer, e.g. welcome points</param>
		/// <param name="SkuPoints">The number of SKU points to award the customer, e.g. points for buying a product</param>
		/// <param name="message">Error Message if PosGet fails</param>
		/// <param name="returnCode">The Status of PosGet, 0=success, otherwise an error</param>
		/// <param name="resultXml">Detail description of errors</param>
		/// <returns>True if PosSet is successfully responds to request</returns>
		  bool PosSet(
			ITrace trace,
			string cardAccountNumber,
			string customerName,
			string txnSourceCode,
			string txnTypeCode,
			string storeCode,
			string storeDate,
			string futureTime,
			string posId,
			string txnNbr,
			string cashierId,
			string amountSpent,
			string points,
			string bonusPoints,
			string SkuPoints,
			out string message,
			out int returnCode,
			out string resultXml) ;
		
		/// <summary>
		/// Update Card Account and Customer with details of vouchers redeemed
		/// </summary>
		/// <param name="trace">Instance of the trace file</param>
		/// <param name="storeCode">The store where the card is used</param>
		/// <param name="storeDate">The date at the store where the card is used</param>
		/// <param name="posId">The number of the POS terminal</param>
		/// <param name="txnNbr">The transaction number of the POS terminal</param>
		/// <param name="voucherValue">The value of the voucher being redeemed</param>
		/// <param name="voucherExpiryDate">The expiry on the voucher being redeemed</param>
		/// <param name="message">Error Message if PosGet fails</param>
		/// <param name="returnCode">The Status of PosGet, 0=success, otherwise an error</param>
		/// <param name="resultXml">Detail description of errors</param>
		/// <returns>True if PosRedeem is successfully responds to request</returns>
		  bool PosVoucherRedeem(
			ITrace trace,
			string storeCode,
			string storeDate,
			string posId,
			string txnNbr,
			string voucherValue,
			string voucherExpiryDate,
			out string message,
			out int returnCode,
			out string resultXml);
	
		 bool GetStorePorts(
			ITrace trace,
			out string[] pineIPAddresses,
			out short[] storeCodes,
			out short[] irtPorts,
			out short[] tranPorts,
			out string resultXml);
		#endregion
		
	}
}