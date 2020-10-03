# Changelog

All notable changes to `ReSharper.ExportAnnotations` will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Unreleased changes

### New features

- `ReSharper.ExportAnnotations.Core` is now distributed with a symbol package, thanks to [SourceLink](https://github.com/dotnet/sourcelink).
- `ReSharper.ExportAnnotations.Task` checks for the presence of a package reference to `JetBrains.Annotations`, issuing a build-time error if it is missing. This functionality may be disabled by setting the `CheckForJetBrainsAnnotationsPackageReference` property to `false`.
- `ReSharper.ExportAnnotations.Task` automatically updates the package reference to `JetBrains.Annotations`, ensuring that it is treated as a development dependency. This functionality may be disabled by setting the `UpdateJetBrainsAnnotationsPackageReferencee` property to `false`.
- The name of the package containing the annotations DLL, for the purposes of the two features described above, may be specified via the `JetBrainsAnnotationsPackageName` property. When no name is specified, the default is `JetBrains.Annotations`.

### Changes to existing features

- `ReSharper.ExportAnnotations.Task` does no more strip annotations from compiled assemblies; instead, it invokes the Build target again without the `JETBRAINS_ANNOTATIONS` constant. This avoids the problems caused by the `Mono.Cecil` library not saving assemblies properly (see https://github.com/tenacom/ReSharper.ExportAnnotations/issues/2, https://github.com/tenacom/ReSharper.ExportAnnotations/issues/13, and https://github.com/jbevain/cecil/issues/610 for details). Note that the `--strip` parameter of `ExportAnnotations` and the associated functionality remain unchanged, although they may be removed or deprecated in a future release.

### Bugs fixed in this release

- https://github.com/tenacom/ReSharper.ExportAnnotations/issues/2 - Stripping annotations also strips the signature away from strong-named assemblies
- https://github.com/tenacom/ReSharper.ExportAnnotations/issues/13 - Stripping annotations also removes the portable PDB checksum

### Known problems introduced by this release

## [1.1.0](https://github.com/tenacom/ReSharper.ExportAnnotations/releases/tag/1.1.0) (2020-09-17)

### New features

- `ExportAnnotations` has a new `--no-export` option to skip the exporting phase entirely.

### Changes to existing features

- Ownership of the project changed from @rdeago to @Tenacom.

### Bugs fixed in this release

- https://github.com/tenacom/ReSharper.ExportAnnotations/issues/3 - ReSharper.ExportAnnotations.Task does not work with `dotnet build` and `dotnet msbuild`
- https://github.com/tenacom/ReSharper.ExportAnnotations/issues/6 - Annotations are not stripped from executables

## [1.0.0](https://github.com/tenacom/ReSharper.ExportAnnotations/releases/tag/1.0.0) (2019-06-30) [DEPRECATED]

Initial release, deprecated because of ownership change.
