using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Ground_Control
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>



    public sealed partial class MainPage : Page
    {
        static domain.Application appActive;

        static int CMD_SIZE = 0;
        static int ARG_SIZE = 0;
        static int PROP_SIZE = 0;

        public MainPage()
        {
            this.InitializeComponent();
            this.GenerateScriptList();
        }

        /// <summary>
        /// 初始化脚本列表
        /// </summary>
        public void GenerateScriptList()
        {
            foreach (DictionaryEntry e in App.plugins)
            {
                this.script_chose.Items.Add((string)e.Key);
            }
        }

        /// <summary>
        /// 当选择的脚本发生变更时触发
        /// 创建新的 cmd、args、prop
        /// </summary>
        private void ScriptChange(object sender, SelectionChangedEventArgs e)
        {
            this.cmd_list.Children.Clear();
            //foreach (UIElement u in this.cmd_list.Children) 
            //{
            //    this.cmd_list.Children.Remove(u.get);
            //}
            string scriptName = e.AddedItems[0].ToString();
            foreach (DictionaryEntry d in App.plugins)
            {
                if (scriptName.Equals((string)d.Key))
                {
                    MainPage.appActive = (domain.Application)d.Value;
                }
            }
            // 创建 cmd、args、prop 元素
            int index = 0;
            foreach (string s in appActive.cmds)
            {
                RelativePanel rp = new RelativePanel
                {
                    Margin = new Thickness(0, 42 * index, 0, 10)
                };
                TextBox tb = new TextBox
                {
                    Name = "cmd_" + index,
                    Width = 200,
                    Text = s
                };
                rp.Children.Add(tb);
                Button b = new Button()
                {
                    Content = "删除",
                    Margin = new Thickness(210, 0, 0, 0),
                    Name = "btn_" + index
                };
                b.Click += DeleteCmd;
                rp.Children.Add(b);
                this.cmd_list.Children.Add(rp);
                index++;
            }
            CMD_SIZE = index - 1;
        }

        private void SubmitCmd(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("cmd submit");

        }

        private void AddCmd(object sender, RoutedEventArgs e)
        {
            if (null == appActive || 5 > CMD_SIZE)
            {
                return;
            }
            System.Diagnostics.Trace.WriteLine("add cmd");
            CMD_SIZE++;
            RelativePanel rp = new RelativePanel
            {
                Margin = new Thickness(0, 42 * CMD_SIZE, 0, 10)
            };
            TextBox tb = new TextBox
            {
                Name = "cmd_" + CMD_SIZE,
                Width = 200
            };
            rp.Children.Add(tb);
            Button b = new Button()
            {
                Content = "删除",
                Margin = new Thickness(210, 0, 0, 0),
                Name = "btn_" + CMD_SIZE
            };
            b.Click += DeleteCmd;
            rp.Children.Add(b);
            this.cmd_list.Children.Add(rp);
            
        }

        private void DeleteCmd(object sender, RoutedEventArgs e)
        {
            //Button btn = sender as Button;
            //RelativePanel rp = VisualTreeHelper.GetParent(btn) as RelativePanel;

            //string name = btn.Name;
            CMD_SIZE--;
            FrameworkElement btn = e.OriginalSource as FrameworkElement;
            FrameworkElement rp = VisualTreeHelper.GetParent(btn) as FrameworkElement;
            System.Diagnostics.Trace.WriteLine("delete " + rp.Name);
            cmd_list.Children.Remove(rp);
        }
        private void SubmitArg(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
