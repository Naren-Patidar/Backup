using System;
using System.Text.RegularExpressions;

namespace Fujitsu.eCrm.Generic.ControlLibrary.CSAComparer {

	/// <summary>
	/// Compare two objects, treat them as integers
	/// </summary>
	public class IntegerComparer : GenericComparer, ICSAComparer {

		private Regex integerRegex = new Regex(@"(?<integer>\-?\d+)",RegexOptions.Compiled);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int Compare(object x, object y) {

			Match integerXMatch = integerRegex.Match((string)x);
			long intX = long.MaxValue;
			if (integerXMatch.Success) {
				try {
					intX = Int64.Parse(integerXMatch.Groups["integer"].Value);
				} catch {}
			}

			Match integerYMatch = integerRegex.Match((string)y);
			long intY = long.MaxValue;
			if (integerYMatch.Success) {
				try {
					intY = Int64.Parse(integerYMatch.Groups["integer"].Value);
				} catch {}
			}

			int comparison;
			if ((intY == long.MaxValue) && (intY == long.MaxValue)) {
				comparison = ((string)x).CompareTo((string)y);
			} else {
				comparison = intX.CompareTo(intY);
			}

			if (this.Descending) {
				comparison *= -1;
			}

			return comparison;
		}

	}
}
