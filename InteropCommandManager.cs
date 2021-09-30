using Dalamud.Game.Command;
using System.Collections.Generic;

namespace DalamudPluginProjectTemplateLua
{
    public class InteropCommandManager
    {
        private IList<dynamic> commands;

        private readonly CommandManager commandManager;

        public InteropCommandManager(CommandManager commandManager)
        {
            this.commands = new List<dynamic>();
            this.commandManager = commandManager;
        }

        public void Install(IList<dynamic> commands)
        {
            this.commands = commands;
            foreach (var command in this.commands)
            {
                var commandInfo = new CommandInfo((cmd, args) =>
                {
                    command.Command(cmd, args);
                })
                {
                    HelpMessage = command.HelpMessage,
                    ShowInHelp = command.ShowInHelp,
                };

                this.commandManager.AddHandler(command.Name, commandInfo);

                foreach (KeyValuePair<object, object> alias in command.Aliases)
                {
                    this.commandManager.AddHandler((string)alias.Value, commandInfo);
                }
            }
        }

        public void Uninstall()
        {
            foreach (var command in this.commands)
            {
                this.commandManager.RemoveHandler(command.Name);

                foreach (KeyValuePair<object, object> alias in command.Aliases)
                {
                    this.commandManager.RemoveHandler((string)alias.Value);
                }
            }
        }
    }
}