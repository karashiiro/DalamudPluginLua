using System.IO;
using System.Reflection;

namespace DalamudPluginProjectTemplateLua
{
    internal static class Util
    {
        public static string GetRelativeFile(string fileName)
        {
            return Path.Combine(Assembly.GetExecutingAssembly().Location, "..", fileName);
        }
    }
}