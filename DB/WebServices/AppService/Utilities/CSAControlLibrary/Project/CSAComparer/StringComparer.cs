using System;

namespace Fujitsu.eCrm.Generic.ControlLibrary.CSAComparer {

	/// <summary>
	/// Compare two objects, treat them as strings
	/// </summary>
	public class StringComparer : GenericComparer, ICSAComparer {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int Compare(object x, object y) {

			int comparison = ((string)x).CompareTo((string)y);
			if (this.Descending) {
				comparison *= -1;
			}

			return comparison;
		}

	}
}
