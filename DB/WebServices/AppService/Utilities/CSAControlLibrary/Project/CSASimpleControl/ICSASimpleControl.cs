using System;
using System.Collections;

namespace Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl {

	/// <summary>
	/// Summary description for ISimpleControl.
	/// </summary>
	public interface ISimpleControl {

		/// <summary>
		/// Extract the Control Values and insert it into a Hashtable
		/// </summary>
		/// <param name="ht">The Hashtable of values</param>
		void GetValue(Hashtable ht);

	}
}
