// ReSharper disable once CheckNamespace
using NAudio.Flac;

namespace NAudio.Flac.SubFrames
{
    internal sealed class FlacSubFrameVerbatim : FlacSubFrameBase
    {
        public FlacSubFrameVerbatim(FlacBitReader reader, FlacFrameHeader header, FlacSubFrameData data, int bitsPerSample)
            : base(header)
        {
            unsafe
            {
                int* ptrDest = data.DestinationBuffer, ptrResidual = data.ResidualBuffer;

                for (int i = 0; i < header.BlockSize; i++)
                {
                    int x = (int)reader.ReadBits(bitsPerSample);
                    *ptrDest++ = x;
                    *ptrResidual++ = x;
                }
            }
        }
    }
}