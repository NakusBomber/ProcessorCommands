using ProcessorCommands.Models;
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

namespace ProcessorCommands.Components
{
    /// <summary>
    /// Логика взаимодействия для InputBox.xaml
    /// </summary>
    public partial class InputBox : UserControl
    {


        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register("HeaderText", typeof(string), typeof(InputBox), new PropertyMetadata(""));



        public bool IsBlockInput
        {
            get { return (bool)GetValue(IsBlockInputProperty); }
            set { SetValue(IsBlockInputProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsBlockInput.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBlockInputProperty =
            DependencyProperty.Register("IsBlockInput", typeof(bool), typeof(InputBox), new PropertyMetadata(false));




        public InputItem Item
        {
            get { return (InputItem)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Item.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(InputItem), typeof(InputBox), new PropertyMetadata(null));



        public InputBox()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
