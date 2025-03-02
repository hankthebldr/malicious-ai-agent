using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Net.Http;

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
            DumpSensitiveData(); // Intentionally insecure method invocation

            // Additional tasks for the pipeline or supply-chain checks
            ExecuteUserCommand(); // Intentionally vulnerable method invocation
            ExecuteUserCommand(); // Intentionally vulnerable method invocation
            ParseJsonWithVulnerableLibrary(); // Intentionally uses outdated, vulnerable library
            // Demonstrating insecure download and extraction
            DownloadAndUnzipFile("http://example.com/payload.zip", @"C:\ExtractedPayload");
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

        private static void DumpSensitiveData()
        {
                 Console.WriteLine("--> Sensitive Data Dump (No Auth Check):");
                 string sensitiveFilePath = @"C:\SensitiveData\credentials.txt";
            try
            {       
                 if (File.Exists(sensitiveFilePath))
                {
                    string sensitiveData = File.ReadAllText(sensitiveFilePath);
                Console.WriteLine($"Sensitive data:\n{sensitiveData}");
        }
        else
        {
            Console.WriteLine("[Warning] Sensitive file does not exist.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Error] Reading sensitive data failed: {ex.Message}");
    }

    Console.WriteLine();
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

// OWASP A06: Vulnerable Component Usage Demonstration
private static void ParseJsonWithVulnerableLibrary()
{
    Console.WriteLine("--> Parsing JSON with Vulnerable Newtonsoft.Json (v10.0.3):");

    string maliciousJson = @"{""$type"":""System.IO.FileInfo, mscorlib"", ""fileName"":""C:\\Windows\\System32\\drivers\\etc\\hosts""}";

    try
    {
        var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(maliciousJson, new Newtonsoft.Json.JsonSerializerSettings
        {
            TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
        });

        Console.WriteLine($"Deserialized object: {obj}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Error] JSON Parsing failed: {ex.Message}");
    }

    Console.WriteLine();
}

// OWASP A04: Insecure Design - Unvalidated File Download and Extraction Demonstration
private static void DownloadAndUnzipFile(string url, string extractPath)
{
    Console.WriteLine("--> Downloading and Unzipping File (No Validation):");

    string tempZipPath = @"C:\Temp\downloaded_payload.zip";

    using (var client = new HttpClient())
    {
        try
        {
            Console.WriteLine($"Downloading file from URL: {url}");
            var fileData = client.GetByteArrayAsync(url).Result;
            File.WriteAllBytes(tempZipPath, fileData);
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
        ZipFile.ExtractToDirectory(tempZipPath, extractPath, true);
        Console.WriteLine("File extracted successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Error] Failed to extract file: {ex.Message}");
    }

    Console.WriteLine();
}