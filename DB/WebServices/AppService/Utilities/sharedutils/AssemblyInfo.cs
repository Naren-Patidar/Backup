using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Permissions;

//
// Included for Code Access Security
//

using System.Data.OleDb;
using System.Net;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Security;

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyTitle("Barcelona Shared Utilities")]
[assembly: AssemblyDescription("A Class Library required by many of the Barcelona Products")]
[assembly: AssemblyConfiguration("Debug")]
[assembly: AssemblyCompany("Fujitsu Consulting")]
[assembly: AssemblyProduct("Barcelona Shared Utilities")]
[assembly: AssemblyCopyright("Fujitsu Consulting, 2003")]
[assembly: AssemblyTrademark("Fujitsu Consulting")]
[assembly: AssemblyCulture("")]			// Lacks any culture

// Assign Code Access Security settings. The following permissions
// have been set to RequestMinimum because if left out will cause
// a security exception, even if it intially appear that it is
// needed
//
//	EventLog
//	OleDbPermission
//	PeformanceCounter
//	SqlClient
//

[assembly:DnsPermissionAttribute(SecurityAction.RequestRefuse)]
[assembly:EnvironmentPermissionAttribute(SecurityAction.RequestRefuse)]
[assembly:EventLogPermissionAttribute(SecurityAction.RequestMinimum)]
[assembly:FileDialogPermissionAttribute(SecurityAction.RequestRefuse)]
[assembly:FileIOPermissionAttribute(SecurityAction.RequestMinimum)]
[assembly:IsolatedStorageFilePermissionAttribute(SecurityAction.RequestRefuse)]
[assembly:OleDbPermissionAttribute(SecurityAction.RequestMinimum)]
[assembly:PerformanceCounterPermissionAttribute(SecurityAction.RequestMinimum)]
[assembly:PublisherIdentityPermissionAttribute(SecurityAction.RequestRefuse)]
[assembly:ReflectionPermissionAttribute(SecurityAction.RequestRefuse)]
[assembly:RegistryPermissionAttribute(SecurityAction.RequestRefuse)]
[assembly:SecurityPermissionAttribute(SecurityAction.RequestRefuse)]
[assembly:SiteIdentityPermissionAttribute(SecurityAction.RequestRefuse)]
[assembly:SqlClientPermissionAttribute(SecurityAction.RequestMinimum)]
[assembly:StrongNameIdentityPermissionAttribute(SecurityAction.RequestRefuse)]
[assembly:UIPermissionAttribute(SecurityAction.RequestRefuse)]
[assembly:UrlIdentityPermissionAttribute(SecurityAction.RequestRefuse)]
[assembly:WebPermissionAttribute(SecurityAction.RequestRefuse)]
[assembly:ZoneIdentityPermissionAttribute(SecurityAction.RequestRefuse)]

//
// In order to sign your assembly you must specify a key to use. Refer to the 
// Microsoft .NET Framework documentation for more information on assembly signing.
//
// Use the attributes below to control which key is used for signing. 
//
// Notes: 
//   (*) If no key is specified, the assembly is not signed.
//   (*) KeyName refers to a key that has been installed in the Crypto Service
//       Provider (CSP) on your machine. KeyFile refers to a file which contains
//       a key.
//   (*) If the KeyFile and the KeyName values are both specified, the 
//       following processing occurs:
//       (1) If the KeyName can be found in the CSP, that key is used.
//       (2) If the KeyName does not exist and the KeyFile does exist, the key 
//           in the KeyFile is installed into the CSP and used.
//   (*) In order to create a KeyFile, you can use the sn.exe (Strong Name) utility.
//       When specifying the KeyFile, the location of the KeyFile should be
//       relative to the project output directory which is
//       %Project Directory%\obj\<configuration>. For example, if your KeyFile is
//       located in the project directory, you would specify the AssemblyKeyFile 
//       attribute as [assembly: AssemblyKeyFile("..\\..\\mykey.snk")]
//   (*) Delay Signing is an advanced option - see the Microsoft .NET Framework
//       documentation for more information on this.
//
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile(@"..\..\SharedUtils.snk")]
[assembly: AssemblyKeyName("")]
