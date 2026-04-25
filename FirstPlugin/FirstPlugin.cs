using PluginBase;
using System;
using System.IO;
using System.Windows.Markup;
using System.Xml;

namespace FirstPlugin
{
    public class FirstPlugin : IPlugin
    {
        public void Run()
        {
            Console.WriteLine("FirstPlugin: running");
            Helpers.Helpers.Version1Function();

            var xaml = @"
<ResourceDictionary
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:p=""clr-namespace:Helpers;assembly=Helpers"">
    <Style x:Key=""ButtonWithName"" TargetType=""{x:Type Button}"">
        <Setter Property=""p:PropertiesVersion1.ButtonName"" Value=""Frank""/>
    </Style>
</ResourceDictionary>
";
            // The below XAML uses "http://helpers/wpf" to reference the Helpers assembly, which in this plugin is Version 1.0.0.0
            var xaml1 = @"
<ResourceDictionary
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:p=""http://helpers/wpf"">
    <Style x:Key=""ButtonWithName"" TargetType=""{x:Type Button}"">
        <Setter Property=""p:PropertiesVersion1.ButtonName"" Value=""Frank""/>
    </Style>
</ResourceDictionary>
";
            xaml = xaml1;
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
