﻿using NAudio.Utils;
using System;
using System.Diagnostics;

namespace NAudio.Flac
{
    public sealed class FlacSubFrameLPC : FlacSubFrameBase
    {
        private readonly int[] _warmup;
        private readonly int[] _qlpCoeffs;
        private readonly int _lpcShiftNeeded;
        private readonly int _qlpCoeffPrecision;

        public int QLPCoeffPrecision { get { return _qlpCoeffPrecision; } }

        public int LPCShiftNeeded { get { return _lpcShiftNeeded; } }

        public int[] QLPCoeffs { get { return _qlpCoeffs; } }

        public int[] Warmup { get { return _warmup; } }

        public FlacResidual Residual { get; private set; }

        public unsafe FlacSubFrameLPC(FlacBitReader reader, FlacFrameHeader header, FlacSubFrameData data, int bps, int order)
            : base(header)
        {

            //warmup
            _warmup = new int[FlacConstant.MaxLpcOrder];
            for (int i = 0; i < order; i++)
            {
                _warmup[i] = data.ResidualBuffer[i] = reader.ReadBitsSigned(bps);
            }

            //header
            int u32 = (int)reader.ReadBits(FlacConstant.SubframeLpcQlpCoeffPrecisionLen);
            if (u32 == (1 << FlacConstant.SubframeLpcQlpCoeffPrecisionLen) - 1)
            {
                Debug.WriteLine("Invalid FlacLPC qlp coeff precision.");
                return; //return false;
            }
            _qlpCoeffPrecision = u32 + 1;

            int level = reader.ReadBitsSigned(FlacConstant.SubframeLpcQlpShiftLen);
            if (level < 0)
                throw new Exception("negative shift");
            _lpcShiftNeeded = level;

            _qlpCoeffs = new int[FlacConstant.MaxLpcOrder];

            //qlp coeffs
            for (int i = 0; i < order; i++)
            {
                _qlpCoeffs[i] = reader.ReadBitsSigned(_qlpCoeffPrecision);
            }

            Residual = new FlacResidual(reader, header, data, order);

            for (int i = 0; i < order; i++)
            {
                data.DestBuffer[i] = data.ResidualBuffer[i];
            }

            int i1 = order;
            int result = 0;
            while ((i1 >>= 1) != 0)
            {
                result++;
            }
            if (bps + _qlpCoeffPrecision + result <= 32)
            {
                if (bps <= 16 && _qlpCoeffPrecision <= 16)
                    RestoreLPCSignal(data.ResidualBuffer + order, data.DestBuffer + order, header.BlockSize - order, order); //Restore(data.residualBuffer + order, data.destBuffer, Header.BlockSize - order, order, order);
                else
                    RestoreLPCSignal(data.ResidualBuffer + order, data.DestBuffer + order, header.BlockSize - order, order);
            }
            else
            {
                RestoreLPCSignalWide(data.ResidualBuffer + order, data.DestBuffer + order, header.BlockSize - order, order);//RestoreWide(data.residualBuffer + order, data.destBuffer, Header.BlockSize - order, order, order);
            }
        }

        private unsafe void Restore(int* residual, int* dest, int length, int predictorOrder, int destOffset)
        {
            for (int i = 0; i < length; i++)
            {
                int sum = 0;
                for (int j = 0; j < predictorOrder; j++)
                {
                    sum += (int)_qlpCoeffs[j] * (int)dest[destOffset + i - j - 1];
                }
                //System.Diagnostics.Debug.WriteLine(i + " " + (residual[i] + (int)(sum >> LPCShiftNeeded)));
                dest[destOffset + i] = residual[i] + (int)(sum >> _lpcShiftNeeded);
            }
        }

        private unsafe void RestoreLPCSignal(int* residual, int* destination, int length, int order)
        {
            int sum = 0;

            int* r = residual;
            int* history;
            int* dest = destination;

            for (int i = 0; i < length; i++)
            {
                sum = 0;
                history = dest;
                for (int j = 0; j < order; j++)
                {
                    sum += _qlpCoeffs[j] * *(--history);
                }

                *(dest++) = *(r++) + (sum >> _lpcShiftNeeded);
            }
        }

        private unsafe void RestoreLPCSignalWide(int* residual, int* destination, int length, int order)
        {
            long sum = 0;

            int* r = residual;
            int* history;
            int* dest = destination;

            for (int i = 0; i < length; i++)
            {
                sum = 0;
                history = dest;
                for (int j = 0; j < order; j++)
                {
                    sum += (long)_qlpCoeffs[j] * ((long)*(--history));
                }

                *(dest++) = *(r++) + (int)(sum >> _lpcShiftNeeded);
            }
        }
    }
}