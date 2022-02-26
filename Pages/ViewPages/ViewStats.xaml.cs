using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewStats : Page
    {
        public Color SelectedYearColor { get; private set; }

        public static Func<double, string> Formatter
        {
            get { return val => val.ToString("C"); }
        }

        private int CustomerTotal;
        public ObservableCollection<ISeries> PieSeriesCollection { get; set; }
        public ObservableCollection<ISeries> BarSeriesCollection { get; set; }

        public ViewStats()
        {
            SelectedYearColor = Colors.Olive;
            this.InitializeComponent();

            InitializeBarChart();
            InitializePieChart();
            Debug.WriteLine("App.companyActive: " + App.PathToCompanies.ToString());
            CustomerTotal = App.companyActive.TotalCustomers;
            DataContext = this;
        }

        #region BARCHART

        private ColumnSeries<double> CurrentYear = new ColumnSeries<double>()
        {
            Name = DateTime.Now.Year.ToString(),
            Values = App.companyActive.Revenue,
            Fill = new SolidColorPaint(SKColors.Olive),
            DataLabelsPadding = new LiveChartsCore.Drawing.Padding(0, 0, 20, 0),
        };

        private ColumnSeries<double> PreviousYear = new ColumnSeries<double>()
        {
            Name = (DateTime.Now.Year - 1).ToString(),
            Values = App.companyActive.PreviousRevenue,
            Fill = new SolidColorPaint(SKColors.DarkSlateGray),
            IsVisible = false,
            GroupPadding = 10
        };

        private ColumnSeries<double> PriorYear = new ColumnSeries<double>()
        {
            Name = (DateTime.Now.Year - 1).ToString(),
            Values = App.companyActive.PriorRevenue,
            Fill = new SolidColorPaint(SKColors.OrangeRed),
            IsVisible = false,
            GroupPadding = 10
        };


        private void InitializeBarChart()
        {

            ThisYear_Toggle.IsChecked = true;
            ThisYear_Toggle.Content = DateTime.Now.Year.ToString();



            PreviousYear_Toggle.IsChecked = false;
            PreviousYear_Toggle.Content = (DateTime.Now.Year - 1).ToString();

            PriorYear_Toggle.IsChecked = false;
            PriorYear_Toggle.Content = (DateTime.Now.Year - 2).ToString();

            BarSeriesCollection = new ObservableCollection<ISeries>
            {
                CurrentYear, PreviousYear , PriorYear
            };
        }

        public Axis[] XAxes { get; set; } = { new Axis
            {

                ShowSeparatorLines = true,
                Labels = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"},
                ForceStepToMin = true,
                MinLimit = null
            }
        };
        public Axis[] YAxes { get; set; } = { new Axis
            {

                ShowSeparatorLines = true,
                Labeler = Formatter,

            }
        };

        private void BarChart_ToggleClick(object sender, RoutedEventArgs e)
        {
            ToggleButton selection = (ToggleButton)sender;
            Debug.WriteLine("BarChart_ToggleClick: " + selection.Name);
            switch (selection.Name)
            {
                case "ThisYear_Toggle":
                    SelectedYearColor = Colors.Olive;
                    if (selection.IsChecked == false)
                    {
                        CurrentYear.IsVisible = false;

                    }
                    else
                    {

                        CurrentYear.IsVisible = true;
                    }

                    break;
                case "PreviousYear_Toggle":
                    SelectedYearColor = Colors.DarkSlateGray;
                    if (selection.IsChecked == false)
                    {
                        PreviousYear.IsVisible = false;
                    }
                    else
                    {
                        PreviousYear.IsVisible = true;
                    }
                    break;
                case "PriorYear_Toggle":
                    SelectedYearColor = Colors.OrangeRed;
                    if (selection.IsChecked == false)
                    {
                        PriorYear.IsVisible = false;
                    }
                    else
                    {
                        PriorYear.IsVisible = true;
                    }
                    break;

            }
            Debug.WriteLine("CurrentYear: " + CurrentYear.IsVisible);
            Debug.WriteLine("PreviousYear: " + PreviousYear.IsVisible);
            BarSeriesCollection = new ObservableCollection<ISeries>
            {
                CurrentYear, PreviousYear, PriorYear
            };
            BarChart.UpdateLayout();
        }

       
        #endregion

        #region PIECHART
        private void InitializePieChart()
        {
            int INV = App.companyActive.CompleteInvoices;
            int PENDING = App.companyActive.PendingInvoices;
            int QUOTES = App.companyActive.TotalQuotes;

            if (INV == 0 && PENDING == 0 && QUOTES == 0)
            {
                PieEmpty.Visibility = Visibility.Visible;
            }
            else
            {
                PieEmpty.Visibility = Visibility.Collapsed;
            }
            PieSeriesCollection = new ObservableCollection<ISeries>
            {
                new PieSeries<double> { Name="Complete Invoices", Values = new ObservableCollection<double> { INV }, InnerRadius = 50 },
                new PieSeries<double> { Name="Pending Invoices", Values = new ObservableCollection<double> { PENDING }, InnerRadius = 50 },
                new PieSeries<double> { Name="Total Quotes", Values = new ObservableCollection<double> { QUOTES }, InnerRadius = 50 }
               
            };
        }
        #endregion
    }
}