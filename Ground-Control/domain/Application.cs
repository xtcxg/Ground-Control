using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace Ground_Control.domain
{
    class Application
    {
        public enum ScriptType { CMD_SCRIPT,PS_SCRIPT}
        public string name = "";
        public string version = "";
        public string type = "ps";
        public string describe = "";

        /// <summary>
        /// 命令列表 <br/>
        /// 别名 = 实际命令
        /// </summary>
        public Hashtable cmds = new Hashtable();

        /// <summary>
        /// 自定义参数列表 <br/>
        /// 参数别名 = 实际参数
        /// </summary>
        public Hashtable args = new Hashtable();

        /// <summary>
        /// 自定义配置
        /// </summary>
        public ArrayList props = new ArrayList();

        public string Name { get => name; set => name = value; }
        public string Version { get => version; set => version = value; }
        public string Type { get => type; set => type = value; }
        public Hashtable Args { get => args; set => args = value; }
        public Hashtable Cmds { get => cmds; set => cmds = value; }
        public ArrayList Props { get => props; set => props = value; }
        public string Describe { get => describe; set => describe = value; }

        private Application() { }
        public Application(string name)
        {
            this.name = name;
        }
        public void Execute(string cmd,string arg)
        {
            Console.WriteLine(cmd + " " + arg + " {[<" + props + ">]}");
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        public void Execute(string[] array)
        {
            string script = @".\script\" + this.name + @"\app.ps1"; // 脚本地址
            if (!File.Exists(script))
            {
                MessageBox.Show("<" + script + ">" + "不存在");
                return;
            }
            
            //string script = "Set-ExecutionPolicy -Scope Process -ExecutionPolicy Unrestricted; Get-ExecutionPolicy";
            PowerShell ps = PowerShell.Create();
            ps.AddScript("Set-ExecutionPolicy -Scope Process -ExecutionPolicy Unrestricted;");
            ps.Invoke();
            ps.AddCommand(script);  // 添加执行命令

            // 开始组装完整命令
            string complate = (string)this.cmds[array[0]];
            if (array.Length > 1)
            {
                for (int i = 1; i < array.Length; i++)
                {
                    if (args.ContainsKey(array[i]))
                        complate += " " + args[array[i]];
                    else
                        complate += " " + array[i];
                }
            }
            if(props.Count > 0)
            {
                complate += " [[[";
                foreach(string p in props)
                {
                    complate += p + "+++";
                }
                complate = complate.Substring(0, complate.Length-3) + "]]]";
            }
            Console.WriteLine(complate);
            // 结束组装完整命令
            
            RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();
            Runspace runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
            runspace.Open();

            // 将完整命令转为脚本参数
            array = SplitOrder(complate);
            foreach(string a in array)
            {
                Console.WriteLine(a);
                ps.AddArgument(a);
            }
            Collection<PSObject> res = ps.Invoke(); // 执行脚本
        }

        private string[] SplitOrder(string order)
        {
            List<string> list = new List<string>();
            List<char> chs = new List<char>();
            Boolean flag = false;
            foreach (char ch in order)
            {
                if (!' '.Equals(ch) || flag)
                {
                    if ('"'.Equals(ch))
                        if (flag) flag = false;
                        else flag = true;
                    chs.Add(ch);
                }
                else
                {
                    if (chs.Count != 0)
                    {
                        list.Add(string.Concat(chs));
                        chs.Clear();
                    }
                }
            }
            return list.ToArray();
        }
    }
}
