using ProcessorCommands.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ProcessorCommands.Components
{
    /// <summary>
    /// Логика взаимодействия для InputDataGrid.xaml
    /// </summary>
    public partial class InputDataGrid : UserControl
    {


        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<InputItem>), typeof(InputDataGrid), new PropertyMetadata(null));

        public ObservableCollection<InputItem> ItemsSource
        {
            get { return (ObservableCollection<InputItem>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }



        public bool IsBlockInput
        {
            get { return (bool)GetValue(IsBlockInputProperty); }
            set { SetValue(IsBlockInputProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsBlockInput.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBlockInputProperty =
            DependencyProperty.Register("IsBlockInput", typeof(bool), typeof(InputDataGrid), new PropertyMetadata(false));



        public string HeaderLabel
        {
            get { return (string)GetValue(HeaderLabelProperty); }
            set { SetValue(HeaderLabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderLabel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderLabelProperty =
            DependencyProperty.Register("HeaderLabel", typeof(string), typeof(InputDataGrid), new PropertyMetadata(string.Empty));



        public string HeaderValue
        {
            get { return (string)GetValue(HeaderValueProperty); }
            set { SetValue(HeaderValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderValueProperty =
            DependencyProperty.Register("HeaderValue", typeof(string), typeof(InputDataGrid), new PropertyMetadata(string.Empty));
        

        

        public InputDataGrid()
        {
            InitializeComponent();
            DataContext = this;
            
        }

    }
}
