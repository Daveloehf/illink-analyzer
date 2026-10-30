# illink-analyzer

The .NET trim, AOT and single-file analyzer (ILLink.RoslynAnalyzer) from
[dotnet/runtime](https://github.com/dotnet/runtime/tree/main/src/tools/illink), forked at the commit in
[`upstream-commit`](upstream-commit) (tag `v10.0.14`), and published as the
`Sharpity.ILLink.RoslynAnalyzer` package. Sharpity's desktop build uses it in place of the SDK's copy.

## Changes from upstream

Only these files differ from the upstream commit:

- `Shared/DataFlow/ForwardDataFlowAnalysis.cs`: the fixpoint solver revisits only the blocks whose
  inputs changed, and still ends on a full sweep that changes nothing. Its visit limit scales with the
  method; upstream's fixed limit of 10,000 visits stops large methods early and silently drops their
  warnings.
- `Shared/DataFlow/ValueSet.cs`: a set whose values are all immutable is shared between states instead
  of being copied and rehashed on every meet.
- `Analyzer/TrimAnalysis/FieldValue.cs`, `Analyzer/TrimAnalysis/MethodReturnValue.cs`: the symbol hash
  is computed once per value.

Paths are under `src/ILLink.RoslynAnalyzer/`. The rest of `Analyzer/` and `Shared/` is upstream's
`src/tools/illink/src/ILLink.RoslynAnalyzer/` and `ILLink.Shared/`, unchanged, so code-quality changes to
those files belong upstream. `External/` holds the `src/libraries/` files upstream links into the
analyzer; `SR.cs` and the project file stand in for Arcade's generated resources.

## Testing

`scripts/test-upstream` fetches upstream's test suite at `upstream-commit`, puts this fork in place of
upstream's analyzer and runs the suite together with the cases in `test/`.
`test/LargeMethodDataFlowTests.cs` covers the visit limit; the rest of `test/` is build glue that lets
upstream's projects build without Arcade.

## Releasing

CI runs the suite on every pull request and push. On `main`, it packs the analyzer and pushes the
package to GitHub Packages; a version that already exists is skipped, so raising `<Version>` in
`src/ILLink.RoslynAnalyzer/ILLink.RoslynAnalyzer.csproj` publishes a release.

## Updating upstream

Replace `Analyzer/`, `Shared/` and `External/` with the new upstream files, reapply the changes above,
update `upstream-commit` and the version prefix, and run `scripts/test-upstream`.
