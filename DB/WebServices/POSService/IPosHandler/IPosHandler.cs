using System;
using System.Collections;
using System.Diagnostics;
using System.Net.Sockets;
using Fujitsu.eCrm.Generic.SharedUtils;

namespace Fujitsu.eCrm.Seoul.PosSocketsService {

	public interface IPosListener : IDisposable {
		
		ArrayList Messages { get; }

		ArrayList StoreCodes(string storeIPAddress);
		
		ITrace Trace { get; }

		EventLog PosSocketsEventLog { get; }

		string NgcIPAddress { get; }

		int NgcPort { get; }

		int TimeoutPeriod { get; }

		int RetryVoucherRedeemTime { get; }

		void Remove(IPosHandler child);

		bool PosGet(
			int interfaceId,
			string cardAccountNumber,
			out string customerTitle,
			out string customerName,
			out string postalCode,
			out decimal pointsBalance,
			out decimal pointsConvertedBalance,
			out decimal welcomePoints,
			out string primaryCardAccountNumber,
			out string uniqueNumber,
			out decimal extraPoints1Balance,
			out decimal extraPoints2Balance,
			out decimal extraPoints3Balance,
			string storeCode,
			out int returnCode);
		
		bool PosSet(
			int interfaceId,
			string cardAccountNumber,
			string customerName,
			string txnSourceCode,
			string txnTypeCode,
			string storeCode,
			string storeDate,
			string posId,
			string txnNbr,
			string cashierId,
			string amountSpent,
			string points,
			string welcomePoints,
			string skuPoints,
			string extraPointsType1,
			string extraPointsType2,
			string extraPointsType3,
			out int returnCode);

		bool PosVoucherRedeem(
			int interfaceId,
			string storeCode,
			string storeDate,
			string posId,
			string txnNbr,
			string voucherValue,
			string voucherExpiryDate,
			out int returnCode);

		//void PublishMessage(string actor, string category, string message, params string[] args);
	}

	public interface IPosHandler : IDisposable {

		string StoreIPAddress { get; }

		int StorePort { get; }
	}
}