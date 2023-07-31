using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace CurrencyConverter
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BindCurrency();
        }
        private void BindCurrency()
        {
            DataTable dtCurrency = new DataTable();
            dtCurrency.Columns.Add("Text");
            dtCurrency.Rows.Add("EUR");
            dtCurrency.Rows.Add("USD");
            dtCurrency.Rows.Add("JPY");
            dtCurrency.Rows.Add("BGN");
            dtCurrency.Rows.Add("CZK");
            dtCurrency.Rows.Add("DKK");
            dtCurrency.Rows.Add("GBP");
            dtCurrency.Rows.Add("HUF");
            dtCurrency.Rows.Add("PLN");
            dtCurrency.Rows.Add("RON");
            dtCurrency.Rows.Add("SEK");
            dtCurrency.Rows.Add("CHF");
            dtCurrency.Rows.Add("ISK");
            dtCurrency.Rows.Add("NOK");
            dtCurrency.Rows.Add("HRK");
            dtCurrency.Rows.Add("RUB");
            dtCurrency.Rows.Add("TRY");
            dtCurrency.Rows.Add("AUD");
            dtCurrency.Rows.Add("BRL");
            dtCurrency.Rows.Add("CAD");
            dtCurrency.Rows.Add("CNY");
            dtCurrency.Rows.Add("HKD");
            dtCurrency.Rows.Add("IDR");
            dtCurrency.Rows.Add("ILS");
            dtCurrency.Rows.Add("INR");
            dtCurrency.Rows.Add("KRW");
            dtCurrency.Rows.Add("MXN");
            dtCurrency.Rows.Add("MYR");
            dtCurrency.Rows.Add("NZD");
            dtCurrency.Rows.Add("PHP");
            dtCurrency.Rows.Add("SGD");
            dtCurrency.Rows.Add("THB");
            dtCurrency.Rows.Add("ZAR");

            cmbFromCurrency.ItemsSource = dtCurrency.DefaultView;
            cmbFromCurrency.DisplayMemberPath = "Text";
            cmbToCurrency.ItemsSource = dtCurrency.DefaultView;
            cmbToCurrency.DisplayMemberPath = "Text";
        }
        private async void GetExchangeRateAsync(double started, string startCurr, string endCurr)
        {
            double final = 0;
            try
            {
                string baseCurrency = startCurr;
                string targetCurrency = endCurr;
                string apiUrl = $"https://api.freecurrencyapi.com/v1/latest?apikey=fca_live_7I5HvJAEsA5zK48UDCxgvpWwjCgDURyveWjLoGc7&currencies={targetCurrency}&base_currency={baseCurrency}";
                double valuestarted = started;

                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        string[] responseParts = jsonResponse.Split(':');
                        string rate = responseParts[responseParts.Length - 1].Trim(' ', '}', '\n', '\r');
                        if (double.TryParse(rate, NumberStyles.Float, CultureInfo.InvariantCulture, out double exchangeRate))
                        {
                            final = exchangeRate;
                        }
                        else
                        {
                            MessageBox.Show($"Failed to parse the exchange rate.");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Failed to fetch data. Status code: {response.StatusCode}");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            lblCurrency.Content = Math.Round(final * started, 2) + " " + endCurr;


        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            double ConvertedValue;
            if (txtCurrency.Text == null || txtCurrency.Text.Trim() == "" || cmbToCurrency.SelectedValue == null || cmbFromCurrency.SelectedValue == null)
            {
                MessageBox.Show("Wrong inputs.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                txtCurrency.Focus();
                return;
            }
            else if (cmbFromCurrency.Text == cmbToCurrency.Text)
            {
                MessageBox.Show("Please select diffrent currencies!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbFromCurrency.Focus();
                return;
            }
            bool succesful = Double.TryParse(txtCurrency.Text, out ConvertedValue);
            if (succesful)
            {
                GetExchangeRateAsync(ConvertedValue, cmbFromCurrency.Text, cmbToCurrency.Text);
            }
            else
            {
                MessageBox.Show("Enter valid inputs!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            lblCurrency.Content = "";
            cmbFromCurrency = null;
            cmbToCurrency = null;
        }
    }
}
