#!/usr/bin/env bash
set -e

# -------- Configuration --------
CHANNEL="11.0"
QUALITY="GA"
# Uncomment the workloads your project needs:
# WORKLOADS="maui wasm-tools"
# --------------------------------

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
INSTALL_DIR="$SCRIPT_DIR/.dotnet"

# shellcheck disable=SC2154
DOTNET_INSTALL_DIR="$LocalAppData\\Microsoft\\dotnet"

isUnixLike=false

case $OSTYPE in
  solaris*) ;&
  darwin*)  ;&
  linux*)   ;&
  bsd*) isUnixLike=true ;;

  *) ;;
esac

if [ $isUnixLike ]; then
  DOTNET_INSTALL_DIR="$HOME/.dotnet"
  echo 'DOTNET_INSTALL_DIR="$HOME/.dotnet"' >> $HOME/.config/environment.d/00-dotnet.conf || true
else reg.exe add HKCU\\Environment /v DOTNET_INSTALL_DIR /t REG_SZ /d "$DOTNET_INSTALL_DIR"
fi



if [[ ! -e "$INSTALL_DIR" ]]; then
  if [[ ! -e "$DOTNET_INSTALL_DIR" ]]; then
    # shellcheck disable=SC2016
    echo 'DOTNET_INSTALL="$HOME/.dotnet"'
  fi

 ln -s "$DOTNET_INSTALL_DIR" "$INSTALL_DIR"
fi

echo "Installing .NET $CHANNEL ($QUALITY) SDK to $INSTALL_DIR ..."

curl -sSL https://dot.net/v1/dotnet-install.sh -o "$SCRIPT_DIR/dotnet-install.sh"
chmod +x "$SCRIPT_DIR/dotnet-install.sh"
"$SCRIPT_DIR/dotnet-install.sh" --channel "$CHANNEL" --quality "$QUALITY" --install-dir "$INSTALL_DIR"
rm -f "$SCRIPT_DIR/dotnet-install.sh"

# Auto-detect the installed SDK version
SDK_VERSION="$("$INSTALL_DIR/dotnet" --version)"

# Create global.json with the installed version
cat > "$SCRIPT_DIR/global.json" << EOF
{
  "sdk": {
    "version": "$SDK_VERSION",
    "allowPrerelease": false,
    "rollForward": "latestFeature",
    "paths": [".dotnet", "\$host\$"],
    "errorMessage": "Required .NET SDK not found. Run ./install-dotnet.sh (macOS/Linux) or .\\\\install-dotnet.ps1 (Windows) to install it locally."
  }
}
EOF

# Ensure .dotnet is in .gitignore
if ! grep -qxF '.dotnet' "$SCRIPT_DIR/.gitignore" 2>/dev/null; then
    echo '.dotnet' >> "$SCRIPT_DIR/.gitignore"
fi

# Install workloads if configured
if [ -n "${WORKLOADS:-}" ]; then
    echo "Installing workloads: $WORKLOADS"
    # shellcheck disable=SC2086
    "$INSTALL_DIR/dotnet" workload install $WORKLOADS
fi

echo ""
echo "Done! SDK $SDK_VERSION installed to $INSTALL_DIR"
echo "Run 'dotnet --version' to verify."