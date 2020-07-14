using System;
using System.IO;
using System.Runtime.InteropServices;

namespace FumoCs
{
    internal static class Program
    {
        /// <summary>
        /// Starts compiling a FumoLang program
        /// </summary>
        /// <param name="args">Path to .fumo file</param>
        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("No file provided!");
                return;
            }

            var fileName = Path.GetFileNameWithoutExtension(args[0]);
            // TODO: cmd arguments for "cross-compilation" etc.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                fileName += ".exe";

            Compile(args[0], fileName);
        }

        private static void Compile(string srcPath, string dstPath)
        {
            var writer = new BitWriter();
            using var srcFile = File.OpenText(srcPath);
            using var dstFile = File.Create(dstPath);

            string line;
            while ((line = srcFile.ReadLine()) != null)
            {
                FumoToBytes(writer, line.ToLower());
            }

            if (writer.Flushable)
                writer.Flush();

            dstFile.Write(writer.GetBytes(), 0, writer.GetBytes().Length);
        }

        private static void FumoToBytes(BitWriter writer, string line)
        {
            var words = line.Split(' ', '\n');
            foreach (var word in words)
            {
                if (word.Equals("fumo"))
                    writer.Write(0);
                else if (word.Equals("fumofumo"))
                    writer.Write(1);
            }
        }
    }
}
