using System;
using System.Threading;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Configuration.Assemblies;
using Fujitsu.eCrm.Generic.SharedUtils;
using Fujitsu.eCrm.Generic.CrmService;
using System.Xml;

namespace Fujitsu.eCrm.Seoul.PosSocketsService
{
	/// <summary>
	/// Summary description for ProjectInstaller.
	/// </summary>
	[RunInstaller(true)]
	public class ProjectInstaller : System.Configuration.Install.Installer
	{
		private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
		private System.ServiceProcess.ServiceInstaller serviceInstaller1;

		private static string instUser=null;
		private static string instPass=null;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stateSaver"></param>
		public override void Install(System.Collections.IDictionary stateSaver)
		{
			base.Install(stateSaver);
			
			System.Reflection.Assembly Asm = System.Reflection.Assembly.GetExecutingAssembly();
			System.IO.FileInfo Finfo = new System.IO.FileInfo(Asm.Location + ".config");

			char [] sep = {'\\'};
			string[] s = Asm.Location.Split(sep);
			string progDir = s[0];
			int ccc=s.Length;
			for (int i=1; i < ccc-3; i++)
			{
				progDir += "\\"+s[i];
			}
			progDir += "\\Common Files\\NGC";

			string myHandler = "BangkokPosHandler";

			string locLoc = string.Empty;
			locLoc = this.Context.Parameters["LOCLOC"];

			string myCulture = string.Empty;
			myCulture = this.Context.Parameters["CULT"];

			if ( myCulture == string.Empty ) 
			{
				myCulture = this.Context.Parameters["SVCCULT"];
			}

			if ( myCulture != null )
			{
				if ( myCulture == "ko-KR" )
				{
					myHandler = "SeoulPosHandler";
				}

				char[] commas = ",".ToCharArray();
				char[] equals = "=".ToCharArray();

				// Storage for assembly versions
				string suVer="";
				string emVer="";
				string suPub="";
				string emPub="";

				WebException aaa = new WebException();
				// Get all the assemblies currently loaded in the application domain.
				Assembly[] myAssemblies = Thread.GetDomain().GetAssemblies();

				// Get the dynamic assembly named 'MyAssembly'. 
				Assembly myAssembly = null;
				for (int i = 0; i < myAssemblies.Length; i++)
				{
					myAssembly = myAssemblies[i];
					if(String.Compare(myAssemblies[i].GetName().Name, "SharedUtils") == 0)
					{
						string aFullName = myAssembly.GetName().FullName;
						string[] aElements = aFullName.Split(commas);
						string aName = aElements[0];
						string[] aVersionArray = aElements[1].Split(equals);
						string aVersion = aVersionArray[1];
						string[] aCultureArray = aElements[2].Split(equals);
						string aCulture = aCultureArray[1];
						string[] aPublicKeyTokenArray = aElements[3].Split(equals);
						string aPublicKeyToken = aPublicKeyTokenArray[1];
						suPub = aPublicKeyToken;
						suVer = aVersion;
					}
					if(String.Compare(myAssemblies[i].GetName().Name, "Microsoft.ApplicationBlocks.ExceptionManagement.Interfaces") == 0)
					{
						string aFullName = myAssembly.GetName().FullName;
						string[] aElements = aFullName.Split(commas);
						string aName = aElements[0];
						string[] aVersionArray = aElements[1].Split(equals);
						string aVersion = aVersionArray[1];
						string[] aCultureArray = aElements[2].Split(equals);
						string aCulture = aCultureArray[1];
						string[] aPublicKeyTokenArray = aElements[3].Split(equals);
						string aPublicKeyToken = aPublicKeyTokenArray[1];
						emPub = aPublicKeyToken;
						emVer = aVersion;
					}
				}

				System.Xml.XmlDocument XmlDocument = new System.Xml.XmlDocument();
				XmlDocument.Load(Finfo.FullName);
	
				string cItem = "add";
				XmlNodeList cNode = XmlDocument.DocumentElement.GetElementsByTagName(cItem);

				string el1;

				for (int i=0; i < cNode.Count; i++)
				{
					el1 = cNode[i].Attributes[0].InnerText;
					if ( el1 == "CultureDefault" )
					{
						cNode[i].Attributes[1].InnerText = myCulture;
						//System.Diagnostics.EventLog.WriteEntry("NGC Batch Service", "culture set to "+myCulture);
					}
					if ( el1 == "UserCultureDefault" )
					{
						cNode[i].Attributes[1].InnerText = myCulture;
						//System.Diagnostics.EventLog.WriteEntry("NGC Batch Service", "culture set to "+myCulture);
					}
					if ( el1 == "PosHandlerDll" )
					{
						cNode[i].Attributes[1].InnerText = myHandler;
						//System.Diagnostics.EventLog.WriteEntry("NGC Batch Service", "culture set to "+myCulture);
					}
					if ( el1 == "LocalizationDirectory" ) 
					{
						if ( locLoc == string.Empty ) 
						{
							cNode[i].Attributes[1].InnerText = progDir+"\\Localization";
						} 
						else 
						{
							cNode[i].Attributes[1].InnerText = locLoc;
						}
					}

				}

				cItem = "section";
				XmlNodeList bNode = XmlDocument.DocumentElement.GetElementsByTagName(cItem);

				for (int i=0; i < bNode.Count; i++)
				{
					el1 = bNode[i].Attributes[0].InnerText;
					if ( el1 == "exceptionManagement" )
					{
						bNode[i].Attributes[1].InnerText = "Microsoft.ApplicationBlocks.ExceptionManagement.ExceptionManagerSectionHandler, Microsoft.ApplicationBlocks.ExceptionManagement, culture=Neutral, version="+emVer+", publicKeyToken="+emPub;
					}
				}

				cItem = "publisher";
				XmlNodeList aNode = XmlDocument.DocumentElement.GetElementsByTagName(cItem);
	
				for (int i=0; i < aNode.Count; i++)
				{
					el1 = aNode[i].Attributes[1].InnerText;
					if ( el1 == "SharedUtils" )
					{
						aNode[i].Attributes[1].InnerText = "SharedUtils, culture=Neutral, version="+suVer+", publicKeyToken="+suPub;
					}
				}

				XmlDocument.Save(Finfo.FullName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void BeforeInstallEventHandler(object sender, InstallEventArgs e)
		{

			// This method overrides the default process installer to create the
			// service to run under a username specified through the username
			// dialogue or from the command line.

			System.ServiceProcess.ServiceProcessInstaller x = (System.ServiceProcess.ServiceProcessInstaller)sender;

			//System.Diagnostics.EventLog.WriteEntry("NGC Batch Service", x.Username+":"+x.Password);

			x.Account = System.ServiceProcess.ServiceAccount.User;
			string luser = string.Empty;
			string lpass = string.Empty;

			luser = x.Parent.Context.Parameters["SVCUSERNAME"];
			lpass = x.Parent.Context.Parameters["SVCPASSWORD"];

			//System.Diagnostics.EventLog.WriteEntry("NGC Batch Service1", luser+":"+lpass);

			if (luser == string.Empty) 
			{
				x.Username = x.Parent.Context.Parameters["CLIUSER"];
				x.Password = x.Parent.Context.Parameters["CLIPASS"];
			} 
			else 
			{
				x.Username = luser;
				x.Password = lpass;
			}

			//string myEV = "NGC Batch Service will run under user "+x.Username;
			//System.Diagnostics.EventLog.WriteEntry("NGC Batch Service", myEV);
			instUser = x.Username;
			instPass = x.Password;


		}


		// <summary>
		// Required designer variable.
		// </summary>
		//private System.ComponentModel.Container components = null;

		/// <summary>
		/// This call is required by the Designer.
		/// </summary>
		public ProjectInstaller()
		{
			
			InitializeComponent();

			// TODO: Add any initialization after the InitComponent call
			foreach (PerformanceCounterInstaller installer in PerfMon.Installer(Assembly.GetExecutingAssembly()).Values) {
				this.Installers.Add(installer);
			}		
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
			this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
			// 
			// serviceProcessInstaller1
			// 
			this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
			this.serviceProcessInstaller1.Password = null;
			this.serviceProcessInstaller1.Username = null;
			// 
			// serviceInstaller1
			// 
			this.serviceProcessInstaller1.BeforeInstall += new System.Configuration.Install.InstallEventHandler(this.BeforeInstallEventHandler);

			this.serviceInstaller1.ServiceName = "NGCPOSService";
			this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
			// 
			// ProjectInstaller
			// 
			this.Installers.AddRange(new System.Configuration.Install.Installer[] {
																					  this.serviceProcessInstaller1,
																					  this.serviceInstaller1});

		}
		#endregion
	}
}
