using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace FileDuplicationListAgent
{
    internal static class Program
    {
        private class File
        {
            public string Path;
            public byte[] Content;
            public readonly List<string> Duplicates = new List<string>();
        }

        private static readonly MD5 Hash = new MD5CryptoServiceProvider();

        private static readonly List<string> Candidates = new List<string>();

        private static readonly Dictionary<string, List<File>> Unique = new Dictionary<string, List<File>>();

        private static void ListFiles(string directory)
        {
            foreach (var f in Directory.GetFiles(directory))
            {
                Candidates.Add(f);
            }
        }
        private static byte[] LoadFile(string path)
        {
            return System.IO.File.ReadAllBytes(path);
        }

        private static void WalkCandidates()
        {
            var count = 0;
            var oldPercentage = 0;
            foreach (var file in Candidates)
            {
                var percentage = (int)((double) count++ / Candidates.Count * 100.0);
                if (oldPercentage != percentage)
                {
                    oldPercentage = percentage;
                    Console.WriteLine($"Processing({percentage}%): {file}");
                }

                HandleFile(file);
            }
        }

        private static void HandleFile(string path)
        {
            var bytes = LoadFile(path);
            var md5 = Hash.ComputeHash(bytes);
            var search = bytes.Length + Convert.ToBase64String(md5);
            if (Unique.TryGetValue(search, out var list))
            {
                foreach (var file in list)
                {
                    if (!file.Content.SequenceEqual(bytes)) continue;
                    file.Duplicates.Add(path);
                    return;
                }
                list.Add(new File {Path = path, Content = bytes});
            }
            else
            {
                Unique.Add(search, new List<File> {new File {Path = path, Content = bytes}});
            }
        }

        private static void Process(string directory, string output)
        {
            EnsureOutputDirectory(output);
            ListFiles(directory);
            WalkCandidates();
            DumpResults(output);
        }

        private static void DumpResults(string output)
        {
            foreach (var (_, list) in Unique)
            {
                foreach (var file in list)
                {
                    if (file.Duplicates.Count == 0) continue;
                    var name = Path.GetFileName(file.Path);
                    using (var write = new StreamWriter($"{output}/{name}.txt"))
                    {
                        foreach (var duplicate in file.Duplicates)
                        {
                            write.WriteLine(Path.GetFileName(duplicate));
                        }
                        write.WriteLine();
                    }
                }
            }
        }

        private static void EnsureOutputDirectory(string output)
        {
            if (Directory.Exists(output))
            {
                if (Directory.GetFiles(output).Length != 0)
                {
                    throw new Exception("Output Directory Contains Files, Aborting");
                }
            }
            else
            {
                Directory.CreateDirectory(output);
            }
        }

        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please Input Target Directory");
                var target = Console.ReadLine();
                Console.WriteLine("Please Input Output Directory");
                var output = Console.ReadLine();
                Process(target, output);
            }
            else if (args.Length != 2)
            {
                Console.WriteLine("Not Enough Arguments. Usage:tool_path TargetDirectory OutputDirectory");
                throw new ArgumentException("Incorrect Arguments");
            }
            else
            {
                Process(args[0], args[1]);
            }
        }
    }
}