using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Dalamud.Plugin;
using Neo.IronLua;

namespace DalamudPluginProjectTemplateLua
{
    public class Plugin : IDalamudPlugin
    {
        public string Name => "Your Plugin's Display Name";

        private Lua lua;
        private dynamic env;

        private Configuration config;
        private DalamudPluginInterface pluginInterface;
        private InteropCommandManager commandManager;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;

            this.config = (Configuration)this.pluginInterface.GetPluginConfig() ?? new Configuration();
            this.config.Initialize(pluginInterface);

            this.lua = new Lua();
            this.env = this.lua.CreateEnvironment();
            
            this.commandManager = new InteropCommandManager(pluginInterface);

            ConfigureScope();
            Execute("plugin.lua");
        }

        private void ConfigureScope()
        {
            this.env.Configuration = this.config;
            this.env.PluginInterface = this.pluginInterface;
        }

        private void Execute(string scriptFile)
        {
            var filePath = GetRelativeFile(scriptFile);

            var chunk = this.lua.CompileChunk(filePath, new LuaCompileOptions());
            var res = ((LuaGlobal)this.env).DoChunk(chunk);

            var commands = (IList<dynamic>)res.Values[0];
            this.commandManager.Install(commands);
        }

        private static string GetRelativeFile(string fileName)
        {
            return Path.Combine(Assembly.GetExecutingAssembly().Location, "..", fileName);
        }

        #region IDisposable Support
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            this.lua.Dispose();

            this.config.Save();
            this.commandManager.Uninstall();
            this.pluginInterface.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
