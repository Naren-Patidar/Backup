#region Using
using System;
using System.Collections;
using System.Globalization;
using System.Resources;
using System.Reflection;
using System.Threading;
#endregion

namespace Fujitsu.eCrm.Generic.SharedUtils
{

	#region Header
	/// <department>Fujitsu, e-Innovations, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// 
	/// <summary>
	/// Builds an Error Message using resources and supplied parameters
	/// </summary>
	/// 
	/// <development> 
		///    <version number="1.10" day="16" month="01" year="2003">
		///			<developer>Tom Bedwell</developer>
		///			<checker>Steve Lang</checker>
		///			<work_packet>WP/Barcelona/046</work_packet>
		///			<description>Namespaces conform to standards</description>
	///	</version>
	///		<version number="1.01" day="29" month="10" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Stephen Lang</checker>
	///			<work_packet>Bugzilla #60</work_packet>
	///			<description>Add a static method to simply get a message from the
	///			text messages resource file.</description>
	///		</version>
	///		<version number="1.00" day="06" month="03" year="2002">
	///			<developer>Andy Kirk</developer>
	///			<checker></checker>
	///			<work_packet>WP/Dogon/001</work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	#endregion

	public class CrmStringBuilder 
	{
		#region Static

		/// <summary>
		/// Get a formatted message from the general TextMessages resource file.
		/// </summary>
		/// <param name="messageName">The index name of the text message.</param>
		/// <returns></returns>
		public static string GetMessage(string messageName) 
		{
			return BuildMessage(CrmServiceException.ResourceList,messageName,"");
		}

		/// <summary>
		/// Get a formatted message from the general TextMessages resource file
		/// using parameter arguments to replace placeholders.
		/// </summary>
		/// <param name="messageName">The index name of the text message.</param>
		/// <param name="args">The arguments to replace the placeholders.</param>
		/// <returns></returns>
		public static string GetMessage(string messageName, params string[] args) 
		{
			return BuildMessage(CrmServiceException.ResourceList,messageName,args);
		}
	
		/// <summary>
		/// Generate a formatted Error Message using UI Culture
		/// </summary>
		/// <param name="resourceManager">The resource</param>
		/// <param name="messageName">The name of the template message to use</param>
		/// <param name="args">Values to insert into the template message</param>
		/// <returns>The formatted Error Message</returns>
		public static string BuildMessage(
			ResourceManager resourceManager,
			string messageName, 
			params string[] args) {

			string message;
			try {
				message = resourceManager.GetString(messageName,Thread.CurrentThread.CurrentUICulture);
				message = String.Format(message, args);
			} catch {
				message = messageName;
				foreach (string arg in args) {
					message += " '"+arg+"'";
				}
			}
			return message;
		}
			
		/// <summary>
		/// Generate a formatted Error Message
		/// </summary>
		/// <param name="resourceList">The list of resources</param>
		/// <param name="messageName">The name of the template message to use</param>
		/// <param name="args">Values to insert into the template message</param>
		/// <returns>The formatted Error Message</returns>
		public static string BuildMessage(
			Stack resourceList,
			string messageName, 
			params string[] args) {

			string message = String.Empty;
			foreach (ResourceManager resourceManager in resourceList) {

				try {
					message = resourceManager.GetString(messageName,Thread.CurrentThread.CurrentUICulture);
					if (message == null) {
						continue;
					}
				} catch {
					continue;
				}

				try {
					message = String.Format(message, args);
					return message;
				} catch {
					break;
				}
			}

			// Last Resort, if all else fails
			message = messageName;
			foreach (string arg in args) {
				message += " '"+arg+"'";
			}
			return message;
		}
		#endregion
	}
}