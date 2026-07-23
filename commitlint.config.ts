import hceShared from '@halospv3/hce.shared-config/commitlintConfig';

const scopes = {
  App: 'Affects "src/App.*".',
  CHANGELOG: 'Affects "CHANGELOG.md".',
  commitlint: 'Affects this repo\'s commitlint config, esp. its commit scopes.',
  Compile: 'Affects "src/Compile.*".',
  Configuration: 'Affects "src/Configuration*".',
  Context: 'Affects "src/Context.cs".',
  contributing: 'Affects CONTRIBUTING.md.',
  ControllerPreset: 'Affects "src/ControllerPreset*".',
  'conv-pr': 'Affects ".github/workflows/conv-pull-requests.yml".',
  deps: 'Affects dependencies bundled with or depended on by published packages and artifacts. '
    + 'For NuGet package/PackageReferences, this means anything that has "runtime", "native" or '
    + '"contentfiles" included.',
  'deps-dev': 'Affects dependencies used by CI, dev environments, or the build system(s); '
    + 'but are not required at runtime nor bundled with or statically linked into the published '
    + 'binaries or packages. For NuGet packages/PackageReferences, this would be anything with'
    + 'PrivateAssets="All" and no "runtime", "native", or "contentfiles" to be included in output.',
  Information: 'Affects "src/Information.*".',
  Install: 'Affects "src/Main.Install.cs", its section in "src/Main.Window.*", or "src/Install*".',
  Kernel: 'Affects "src/Kernel*".',
  Main: 'Affects "src/Main.*".',
  News: 'Affects "src/News.*".',
  Paths: 'Affects "src/Paths.cs".',
  README: 'Affects README.md or any other README documents.',
  release: 'Reserved for release commits.',
  Version: 'Affects "src/Version.*".',
  TODO: 'Affects TODO.md or any todo comments.',
  vscode: 'Affects "./vscode/**/*".',
};

hceShared.rules['scope-enum'] = [
  2,
  'always',
  Reflect.ownKeys(scopes)
    .map(key => String(key)),
];

export default hceShared;
