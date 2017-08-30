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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EncryptDecryptTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        OAuthBase _cipher = new OAuthBase();

        private void btnParse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(this.txtInput.Text))
                {
                    this.txtResult.Text = "Input text data is blank";
                    return;
                }

                string encryptedData = String.Empty, decryptedData = String.Empty;

                try
                {
                    encryptedData = this.chkMCACrypt.IsChecked.Value ? CryptoUtility.EncryptTripleDES(this.txtInput.Text) : this._cipher.EncryptText(this.txtInput.Text);
                }
                catch {}

                try
                {
                    decryptedData = this.chkMCACrypt.IsChecked.Value ? CryptoUtility.DecryptTripleDES(this.txtInput.Text) : this._cipher.DecryptText(this.txtInput.Text);
                }
                catch { }

                this.txtResult.Text = String.Format(@"Encrypted Text: {0}
Decrypted Text: {1}", encryptedData, decryptedData);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            this.txtInput.Text = this.txtResult.Text = String.Empty;
        }

        private void btnEncryptFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(this.txtInput.Text))
                {
                    this.txtResult.Text = "Input File path is blank";
                    return;
                }

                if (!System.IO.File.Exists(this.txtInput.Text))
                {
                    this.txtResult.Text = "Input File path is not present";
                    return;
                }

                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "encryptedclientauth.json"; // Default file name
                dlg.DefaultExt = ".json"; // Default file extension
                dlg.Filter = "Json files (*.json)|*.json;*.json|All files (*.*)|*.*"; // Filter files by extension

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result.HasValue && result.Value)
                {
                    this._cipher.EncryptFile(this.txtInput.Text, dlg.FileName);
                    this.txtResult.Text = String.Format("File has been successfully encrypted and placed at - {0}", dlg.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDecryptFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(this.txtInput.Text))
                {
                    this.txtResult.Text = "Input File path is blank";
                    return;
                }

                if (!System.IO.File.Exists(this.txtInput.Text))
                {
                    this.txtResult.Text = "Input File path is not present";
                    return;
                }

                this.txtResult.Text = this._cipher.DecryptFile(this.txtInput.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}