/// <reference types="@halospv3/hce.shared-config/semantic-release__commit-analyzer" />
import { getConfig } from '@halospv3/hce.shared-config/semanticReleaseConfigDotnet';
import type { Options } from 'semantic-release';
import type { PluginSpecSRCommitAnalyzer, PluginSpecSRReleaseNotesGen } from '@halospv3/hce.shared-config/semanticReleaseConfig';
import type { RuleObjects } from '@semantic-release/commit-analyzer';

const projectsToPublish = ['./src/HXE.csproj'];
const projectsToPackAndPush: Parameters<typeof getConfig>[1] = [];
const config: Options | Error = await getConfig(
  projectsToPublish,
  projectsToPackAndPush,
).catch((error: unknown) => {
  const _error = Error.isError(error)
    ? error
    : new Error('unknown error', { cause: error });
  console.error(_error);
  return _error;
});

if (!Error.isError(config)) {
  const sRCA = config.plugins?.find<PluginSpecSRCommitAnalyzer>(
    (p): p is PluginSpecSRCommitAnalyzer => p[0] === '@semantic-release/commit-analyzer'
  );
  if (sRCA) {
    if (typeof sRCA[1].releaseRules === 'string')
      throw new Error('releaseRules was a string; the path to a module whose default export provides RuleObject[]. I don\'t want to deal with it.');
    let releaseRules = sRCA[1].releaseRules ??= [];
    sRCA[1].releaseRules = [
      ...releaseRules,
      { type: 'revert', subject: '!(feat|fix|perf)', release: false },
      { type: 'revert', subject: '(build|chore|ci|docs|refactor|revert|style|test)', release: false },
    ] as RuleObjects.ConventionalCommits[];
  }

  const releaseNotesGen = config.plugins?.find<PluginSpecSRReleaseNotesGen>(
    (p): p is PluginSpecSRReleaseNotesGen => p[0] === '@semantic-release/release-notes-generator'
  );
  if (releaseNotesGen) {
    releaseNotesGen[1].preset = 'conventionalcommits';
  }
}
export default config;
