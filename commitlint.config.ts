import hceShared from '@halospv3/hce.shared-config/commitlintConfig';

const scopes = {
  AmaiSosu: 'src/AmaiSosu.cs',
  App: 'src/App.*',
  CHANGELOG: 'CHANGELOG.md',
  commitlint: 'This repo\'s commitlint config, esp. its commit scopes.',
  Compile: 'src/Compile.*',
  Configuration: 'src/Configuration*',
  Context: 'src/Context.cs',
  contributing: 'Affects CONTRIBUTING.md.',
  ControllerPreset: 'src/ControllerPreset*',
  'conv-pr': '.github/workflows/conv-pull-requests.yml',
  deps: 'Affects dependencies bundled with or depended on by published packages and artifacts. '
    + 'For NuGet package/PackageReferences, this means anything that has "runtime", "native" or '
    + '"contentfiles" included.',
  'deps-dev': 'Affects dependencies used by CI, dev environments, or the build system(s); '
    + 'but are not required at runtime nor bundled with or statically linked into the published '
    + 'binaries or packages. For NuGet packages/PackageReferences, this would be anything with'
    + 'PrivateAssets="All" and no "runtime", "native", or "contentfiles" to be included in output.',
  Information: 'src/Information.*',
  Install: 'src/Main.Install.cs, src/Main.Window.*, or src/Install*',
  Kernel: 'src/Kernel*',
  Main: 'src/Main.*',
  News: 'src/News.*',
  Paths: 'src/Paths.cs',
  README: 'README.md or any other README documents',
  Report: 'src/Report.*',
  release: 'Reserved for release commits.',
  Setup: 'src/Setup.cs',
  Social: 'src/Social.*',
  Version: 'src/Version.*',
  TODO: 'TODO.md or any todo comments.',
  vscode: './vscode/**/*',
};

hceShared.rules['scope-enum'] = [
  2,
  'always',
  Reflect.ownKeys(scopes)
    .map(key => String(key)),
];

export default hceShared;
