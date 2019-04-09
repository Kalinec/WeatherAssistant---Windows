using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using weatherAssistant.Services;
using StringBuilder = System.Text.StringBuilder;

namespace weatherAssistant.Controls
{
    /// <summary>
    /// Interaction logic for WeatherWarningInfo.xaml
    /// </summary>
    public partial class WeatherWarningInfo : UserControl
    {
        #region Dependency Properties

        public String Type
        {
            get { return (String) GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(string), typeof(WeatherWarningInfo), new PropertyMetadata());

        public String Degree
        {
            get { return (String) GetValue(DegreeProperty); }
            set { SetValue(DegreeProperty, value); }
        }

        public static readonly DependencyProperty DegreeProperty = 
            DependencyProperty.Register("Degree", typeof(string), typeof(WeatherWarningInfo), new PropertyMetadata());

        public String Description
        {
            get { return (String) GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty = 
            DependencyProperty.Register("Description", typeof(string), typeof(WeatherWarningInfo), new PropertyMetadata());

        public String From
        {
            get { return (String)GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }

        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register("From", typeof(string), typeof(WeatherWarningInfo), new PropertyMetadata());

        public String To
        {
            get { return (String)GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To", typeof(string), typeof(WeatherWarningInfo), new PropertyMetadata());

        public String ImagePath
        {
            get { return (String) GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }

        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.Register("ImagePath",typeof(string), typeof(WeatherWarningInfo), new PropertyMetadata());

        #endregion
        public WeatherWarningInfo()
        {
            InitializeComponent();

            LayoutRoot.DataContext = this;
        }
    }
}
