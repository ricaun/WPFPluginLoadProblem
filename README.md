# WPFPluginLoadProblem

This project is a sample to reproduce the issue in: [Xaml parser doesn't work with AssemblyLoadContext](https://github.com/dotnet/wpf/issues/1700)

## Issue

The `Xaml` parser ignore the `AssemblyLoadContext` and search for the `AssemblyName` inside the `AppDomain.CurrentDomain.GetAssemblies` using the method [GetLoadedAssembly](https://github.com/dotnet/wpf/blob/ac9d1b7a6b0ee7c44fd2875a1174b820b3940619/src/Microsoft.DotNet.Wpf/src/Shared/MS/Internal/SafeSecurityHelper.cs#L133).

This could be problematic if your have two assemblies with the same with different version in different `AssemblyLoadContext`.

In this sample the `xmlns:p=""clr-namespace:Helpers;assembly=Helpers""` trigger the `Xaml` parser to use the `GetLoadedAssembly` method to search for the `Helpers` assembly, and becouse `FirstPlugin` uses `Helpers` version 1 and `SecondPlugin` uses `Helpers` version 2, the `Xaml` parser will always find the first loaded assembly, which make the `Xaml` parser to fail.

---

Do you like this project? Please [star this project on GitHub](../../stargazers)!