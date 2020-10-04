# ![ReSharper.ExportAnnotations](https://raw.githubusercontent.com/Tenacom/ReSharper.ExportAnnotations/main/graphics/Logo.png)

[![License](https://img.shields.io/github/license/Tenacom/ReSharper.ExportAnnotations.svg)](https://github.com/Tenacom/ReSharper.ExportAnnotations/blob/main/LICENSE)
[![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/Tenacom/ReSharper.ExportAnnotations?include_prereleases)](https://github.com/Tenacom/ReSharper.ExportAnnotations/releases)
[![Changelog](https://img.shields.io/badge/changelog-Keep%20a%20Changelog%20v1.0.0-%23E05735)](https://github.com/Tenacom/ReSharper.ExportAnnotations/blob/main/CHANGELOG.md)

[![Last commit](https://img.shields.io/github/last-commit/Tenacom/ReSharper.ExportAnnotations.svg)](https://github.com/Tenacom/ReSharper.ExportAnnotations/commits/main)
[![Open issues](https://img.shields.io/github/issues-raw/Tenacom/ReSharper.ExportAnnotations.svg?label=open+issues)](https://github.com/Tenacom/ReSharper.ExportAnnotations/issues?q=is%3Aissue+is%3Aopen+sort%3Aupdated-desc)
[![Closed issues](https://img.shields.io/github/issues-closed-raw/Tenacom/ReSharper.ExportAnnotations.svg?label=closed+issues)](https://github.com/Tenacom/ReSharper.ExportAnnotations/issues?q=is%3Aissue+is%3Aclosed+sort%3Aupdated-desc)

The ultimate, one-stop solution to distribute ReSharper<sup>[(*)](#jetbrains-disclaimer)</sup> code annotations in XML format along with your libraries.

Enjoy ReSharper recognizing your code annotations when working on dependent projects, without dragging transitive dependencies around, and without [redeclaring the same attributes](https://www.jetbrains.com/help/resharper/Code_Analysis__Annotations_in_Source_Code.html#embedding-declarations-of-code-annotations-in-your-source-code) over and over.

---

- [Quick start](#quick-start)
- [How it works](#how-it-works)
- [FAQ](#faq)
- [Configuration](#configuration)
  - [Exporting / not exporting code annotations](#exporting--not-exporting-code-annotations)
  - [Stripping / not stripping code annotations](#stripping--not-stripping-code-annotations)
  - [Using a different package for code annotations](#using-a-different-package-for-code-annotations)
  - [Skipping the check for the code annotations package reference](#skipping-the-check-for-the-code-annotations-package-reference)
- [Credits](#credits)

---

## Quick start

1. Add package references to both [`ReSharper.ExportAnnotations.Task`](https://www.nuget.org/packages/ReSharper.ExportAnnotations.Task) and [`JetBrains.Annotations`](https://www.nuget.org/packages/Jetbrains.Annotations) to your library's `.csproj` or `.vbproj` file. You can either do it manually, or use Visual Studio Package Manager.
2. **Do NOT add `JETBRAINS_ANNOTATIONS` to your project's `DefineConstants`!** Remove the constant if you had previously added it. Again, you can either manually edit the project file, or use Visual Studio's UI.
3. Build and pack your library as usual.
4. Just kidding, there is no step 4. :) An `<assembly_name>.ExternalAnnotations.xml` file has been added alongside your compiled assembly, both in the output directory and in the NuGet package. No `JetBrains.Annotations` dependency will be propagated to projects referencing your library.

## How it works

Here is, in a nutshell, how `ReSharper.ExportAnnotations.Task` performs its magic:

1. Add `JETBRAINS_ANNOTATIONS` to the project's `DefineConstants`.
2. Just after the output assembly has been compiled, examine it looking for code annotations and export them in XML format. By default this step is performed only on libraries, not on executables, but this behavior [can be configured](#exporting--not-exporting-code-annotations).
3. After the `Build` target has completed, if code annotations stripping is requested (which is true by default but [can be configured](#stripping--not-stripping-code-annotations)):
   - delete the compiled assembly (otherwise the compiler wouldn't be invoked again, due to incremental building);
   - run the `Build` target again without the `JETBRAINS_ANNOTATIONS` constant. The resulting assembly will have no annotations built in.
4. If an XML file was generated in step 2, carry out some MSBuild trickery to make it an "official" part of the build output.

## FAQ

**Which languages are supported?**

Both C# and Visual Basic projects whose build output is either a `.dll` or a `.exe` file are supported. Support for other languages may be added on request.

**Are legacy (non-SDK) projects supported?**

Yes, as long as they use `PackageReference`.

**Does it work when building with .NET CLI / Mono / VS Code / Visual Studio / Visual Studio for Mac?**

Yes, yes, yes, yes, and most probably. If you encounter problems when using `ReSharper.ExportAnnotations.Task` with your preferred toolchain, please [open an issue](https://github.com/tenacom/ReSharper.ExportAnnotations/issues/new/choose) to let us know.

**Is the assembly's signature preserved?**

In a word, yes. `ReSharper.ExportAnnotations.Task` does not modify assemblies: instead, it rebuilds them from scratch, thus ensuring that the final output assembly is every bit as good as if built without annotations from the get-go.

**Is the PDB checksum preserved? Will I be able to distribute symbol packages?**

Yes, of course. See the answer to the previous question.

**What about my custom MSBuild targets running before / after Build? Will they run twice?**

By default, yes they will: the `Build` target, with all its dependee and attached targets, will run twice for every `TargetPlatform`.

There are two ways to prevent a custom build-related target from running twice:

1. implement [incremental building](https://docs.microsoft.com/en-us/visualstudio/msbuild/how-to-build-incrementally) to make sure a target does not run again if its outputs are already in sync with its inputs;
2. check the value of the `RebuildingWithoutJetBrainsAnnotations` property and skip your target if it is equal to `true`, like this:

```XML
  <Target Name="TargetThatWillNotRunWhenRebuilding"
          AfterTargets="Build"
          Condition="'RebuildingWithoutJetBrainsAnnotations' != 'true'">

    <!-- Anything your target does -->

  </Target>
```

**My solution's build process is so complicated that I have to drive it with an external program / a PowerShell script. Can I just export code annotations programmatically and take care of the rest myself?**

Of course you can. Just reference [`ReSharper.ExportAnnotations.Core`](https://www.nuget.org/packages/ReSharper.ExportAnnotations.Core) (it's a .NET Standard 2.0 library) and take a look at the `AnnotationsExporter` class. It's very easy to use, has full XML documentation, and of course the package also contains external annotations.

**I use a different package than `JetBrains.Annotations`. I get an error upon restore / build because there is no package reference to `JetBrains.Annotations`. What do I do?**

You can either change the name of the required package, as explained [below](#using-a-different-package-for-code-annotations), or disable the check completely by following [these instructions](#skipping-the-check-for-the-code-annotations-package-reference).

**I use a custom MSBuild SDK that adds the `JetBrains.Annotations` package reference from `Sdk.targets`. I get the same error as in the previous question, although the package reference is actually there. What do I do?**

`Sdk.targets` from SDKs are evaluated after `.targets` files from packages; therefore, `ReSharper.ExportAnnotations.Task` will check for a `PackageReference` item that has yet to be added,

In this case you have no other choice than to disable the check following [the instructions below](#skipping-the-check-for-the-code-annotations-package-reference).

## Configuration

The behavior of `ReSharper.ExportAnnotations.Task` may be controlled using MSBuild properties.

### Exporting / not exporting code annotations

Set the `ExportJetBrainsAnnotations` property to `true` to generate a `ExternalAnnotations.xml` file, `false` to skip the export phase.

The default value is `true` for projects whose `OutputType` is `Library`, `false` for other projects.

### Stripping / not stripping code annotations

Please note that "stripping" here does not mean that the assembly is modified: it is simply rebuilt without annotations.

Set the `StripJetBrainsAnnotations` property to `true` to rebuild the assembly without annotations, `false` to skip the rebuild phase.

The default value is `true` for all projects.

### Using a different package for code annotations

`ReSharper.ExportAnnotations.Task` automatically modifies the `PackageReference` item referencing `JetBrains.Annotations` to make sure it does not propagate as a transitive dependency.

If for some reason you prefer to use another package to provide code annotations, just set the `JetBrainsAnnotationsPackageName` property to the name of your preferred package.

You can also disable this feature completely by setting `UpdateJetBrainsAnnotationsPackageReference` to `false`. In this case, make sure that the `PrivateAssets` metadata on your package reference is set to `all`, lest every project referencing your library will also depend on the code annotations package, even if your distributed assembly contains no annotations.

### Skipping the check for the code annotations package reference

Before building, or even restoring dependencies, `ReSharper.ExportAnnotations.Task` checks whether a `PackageReference` exists for the package containing the code annotations DLL. This can either be `JetBrains.Annotations`, or any other name set via the `JetBrainsAnnotationsPackageName` property. If the reference is missing, the build is stopped with an error.

You may disable this check by setting the `CheckForJetBrainsAnnotationsPackageReference` property to `false`.

## Credits

The font used in the package icon is [Inconsolata Bold](https://fontlibrary.org/en/font/inconsolata#Inconsolata-Bold) by Raph Levien, Kirill Tkachev (cyreal.org), from [Font Library](https://fontlibrary.org).

The font used in the logo is [BloggerSans.otf](https://fontlibrary.org/en/font/blogger-sans-otf) by Sergiy S. Tkachenko, from [Font Library](https://fontlibrary.org).

---

<a name="jetbrains-disclaimer">(*)</a> [ReSharper](https://www.jetbrains.com/resharper) is a product of [JetBrains s.r.o.](https://www.jetbrains.com). The authors and owners of this project are not affiliated with JetBrains in any way. The use of the ReSharper name does not imply endorsement of this project by JetBrains.
