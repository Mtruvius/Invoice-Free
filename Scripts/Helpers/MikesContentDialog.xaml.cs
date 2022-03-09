using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Text;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Invoice_Free
{
    public sealed partial class MikesContentDialog : ContentDialog
    {
        #region Overall Dialog
        public double DialogWidth
        {
            get => (double)GetValue(DialogWidthProperty);
            set => SetValue(DialogWidthProperty, value);
        }

        DependencyProperty DialogWidthProperty = DependencyProperty.Register(
             nameof(DialogWidth),
             typeof(double),
             typeof(MikesContentDialog),
             new PropertyMetadata(default(double)));

        public double DialogContentMaxWidth
        {
            get => (double)GetValue(DialogContentMaxWidthProperty);
            set => SetValue(DialogContentMaxWidthProperty, value);
        }
        DependencyProperty DialogContentMaxWidthProperty = DependencyProperty.Register(
             nameof(DialogContentMaxWidth),
             typeof(double),
             typeof(MikesContentDialog),
             new PropertyMetadata(default(double)));

        public double DialogMinHeight
        {
            get => (double)GetValue(DialogMinHeightProperty);
            set => SetValue(DialogMinHeightProperty, value);
        }
        DependencyProperty DialogMinHeightProperty = DependencyProperty.Register(
             nameof(DialogMinHeight),
             typeof(double),
             typeof(MikesContentDialog),
             new PropertyMetadata(default(double)));

        public Brush DialogBackground
        {
            get => (Brush)GetValue(DialogBackgroundProperty);
            set => SetValue(DialogBackgroundProperty, value);
        }

        DependencyProperty DialogBackgroundProperty = DependencyProperty.Register(
             nameof(DialogBackground),
             typeof(Brush),
             typeof(MikesContentDialog),
             new PropertyMetadata(default(Brush)));


        public bool FullScreen
        {
            get => (bool)GetValue(FullScreenProperty);
            set => SetValue(FullScreenProperty, value );
        }
        DependencyProperty FullScreenProperty = DependencyProperty.Register(
             nameof(FullScreen),
             typeof(bool),
             typeof(MikesContentDialog),
             new PropertyMetadata(default(bool)));

        private int[] FullScreenMode
        {
            get => (int[])GetValue(FullScreenModeProperty);
            set => SetValue(FullScreenModeProperty, value );
        }
        DependencyProperty FullScreenModeProperty = DependencyProperty.Register(
             nameof(FullScreenMode),
             typeof(int[]),
             typeof(MikesContentDialog),
             new PropertyMetadata(default(int[])));
        #endregion

        #region Title Property
        public Brush TitleForeground
        {
            get => (Brush)GetValue(TitleForegroundProperty);
            set => SetValue(TitleForegroundProperty, value);
        }
        DependencyProperty TitleForegroundProperty = DependencyProperty.Register(
            nameof(TitleForeground),
            typeof(Brush),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(Brush)));

        public Brush TitleBackground
        {
            get => (Brush)GetValue(TitleBackgroundProperty);
            set => SetValue(TitleBackgroundProperty, value);
        }
        DependencyProperty TitleBackgroundProperty = DependencyProperty.Register(
            nameof(TitleBackground),
            typeof(Brush),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(Brush)));

        public double TitleFontSize
        {
            get => (double)GetValue(TitleFontSizeProperty);
            set => SetValue(TitleFontSizeProperty, value);
        }
        DependencyProperty TitleFontSizeProperty = DependencyProperty.Register(
            nameof(TitleFontSize),
            typeof(double),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(double)));

        public FontFamily TitleFontFamily
        {
            get => (FontFamily)GetValue(TitleFontFamilyProperty);
            set => SetValue(TitleFontFamilyProperty, value);
        }
        DependencyProperty TitleFontFamilyProperty = DependencyProperty.Register(
            nameof(TitleFontFamily),
            typeof(FontFamily),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(FontFamily)));

        public FontWeight TitleFontWeight
        {
            get => (FontWeight)GetValue(TitleFontWeightProperty);
            set => SetValue(TitleFontWeightProperty, value);
        }
        DependencyProperty TitleFontWeightProperty = DependencyProperty.Register(
            nameof(TitleFontWeight),
            typeof(FontWeight),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(FontWeight)));

        public FontStyle TitleFontStyle
        {
            get => (FontStyle)GetValue(TitleFontStyleProperty);
            set => SetValue(TitleFontStyleProperty, value);
        }
        DependencyProperty TitleFontStyleProperty = DependencyProperty.Register(
            nameof(TitleFontStyle),
            typeof(FontStyle),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(FontStyle)));

        public HorizontalAlignment TitleHorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(TitleHorizontalAlignmentProperty);
            set => SetValue(TitleHorizontalAlignmentProperty, value);
        }
        DependencyProperty TitleHorizontalAlignmentProperty = DependencyProperty.Register(
            nameof(TitleHorizontalAlignment),
            typeof(HorizontalAlignment),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(HorizontalAlignment)));
        #endregion

        #region Content

        public HorizontalAlignment DialogContentHorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(DialogContentHorizontalAlignmentProperty);
            set => SetValue(DialogContentHorizontalAlignmentProperty, value);
        }
        DependencyProperty DialogContentHorizontalAlignmentProperty = DependencyProperty.Register(
        nameof(DialogContentHorizontalAlignment),
        typeof(HorizontalAlignment),
        typeof(MikesContentDialog),
        new PropertyMetadata(default(HorizontalAlignment)));
        #endregion

        #region Content Header Property
        public string ContentHeaderText
        {
            get => (string)GetValue(ContentHeaderTextProperty);
            set => SetValue(ContentHeaderTextProperty, value);
        }
        DependencyProperty ContentHeaderTextProperty = DependencyProperty.Register(
           nameof(ContentHeaderText),
           typeof(string),
           typeof(MikesContentDialog),
           new PropertyMetadata(default(string)));

        public Brush ContentHeaderForeground
        {
            get => (Brush)GetValue(ContentHeaderForegroundProperty);
            set => SetValue(ContentHeaderForegroundProperty, value);
        }
        DependencyProperty ContentHeaderForegroundProperty = DependencyProperty.Register(
            nameof(ContentHeaderForeground),
            typeof(Brush),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(Brush)));

        public Brush ContentHeaderBackground
        {
            get => (Brush)GetValue(ContentHeaderBackgroundProperty);
            set => SetValue(ContentHeaderBackgroundProperty, value);
        }
        DependencyProperty ContentHeaderBackgroundProperty = DependencyProperty.Register(
            nameof(ContentHeaderBackground),
            typeof(Brush),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(Brush)));

        public double ContentHeaderFontSize
        {
            get => (double)GetValue(ContentHeaderFontSizeProperty);
            set => SetValue(ContentHeaderFontSizeProperty, value);
        }
        DependencyProperty ContentHeaderFontSizeProperty = DependencyProperty.Register(
            nameof(ContentHeaderFontSize),
            typeof(double),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(double)));

        public FontFamily ContentHeaderFontFamily
        {
            get => (FontFamily)GetValue(ContentHeaderFontFamilyProperty);
            set => SetValue(ContentHeaderFontFamilyProperty, value);
        }
        DependencyProperty ContentHeaderFontFamilyProperty = DependencyProperty.Register(
            nameof(ContentHeaderFontFamily),
            typeof(FontFamily),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(FontFamily)));

        public FontWeight ContentHeaderFontWeight
        {
            get => (FontWeight)GetValue(ContentHeaderFontWeightProperty);
            set => SetValue(ContentHeaderFontWeightProperty, value);
        }
        DependencyProperty ContentHeaderFontWeightProperty = DependencyProperty.Register(
            nameof(ContentHeaderFontWeight),
            typeof(FontWeight),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(FontWeight)));

        public FontStyle ContentHeaderFontStyle
        {
            get => (FontStyle)GetValue(ContentHeaderFontStyleProperty);
            set => SetValue(ContentHeaderFontStyleProperty, value);
        }
        DependencyProperty ContentHeaderFontStyleProperty = DependencyProperty.Register(
            nameof(ContentHeaderFontStyle),
            typeof(FontStyle),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(FontStyle)));

        public HorizontalAlignment ContentHeaderHorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(ContentHeaderHorizontalAlignmentProperty);
            set => SetValue(ContentHeaderHorizontalAlignmentProperty, value);
        }
        DependencyProperty ContentHeaderHorizontalAlignmentProperty = DependencyProperty.Register(
            nameof(ContentHeaderHorizontalAlignment),
            typeof(HorizontalAlignment),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(HorizontalAlignment)));

        private Visibility ContentHeaderVisibility
        {
            get => (Visibility)GetValue(ContentHeaderVisibilityProperty);
            set => SetValue(ContentHeaderVisibilityProperty, value);
        }
        DependencyProperty ContentHeaderVisibilityProperty = DependencyProperty.Register(
            nameof(ContentHeaderVisibility),
            typeof(Visibility),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(Visibility)));

        #endregion
      
        #region Footer Header Property
        public string FooterHeaderText
        {
            get => (string)GetValue(FooterHeaderTextProperty);
            set => SetValue(FooterHeaderTextProperty, value);
        }
        DependencyProperty FooterHeaderTextProperty = DependencyProperty.Register(
            nameof(FooterHeaderText),
            typeof(string),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(string)));

         public Brush FooterHeaderForeground
        {
            get => (Brush)GetValue(FooterHeaderForegroundProperty);
            set => SetValue(FooterHeaderForegroundProperty, value);
        }
        DependencyProperty FooterHeaderForegroundProperty = DependencyProperty.Register(
            nameof(FooterHeaderForeground),
            typeof(Brush),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(Brush)));

        public Brush FooterHeaderBackground
        {
            get => (Brush)GetValue(FooterHeaderBackgroundProperty);
            set => SetValue(FooterHeaderBackgroundProperty, value);
        }
        DependencyProperty FooterHeaderBackgroundProperty = DependencyProperty.Register(
            nameof(FooterHeaderBackground),
            typeof(Brush),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(Brush)));

        public double FooterHeaderFontSize
        {
            get => (double)GetValue(FooterHeaderFontSizeProperty);
            set => SetValue(FooterHeaderFontSizeProperty, value);
        }
        DependencyProperty FooterHeaderFontSizeProperty = DependencyProperty.Register(
            nameof(FooterHeaderFontSize),
            typeof(double),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(double)));

        public FontFamily FooterHeaderFontFamily
        {
            get => (FontFamily)GetValue(FooterHeaderFontFamilyProperty);
            set => SetValue(FooterHeaderFontFamilyProperty, value);
        }
        DependencyProperty FooterHeaderFontFamilyProperty = DependencyProperty.Register(
            nameof(FooterHeaderFontFamily),
            typeof(FontFamily),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(FontFamily)));

        public FontWeight FooterHeaderFontWeight
        {
            get => (FontWeight)GetValue(FooterHeaderFontWeightProperty);
            set => SetValue(FooterHeaderFontWeightProperty, value);
        }
        DependencyProperty FooterHeaderFontWeightProperty = DependencyProperty.Register(
            nameof(FooterHeaderFontWeight),
            typeof(FontWeight),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(FontWeight)));

        public FontStyle FooterHeaderFontStyle
        {
            get => (FontStyle)GetValue(FooterHeaderFontStyleProperty);
            set => SetValue(FooterHeaderFontStyleProperty, value);
        }
        DependencyProperty FooterHeaderFontStyleProperty = DependencyProperty.Register(
            nameof(FooterHeaderFontStyle),
            typeof(FontStyle),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(FontStyle)));

        public HorizontalAlignment FooterHeaderHorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(FooterHeaderHorizontalAlignmentProperty);
            set => SetValue(FooterHeaderHorizontalAlignmentProperty, value);
        }
        DependencyProperty FooterHeaderHorizontalAlignmentProperty = DependencyProperty.Register(
            nameof(FooterHeaderHorizontalAlignment),
            typeof(HorizontalAlignment),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(HorizontalAlignment)));

        private Visibility FooterHeaderVisibility
        {
            get => (Visibility)GetValue(FooterHeaderVisibilityProperty);
            set => SetValue(FooterHeaderVisibilityProperty, value);
        }
        DependencyProperty FooterHeaderVisibilityProperty = DependencyProperty.Register(
            nameof(FooterHeaderVisibility),
            typeof(Visibility),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(Visibility)));
        #endregion

        #region Buttons
        public HorizontalAlignment ButtonsAlignment
        {
            get => (HorizontalAlignment)GetValue(ButtonsAlignmentProperty);
            set => SetValue(ButtonsAlignmentProperty, value);
        }
        DependencyProperty ButtonsAlignmentProperty = DependencyProperty.Register(
            nameof(ButtonsAlignment),
            typeof(HorizontalAlignment),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(HorizontalAlignment)));

        public double ButtonsFontSize
        {
            get => (double)GetValue(ButtonsFontSizeProperty);
            set => SetValue(ButtonsFontSizeProperty, value);
        }
        DependencyProperty ButtonsFontSizeProperty = DependencyProperty.Register(
            nameof(ButtonsFontSize),
            typeof(double),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(double)));

        public FontFamily ButtonsFontFamily
        {
            get => (FontFamily)GetValue(ButtonsFontFamilyFamilyProperty);
            set => SetValue(ButtonsFontFamilyFamilyProperty, value);
        }
        DependencyProperty ButtonsFontFamilyFamilyProperty = DependencyProperty.Register(
            nameof(ButtonsFontFamily),
            typeof(FontFamily),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(FontFamily)));

        private Visibility CancelButtonVisibility
        {
            get => (Visibility)GetValue(CancelButtonVisibilityProperty);
            set => SetValue(CancelButtonVisibilityProperty, value);
        }
        DependencyProperty CancelButtonVisibilityProperty = DependencyProperty.Register(
            nameof(CancelButtonVisibility),
            typeof(Visibility),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(Visibility)));
      
        private Visibility SecondButtonVisibility
        {
            get => (Visibility)GetValue(SecondButtonVisibilityProperty);
            set => SetValue(SecondButtonVisibilityProperty, value);
        }
        DependencyProperty SecondButtonVisibilityProperty = DependencyProperty.Register(
            nameof(SecondButtonVisibility),
            typeof(Visibility),
            typeof(MikesContentDialog),
            new PropertyMetadata(default(Visibility)));

        #endregion


        public delegate void OnPrimaryBtnClick(ContentDialog dialog, RoutedEventArgs args);
        public new event OnPrimaryBtnClick PrimaryButtonClick;
        public delegate void OnSecondaryBtnClick(ContentDialog dialog, RoutedEventArgs args);
        public new event OnSecondaryBtnClick SecondaryButtonClick;
        public delegate void OnCloseBtnClick(ContentDialog dialog, RoutedEventArgs args);
        public new event OnCloseBtnClick CloseButtonClick;

        public enum ContentDialogResults
        {
            Primary,
            Secondary,
            Close
        }

        public ContentDialogResult ContentDialogResult;
        public MikesContentDialog()
        {
            

            InitializeComponent();
            this.Loading += DialogLoading;

            FullScreen = false;
            TitleFontSize = 20;
            ContentHeaderFontSize = 18;
            FooterHeaderFontSize = 18;
            DialogContentMaxWidth = App.m_window.Content.ActualSize.X;            
            Color accentColor = (Color)Application.Current.Resources["SystemAccentColor"];
            DialogBackground = new SolidColorBrush(accentColor);
            ButtonsAlignment = HorizontalAlignment.Stretch;
            DialogContentHorizontalAlignment = HorizontalAlignment.Left;

            DataContext = this;
        }


        private void DialogLoading(FrameworkElement sender, object args)
        {
           
            CancelButtonVisibility = ElementVisibilityCheck(CloseButtonText);
            SecondButtonVisibility = ElementVisibilityCheck(SecondaryButtonText);
            ContentHeaderVisibility = ElementVisibilityCheck(ContentHeaderText);
            FooterHeaderVisibility = ElementVisibilityCheck(FooterHeaderText);
            if (FullScreen)
            {
                FullScreenMode = new int[] { 0, 3 };
                DialogWidth = App.m_window.Content.ActualSize.X;
            }
            else
            {
                FullScreenMode = new int[] { 1, 1 };
                if (DialogWidth <= 0)
                {
                    DialogWidth = App.m_window.Content.ActualSize.X;
                }
                
            }

        }

        private void PrimaryButton_Click(object sender, RoutedEventArgs e)
        {
            PrimaryButtonClick(this, e);            
        }

        private void SecondaryButton_Click(object sender, RoutedEventArgs e)
        {
            SecondaryButtonClick(this, e);            
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseButtonClick(this, e);
            this.Hide();
        }

        private Visibility ElementVisibilityCheck(string content)
        {
            Visibility state;
            if (string.IsNullOrEmpty(content))
            {
                state = Visibility.Collapsed;
            }
            else
            {
                state = Visibility.Visible;
            }
            return state;
        }

    }
}


          
        

      
