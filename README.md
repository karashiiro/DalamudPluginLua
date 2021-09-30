# DalamudPluginLua
A Visual Studio template for creating a new Dalamud plugin project in Lua.

Be sure to set "Copy to output directory" to "Always" on any new Lua script files.

## Hint paths
The project file includes hint paths for the dependencies, which are set up to not include much information about the developer's filesystem.
You may need to adjust these paths yourself, if they don't represent your development environment.

## GitHub Actions
Running the shell script `DownloadGithubActions.sh` will download some useful GitHub actions for you. You can also delete this file if you have no need for it.

### Current Actions
  * dotnet build/test