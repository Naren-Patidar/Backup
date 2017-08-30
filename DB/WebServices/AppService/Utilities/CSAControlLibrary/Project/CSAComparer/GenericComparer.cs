using System;

namespace Fujitsu.eCrm.Generic.ControlLibrary.CSAComparer {

	/// <summary>
	/// Provides basic support for any class implementing ICSAComparer
	/// </summary>
	public abstract class GenericComparer {

		private bool ascending = true; // default a-z, 0-9

		/// <summary>Get/Set order by ascending (equals not descending)</summary>
		public bool Ascending {
			get { return this.ascending; }
			set { this.ascending = value; }
		}

		/// <summary>Get/Set order by descending (equals not ascending)</summary>
		public bool Descending {
			get { return !this.ascending; }
			set { this.ascending = !value; }
		}

	}
}
