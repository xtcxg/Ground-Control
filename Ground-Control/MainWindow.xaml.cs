using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;

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
        HotKey HotKey = new HotKey();
        public MainWindow()
        {
            InitializeComponent();
            FileInit();
            LoadApp();
        }

        /// <summary>
        /// 别名<br/>
        /// alias1 -> application<br/>
        /// alias2 -> application
        /// </summary>
        public void FileInit()
        {
            if(!Directory.Exists(@".\script"))
            {
                Directory.CreateDirectory(@".\script");
            }
            if (!Directory.Exists(@".\data"))
            {
                Directory.CreateDirectory(@".\data");
                File.Create(@".\data\app.json");
            }
        }

        public void LoadApp()
        {
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
                domain.Application app = new domain.Application(e.Name);
                apps.Add(e.Name, app);
                JsonElement j1 = e.Value;
                foreach(JsonProperty e1 in j1.EnumerateObject())
                {
                    JsonElement v = e1.Value;
                    if (e1.Name.Equals("version"))
                    {
                        app.version = e1.Value.GetString();
                    }
                    if (e1.Name.Equals("cmd"))
                    {
                        foreach (JsonProperty c in v.EnumerateObject())
                        {
                            app.cmds.Add(c.Name, c.Value.GetString());
                            alias.Add(c.Name, app);
                        }
                    }
                    if (e1.Name.Equals("arg"))
                    {
                        foreach (JsonProperty a in v.EnumerateObject())
                        {
                            app.args.Add(a.Name, a.Value.GetString());
                        }
                    }
                    if (e1.Name.Equals("prop"))
                    {
                        foreach(JsonElement p in v.EnumerateArray())
                        {
                            app.props.Add(p.GetString());
                        }
                    }
                }
                Console.WriteLine(apps);
            }

            //string name = "open";
            //domain.Application app1 = new domain.Application(name);
            //apps.Add(name, app1);

            //app1.cmds.Add("open", "open");
            //alias.Add("open", app1);

            //app1.cmds.Add("op", "open");
            //alias.Add("op", app1);

            //app1.cmds.Add("opu", "open -u");
            //alias.Add("opu", "open -u");

            //app1.cmds.Add("download", "download");
            //alias.Add("download", app1);

            //app1.args.Add("git","https://github.com");

            //app1.props.Add("brower=chrome");
            //app1.props.Add("edit=notpad");

        }
    }

    class HotKey : System.Windows.Forms.Control
    {
        
        public HotKey()
        {
            RegisterHotKey(Handle, 100, KeyModifiers.Ctrl, Keys.I);
        }

        protected override void WndProc(ref Message m)
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
                Keys vk                      //定义热键的内容  
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
