# malicious AI agent

malicous AI agent for cortex on cloud demo purposes. Code assets to be packacged into a local msi installer that will simulate endpoint detections 


## Security Demonstration Disclaimer:

This application intentionally demonstrates OWASP vulnerabilities for educational purposes:

- **A01: Broken Access Control**
  - `DumpSensitiveData()` exposes sensitive data without auth.

- **A03: Command Injection**
  - `ExecuteUserCommand()` directly executes file contents as system commands.

- **A06: Vulnerable and Outdated Components**
  - `ParseJsonWithVulnerableLibrary()` uses outdated Newtonsoft.Json vulnerable to deserialization attacks.

**Do NOT deploy this application to production environments.**

- **A04: Insecure Design**
  - `DownloadAndUnzipFile()` downloads files from external sources and extracts contents without validation, allowing potential malicious file injection.
