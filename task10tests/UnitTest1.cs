namespace task10tests;

using System.Reflection;
using task10;
using pluginLoader;
public class PluginLoaderTests
    {
        private readonly string _pluginsPath = "../../../../plugins";

        [Fact]
        public void Should_Load_All_Plugin_Assemblies()
        {
            var loader = new PluginLoader(_pluginsPath);
            loader.LoadAllAssemblies();
            var assemblies = GetPrivateField<List<Assembly>>(loader, "_loadedAssemblies");
            
            Assert.Contains(assemblies, a => a.FullName.Contains("plugin1"));
            Assert.Contains(assemblies, a => a.FullName.Contains("plugin2"));
        }

        [Fact]
        public void Should_Find_Only_Plugin_Types()
        {
            var loader = new PluginLoader(_pluginsPath);
            loader.LoadAllAssemblies();
            loader.FindAllPluginTypes();

            var pluginTypes = GetPrivateField<List<Type>>(loader, "_pluginTypes");
            var typeNames = pluginTypes.Select(t => t.Name).ToList();
            
            Assert.Equal(4, pluginTypes.Count); 
            Assert.Contains("PluginA", typeNames);
            Assert.Contains("PluginB", typeNames);
            Assert.Contains("PluginC", typeNames);
            Assert.Contains("PluginD", typeNames);
            Assert.DoesNotContain("NotPlugin", typeNames);
        }

        [Fact]
        public void Should_Sort_By_Dependencies_Correctly()
        {
            var loader = new PluginLoader(_pluginsPath);
            loader.LoadAllAssemblies();
            loader.FindAllPluginTypes();
            var sorted = loader.SortByDependenses();

            var pluginNames = sorted.Select(t => t.Name).ToList();
            Assert.Equal("PluginB", pluginNames[0]);
            
            var indexB = pluginNames.IndexOf("PluginB");
            var indexA = pluginNames.IndexOf("PluginA");
            var indexD = pluginNames.IndexOf("PluginD");
            var indexC = pluginNames.IndexOf("PluginC");
            
            if (indexA >= 0 && indexB >= 0)
            {
                Assert.True(indexB < indexA);
            }
            
            if (indexD >= 0 && indexC >= 0)
            {
                Assert.True(indexD > indexC);
            }
        }

        [Fact]
        public void Should_Execute_All_Plugins()
        {
            var loader = new PluginLoader(_pluginsPath);
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            loader.LoadPlugins();
            var output = consoleOutput.ToString();
            
            Assert.Contains("Executing plugin: plugin1.PluginA", output);
            Assert.Contains("Executing plugin: plugin1.PluginB", output);
            Assert.Contains("Executing plugin: plugin1.PluginC", output);
            Assert.Contains("Executing plugin: plugin2.PluginD", output);
            Assert.DoesNotContain("NotPlugin", output);
        }

        [Fact]
        public void Should_Handle_Missing_Plugins_Directory()
        {
            var invalidPath = Path.Combine("..", "non_existing_plugins");
            var loader = new PluginLoader(invalidPath);
            
            Assert.Throws<DirectoryNotFoundException>(() => loader.LoadAllAssemblies());
        }

        private T GetPrivateField<T>(object obj, string fieldName)
        {
            var field = obj.GetType().GetField(fieldName, 
                BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)field.GetValue(obj);
        }
    }