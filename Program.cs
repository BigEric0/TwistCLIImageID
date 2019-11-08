using System;
using System.IO;
using System.Runtime.InteropServices;

namespace TwistCLI
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string address = "https://52.176.91.63:8083";
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
                for (int i = 2; i <= lineInt; i++)
                {
                    string dockerImageCom = coms.Bash($"docker images | head -{lineInt} | awk '{{print $3}}' | sed -n '{i} p'");
                    string twistScan = coms.Bash($"{twistLockUnix} images scan -u {username} -p {password} --address {address} --vulnerability-threshold high {dockerImageCom}");
                    Console.Write(twistScan);
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
                for (int i = 2; i <= lineInt; i++)
                {
                    string dockerImageCom = coms.Bash($"docker images | head -{lineInt} | awk '{{print $3}}' | sed -n '{i} p'");
                    string twistScan = coms.Bash($"{twistLockUnix} images scan -u {username} -p {password} --address {address} --vulnerability-threshold high {dockerImageCom}");
                    Console.Write(twistScan);
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
                string getImageIds = coms.WinCli(imageIds);
                string[] lineInt = getImageIds.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (var i in lineInt)
                {
                    string twistScan = coms.WinCli($"{twistLock} images scan -u {username} -p {password} --address {address} --vulnerability-threshold high {i}");
                    Console.WriteLine(twistScan);
                }
            }
            else
            {
                Console.Write("Cannot identify the underlying OS");
            }
        }
    }
}
