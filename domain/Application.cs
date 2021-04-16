using System;
using System.Collections.Generic;
using Windows.Foundation.Collections;

namespace Ground_Control.domain
{
    public class Application
    {
        string name;
        string version;
        public List<string> cmds = new List<string>();
        public StringMap args = new StringMap();
        public List<string> props = new List<string>();

        public Application(string name, string version)
        {
            this.name = name;
            this.version = version;
        }
        public void SetName(string name)
        {
            this.name = name;
        }
        public string GetName() 
        {
            return this.name;
        }
        public List<string> GetCmds() 
        {
            return this.cmds;
        }

        public void SetVersion(string version)
        {
            this.version = version;
        }
        public string GetVersion()
        {
            return this.version;
        }
        public Boolean run()
        {
            return true;
        }
    }
}
