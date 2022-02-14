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

        public SeriesCollection SeriesCollection { get; set; }

        public ViewStats()
        {
            InitializeComponent();

            InitializeBarChart();
            InitializePieChart();

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
            PriorYearValues = new ChartValues<int>(new int[] { 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24 });
            PreviousYearValues = new ChartValues<int>(new int[] { 3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36 });
            ThisYearValues = new ChartValues<int>(new int[] { 4, 8, 12, 16, 20, 24, 28, 32, 36, 40, 44, 48 });
        }
        private void AdjustColumnSize(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine("AdjustColumnSize Was Called");
            Debug.WriteLine(BARCHART.Series.Chart.AreComponentsLoaded);

            BARCHART.Series.Chart.Updater.Run(true, false);
        }

        private void InitializePieChart()
        {
            int date = DateTime.Now.Year;
            prior_Year = (date - 2).ToString();
            previous_Year = (date - 1).ToString();
            this_Year = date.ToString();

            SeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Invoices Created",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(8) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "New Customers",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(6) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Quotes Created",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(10) },
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


