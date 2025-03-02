using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace WindowsAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            // Start-up message
            Console.WriteLine("========================================");
            Console.WriteLine(" Windows Agent: Start-up Diagnostics ");
            Console.WriteLine("========================================");

            // 1. Log basic environment info
            LogEnvironmentInfo();

            // 2. Log running processes (simple example)
            LogRunningProcesses();

            // 3. Potentially check for suspicious files (placeholder example)
            DetectSuspiciousFiles(@"C:\Temp");

            // Additional tasks for the pipeline or supply-chain checks
            // (Placeholder: In real usage, you might integrate with a scanning library or APIs)

            Console.WriteLine("========================================");
            Console.WriteLine(" Windows Agent finished all tasks.");
            Console.WriteLine("========================================");
        }

        private static void LogEnvironmentInfo()
        {
            Console.WriteLine("--> Environment Information:");
            Console.WriteLine($"    Machine Name: {Environment.MachineName}");
            Console.WriteLine($"    OS Version: {Environment.OSVersion}");
            Console.WriteLine($"    64-bit OS: {Environment.Is64BitOperatingSystem}");
            Console.WriteLine($"    Processor Count: {Environment.ProcessorCount}");
            Console.WriteLine($"    Current Directory: {Environment.CurrentDirectory}");
            Console.WriteLine();
        }

        private static void LogRunningProcesses()
        {
            try
            {
                Console.WriteLine("--> Running Processes:");
                var processes = Process.GetProcesses()
                                       .OrderBy(p => p.ProcessName)
                                       .Take(10); // limit for demonstration

                foreach (var process in processes)
                {
                    Console.WriteLine($"    PID: {process.Id}, Name: {process.ProcessName}");
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to retrieve running processes: {ex.Message}");
            }
        }

        private static void DetectSuspiciousFiles(string directoryPath)
        {
            Console.WriteLine("--> Suspicious File Check (Example):");
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"    Directory '{directoryPath}' does not exist. Skipping file checks.");
                Console.WriteLine();
                return;
            }

            // Placeholder: In real usage, define actual suspicious file patterns or contents
            var suspiciousPattern = ".exe";
            try
            {
                var files = Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories)
                                     .Where(f => f.EndsWith(suspiciousPattern, StringComparison.OrdinalIgnoreCase))
                                     .Take(5); // limit for demonstration

                if (files.Any())
                {
                    Console.WriteLine($"    Found potential suspicious files (pattern: '{suspiciousPattern}'):");
                    foreach (var file in files)
                    {
                        Console.WriteLine($"      --> {file}");
                    }
                }
                else
                {
                    Console.WriteLine($"    No suspicious files found with pattern '{suspiciousPattern}'.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Unable to scan for suspicious files: {ex.Message}");
            }

            Console.WriteLine();
        }
    }
}