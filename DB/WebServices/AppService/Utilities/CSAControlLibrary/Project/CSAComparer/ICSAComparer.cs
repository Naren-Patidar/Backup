using System;

namespace Fujitsu.eCrm.Generic.ControlLibrary.CSAComparer {

	/// <summary>
	/// Extension of IComparer Interface for identifying direction of order by
	/// </summary>
	public interface ICSAComparer : System.Collections.IComparer {

		/// <summary>Get/Set order by ascending (equals not descending)</summary>
		bool Ascending { get; set; }

		/// <summary>Get/Set order by descending (equals not ascending)</summary>
		bool Descending { get; set; }
	}
}
