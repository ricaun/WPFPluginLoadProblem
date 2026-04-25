using PluginBase;
using System;
using System.IO;
using System.Windows.Markup;
using System.Xml;

namespace SecondPlugin
{
    public class SecondPlugin : IPlugin
    {
        public void Run()
        {
            Console.WriteLine("SecondPlugin: running");
            Helpers.Helpers.Version2Function();

            // Here we refer to Helpers.Helpers.PropertiesVersion2
            // just like the above call, the second plugin has a dependency on Helpers.dll Version=2.0.0.0.
            // The above call works because the runtime correctly resolves that reference using the current
            // assembly load context.
            // But the XAML parser below walks all assemblies in the AppDomain and finds a first lose match
            // for the "Helpers" assembly, which will resolve to Helpers.dll Version=1.0.0.0 (since that was loaded
            // by the first plugin).
            // This happens even though the entire plugin runs with contextual reflection set to the second
            // plugin's load context.
            var xaml = @"
<ResourceDictionary
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:p=""clr-namespace:Helpers;assembly=Helpers"">
    <Style x:Key=""ButtonWithName"" TargetType=""{x:Type Button}"">
        <Setter Property=""p:PropertiesVersion2.ButtonName"" Value=""Frank""/>
    </Style>
</ResourceDictionary>
";
            // The below XAML uses "http://helpers/wpf" to reference the Helpers assembly, which in this plugin is Version 2.0.0.0
            var xaml2 = @"
<ResourceDictionary
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:p=""http://helpers/wpf"">
    <Style x:Key=""ButtonWithName"" TargetType=""{x:Type Button}"">
        <Setter Property=""p:PropertiesVersion2.ButtonName"" Value=""Frank""/>
    </Style>
</ResourceDictionary>
";
            xaml = xaml2;
            Console.WriteLine(xaml);
            var reader = new StringReader(xaml);
            var xml = XmlReader.Create(reader);
            var o = XamlReader.Load(xml);
            if (o is System.Windows.ResourceDictionary rd)
            {
                var style = rd["ButtonWithName"] as System.Windows.Style;
                Console.WriteLine($"Successfully loaded XAML with style: {style}");
                // get setter property value
                var setter = style.Setters[0] as System.Windows.Setter;
                Console.WriteLine($"Successfully loaded XAML with setter: {setter}");

                var property = setter.Property;
                var value = setter.Value;
                Console.WriteLine($"Successfully loaded XAML with setter property: {property} and value: {value}");
                var assembly = property.OwnerType.Assembly;
                Console.WriteLine($"Successfully loaded XAML with setter property assembly: {assembly}");
                var context = System.Runtime.Loader.AssemblyLoadContext.GetLoadContext(assembly);
                Console.WriteLine($"Successfully loaded XAML with setter property assembly load context: {context}");
            }
        }
    }
}
