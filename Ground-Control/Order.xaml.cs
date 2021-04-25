using System;
using System.Windows;
using System.Windows.Input;

namespace Ground_Control
{
    /// <summary>
    /// Order.xaml 的交互逻辑
    /// </summary>
    public partial class Order : Window
    {
        public Order()
        {
            InitializeComponent();
        }

        private void OrderCommit(object sender, RoutedEventArgs e)
        {
            string order = this.order.Text;
            order = order.Trim();
            if (null != order)
            {
                // 执行order
                Console.WriteLine("commit " + order);
                order = order.Replace("  ", " ");
                string[] arr = order.Split(' ');
                
                if (MainWindow.alias.ContainsKey(arr[0]))
                {
                    domain.Application app = (domain.Application)MainWindow.alias[arr[0]];
                    app.Execute(arr);
                    this.Close();
                } 
                else
                {
                    MessageBox.Show("命令不存在");
                }
            }
        }

        private void OrderExit(object sender, CanExecuteRoutedEventArgs e)
        {
            Console.WriteLine("exit");
            this.Close();
        }
    }
}
