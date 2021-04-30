using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows.Controls;
using System.Net;
using ICSharpCode.SharpZipLib.Zip;
using System.Windows.Media;

namespace Ground_Control
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 应用列表 <br/>
        /// name -> application
        /// </summary>
        public Hashtable apps = new Hashtable();

        /// <summary>
        /// 别名<br/>
        /// alias1 -> application<br/>
        /// alias2 -> application
        /// </summary>
        public static Hashtable alias = new Hashtable();

        private static domain.Application appActive;

        HotKey HotKey = new HotKey();
        public MainWindow()
        {
            InitializeComponent();
            FileInit();
            LoadApp();
            InitCombox();
        }

        /// <summary>
        /// 别名<br/>
        /// alias1 -> application<br/>
        /// alias2 -> application
        /// </summary>
        public void FileInit()
        {
            if (!Directory.Exists(@".\script"))
            {
                Directory.CreateDirectory(@".\script");
                new System.Threading.Thread( () =>
                {
                    GetScript();
                }).Start();
            }
            if (!Directory.Exists(@".\data"))
            {
                Directory.CreateDirectory(@".\data");
                FileStream fs = File.Open(@".\data\app.json", FileMode.OpenOrCreate);
                fs.Write(Encoding.Default.GetBytes("{}"),0,2);
                fs.Close();
            }
        }

        private void GetScript()
        {
            string url = "https://codeload.github.com/xtcxg/Ground-Control-Script/zip/refs/heads/main";
            string file = @".\script\main.zip";

            /// 开始下载脚本文件
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();

                //创建本地文件写入流
                Stream stream = new FileStream(file, FileMode.Create);

                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    stream.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                stream.Close();
                responseStream.Close();
            }
            catch
            {
                MessageBox.Show("未能初始化脚本,https://github.com/xtcxg/Ground-Control-Script");
            }

            /// 结束下载

            /// 开始解压main.zip

            ZipInputStream zipStream = new ZipInputStream(File.OpenRead(file));
            ZipEntry entry = null;
            string fileName;
            FileStream fs = null;
            try
            {
                while ((entry = zipStream.GetNextEntry()) != null)
                {
                    if (!string.IsNullOrEmpty(entry.Name))
                    {
                        fileName = Path.Combine(@".\script\", entry.Name);
                        fileName = fileName.Replace("Ground-Control-Script-main", "");
                        fileName = fileName.Replace("//", "/");
                        fileName = fileName.Replace('/', '\\');
                        

                        if (fileName.EndsWith("\\"))
                        {
                            Directory.CreateDirectory(fileName);
                            continue;
                        }

                        fs = File.Create(fileName);
                        int s = 2048;
                        byte[] data = new byte[s];
                        while (true)
                        {
                            s = zipStream.Read(data, 0, data.Length);
                            if (s > 0)
                                fs.Write(data, 0, s);
                            else
                                break;
                        }
                        fs.Close();
                    }
                }
                File.Delete(file);
            }
            catch
            {
                MessageBox.Show("未能初始化脚本,https://github.com/xtcxg/Ground-Control-Script");
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
                if (zipStream != null)
                {
                    zipStream.Close();
                    zipStream.Dispose();
                }
                if (entry != null)
                {
                    entry = null;
                }
                GC.Collect();
                GC.Collect(1);
            }
            /// 结束解压main.zip
        }

        void InitCombox()
        {
            this.script_list.Items.Clear();
            foreach (string name in apps.Keys)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Content = name
                };
                this.script_list.Items.Add(item);
            }
        }

        public void LoadApp()
        {
            if (!File.Exists(@".\data\app.json"))
            {
                FileStream fs = File.Open(@".\data\app.json", FileMode.OpenOrCreate);
                fs.Write(Encoding.Default.GetBytes("{}"), 0, 2);
                fs.Close();
            }
            string jsonString = File.ReadAllText(@".\data\app.json");
            //Console.WriteLine(jsonString);
            //var writerOptions = new JsonWriterOptions
            //{
            //    Indented = true
            //};
            var documentOptions = new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip
            };
            //FileStream fs = File.Open(@".\data\app.json",FileMode.Open);
            //var writer = new Utf8JsonWriter(fs, options: writerOptions);
            JsonDocument json = JsonDocument.Parse(jsonString, documentOptions);
            JsonElement root = json.RootElement;
            foreach (JsonProperty e in root.EnumerateObject())
            {
                Console.WriteLine(e);
                CreateApplication(e);
            }
        }
        private void CreateApplication(JsonProperty e)
        {
            domain.Application app = new domain.Application(e.Name);
            apps.Add(e.Name, app);
            JsonElement j1 = e.Value;
            foreach (JsonProperty e1 in j1.EnumerateObject())
            {
                JsonElement v = e1.Value;
                if (e1.Name.Equals("version"))
                {
                    app.version = e1.Value.GetString();
                }
                if (e1.Name.Equals("cmds"))
                {
                    foreach (JsonProperty c in v.EnumerateObject())
                    {
                        app.cmds.Add(c.Name, c.Value.GetString());
                        alias.Add(c.Name, app);
                    }
                }
                if (e1.Name.Equals("args"))
                {
                    foreach (JsonProperty a in v.EnumerateObject())
                    {
                        app.args.Add(a.Name, a.Value.GetString());
                    }
                }
                if (e1.Name.Equals("props"))
                {
                    foreach (JsonElement p in v.EnumerateArray())
                    {
                        app.props.Add(p.GetString());
                    }
                }
                if (e1.Name.Equals("type"))
                {
                    app.type = e1.Value.GetString();
                }
                if (e1.Name.Equals("describe"))
                {
                    app.Describe = e1.Value.GetString();
                }
            }
            Console.WriteLine(apps);
        }

        private void ScriptChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = (sender as System.Windows.Controls.ComboBox).SelectedItem as ComboBoxItem;
            string name = item.Content.ToString();
            Console.WriteLine(name);

            foreach(DictionaryEntry a in apps)
            {
                if (a.Key.Equals(name))
                {
                    appActive = a.Value as domain.Application;
                }
            }
            // 根据选择的script, 刷新页面数据
            this.cmd_list.Children.Clear();
            this.arg_list.Children.Clear();
            this.prop_list.Children.Clear();

            // cmd 
            foreach (DictionaryEntry cmd in appActive.cmds)
            {
                CreateCmd(cmd.Key.ToString(), cmd.Value.ToString());
            }

            // args
            foreach(DictionaryEntry a in appActive.args)
            {
                CreateArg(a.Key.ToString(), a.Value.ToString());
            }

            // prop 
            foreach(string p in appActive.props)
            {
                CreateProp(p);
            }
        }

        // cmd 新增一条数据
        private void CmdAdd(object sender, RoutedEventArgs e)
        {
            if (null == appActive) return;
            CreateCmd();
        }

        private void CreateCmd(string l = "",string r = "")
        {
            DockPanel panel = new DockPanel()
            {
                Margin = new Thickness(0, 3, 0, 0)
            };
            TextBox alias = new TextBox()
            {
                Text = l,
                MinWidth = 80
            };
            TextBlock block = new TextBlock()
            {
                Text = "=",
                Margin = new Thickness(5, 0, 5, 0)
            };
            TextBox full = new TextBox()
            {
                Text = r,
                MinWidth = 120
            };
            Button del = new Button()
            {
                Width = 45,
                Margin = new Thickness(3, 0, 0, 0),
                Content = "删除",
                HorizontalAlignment = HorizontalAlignment.Left
            };
            del.Click += CmdDelete;

            panel.Children.Add(alias);
            panel.Children.Add(block);
            panel.Children.Add(full);
            panel.Children.Add(del);
            this.cmd_list.Children.Add(panel);
        }

        // cmd 删除一条数据
        private void CmdDelete(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Console.WriteLine(btn);
            DockPanel panel = btn.Parent as DockPanel;
            this.cmd_list.Children.Remove(panel);
        }

        // cmd 提交变更
        private void CmdSubmit(object sender, RoutedEventArgs e)
        {
            if (null == appActive) return;
            Console.WriteLine("cmd submit");
            UIElementCollection ps = this.cmd_list.Children;
            Hashtable temp = new Hashtable();
            foreach (UIElement p in ps)
            {
                DockPanel panel = p as DockPanel;
                UIElement[] array = new UIElement[panel.Children.Count];
                panel.Children.CopyTo(array, 0);
                if ("".Equals((array[0] as TextBox).Text.Trim()))
                {
                    MessageBox.Show("命令key值不能为空");
                    return;
                }
                Console.WriteLine((array[0] as TextBox).Text + "=" + (array[2] as TextBox).Text);
                temp.Add((array[0] as TextBox).Text, (array[2] as TextBox).Text);
            }
            appActive.cmds.Clear();
            appActive.cmds = temp;
            Write();
        }

        // arg 新增一条数据
        private void ArgAdd(object sender, RoutedEventArgs e)
        {
            if (null == appActive) return;
            CreateArg();
        }

        private void CreateArg(string l = "",string r = "")
        {
            DockPanel panel = new DockPanel()
            {
                Margin = new Thickness(0, 3, 0, 0)
            };
            TextBox simple = new TextBox()
            {
                Text = l,
                MinWidth = 70
            };
            TextBlock block = new TextBlock()
            {
                Text = "=",
                Margin = new Thickness(5, 0, 5, 0)
            };
            TextBox full = new TextBox()
            {
                Text = r,
                Width = 130
            };
            Button del = new Button()
            {
                Width = 45,
                Margin = new Thickness(3, 0, 0, 0),
                Content = "删除",
                HorizontalAlignment = HorizontalAlignment.Left
            };
            del.Click += ArgDelete;

            panel.Children.Add(simple);
            panel.Children.Add(block);
            panel.Children.Add(full);
            panel.Children.Add(del);
            this.arg_list.Children.Add(panel);
        }

        private void ArgDelete(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Console.WriteLine(btn);
            DockPanel panel = btn.Parent as DockPanel;
            this.arg_list.Children.Remove(panel);
        }

        // arg 提交变更
        private void ArgSubmit(object sender, RoutedEventArgs e)
        {
            if (null == appActive) return;
            Console.WriteLine("cmd submit");
            UIElementCollection ps = this.arg_list.Children;
            
            Hashtable temp = new Hashtable();
            foreach (UIElement p in ps)
            {
                DockPanel panel = p as DockPanel;
                UIElement[] array = new UIElement[panel.Children.Count];
                panel.Children.CopyTo(array, 0);
                if ("".Equals((array[0] as TextBox).Text.Trim()))
                {
                    MessageBox.Show("参数key值不能为空");
                    return;
                }
                Console.WriteLine((array[0] as TextBox).Text + "=" + (array[2] as TextBox).Text);
                temp.Add((array[0] as TextBox).Text, (array[2] as TextBox).Text);
            }
            appActive.args.Clear();
            appActive.args = temp;
            Write();
        }

        private void PropDelete(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Console.WriteLine(btn);
            DockPanel panel = btn.Parent as DockPanel;
            this.prop_list.Children.Remove(panel);
        }

        private void PropAdd(object sender, RoutedEventArgs e)
        {
            if (null == appActive) return;
            CreateProp();
        }
        private void PropSubmit(object sender, RoutedEventArgs e)
        {
            if (null == appActive) return;
            Console.WriteLine("prop submit");
            UIElementCollection ps = this.prop_list.Children;
            
            ArrayList temp = new ArrayList();
            foreach (UIElement p in ps)
            {
                DockPanel panel = p as DockPanel;
                UIElement[] array = new UIElement[panel.Children.Count];
                panel.Children.CopyTo(array, 0);
                if ("".Equals((array[0] as TextBox).Text.Trim()))
                {
                    MessageBox.Show("配置信息不能出现全空数据");
                    return;
                }
                Console.WriteLine((array[0] as TextBox).Text);
                temp.Add((array[0] as TextBox).Text);
            }
            appActive.props.Clear();
            appActive.props = temp;
            Write();
        }

        private void CreateProp(string value = "")
        {
            DockPanel panel = new DockPanel()
            {
                Margin = new Thickness(0, 3, 0, 0)
            };
            TextBox box = new TextBox()
            {
                Width = 200,
                Text = value
            };
            Button button = new Button()
            {
                Margin = new Thickness(3, 0, 0, 0),
                Width = 45,
                Content = "删除",
                HorizontalAlignment = HorizontalAlignment.Left
            };
            button.Click += PropDelete;

            panel.Children.Add(box);
            panel.Children.Add(button);
            this.prop_list.Children.Add(panel);
        }

        private void Write()
        {
            // 将变更写入文件
            JsonSerializerOptions jso = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(new System.Text.Encodings.Web.TextEncoderSettings(System.Text.Unicode.UnicodeRanges.All))
            };
            byte[] bs = JsonSerializer.SerializeToUtf8Bytes<Hashtable>(apps, jso);
            Console.WriteLine(System.Text.Encoding.Default.GetString(bs));
            FileStream fs = File.Open(@".\data\app.json", FileMode.Truncate, FileAccess.ReadWrite);
            fs.Write(bs, 0, bs.Length);
            fs.Close();
        }

        private void ScriptSelected(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("script select");
            this.scrips.Children.Clear();
            System.Collections.Generic.IEnumerable<string> dirs = Directory.EnumerateDirectories(@".\script\");
            
            foreach(string dir in dirs)
            {
                string name = dir.Replace(".\\script\\", "");

                DockPanel panel = new DockPanel()
                {
                    Margin = new Thickness(5, 3, 0, 3),
                };
                // 为已加载的脚本上色
                if (apps.ContainsKey(name))
                {
                    panel.Background = new SolidColorBrush(Color.FromArgb(120, 127, 255, 170));
                }
                TextBlock box = new TextBlock()
                {
                    Text = name,
                    Width = 300
                };
                Button add = new Button
                {
                    Name = name,
                    Width = 45,
                    Margin = new Thickness(5, 0, 0, 0),
                    Content = "加入",
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                add.Click += ScriptAdd;
                Button rem = new Button
                {
                    Name = name,
                    Width = 45,
                    Margin = new Thickness(5, 0, 0, 0),
                    Content = "移除",
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                rem.Click += ScriptRemove;

                panel.Children.Add(box);
                panel.Children.Add(add);
                panel.Children.Add(rem);
                this.scrips.Children.Add(panel);
                Console.WriteLine(dir);
            }
        }

        private void ScriptAdd(object sender, RoutedEventArgs e)
        {
            
            Button btn = sender as Button;
            string name = btn.Name;
            if (apps.ContainsKey(name))
                return;
            
            DockPanel panel = btn.Parent as DockPanel;
            panel.Background = new SolidColorBrush(Color.FromArgb(120, 127, 255, 170));

            string conf = File.ReadAllText(@".\script\" + name + @"\conf.json");
            Console.WriteLine(conf);
            var documentOptions = new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip
            };
            //FileStream fs = File.Open(@".\data\app.json",FileMode.Open);
            //var writer = new Utf8JsonWriter(fs, options: writerOptions);
            JsonDocument json = JsonDocument.Parse(conf, documentOptions);
            JsonElement root = json.RootElement;

            foreach (JsonProperty j in root.EnumerateObject())
            {
                Console.WriteLine(j);
                CreateApplication(j);
            }
            Console.WriteLine(apps);
            InitCombox();
            Write();
        }
        private void ScriptRemove(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string name = btn.Name;
            Console.WriteLine(name);
            DockPanel panel = btn.Parent as DockPanel;
            panel.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            if (!apps.ContainsKey(name))
                return;
            foreach(DictionaryEntry app in apps)
            {
                if (name.Equals(app.Key))
                {
                    foreach(string c in (app.Value as domain.Application).Cmds.Keys)
                    {
                        alias.Remove(c);
                    }
                }
            }
            apps.Remove(name);
            InitCombox();
            Write();
        }
    }

    class HotKey : System.Windows.Forms.Control
    {
        
        public HotKey()
        {
            RegisterHotKey(Handle, 100, KeyModifiers.Ctrl, System.Windows.Forms.Keys.I);
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            const int WM_HOTKEY = 0x0312;
            //按快捷键 
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:
                            //按下的是ctrl + i
                            Console.WriteLine("press");
                            Order order = new Order();
                            order.Show();
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        //如果函数执行成功，返回值不为0。  
        //如果函数执行失败，返回值为0。要得到扩展错误信息，调用GetLastError。  
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(
                IntPtr hWnd,                 //要定义热键的窗口的句柄  
                int id,                      //定义热键ID（不能与其它ID重复）            
                KeyModifiers fsModifiers,    //标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效  
                System.Windows.Forms.Keys vk                      //定义热键的内容  
                );

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(
            IntPtr hWnd,                 //要取消热键的窗口的句柄  
            int id                       //要取消热键的ID  
            );

        //定义了辅助键的名称（将数字转变为字符以便于记忆，也可去除此枚举而直接使用数值）  
        [Flags()]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            WindowsKey = 8
        }
    }
}
