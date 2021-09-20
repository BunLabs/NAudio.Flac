# BunLabs.NAudio.Flac

[![Nuget](https://img.shields.io/nuget/v/BunLabs.NAudio.Flac)](https://www.nuget.org/packages/BunLabs.NAudio.Flac)

This is a FLAC library for [NAudio], based on the FLAC codec from the [CSCore] audio library. NAudio is currently lacking a FLAC library, and while the included Media Foundation back-end works for most FLAC files, it doesn't work for everything.

This port takes the [CSCore] FLAC codec and makes it available for NAudio (≥2.0) using .NET Standard 2.0. Most other NAudio FLAC libraries are based on an old, archived [fork][naudio-flac] from Google Code. This code was originally also based on CSCore, but CSCore has since received a number of FLAC bug fixes, which are included in this library.

[NAudio]: https://github.com/naudio/NAudio
[CSCore]: https://github.com/filoe/cscore
[naudio-flac]: https://code.google.com/archive/p/naudio-flac/
