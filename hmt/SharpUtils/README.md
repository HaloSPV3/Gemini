<<<<<<< HEAD
<html>
<p align="center">
<img src="https://user-images.githubusercontent.com/10241434/60400157-bf6b5380-9ba2-11e9-9c25-432e47b845b4.png"/>
<p>
<p align="center">
<img src="https://user-images.githubusercontent.com/10241434/67626557-5ad5b200-f87f-11e9-8da6-0e9606df2165.png">
<img src="https://user-images.githubusercontent.com/10241434/67626558-5d380c00-f87f-11e9-9654-b59fa038bcb3.png">
<p>
<p align="center">
<a href="https://file.n2.network/f/c1c36919f366499793fc/?dl=1"> Download
</a> • <a href="https://www.reddit.com/r/halospv3/"> Support </a>
</p>
</html>

The SPV3 Loader
===============

This repository contains the source for the SPV3.2 loader, which has
been rewritten from the ground up for improved versatility, reliability,
and aesthetics.

Features
--------

-   SPV3.2 loading, updating, installing
-   SPV3.2/Lumoria campaign resuming
-   Automatic video/audio enhancements
-   Borderless HCE/SPV3 window mode
-   News, social links, announcements
-   Tweak SPV3.2 post-processing
-   Automatic field of view
-   Apply custom video resolution
-   Tweak OpenSauce post-processing
-   Tweak Chimera interpolation
-   DOOM & Photo modes
-   Backwards compatibility with SPV3.1
    -   Campaign resume
    -   Maps unlocking
-   Auto profile detection
-   Isolation from HCE data

Sources
-------

Source code and binaries are officially provided at the following
sources:

-   https://source.n2.network/spv3 - upstream source code
-   https://builds.n2.network/spv3 - compiled executables

Cloning
-------

Clone the repository using the following command:

    git clone --recurse-submodules -j8 https://source.n2.network/spv3
    # can replace source.n2.network/spv3 with github.com/yumiris/spv3

Licence
-------

Please refer to the COPYING file located in this repository. Also note
that the respective licence applies only to this repository, and not to
the rest of the SPV3 source code or assets.
=======
## SharpUtils  
This is a personal class library of common utility/helper classes that I frequently make use of in my various C# projects.  
Their uses range from File IO related to Web related. The classes are grouped in a folder with the name of the particular  
thing they focus on  + Utils at the end(EG: classes focusing on WebRequests are in a folder called WebUtils).  
The classes are also all in a namespace they all share with the same name as the containing folder.
  
## Current Utility List  
### WebUtils  
Contains various helper/util classes related to making external  
web requests.  
*WebRequests.cs*: Helps make simple requests (downloading a string, parsing JSON, etc)  
*GithubReleaseParser.cs*: Helps get the latest released version of a given Repo,  
and also has a function which does a check to see if the current application version  
is not the currently released one.  
*GithubReadmeParser.cs*: Helps get a specific line from a README.MD file starting with  
a prefix, so I can for example, have a line in a README.MD file "Reddit Thread: link_here"  
and search for the line starting with "Reddit Thread" and then get the link.  
### WPFUtils  
Contains various helper/util classes related to WPF projects.  
*WindowHelper.cs*: Checks if a window of a certain type is currently open in the host  
WPF Application.  
### FileUtils  
Contains various helper/util classes related to File IO.  
*DialogHelpers.cs*: Simple OpenFileDialog/OpenFolderDialog helpers. Bare bones right now.  
*FileSizeHelper.cs*: Helps convert Bytes to a human readable string representation.  
### MiscUtils
Utilities/Helpers which don't fit anywhere else will go here.
*AdminCheckHelper.cs*: Checks if the current application is being run as an administrator.
*WriteMemoryHelper.cs*: Provides a PINVOKE wrapper around WriteProcessMemory.
  
## State of the code  
At the moment, the existing utilities are not extensively tested, as I only recently  
developed them for StarboundAssetUnpacker, so there's probably some things which need  
to be worked out. I think they all work as they should though from what I've seen using  
them in that project though.
  
## Usage  
I've tried to document the code the best I can with XML documentation comments.  
All the classes are static (since... I really don't see the point in using OOP related  
constructs for simple things like this). I've also tried to make sure all the functions  
have no side effects - they just take something in, and then pop something out in return.  
  
This is licensed under the *GNU GENERAL PUBLIC LICENSE*, so feel free to take the code  
and do with it what you will.  
  
## Credits  
Since I don't like copying code from someone else without giving due credit, I should  
note that I got the code for the GetHumanReadableSize function from this StackOverflow  
https://stackoverflow.com/questions/281640/how-do-i-get-a-human-readable-file-size-in-bytes-abbreviation-using-net  
I originally did use my own code to do it but it wasn't very elegant and I didn't have time  
to try to roll my own since I was more interested in the task at hand.
>>>>>>> refs/rewritten/a8b66c076d8e3db0020eeaebce949b439a2b5668-2
