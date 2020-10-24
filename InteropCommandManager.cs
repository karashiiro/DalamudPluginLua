using Dalamud.Game.Command;
using Dalamud.Plugin;
using System.Collections.Generic;
using NLua;

namespace DalamudPluginProjectTemplateLua
{
    public class InteropCommandManager
    {
        private IList<dynamic> commands;

        private readonly DalamudPluginInterface pluginInterface;

        public InteropCommandManager(DalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;
        }

        public void Install(IList<dynamic> commands)
        {
            this.commands = commands;
            foreach (var command in this.commands)
            {
                command.Command = (CommandInfo.HandlerDelegate)((cmd, args) =>
                {
                    LuaFunction fn = command.Command;
                    fn.Call(cmd, args);
                });

                var commandInfo = new CommandInfo(command.Command)
                {
                    HelpMessage = command.HelpMessage,
                    ShowInHelp = command.ShowInHelp,
                };

                this.pluginInterface.CommandManager.AddHandler(command.Name, commandInfo);

                foreach (var alias in command.Aliases)
                {
                    this.pluginInterface.CommandManager.AddHandler(alias, commandInfo);
                }
            }
        }

        public void Uninstall()
        {
            foreach (var command in this.commands)
            {
                this.pluginInterface.CommandManager.RemoveHandler(command.Name);

                foreach (var alias in command.Aliases)
                {
                    this.pluginInterface.CommandManager.RemoveHandler(alias);
                }
            }
        }
    }
}