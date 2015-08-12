using System;

namespace NAudio.Flac
{
    [Flags]
    public enum ID3v2ExtendedHeaderFlags
    {
        None = 0x0,
        TagUpdate = 0x4,
        CrcPresent = 0x2,
        Restrict = 0x1
    }
}