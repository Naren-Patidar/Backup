using System;
using System.Threading;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Reflection;
using System.Reflection.Emit;
using System.Configuration.Assemblies;
using System.Xml;
using System.Web;

namespace NGCwebinstaller
{
    /// <summary>
    /// Summary description for ProjectInstaller.
    /// </summary>
    [RunInstaller(true)]
    public class ProjectInstaller : System.Configuration.Install.Installer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);

            // Locate web.config because we need to edit it.
            // First, identify where we are from the path of the executing assembly...
            System.Reflection.Assembly Asm = System.Reflection.Assembly.GetExecutingAssembly();

            // From here, strip out the assembly name, leaving the path (minus the bin bit)...
            char[] sep = { '\\' };

            string[] splitAsm = Asm.Location.Split(sep);
            string instDir = splitAsm[0];
            int ccc = splitAsm.Length;
            for (int i = 1; i < ccc - 2; i++)
            {
                instDir += "\\" + splitAsm[i];
            }
            // Finally put the directory and web.config together...
            //instDir="C:\\Inetpub\\wwwroot\\NGCSSC_V3";
            System.IO.FileInfo Finfo = new System.IO.FileInfo(instDir + "\\web.config");

            string strLDAPPath = string.Empty;
            strLDAPPath = this.Context.Parameters["LDAP"];
            strLDAPPath = strLDAPPath.Substring(0, strLDAPPath.Length - 1);

            string strDomain = string.Empty;
            strDomain = this.Context.Parameters["DOMN"];

            if (strLDAPPath != null && strDomain != null)
            {

                System.Xml.XmlDocument XmlDocument = new System.Xml.XmlDocument();
                XmlDocument.Load(Finfo.FullName);

                string cItem = "add";
                XmlNodeList cNode = XmlDocument.DocumentElement.GetElementsByTagName(cItem);

                string el1;

                for (int i = 0; i < cNode.Count; i++)
                {
                    el1 = cNode[i].Attributes[0].InnerText;
                    if (el1 == "LDAPPath")
                    {
                        cNode[i].Attributes[1].InnerText = strLDAPPath;
                        System.Diagnostics.EventLog.WriteEntry("NGC ", "LDAP path set to " + strLDAPPath);
                    }
                    if (el1 == "Domain")
                    {
                        cNode[i].Attributes[1].InnerText = strDomain;
                        System.Diagnostics.EventLog.WriteEntry("NGC ", "Domain name set to " + strDomain);
                    }
                    if (el1 == "LoyaltyEntityServiceLayerPath")
                    {
                        cNode[i].Attributes[1].InnerText = instDir + "\\bin\\LoyaltyEntityServiceLayer.dll";
                        System.Diagnostics.EventLog.WriteEntry("NGC ", "BusinessLayer path set to " + instDir + "\\bin\\LoyaltyEntityServiceLayer.dll");
                    }
                    if (el1 == "CampaignEntityServiceLayerPath")
                    {
                        cNode[i].Attributes[1].InnerText = instDir + "\\bin\\CampaignEntityServiceLayer.dll";
                        System.Diagnostics.EventLog.WriteEntry("NGC ", "BusinessLayer path set to " + instDir + "\\bin\\CampaignEntityServiceLayer.dll");
                    }
 
                }
                System.Diagnostics.EventLog.WriteEntry("FullName", Finfo.FullName);
                XmlDocument.Save(Finfo.FullName);
            }
        }

        public ProjectInstaller()
        {
            // This call is required by the Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitComponent call
        }

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion
    }
}
