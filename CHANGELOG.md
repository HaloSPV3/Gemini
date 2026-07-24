## [2.3.0-alpha.2](https://github.com/HaloSPV3/SPV3.Loader/compare/v2.3.0-alpha.1...v2.3.0-alpha.2) (2026-07-24)

### Bug Fixes

* **Configuration:** prevent possible `InvalidOperationException` in `AdaptiveHDRChanged` ([7454ea0](https://github.com/HaloSPV3/SPV3.Loader/commit/7454ea01fb33c1bc374342ae6caaebd87047caf2))
* **Install:** add detailed Exception when the install path lacks a path root i.e. drive/volume letter ([a0f17da](https://github.com/HaloSPV3/SPV3.Loader/commit/a0f17da2263dfd6a6fa79fd3417fc72c12671007))
* **Main:** change Assets (update.hxe) address to https://raw.githubusercontent.com/HaloSPV3/HCE/meta/update.hxe ([c869e93](https://github.com/HaloSPV3/SPV3.Loader/commit/c869e9329d15fa42128470f4bc4dd7b6b336ab51))
* **Main:** initialize MainError.Content to `string.Empty` ([3419e29](https://github.com/HaloSPV3/SPV3.Loader/commit/3419e29613ce775dbc52ad85a48dd6d58211aacc))
* **News:** add default "Fail" message when News fails to fetch or parse ([f2cb2b1](https://github.com/HaloSPV3/SPV3.Loader/commit/f2cb2b119e41d3977d7bc9b01fc0fbb5966d5f8b))
* **Report:** init Report.Stack to `string.Empty` ([6adc027](https://github.com/HaloSPV3/SPV3.Loader/commit/6adc027d0db3db43dcbdbac226014cfa2fe49648))
* **Version:** gracefully handle `HXE.Latest` null deserialization ([2fea9d2](https://github.com/HaloSPV3/SPV3.Loader/commit/2fea9d22983643ca82e2476e25be8d3fe4ad57db))
* **Version:** init Address, Content to `string.Empty` and `Version` to a new `Version` object to prevent read-before-init errors ([d046e92](https://github.com/HaloSPV3/SPV3.Loader/commit/d046e92b1ad143ab58e07457b1b8b4933ba3a90d))
* **Version:** restore HttpClient timeout in `finally` clause ([56824e4](https://github.com/HaloSPV3/SPV3.Loader/commit/56824e4ebf7f4de2e7dd1d31c1af7b76e0cf2bd0))

## [2.3.0-alpha.1](https://github.com/HaloSPV3/SPV3.Loader/compare/v2.2.2...v2.3.0-alpha.1) (2026-07-23)

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
* **Social:** update Twitter URL to x.com/halo_legacies ([d02c336](https://github.com/HaloSPV3/SPV3.Loader/commit/d02c336bc3497e6b79dc6c84a9e8c589f3bc5fc3))
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
