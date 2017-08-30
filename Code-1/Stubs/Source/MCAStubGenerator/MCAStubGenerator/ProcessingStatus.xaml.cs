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
        string _processingDir = String.Empty;
        string _culture = String.Empty;

        public ProcessingStatus(string path, string culture)
        {
            this._processingDir = path;
            this._culture = culture;

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
                List<ExecutableEndPoint> execPs = new List<ExecutableEndPoint>();

                var payLoadFiles = Directory.GetFiles(this._processingDir, "PayLoadEndPoints-*");

                foreach(string pf in payLoadFiles)
                {
                    string fileP = System.IO.Path.Combine(this._processingDir, pf);
                    var executableEndPoints = JsonConvert.DeserializeObject<List<ExecutableEndPoint>>(File.ReadAllText(fileP));
                    execPs.AddRange(executableEndPoints);
                }

                CollectionViewSource itemCollectionViewSource;
                itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSource"));
                itemCollectionViewSource.Source = execPs;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
