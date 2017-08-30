using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tesco.ClubcardProducts.MCAStubGenerator.ServiceInvoker;
using System.Configuration;
using Newtonsoft.Json;
using System.IO;

namespace Tesco.ClubcardProducts.MCAStubGenerator
{
    /// <summary>
    /// Interaction logic for ProcessingStatus.xaml
    /// </summary>
    public partial class ProcessingStatus : Window
    {
        public ProcessingStatus()
        {
            InitializeComponent();
            this.LoadData();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadData()
        {
            try
            {
                string processingDir = System.IO.Path.Combine(ConfigurationManager.AppSettings["DataRoot"], "ProcessingDirectory");
                string culture = ConfigurationManager.AppSettings["CurrentCulture"];

                string fileP = System.IO.Path.Combine(processingDir, ServicesConfig.PAYLOADFILE);
                var executableEndPoints = JsonConvert.DeserializeObject<List<ExecutableEndPoint>>(File.ReadAllText(fileP));

                CollectionViewSource itemCollectionViewSource;
                itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSource"));
                itemCollectionViewSource.Source = executableEndPoints;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
