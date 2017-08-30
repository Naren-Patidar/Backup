using Microsoft.ApplicationBlocks.ExceptionManagement;
using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Tesco.NGC.Utils {

	/// <summary>
	/// Class for filtering exceptions to publish via the default publisher
	/// </summary>
	public class PublishFilter : IExceptionPublisher {

		private static Regex re = new Regex(@"[\t\s\n]",RegexOptions.Multiline|RegexOptions.Compiled);

		/// <summary>
		/// Constructor
		/// </summary>
		public PublishFilter() {
		}

		void IExceptionPublisher.Publish(Exception exception, NameValueCollection additionalInfo, NameValueCollection configSettings) {

			// Should this exception be published by this publisher
			if (exception is CrmServiceException) {
				CrmServiceException crmServiceException = (CrmServiceException)exception;
				string rawExcludeWheres = configSettings["excludeWhere"];
				if (rawExcludeWheres != null)
				{
					rawExcludeWheres = re.Replace(rawExcludeWheres,"");
					string[] excludeWheres = rawExcludeWheres.Split('|');
					foreach (string excludeWhere in excludeWheres) 
					{
						string[] excludeWhereProperty = excludeWhere.Split(',');
						if (excludeWhereProperty.Length > 0) 
						{
							if ((crmServiceException.Actor() != excludeWhereProperty[0].Trim()) && (excludeWhereProperty[0].Trim() != "*")) 
							{
								continue;
							}
						}
						if (excludeWhereProperty.Length > 1) 
						{
							if ((crmServiceException.Category() != excludeWhereProperty[1].Trim()) && (excludeWhereProperty[1].Trim() != "*")) 
							{
								continue;
							}
						}
						if (excludeWhereProperty.Length > 2) 
						{
							if ((crmServiceException.Name() != excludeWhereProperty[2].Trim()) && (excludeWhereProperty[2].Trim() != "*")) 
							{
								continue;
							}
						}
						return;
					}
				}
			}				

			// Get the Default Publisher
			Microsoft.ApplicationBlocks.ExceptionManagement.DefaultPublisher publisher = new Microsoft.ApplicationBlocks.ExceptionManagement.DefaultPublisher();
			// Publish the exception and any additional information
			publisher.Publish(exception, additionalInfo, null);
		}
	}
}