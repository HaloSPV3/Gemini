<html>
    <p align="center">
        <img src="https://user-images.githubusercontent.com/10241434/57498646-40c70800-730f-11e9-9e20-edbb764df836.png">
    </p>
    <h1 align="center">
        Halo XE (HXE)
    </h1>
    <p align="center">
        Excellent HCE Wrapper • Functional SPV3 Loader • Versatile Library
    </p>
    <p align="center">
        <img src="https://user-images.githubusercontent.com/10241434/57498544-d6ae6300-730e-11e9-9558-1072d5c7c48d.png">
    </p>
    <p align="center">
        <a href="https://dist.n2.network/hxe">
            Download
        </a>
    </p>
</html>

# Introduction

Halo XE (HXE) is one executable with many features. A tiny little beast that rapidly evolves at 4AM in the morning.

In a nutshell:

- a wrapper for `haloce.exe` with automatic native resolution support & compatibility with `haloce.exe` arguments;
- a loader for SPV3.2 with post-processing support, campaign resuming, profile detection, and campaign resuming;
- a library for developers to programmatically detect/load HCE & manipulate, detect and list HCE player profiles;
- a tool-kit for modders to package their mods into compressed packages and distribute them for installation using HXE.

To get this sweetness, simply put `hxe.exe` in your HCE folder.

---

If you want to use it to its fullest potential, read on through the upcoming verbosity. HXE is ...

## A HCE Wrapper

In other words, you're not replacing anything, nor are you compromising anything. Everything that's there remains there, whilst adding a lot of goodies on top.

**Complete compatibility with HCE**

In a nutshell, put `hxe.exe` in your HCE folder, and treat it exactly like how you'd treat `haloce.exe`. Indeed, all you need is to double click on it and it will do its magic.

You can also pass on the HCE arguments you're familiar with. Enjoy the automatic enhancements without needing to learn any additional ones. `-window`, `-vidmode`, `-adapter` and every other major HCE argument works exactly the same.

*Trivia: Because HXE is akin to `haloce.exe` on steroids, the executable used to be called `haloxe.exe`. It turns out that people often confused the names, so it's now shortened to `hxe.exe`.*

**Automatic video enhancements**

No more need for `-vidmode` or patching your HCE profile. HXE automatically sets the game's video mode to your native resolution, both at the profile and executable level.

Of course, if you want to set your own resolution, `-vidmode` will now apply the resolution to the game and to your profile.

*Trivia: After implementing this, I've realised I've obsoleted [Écran](https://github.com/yumiris/ecran). I should have HXE display `RIP Écran` when pressing F.*

## An SPV3 Loader

This nimble little loader is the core of the SPV3.2 launcher. It bootstraps SPV3 and does a few nifty things to enhance _the user experience_ ...

**Post-Processing Settings**

HXE configures SPV3 your post-processing preferences. Using the SPV3 launcher, you can teach HXE what post-processing settings it should instruct SPV3.2 to apply when loading it.

**Campaign Resuming**

Another behind the scenes thing it does is provide campaign resuming for SPV3. Combined with SPV3's nifty UI, the mod is resumed without the need for any manual intervention. We've really turned SPV3 into its own game at this rate.

**Profile Detection**

The bane of the SPV3.1 launcher - a lack of profile detection, which in turn required you to manually sync things up. This is no longer an issue. It's all done automatically now.

**SPV3.1 Compatibility**

If you're adventurous, you can use HXE to load SPV3.1 and unlock its maps. Invoke `.\HXE.exe /config` and enable the legacy mode.

## A Library

Import the HXE into your .NET project, and you get a plethora of options ...

- ability to deserialise a `blam.sav` file into an object representation: `var profile = (Profile) "blam.sav"; profile.Load()`;
- edit almost every property of the `blam.sav`: `Profile.Details.Name = "Red Girl"`, `Profile.Video.Resolution.Width = 1920`, `Profile.Audio.Quality = High`;
- serialise the properties back to the binary on the file-system, with automatic forging of the CRC32 hash;
- detect the default profile by deserialising a provided `lastprof.txt` file, or by simply invoking `Profile.Detect()`;
- retrieve a list of objects representing found profiles (in a specified directory): `var profiles = Profile.List("C:\\profiles")`;
- represent a HCE executable as an object like you do with the profile: `var hce = (Executable) "haloce.exe"`;
- detect the HCE executable through a variety of heuristic methods, including checking the default paths and registry keys;
- progammatically modify the loading parameters for HCE: `hce.Debug.Console = true`, `hce.Video.Adapter = 2`;
- represent the OpenSauce user configuration as an object: `var openSauce = (OpenSauce) "OS_Settings.User.xml"; openSauce.Load()`;
- manipulate the OpenSauce user configuration's properties: `openSauce.Camera.FieldOfView = 86`;
- auto-calculate Field of View for OpenSauce based on the screen resolution: `openSauce.Camera.CalculateFOV()` (credits to Mortis for the formula);

Whew... this list might end up being pretty big if it continues like this.

## A tool-kit

HXE allows you to create packages for files in a specified folder, and then install these packages to another specified folder. SPV3.2 is distributed using this system, and it works well for almost any major HCE mod.

`.\hxe.exe /compile "C:\PathX"` will compress the files in the current directory to packages in `C:\PathX`. and `.\hxe.exe /install "C:\PathY"` will install the packages in the current directory to `C:\PathY`.

Making a sweet-looking installer GUI should be pretty simple, since all it needs to do is call `hxe.exe` with the aforementioned commands.


# Sources

Source code and binaries are officially provided at the following
sources:

-   https://cgit.n2.network/hxe - upstream source code
-   https://dist.n2.network/hxe - compiled executables
-   https://github.com/yumiris/hxe - mirror repository

# Licence

Please refer to the COPYING file located in this repository. Also note
that the respective licence applies only to this repository, and not to
the rest of the SPV3 source code or assets, or to Halo: Custom Edition.
