using System;
using System.Collections.Generic;
using Neo.IronLua;

namespace DalamudPluginProjectTemplateLua
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PluginLuaGlobal : LuaGlobal
    {
        private readonly IDictionary<object, object> loaded;

        public PluginLuaGlobal(Lua lua) : base(lua)
        {
            this.loaded = new Dictionary<object, object>();
        }

        [LuaMember("require")]
        public LuaResult LuaRequire(object modName)
        {
            if (modName == null)
                throw new ArgumentNullException();

            if (loaded.TryGetValue(modName, out var currentlyLoaded))
                return new LuaResult(currentlyLoaded);

            try
            {
                var res = DoChunk(Util.GetRelativeFile((string)modName) + ".lua");
                this.loaded.Add(modName, res);
                return res;
            }
            catch (LuaRuntimeException)
            {
                throw;
            }
            catch
            {
                return LuaResult.Empty;
            }
        }
    }
}