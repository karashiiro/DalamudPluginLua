﻿const ObjectList typeof System.Collections.Generic.List[System.Object];
const PluginLog typeof Dalamud.Plugin.PluginLog

local InteropCommand = require "interopcommand"

function command_1 (command, args)
	local world = ClientState.LocalPlayer.CurrentWorld.GameData
	Chat.Print("Hello, " .. world.Name .. "!")
	PluginLog.Log("Logged message.");
end
local command1 = InteropCommand:new{
	Name = "/example1",
	Aliases = {"/ex1"},
	HelpMessage = "Example help message.",
	Command = command_1
}

local commands = ObjectList()
commands.Add(command1)
return commands