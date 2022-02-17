using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LiveCharts;
using System.ComponentModel;
using LiveCharts.Defaults;
using LiveCharts.Uwp;
using System.Linq;
using System;
using System.Diagnostics;



// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Invoice_Free
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewStats : Page, INotifyPropertyChanged
    {
        public Func<double, string> Formatter { get; set; }

        private int CustomerTotal => App.companyActive.TotalCustomers;

        private bool _thisYear;
        private bool _priorYear;
        private bool _previousYear;

        private string prior_Year;
        private string previous_Year;
        private string this_Year;

        public IChartValues ThisYearValues { get; set; }
        public IChartValues PreviousYearValues { get; set; }
        public IChartValues PriorYearValues { get; set; }

        public string[] Labels { get; set; } = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public SeriesCollection PieSeriesCollection { get; set; }

        public ViewStats()
        {
            this.InitializeComponent();

            InitializeBarChart();
            InitializePieChart();

            Formatter = val => val.ToString("C");
            DataContext = this;
        }

        private void InitializeBarChart()
        {
            int date = DateTime.Now.Year;
            prior_Year = (date - 2).ToString();
            previous_Year = (date - 1).ToString();
            this_Year = date.ToString();

            thisYear = true;
            previousYear = true;
            priorYear = false;

            GetChartValues();
            //BARCHART.Series.Chart.Updater.Run(true, true);
            BARCHART.SizeChanged += AdjustColumnSize;
        }

        private void GetChartValues()
        {
            PriorYearValues = new ChartValues<float>(App.companyActive.PriorRevenue);
            PreviousYearValues = new ChartValues<float>(App.companyActive.PreviousRevenue);
            ThisYearValues = new ChartValues<float>(App.companyActive.Revenue);
        }
        private void AdjustColumnSize(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine("AdjustColumnSize Was Called");
            Debug.WriteLine(BARCHART.Series.Chart.AreComponentsLoaded);

            BARCHART.Series.Chart.Updater.Run(true, false);
        }

        private void InitializePieChart()
        {

            PieSeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Completed",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(App.companyActive.CompleteInvoices) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Pending",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(App.companyActive.PendingInvoices) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Quotes",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(App.companyActive.TotalQuotes) },
                    DataLabels = true
                }
            };

            //adding values or series will update and animate the chart automatically
            //SeriesCollection.Add(new PieSeries());
            //SeriesCollection[0].Values.Add(5);
        }

        /*Bar Chart*/
        public bool priorYear
        {
            get { return _priorYear; }
            set
            {
                _priorYear = value;
                OnPropertyChanged("priorYear");
            }
        }

        public bool previousYear
        {
            get { return _previousYear; }
            set
            {
                _previousYear = value;
                OnPropertyChanged("previousYear");
            }
        }

        public bool thisYear
        {
            get { return _thisYear; }
            set
            {
                _thisYear = value;
                OnPropertyChanged("thisYear");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            BARCHART.Series.Chart.Updater.Run(true, false);
        }

        /*End Bar Chart*/

        /*Pie Chart*/
        /*End Pie Chart*/

    }
}


