using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace TwistCLI
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string address = "https://104.43.131.93:8083";
            string username = "ryan.brown@csiweb.com";
            string password = "n9K63zhyDqrd3k";
            string dir = System.IO.Directory.GetCurrentDirectory();
            string twistDir = $"{dir}/twistlock";
            string twistLockUnix = $"{twistDir}/twistcli";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                bool directoryExists = Directory.Exists(twistDir);
                if (directoryExists == true)
                {
                    Directory.Delete(twistDir, true);
                }
                coms.Bash($"mkdir {twistDir}");
                coms.Bash($"curl -k -u {username}:{password} --output {twistLockUnix} {address}/api/v1/util/osx/twistcli");
                coms.Bash($"chmod u+x {twistLockUnix}");
                string lineNum = coms.Bash("docker images | head | awk 'END{print NR}'");
                int lineInt = Int32.Parse(lineNum);
                List<string> scanList = new List<string>();
                for (int i = 2; i <= lineInt; i++)
                {
                    string dockerImageCom = coms.Bash($"docker images | head -{lineInt} | awk '{{print $3}}' | sed -n '{i} p'");
                    string twistScan = coms.Bash($"{twistLockUnix} images scan -u {username} -p {password} --address {address} --vulnerability-threshold high {dockerImageCom}");
                    Console.Write(twistScan);
                    scanList.Add(twistScan);
                    if (scanList.Contains("FAIL"))
                    {
                        Console.WriteLine("The task failed because images exist with vulnerabilities that exceed the designated threshold");
                        int code = 1;
                        Environment.Exit(code);
                    }
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                bool directoryExists = Directory.Exists(twistDir);
                if (directoryExists == true)
                {
                    Directory.Delete(twistDir, true);
                }
                coms.Bash($"mkdir {twistDir}");

                coms.Bash($"curl -k -u {username}:{password} --output {twistLockUnix} {address}/api/v1/util/twistcli");
                coms.Bash($"chmod u+x {twistLockUnix}");
                string lineNum = coms.Bash("docker images | head | awk 'END{print NR}'");
                int lineInt = Int32.Parse(lineNum);
                List<string> scanList = new List<string>();
                for (int i = 2; i <= lineInt; i++)
                {
                    string dockerImageCom = coms.Bash($"docker images | head -{lineInt} | awk '{{print $3}}' | sed -n '{i} p'");
                    string twistScan = coms.Bash($"{twistLockUnix} images scan -u {username} -p {password} --address {address} --vulnerability-threshold high {dockerImageCom}");
                    Console.Write(twistScan);
                    scanList.Add(twistScan);
                    if (scanList.Contains("FAIL"))
                    {
                        Console.WriteLine("The task failed because images exist with vulnerabilities that exceed the designated threshold");
                        int code = 1;
                        Environment.Exit(code);
                    }
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string dirE = $"{dir}\\twistlock";
                bool directoryExists = Directory.Exists(dirE);
                Console.Write(directoryExists);
                if (directoryExists == true)
                {
                    Directory.Delete(dirE, true);
                }
                coms.WinCli($"mkdir {dirE}");
                string twistLock = $"{dirE}\\twistcli.exe";
                string getTwist = coms.WinCli($"curl -k -u {username}:{password} --output {twistLock} {address}/api/v1/util/windows/twistcli.exe");
                Console.Write(getTwist);
                string imageIds = "for /f \"tokens=3 skip=1\" %i in (\'docker images\') do @echo %i";
                //Console.WriteLine(imageIds);
                string getImageIds = coms.WinCli(imageIds);
                string[] lineInt = getImageIds.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                var temp = new List<string>();
                foreach (var s in lineInt)
                {
                    if (!string.IsNullOrEmpty(s))
                        temp.Add(s);
                    if (s.Contains("Windows"))
                        temp.Remove(s);
                    if (s.Contains("Microsoft"))
                        temp.Remove(s);
                    if (s.Contains(dir))
                        temp.Remove(s);
                }
                lineInt = temp.ToArray();
                List<string> scanList = new List<string>();
                foreach (var i in lineInt)
                {
                    string twistScan = coms.WinCli($"{twistLock} images scan -u {username} -p {password} --address {address} --vulnerability-threshold high {i}");
                    Console.WriteLine(twistScan);
                    scanList.Add(twistScan);
                    scanList.Add(i);
                    if (scanList.Contains("FAIL"))
                    {
                        Console.WriteLine("The task failed because images exist with vulnerabilities that exceed the designated threshold");
                        int code = 1;
                        Environment.Exit(code);
                    }
                }
            }
            else
            {
                Console.Write("Cannot identify the underlying OS");
            }
        }
    }
}
