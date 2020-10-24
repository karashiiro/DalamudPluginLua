import ("Dalamud")

InteropCommand = {
	Name = "",
	Aliases = [],
	HelpMessage = "",
	ShowInHelp = true,
	Command = nil
}

function InteropCommand:new (o)
	o = o or {}
	setmetatable(o, self)
	self.__index = self
	return o
end

function command_1 (command, args)
	local chat = PluginInterface.Framework.Gui.Chat
	local world = PluginInterface.ClientState.LocalPlayer.CurrentWorld.GameData
	chat.Print("Hello " .. world.Name .. "!")
end
local command1 = InteropCommand:new{
	Name = "/example1",
	Aliases = ["/ex1"],
	HelpMessage = "Example help message.",
	Command = command_1
}

commands = [command1]