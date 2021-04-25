using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Collections.ObjectModel;

namespace Ground_Control.domain
{
    class Application
    {
        public enum ScriptType { CMD_SCRIPT,PS_SCRIPT}
        public string name;
        public string version;
        public ScriptType type = ScriptType.PS_SCRIPT;

        private Application() { }
        public Application(string name)
        {
            this.name = name;
        }

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
            PowerShell ps = PowerShell.Create();
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
            array = complate.Split(' ');
            foreach(string a in array)
            {
                Console.WriteLine(a);
                ps.AddArgument(a);
            }
            Collection<PSObject> res = ps.Invoke(); // 执行脚本
        }
    }
}
