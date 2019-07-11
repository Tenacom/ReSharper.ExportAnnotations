[![License](https://img.shields.io/github/license/rdeago/resharper-exportannotations.svg)](https://github.com/rdeago/resharper-exportannotations/blob/master/LICENSE)
[![NuGet downloads](https://img.shields.io/nuget/dt/ReSharper.ExportAnnotations.Task.svg)](https://www.nuget.org/packages/ReSharper.ExportAnnotations.Task/)

[![GitHub downloads](https://img.shields.io/github/downloads/rdeago/resharper-exportannotations/total.svg)](https://github.com/rdeago/resharper-exportannotations/releases)
[![Release date](https://img.shields.io/github/release-date/rdeago/resharper-exportannotations.svg)](https://github.com/rdeago/resharper-exportannotations/releases)
[![Last commit](https://img.shields.io/github/last-commit/rdeago/resharper-exportannotations.svg)](https://github.com/rdeago/resharper-exportannotations/commits/master)
[![Open issues](https://img.shields.io/github/issues-raw/rdeago/resharper-exportannotations.svg)](https://github.com/rdeago/resharper-exportannotations/issues?q=is%3Aissue+is%3Aopen+sort%3Aupdated-desc)
[![Closed issues](https://img.shields.io/github/issues-closed-raw/rdeago/resharper-exportannotations.svg)](https://github.com/rdeago/resharper-exportannotations/issues?utf8=%E2%9C%93&q=is%3Aissue+is%3Aclosed+sort%3Aupdated-desc)

If you find this project useful, please **:star: star it**. Thank you!

## The problem

Let's say you have written a wonderful .NET library that you want to share with the whole world, or just among your own projects.

Let's also say that you, just like me, use [ReSharper](https://www.jetbrains.com/resharper/) for static code analysis, and have constellated your library's code with those nice annotations: `[NotNull]`, `[CanBeNull]`, `[ItemNotNull]`, `[InstantHandle]`, etc. etc. (the complete list is [here](https://www.jetbrains.com/help/resharper/Reference__Code_Annotation_Attributes.html) in case you're curious).

Code annotations are cool, but once you've wrapped your library in a nice NuGet package, how can ReSharper know about the annotations it contains? So far, your options were:
1. Either reference the `JetBrains.Annotations` package in your NuGet package, making it a transient dependency for _all_ projects using your library, and ending up with `JetBrains.Annotations.dll` in your executable directory, where it definitely serves no purpose at all...
2. ...or you can [embed code annotation declarations in your cource code](https://www.jetbrains.com/help/resharper/Code_Analysis__Annotations_in_Source_Code.html#embedding-declarations-of-code-annotations-in-your-source-code). For each project. If you call _this_ an option, that is. I personally do not.
3. You may also _not_ define the `JETBRAINS_ANNOTATIONS` constant when you build your package, thus giving up annotations completely and making code analysis harder and less precise in dependent projects.

Let's be honest: all three options above suck. Either you have to distribute an utterly useless assembly, or you end up with more code to build and maintain, or you just give up a big part of what makes ReSharper worth its price (not to mention loading time).

A fourth option would be to use [Fody](https://github.com/Fody/Fody) with the [JetBrainsAnnotations.Fody](https://github.com/tom-englert/JetBrainsAnnotations.Fody) plugin, that does exactly what this task does. Which is fine, if you _already_ use Fody; otherwise, you have to create a configuration file for it, then reference _two_ packages (Fody itself and the plugin). I've never tried it: it most probably works fine. I was looking for a simpler solution, though, so here it is.

## The solution
```xml
<!-- YourLibrary.csproj -->
<Project Sdk="Microsoft.NET.Sdk"> <!-- It works on legacy projects too (e.g. WinForms control libraries) -->

  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework> <!-- Actually you can use any TFM -->
  </PropertyGroup>

  <PropertyGroup>
    <ExportJetBrainsAnnotations>true</ExportJetBrainsAnnotations> <!-- true by default when OutputType is "Library" -->
    <StripJetBrainsAnnotations>true</StripJetBrainsAnnotations> <!-- true by default -->
  </PropertyGroup>

  <PropertyGroup>
    <!-- Find out latest version here: https://www.nuget.org/packages/JetBrains.Annotations/
         The task will work regardless of this version.
    -->
    <PackageReference Include="JetBrains.Annotations" Version="2019.1.3" PrivateAssets="All" /> <!-- Will not become a transient dependency -->
    <PackageReference Include="ReSharper.ExportAnnotations.Task" Version="1.0.0" PrivateAssets="All" /> <!-- Only used during build -->
  </PropertyGroup>

</Project>
```

That's all you need to do. Here's what happens when you build your project:
* the `JETBRAINS_ANNOTATIONS` constant is automatically defined;
* just after the compiler runs, your compiled assembly is scanned for ReSharper annotations;
* all annotations of exposed types and members of exposed types are exported in an [external annotations file](https://www.jetbrains.com/help/resharper/Code_Analysis__External_Annotations.html);
* the external annotations file is part of the build output, so it is also included in your NuGet package;
* code annotations attributes, as well as the reference to `JetBrains.Annotations.dll`, are stripped from your assembly.

Now, when you reference your library from another project, ReSharper will automatically load annotations from the external annotations file and use them just as if they were compiled into your assembly!

## Compatibility

In short, if you use MSBuild, you can use this task.

|         | .NET Framework | Mono  | .NET Core |
| :------ | :------------: | :---: | :-------: |
| Windows | Yes            | Yes   | Yes       |
| OS/X    | _(n/a)_        | Yes   | Yes       |
| Linux   | _(n/a)_        | Yes   | Yes       |

## Caveats

### Building under Non-Windows operating systems
If you build under a non-Windows operating system, the process of stripping annotations will also strip away debug symbols from your assembly. This is a limitation of the [Mono.Cecil](https://github.com/jbevain/cecil) library. You may want to only strip annotations in Release mode, like this:
```xml
<PropertyGroup>
  <ExportJetBrainsAnnotations>true</ExportJetBrainsAnnotations>
  <StripJetBrainsAnnotations Condition="'$(Configuration)' == 'Release'">true</StripJetBrainsAnnotations>
  <StripJetBrainsAnnotations Condition="'$(Configuration)' != 'Release'">false</StripJetBrainsAnnotations>
</PropertyGroup>
```
Be aware that this way your debug mode assembly will still reference `JetBrains.Annotations.dll`.

### Supported project types

This task has been tested on `.csproj` project files, both "old-style" (no SDK) and using `Microsoft.NET.Sdk`.

It may or may not work with other SDKs.

You should have no problems with `.vbproj` projects, other than the limitations stated above. Projects in languages other than C# and VB are currently not supported.

---

*Disclaimer:* The author of this library is in no way affiliated to [JetBrains s.r.o.](https://www.jetbrains.com/) (the makers of ReSharper) other than being a satisfied cutomer.
