﻿namespace NAudio.Flac
{
    public sealed class FlacSubFrameVerbatim : FlacSubFrameBase
    {
        public FlacSubFrameVerbatim(FlacBitReader reader, FlacFrameHeader header, FlacSubFrameData data, int bps)
            : base(header)
        {
            unsafe
            {
                int* ptrDest = data.DestBuffer, ptrResidual = data.ResidualBuffer;

                for (int i = 0; i < header.BlockSize; i++)
                {
                    int x = (int)reader.ReadBits(bps);
                    *ptrDest++ = x;
                    *ptrResidual++ = x;
                }
            }
        }
    }
}