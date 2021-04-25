using System;
using System.Collections;

namespace Ground_Control.domain
{
    class Application
    {
        public enum ScriptType { CMD_SCRIPT,PS_SCRIPT}
        public string name;
        public string version;
        public ScriptType type;

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

        public void run(string cmd,string arg)
        {
            Console.WriteLine(cmd + " " + arg + " {[<" + props + ">]}");
        }
        public void run(string[] array)
        {
            string complate = (string)this.cmds[array[0]];
            if (array.Length > 1)
            {
                for (int i = 1; i < array.Length; i++)
                {
                    if (args.ContainsKey(array[i]))
                    {
                        complate += " " + args[array[i]];
                    }
                    else
                    {
                        complate += " " + array[i];
                    }
                }
            }
            if(props.Count > 0)
            {
                complate += " {[<";
                foreach(string p in props)
                {
                    complate += p + ",";
                }
                complate = complate.Substring(0, complate.Length - 1) + ">]}";

            }
            Console.WriteLine(complate);
        }
    }
}
