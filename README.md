# WPFPluginLoadProblem

This project is a sample to reproduce the issue in: [Xaml parser doesn't work with AssemblyLoadContext](https://github.com/dotnet/wpf/issues/1700)

## Issue

The `Xaml` parser ignore the `AssemblyLoadContext` and search for the `AssemblyName` inside the `AppDomain.CurrentDomain.GetAssemblies` using the method [GetLoadedAssembly](https://github.com/dotnet/wpf/blob/ac9d1b7a6b0ee7c44fd2875a1174b820b3940619/src/Microsoft.DotNet.Wpf/src/Shared/MS/Internal/SafeSecurityHelper.cs#L133).

This could be problematic if your have two assemblies with the same with different version in different `AssemblyLoadContext`.

In this sample the `xmlns:p=""clr-namespace:Helpers;assembly=Helpers""` trigger the `Xaml` parser to use the `GetLoadedAssembly` method to search for the `Helpers` assembly, and becouse `FirstPlugin` uses `Helpers` version 1 and `SecondPlugin` uses `Helpers` version 2, the `Xaml` parser will always find the first loaded assembly, which make the `Xaml` parser to fail.

## Workaround

There are another way to reference the assembly in the `Xaml` file without using the `clr-namespace:` syntax, and is using the `XmlnsPrefix` and `XmlnsDefinition` attributes in the `Helpers` assembly.

```
[assembly: XmlnsPrefix("http://helpers/wpf", "p")]
[assembly: XmlnsDefinition("http://helpers/wpf", "Helpers")]
```

In the `FirstPlugin` and `SecondPlugin` projects, replace the `xmlns:p="clr-namespace:Helpers;assembly=Helpers"` with `xmlns:p="http://helpers/wpf"`.

After this change the `Xaml` parser will resolve and load the correct assembly in the current `AssemblyLoadContext` and the issue will be fixed.

The sample checks if the `Xaml` parsed uses the correct version of the `Helpers` assembly and show the `AssemblyLoadContext`.

```

---

Do you like this project? Please [star this project on GitHub](../../stargazers)!