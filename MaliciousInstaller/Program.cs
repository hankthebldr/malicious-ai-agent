using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.IO.Compression;

namespace WindowsAgent
{
    class Program
    {
        public static object Newtonsoft { get; private set; }

        public Program()
        {
        }

        static void Main(string[] args)
        {
            // Start-up message
            Console.WriteLine("========================================");
            Console.WriteLine(" Windows Agent: Start-up Diagnostics ");
            Console.WriteLine("========================================");

            // 1. Log basic environment info
            LogEnvironmentInfo();

            // 2. Log running processes (simple example
            // Additional tasks for the pipeline or supply-chain checks
            ExecuteUserCommand(); // Intentionally vulnerable method invocation
        
            // Demonstrating insecure download and extraction
            DownloadAndUnzipFile("http://example.com/payload.zip", @"C:\ExtractedPayload");
            // (Placeholder: In real usage, you might integrate with a scanning library or APIs)

            Console.WriteLine("========================================");
            Console.WriteLine(" Windows Agent finished all tasks.");
            Console.WriteLine("========================================");
        }

        public static void LogEnvironmentInfo()
    {
        try
        {
            // Define log directory and file clearly
            string logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "MaliciousAgent");
            string logFile = Path.Combine(logDirectory, "environment-log.txt");

            // Ensure the log directory exists explicitly
            Directory.CreateDirectory(logDirectory);

            // Gather basic environment info clearly
            string environmentInfo = $"Log Date & Time: {DateTime.Now}\n" +
                                     $"Machine Name: {Environment.MachineName}\n" +
                                     $"User Name: {Environment.UserName}\n" +
                                     $"OS Version: {Environment.OSVersion}\n" +
                                     $"64-Bit OS: {Environment.Is64BitOperatingSystem}\n" +
                                     $"Processor Count: {Environment.ProcessorCount}\n" +
                                     $"Current Directory: {Environment.CurrentDirectory}\n";

            // Write environment details explicitly into the log file
            File.WriteAllText(logFile, environmentInfo);

            Console.WriteLine($"Environment info logged clearly at {logFile}");
        }
        catch (Exception ex)
        {
            // Clearly catch and report exceptions
            Console.WriteLine($"Failed to log environment info: {ex.Message}");
        }
    }

        // OWASP A03: Command Injection Demonstration
        private static void ExecuteUserCommand()
        {
            Console.WriteLine("--> Execute Unvalidated User Command:");

            string commandFilePath = @"C:\Temp\user_command.txt";

            try
            {
                if (File.Exists(commandFilePath))
                {
                    string commandToExecute = File.ReadAllText(commandFilePath).Trim();

                    Console.WriteLine($"Executing command from file: {commandToExecute}");

                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            Arguments = $"/C {commandToExecute}",
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        }
                    };

                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    Console.WriteLine("Command output:");
                    Console.WriteLine(output);
                }
                else
                {
                    Console.WriteLine("[Warning] Command file not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to execute command: {ex.Message}");
            }

            Console.WriteLine();
        }

        // OWASP A04: Insecure Design - Unvalidated File Download and Extraction Demonstration
        private static void DownloadAndUnzipFile(string url, string extractPath)
        {
            Console.WriteLine("--> Downloading and Unzipping File (No Validation):");

            string tempZipPath = Path.Combine(Path.GetTempPath(), "downloaded_payload.zip");

            using (var client = new System.Net.WebClient())
            {
                try
                {
                    Console.WriteLine($"Downloading file from URL: {url}");
                    client.DownloadFile(url, tempZipPath);
                    Console.WriteLine($"File downloaded successfully to {tempZipPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] Failed to download file: {ex.Message}");
                    return;
                }
            }

            // Unzip file without validation (risky!)
            try
            {
                Console.WriteLine($"Extracting ZIP file to: {extractPath}");
                if (!Directory.Exists(extractPath))
                {
                    Directory.CreateDirectory(extractPath);
                }
                ZipFile.ExtractToDirectory(tempZipPath, extractPath);
                Console.WriteLine("File extracted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to extract file: {ex.Message}");
            }

            Console.WriteLine();
        }
    }
}