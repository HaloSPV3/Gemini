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
      public class ConfigurationHXEShaders : INotifyPropertyChanged
      {
        private int  _dof               = 2;
        private bool _dynamicLensFlares = true;
        private bool _filmGrain         = true;
        private bool _hudVisor          = true;
        private bool _lensDirt          = true;
        private int  _motionBlur        = 3;
        private int  _mxao              = 2;
        private bool _volumetrics       = true;

        public bool DynamicLensFlares
        {
          get => _dynamicLensFlares;
          set
          {
            if (value == _dynamicLensFlares) return;
            _dynamicLensFlares = value;
            OnPropertyChanged();
          }
        }

        public bool Volumetrics
        {
          get => _volumetrics;
          set
          {
            if (value == _volumetrics) return;
            _volumetrics = value;
            OnPropertyChanged();
          }
        }

        public bool LensDirt
        {
          get => _lensDirt;
          set
          {
            if (value == _lensDirt) return;
            _lensDirt = value;
            OnPropertyChanged();
          }
        }

        public bool HudVisor
        {
          get => _hudVisor;
          set
          {
            if (value == _hudVisor) return;
            _hudVisor = value;
            OnPropertyChanged();
          }
        }

        public bool FilmGrain
        {
          get => _filmGrain;
          set
          {
            if (value == _filmGrain) return;
            _filmGrain = value;
            OnPropertyChanged();
          }
        }

        public int Mxao
        {
          get => _mxao;
          set
          {
            if (value == _mxao) return;
            _mxao = value;
            OnPropertyChanged();
          }
        }

        public int MotionBlur
        {
          get => _motionBlur;
          set
          {
            if (value == _motionBlur) return;
            _motionBlur = value;
            OnPropertyChanged();
          }
        }

        public int Dof
        {
          get => _dof;
          set
          {
            if (value == _dof) return;
            _dof = value;
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