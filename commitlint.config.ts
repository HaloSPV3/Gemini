import hceShared from "@halospv3/hce.shared-config/commitlintConfig";

const scopes = {
  CHANGELOG: 'Affects "CHANGELOG.md".',
  deps: 'Affects any runtime dependencies.',
  'deps-dev': 'Affects dependencies not included at runtime.',
  README: 'Affects "README.md".',
  release: 'Reserved for release commits.',
  vscode: 'Affects "./vscode/**/*".'
};

hceShared.rules["scope-enum"] = [
  2,
  'always',
  Reflect.ownKeys(scopes)
    .map(key => String(key)) as (keyof typeof scopes)[]
];

export default hceShared;
