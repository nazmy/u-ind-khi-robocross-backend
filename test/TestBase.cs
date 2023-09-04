using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace test
{
	public abstract class TestBase
	{
        private static readonly Assembly ThisAssembly = typeof(TestBase).Assembly;
        private static readonly string AssemblyName = ThisAssembly.GetName().Name;

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = ThisAssembly.CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                string projectPath = Directory.GetParent(path).Parent.Parent.FullName;
                return Path.GetDirectoryName(projectPath);
            }
        }

        protected string GetExpectedJson([CallerMemberName] string name = null)
        {
            var type = GetType().Name;
            var projectFolder = GetType().Namespace;
            var path = Path.Combine(AssemblyDirectory, type + "_" + name + ".json");
            
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("file not found at " + path);
            }

            return File.ReadAllText(path);
        }
    }
}

