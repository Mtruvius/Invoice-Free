﻿using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using System.Diagnostics;
using Windows.UI.Xaml.Media.Imaging;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomerInvoicesView : Page
    {
        bool isHovering_AddBtn;
        BitmapSource addBtnNormal;
        BitmapSource addBtnHover;
        public CustomerInvoicesView()
        {
            InitializeComponent();
            addBtnNormal = App.addBtnNormal;
            addBtnHover = App.addBtnHover;
            AddIcon.Source = addBtnNormal;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Customer customer = (Customer)e.Parameter;
            Debug.WriteLine(customer.CustomerName);
            if (customer.InvoiceCount ==0)
            {
                NoInvoiceText.Visibility = Visibility.Visible;
                InvoiceList.Visibility = Visibility.Collapsed;
            }
            else
            {
                NoInvoiceText.Visibility = Visibility.Collapsed;
                InvoiceList.Visibility = Visibility.Visible;
            }

        }

        private void AddInvoice_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void AddInvoice_OnHover(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (isHovering_AddBtn)
            {
                isHovering_AddBtn = false;
                AddIcon.Source = addBtnNormal;
            }
            else
            {
                isHovering_AddBtn = true;
                AddIcon.Source = addBtnHover;
            }
        }
    }
}
