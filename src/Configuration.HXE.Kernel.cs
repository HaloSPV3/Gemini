/**
 * Copyright (c) 2019 Emilian Roman
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 */

using System.ComponentModel;
using System.Runtime.CompilerServices;
using SPV3.Annotations;

namespace SPV3
{
  public partial class Configuration
  {
    public partial class ConfigurationHXE
    {
      public class ConfigurationHXEKernel : INotifyPropertyChanged
      {
        private bool _enableSpv3KernelMode;
        private bool _enableSpv3LegacyMode;
        private bool _skipInvokeCoreTweaks;
        private bool _skipInvokeExecutable;
        private bool _skipPatchLargeAAware;
        private bool _skipResumeCheckpoint;
        private bool _skipSetShadersConfig;
        private bool _skipVerifyMainAssets;

        public bool SkipVerifyMainAssets
        {
          get => _skipVerifyMainAssets;
          set
          {
            if (value == _skipVerifyMainAssets) return;
            _skipVerifyMainAssets = value;
            OnPropertyChanged();
          }
        }

        public bool SkipInvokeCoreTweaks
        {
          get => _skipInvokeCoreTweaks;
          set
          {
            if (value == _skipInvokeCoreTweaks) return;
            _skipInvokeCoreTweaks = value;
            OnPropertyChanged();
          }
        }

        public bool SkipResumeCheckpoint
        {
          get => _skipResumeCheckpoint;
          set
          {
            if (value == _skipResumeCheckpoint) return;
            _skipResumeCheckpoint = value;
            OnPropertyChanged();
          }
        }

        public bool SkipSetShadersConfig
        {
          get => _skipSetShadersConfig;
          set
          {
            if (value == _skipSetShadersConfig) return;
            _skipSetShadersConfig = value;
            OnPropertyChanged();
          }
        }

        public bool SkipInvokeExecutable
        {
          get => _skipInvokeExecutable;
          set
          {
            if (value == _skipInvokeExecutable) return;
            _skipInvokeExecutable = value;
            OnPropertyChanged();
          }
        }

        public bool SkipPatchLargeAAware
        {
          get => _skipPatchLargeAAware;
          set
          {
            if (value == _skipPatchLargeAAware) return;
            _skipPatchLargeAAware = value;
            OnPropertyChanged();
          }
        }

        public bool EnableSpv3KernelMode
        {
          get => _enableSpv3KernelMode;
          set
          {
            if (value == _enableSpv3KernelMode) return;
            _enableSpv3KernelMode = value;
            OnPropertyChanged();
          }
        }

        public bool EnableSpv3LegacyMode
        {
          get => _enableSpv3LegacyMode;
          set
          {
            if (value == _enableSpv3LegacyMode) return;
            _enableSpv3LegacyMode = value;
            OnPropertyChanged();
          }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
      }
    }
  }
}