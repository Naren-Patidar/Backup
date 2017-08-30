﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.SmartVoucherServices;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder;
using Tesco.ClubcardProducts.MCAStubGenerator.ServiceInvoker;
using Tesco.ClubcardProducts.MCA.Web.Common;
using System.IO;
using System.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tesco.ClubcardProducts.MCAStubGenerator;
using System.Threading;

namespace MCAStubGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TaskScheduler uiScheduler;
        string _processingDir = String.Empty;
        string _culture = String.Empty;

        public MainWindow()
        {
            InitializeComponent();
            this.LoadSettings();            
        }

        private void LoadSettings()
        {
            try
            {
                var apis = ServicesConfig.LoadConfig();
                CollectionViewSource itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSource"));
                itemCollectionViewSource.Source = ServicesConfig.GetAPIMethodDefinitions(apis);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPrepare_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CollectionViewSource itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSource"));
                List<EndPointDefinition> eps = (List<EndPointDefinition>)itemCollectionViewSource.Source;

                List<EndPointDefinition> selectedEPS = eps.Where(ep => ep.IsSelected).ToList();

                using (StreamWriter file = new StreamWriter(System.IO.Path.Combine(this._processingDir, ServicesConfig.ENDPOINTFILE), false))
                {
                    file.Write(selectedEPS.JsonText());
                }

                this.txtResults.Text = String.Empty;

                ServiceInvoker si = new ServiceInvoker(this._processingDir, this._culture);

                si.Prepare((message, status, prepare, start) => this.UpdateGUI(message, status, prepare, start));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateGUI(string message, string status, bool prepare, bool start)
        {
            Task.Factory.StartNew(() =>
            {
                if (!String.IsNullOrWhiteSpace(status))
                {
                    this.lblStatus.Content = status;
                }

                if (!String.IsNullOrWhiteSpace(message))
                {
                    this.txtResults.AppendText(String.Format("{0}{1}", message, Environment.NewLine));
                    this.txtResults.ScrollToEnd();
                }
                this.btnPrepare.IsEnabled = prepare;
                this.btnStart.IsEnabled = start;
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ServiceInvoker si = new ServiceInvoker(this._processingDir, this._culture);
                si.StartProcessing((message, status, prepare, start) => this.UpdateGUI(message, status, prepare, start));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnShowDetails_Click(object sender, RoutedEventArgs e)
        {
            ProcessingStatus window = new ProcessingStatus();
            window.Owner = this;
            window.ShowDialog();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.lblStatus.Content = String.Empty;
            this.txtResults.Text = String.Empty;
            this.btnPrepare.IsEnabled = true;
            this.btnStart.IsEnabled = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            this._processingDir = System.IO.Path.Combine(ConfigurationManager.AppSettings["DataRoot"], "ProcessingDirectory");
            this._culture = ConfigurationManager.AppSettings["CurrentCulture"];
        }        
    }
}
