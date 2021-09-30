using System;
using System.Collections.Generic;
using Dalamud.Game.ClientState;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Plugin;
using Neo.IronLua;

namespace DalamudPluginProjectTemplateLua
{
    public class Plugin : IDalamudPlugin
    {
        [PluginService]
        [RequiredVersion("1.0")]
        private DalamudPluginInterface PluginInterface { get; init; }

        [PluginService]
        [RequiredVersion("1.0")]
        private CommandManager Commands { get; init; }

        [PluginService]
        [RequiredVersion("1.0")]
        private ChatGui Chat { get; init; }

        [PluginService]
        [RequiredVersion("1.0")]
        private ClientState ClientState { get; init; }

        public string Name => "Your Plugin's Display Name";

        private readonly Lua lua;
        private readonly dynamic env;

        private readonly Configuration config;
        private readonly InteropCommandManager commandManager;

        public Plugin()
        {
            this.config = (Configuration)PluginInterface.GetPluginConfig() ?? new Configuration();
            this.config.Initialize(PluginInterface);

            this.lua = new Lua();
            this.env = this.lua.CreateEnvironment<PluginLuaGlobal>();
            
            this.commandManager = new InteropCommandManager(Commands);

            ConfigureScope();
            BeginScript();
        }

        private void ConfigureScope()
        {
            this.env.Configuration = this.config;
            this.env.PluginInterface = PluginInterface;
            this.env.Chat = Chat;
            this.env.ClientState = ClientState;
        }

        private void BeginScript()
        {
            var res = ((LuaGlobal)this.env).DoChunk(Util.GetRelativeFile("plugin.lua"));

            if (res.Values.Length <= 0) return;
            var commands = (IList<dynamic>)res.Values[0];
            this.commandManager.Install(commands);
        }

        #region IDisposable Support
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            this.lua.Dispose();

            this.config.Save();
            this.commandManager.Uninstall();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
