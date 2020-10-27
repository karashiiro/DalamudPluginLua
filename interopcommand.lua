local InteropCommand = {
	Name = "",
	Aliases = {},
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

return InteropCommand