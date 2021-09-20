using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace FlacTest
{
    public static class Program
    {
        [MTAThread]
        static void Main(string[] args)
        {
            var testFiles = new[]
            {
                @"C:\Users\laura\OneDrive\Music\Alistair Lindsay\RimWorld Soundtrack\6 Night And Day.flac",
                @"C:\Users\laura\OneDrive\Music\toby fox\UNDERTALE Soundtrack\toby fox - UNDERTALE Soundtrack - 16 Nyeh Heh Heh!.flac"
            };

            Trace.Listeners.Add(new ConsoleTraceListener(useErrorStream: true));

            foreach (var path in testFiles)
            {
                if (!File.Exists(path))
                {
                    Console.WriteLine($"Could not find file \"{path}\".");
                    continue;
                }

                Console.WriteLine($"Current track: '{path}'");
                PlayWithMediaFoundation(path);
                PlayWithNAudioFlac(path);
                PlayWithCSCore(path);
            }
        }

        public static void PlayWithMediaFoundation(string path)
        {
            try
            {
                using var player = new NAudio.Wave.DirectSoundOut();
                using var reader = new NAudio.Wave.MediaFoundationReader(path);
                player.Init(reader);
                player.Play();

                Console.WriteLine();
                Console.WriteLine("Now playing with Media Foundation back-end, press any key to continue.");
                Console.ReadKey(true);
                player.Stop();
            }
            catch (Exception ex)
            {
                var foregroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Unable to play with Media Foundation back-end: " + ex);
                Console.ForegroundColor = foregroundColor;
            }
        }

        public static void PlayWithNAudioFlac(string path)
        {
            try
            {
                using var reader = new NAudio.Flac.FlacReader(path);
                using var player = new NAudio.Wave.DirectSoundOut();
                player.Init(reader);
                player.Play();

                Console.WriteLine();
                Console.WriteLine("Now playing with NAudio.Flac back-end, press any key to continue.");
                Console.ReadKey(true);
                player.Stop();
            }
            catch (Exception ex)
            {
                var foregroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Unable to play with NAudio.Flac back-end: " + ex);
                Console.ForegroundColor = foregroundColor;
            }
        }

        public static void PlayWithCSCore(string path)
        {
            try
            {
                using var player = new CSCore.SoundOut.DirectSoundOut();
                using var flacFile = new CSCore.Codecs.FLAC.FlacFile(path);
                player.Initialize(flacFile);
                player.Play();

                Console.WriteLine();
                Console.WriteLine("Now playing with CSCore back-end, press any key to continue.");
                Console.ReadKey(true);
                player.Stop();
            }
            catch (Exception ex)
            {
                var foregroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Unable to play with CSCore back-end: " + ex);
                Console.ForegroundColor = foregroundColor;
            }
        }
    }
}
