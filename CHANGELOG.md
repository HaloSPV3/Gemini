## [2.3.0-alpha.1](https://github.com/HaloSPV3/SPV3.Loader/compare/v2.2.2...v2.3.0-alpha.1) (2026-07-22)

### Features

* **Configuration:** change "Advanced" Settings button to "More Settings..." for clarity ([20d4f42](https://github.com/HaloSPV3/SPV3.Loader/commit/20d4f423b328dffba1cd5c1d2ee9af124ffc7fdf)), closes [#126](https://github.com/HaloSPV3/SPV3.Loader/issues/126)
* **deps:** multi-target n462, net480, and net6.0-windows ([c752ef4](https://github.com/HaloSPV3/SPV3.Loader/commit/c752ef424cb4731123615965e922ab5e90e481c8))
* **Install:** enable DRM patch for everyone who needs it [FREE FOR ALL] ([583047b](https://github.com/HaloSPV3/SPV3.Loader/commit/583047b49a1e54b1b6ad1523c5abb285297cd3db))

### Bug Fixes

* **deps:** add `Meziantou.Polyfill` to use newer features of .NET C# ([b2117b2](https://github.com/HaloSPV3/SPV3.Loader/commit/b2117b2a921a5823a6958d9755a2f5b3d95c0f46))
* **deps:** prefer HXE nupkg; conditionally use HXE git submodule ([bf6a230](https://github.com/HaloSPV3/SPV3.Loader/commit/bf6a230e2ef4f94f2200a7383a33f716039554ac))
* **deps:** re-add Costura.Fody for single-file legacy releases; git-ignore FodyWeavers.xsd ([d148851](https://github.com/HaloSPV3/SPV3.Loader/commit/d1488514d476ec79d4d79c05efc3e9ba06aac332))
* **deps:** remove explicit SourceLink; rely on SDK's built-in SourceLink ([e9247d8](https://github.com/HaloSPV3/SPV3.Loader/commit/e9247d85f1599455b5680eb81b396b56b253b7e5))
* **deps:** remove unused dependency `Microsoft.Windows.Compatibility` ([536cd03](https://github.com/HaloSPV3/SPV3.Loader/commit/536cd03d9fd7b2b5a2ac4400a3a679cbbd242e13))
* **deps:** upgrade bundled Discord Rich Presence DLL to v1.0.5 for non-vandalized images + Firefight maps ([754381c](https://github.com/HaloSPV3/SPV3.Loader/commit/754381caf3e821bfeb949fceba270904b17870b3))
* **deps:** upgrade GitVersion.MsBuild to 6.8.2 ([edbccd8](https://github.com/HaloSPV3/SPV3.Loader/commit/edbccd8fb9fc54c5e6ba3fc36ab329a05acff6e6))
* **Install:** failover to `GetDirectoryRoot` if `GetParent` fails in `ValidateTarget(string)` ([c480950](https://github.com/HaloSPV3/SPV3.Loader/commit/c48095029c545c5602c50bbc79445908ba819c71))
* **News:** handle potential null-references ([cd424a7](https://github.com/HaloSPV3/SPV3.Loader/commit/cd424a712103c851ec29e29cc6c15f3b75d84f85))
* **Version:** replace Git commit `hash` resource with GitVersion `ShortSha` ([6c9dd6f](https://github.com/HaloSPV3/SPV3.Loader/commit/6c9dd6f93e8272ec1dd47881242ccc9a89ccb42a))

### Reverts

* undo pointless changes to `release` script; just needed to install semantic-release ([ef1760c](https://github.com/HaloSPV3/SPV3.Loader/commit/ef1760c5a871230d47964cfff6ba39bde0946e96))

## [2.3.0-alpha.1](https://github.com/HaloSPV3/SPV3.Loader/compare/v2.2.2...v2.3.0-alpha.1) (2026-07-22)

### Features

* **Configuration:** change "Advanced" Settings button to "More Settings..." for clarity ([20d4f42](https://github.com/HaloSPV3/SPV3.Loader/commit/20d4f423b328dffba1cd5c1d2ee9af124ffc7fdf)), closes [#126](https://github.com/HaloSPV3/SPV3.Loader/issues/126)
* **deps:** multi-target n462, net480, and net6.0-windows ([c752ef4](https://github.com/HaloSPV3/SPV3.Loader/commit/c752ef424cb4731123615965e922ab5e90e481c8))
* **Install:** enable DRM patch for everyone who needs it [FREE FOR ALL] ([583047b](https://github.com/HaloSPV3/SPV3.Loader/commit/583047b49a1e54b1b6ad1523c5abb285297cd3db))

### Bug Fixes

* **deps:** add `Meziantou.Polyfill` to use newer features of .NET C# ([b2117b2](https://github.com/HaloSPV3/SPV3.Loader/commit/b2117b2a921a5823a6958d9755a2f5b3d95c0f46))
* **deps:** prefer HXE nupkg; conditionally use HXE git submodule ([bf6a230](https://github.com/HaloSPV3/SPV3.Loader/commit/bf6a230e2ef4f94f2200a7383a33f716039554ac))
* **deps:** re-add Costura.Fody for single-file legacy releases; git-ignore FodyWeavers.xsd ([d148851](https://github.com/HaloSPV3/SPV3.Loader/commit/d1488514d476ec79d4d79c05efc3e9ba06aac332))
* **deps:** remove explicit SourceLink; rely on SDK's built-in SourceLink ([e9247d8](https://github.com/HaloSPV3/SPV3.Loader/commit/e9247d85f1599455b5680eb81b396b56b253b7e5))
* **deps:** remove unused dependency `Microsoft.Windows.Compatibility` ([536cd03](https://github.com/HaloSPV3/SPV3.Loader/commit/536cd03d9fd7b2b5a2ac4400a3a679cbbd242e13))
* **deps:** upgrade bundled Discord Rich Presence DLL to v1.0.5 for non-vandalized images + Firefight maps ([754381c](https://github.com/HaloSPV3/SPV3.Loader/commit/754381caf3e821bfeb949fceba270904b17870b3))
* **deps:** upgrade GitVersion.MsBuild to 6.8.2 ([edbccd8](https://github.com/HaloSPV3/SPV3.Loader/commit/edbccd8fb9fc54c5e6ba3fc36ab329a05acff6e6))
* **Install:** failover to `GetDirectoryRoot` if `GetParent` fails in `ValidateTarget(string)` ([c480950](https://github.com/HaloSPV3/SPV3.Loader/commit/c48095029c545c5602c50bbc79445908ba819c71))
* **Version:** replace Git commit `hash` resource with GitVersion `ShortSha` ([6c9dd6f](https://github.com/HaloSPV3/SPV3.Loader/commit/6c9dd6f93e8272ec1dd47881242ccc9a89ccb42a))

### Reverts

* undo pointless changes to `release` script; just needed to install semantic-release ([ef1760c](https://github.com/HaloSPV3/SPV3.Loader/commit/ef1760c5a871230d47964cfff6ba39bde0946e96))
