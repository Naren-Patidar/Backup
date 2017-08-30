using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DBConfigurationXmlUtility.Entities;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using DBConfigurationXmlUtility.Classes;

namespace DBConfigurationXmlUtility
{
    public partial class ConfigurationUtility : Form
    {
        #region Fields

        public static AllCultures allCultures = new AllCultures();

        string culturesFileName = string.Empty;
        string defaultExcelFile = string.Empty;
        string defaultOutputFolder = string.Empty;
        string schemaFile = string.Empty;
        string configurationFileName = string.Empty;

        #endregion

        #region Constructor
        public ConfigurationUtility()
        {
            InitializeComponent();
        }
        #endregion

        #region Handlers

        /// <summary>
        /// Event handler for the Load Event.
        /// Populate the default values to the UI controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigurationUtility_Load(object sender, EventArgs e)
        {
            try
            {
                LoadApplicationConfiguration();
                txtExcel.Text = defaultExcelFile;
                LoadCultures();
                BindClutures();
                BindOutputFolder();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Handler for btnExit click event.
        /// Exit the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Handler for btnBrowse Click event.
        /// Opens the file browse dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (selectExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtExcel.Text = selectExcel.FileName;
            }
        }

        /// <summary>
        /// Event handler for btnGenerate Click event.
        /// Generates the xml configuraiton files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    List<Culture> cultures = new List<Culture>();

                    foreach (int i in lstCultures.SelectedIndices)
                    {
                        Culture culture = (Culture)lstCultures.Items[i];
                        cultures.Add(culture);
                    }
                    foreach (Culture c in cultures)
                    {
                        DataTable data = Utility.exceldata(txtExcel.Text, c.WorksheetName);
                        string config = ConfigurationXML.GenerateXSDConfigurationXml(schemaFile, data);                        
                        Utility.WriteXmlFile(Path.Combine(txtOutput.Text, c.Country,configurationFileName), config);
                    }
                    MessageBox.Show("configuration xml files are generated. Please check the output folder.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            if (selectOutput.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtOutput.Text = selectOutput.SelectedPath;
            }
        }

        #endregion

        #region Private Members

        /// <summary>
        /// Method to load cultures from xml file
        /// </summary>
        private void LoadCultures()
        {
            string culturesFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, culturesFileName);
            string xmlCultures = Utility.LoadXMLFile(culturesFile);
            allCultures = (AllCultures)Utility.XMLStringToObject(typeof(AllCultures), xmlCultures);
        }

        /// <summary>
        /// Method to bind culture combobox
        /// </summary>
        private void BindClutures()
        {
            // bind the culture combobox
            lstCultures.DataSource = allCultures.Cultures;
            lstCultures.DisplayMember = "FullName";
            lstCultures.ValueMember = "Name";
        }

        /// <summary>
        /// Method to set the text of output folder
        /// </summary>
        private void BindOutputFolder()
        {
            // set the output folder
            string outFolder = (ConfigurationManager.AppSettings.AllKeys.Contains("OutputDirectory")) ? ConfigurationManager.AppSettings["OutputDirectory"] : string.Empty;
            outFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, outFolder));
            txtOutput.Text = outFolder;
            // validate the output folder
            if (!Directory.Exists(txtOutput.Text))
            {
                Directory.CreateDirectory(txtOutput.Text);
            }
        }

        /// <summary>
        /// Method to validate the user inputs.
        /// </summary>
        /// <returns></returns>
        private bool ValidateInput()
        {
            bool chk = true;
            List<string> error = new List<string>();
            if (string.IsNullOrEmpty(txtExcel.Text))
            {
                error.Add("Please select the All_Configuration.xlsx file");
                chk = false;
            }
            if (!File.Exists(txtExcel.Text))
            {
                error.Add("File does not exist or moved. Please select again.");
                chk = false;
            }
            if (lstCultures.SelectedItems.Count == 0)
            {
                error.Add("Please select at least one culture.");
                chk = false;
            }
            //show errors
            string formattedError = string.Empty;
            if (error.Count > 0)
            {
                formattedError = "There are following errors, Please correct and try again:" + Environment.NewLine;
                int index = 0;
                error.ForEach(er => formattedError += string.Format("{0}. {1}{2}", ++index, er, Environment.NewLine));
                MessageBox.Show(formattedError);
            }
            return chk;
        }

        /// <summary>
        /// Method to initialize the utility configurations
        /// </summary>
        private void LoadApplicationConfiguration()
        {
            defaultExcelFile = (ConfigurationManager.AppSettings.AllKeys.Contains("DefaultExcelFile")) ? ConfigurationManager.AppSettings["DefaultExcelFile"] : string.Empty;
            defaultExcelFile = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultExcelFile));
            culturesFileName = (ConfigurationManager.AppSettings.AllKeys.Contains("CulturesXMLFile")) ? ConfigurationManager.AppSettings["CulturesXMLFile"] : string.Empty;
            defaultOutputFolder = (ConfigurationManager.AppSettings.AllKeys.Contains("DefaultOutputFolder")) ? ConfigurationManager.AppSettings["DefaultOutputFolder"] : string.Empty;
            schemaFile = (ConfigurationManager.AppSettings.AllKeys.Contains("SchemaFile")) ? ConfigurationManager.AppSettings["SchemaFile"] : string.Empty;
            configurationFileName = (ConfigurationManager.AppSettings.AllKeys.Contains("ConfigurationFileName")) ? ConfigurationManager.AppSettings["ConfigurationFileName"] : string.Empty;
        }

        #endregion
    }
}
