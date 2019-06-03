﻿using BrawlLib.SSBB.ResourceNodes;
using Microsoft.Scripting.Hosting;
using System;
using System.IO;

namespace BrawlCrate.API
{
    public class PluginScript
    {
        public PluginScript(string name, ScriptSource script, ScriptScope scope)
        {
            Name = name;
            Script = script;
            Scope = scope;
        }

        public string Name { get; set; }
        public ScriptSource Script { get; set; }
        public ScriptScope Scope { get; set; }

        public void Execute()
        {
            try
            {
                Script.Execute(Scope);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("BrawlBox") ||
                    e.Message.Contains("bboxapi"))
                {
                    BrawlAPI.ConvertPlugin(Script.Path);
                    Execute();
                }
                else
                {
                    string msg = $"Error running plugin \"{Path.GetFileName(Script.Path)}\"\n{e.Message}";
                    BrawlAPI.ShowMessage(msg, Path.GetFileName(Script.Path));
                }
            }
        }
    }

    public class PluginLoader : ResourceNode
    {
        internal static ResourceNode TryParse(DataSource source)
        {
            ResourceNode n = null;
            foreach (PluginLoader ldr in BrawlAPI.Loaders)
            {
                if ((n = ldr.TryParse(new UnsafeStream(source.Address, (uint)source.Length))) != null)
                {
                    break;
                }
            }

            return n;

        }

        public virtual ResourceNode TryParse(Stream stream) { return null; }
    }
}