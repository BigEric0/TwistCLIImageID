using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.IO;
using TwistLockAPI;

namespace TwistLockAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            string dir = System.IO.Directory.GetCurrentDirectory();
            string dirE = $"{dir}/twistlock";
            Console.Write(dirE);
            bool directoryExists = Directory.Exists(dirE);
            Console.Write(directoryExists);
            if (directoryExists == true)
            {
                Directory.Delete(dirE, true);
            }
            string mkdir = $"mkdir {dirE}";
            Console.Write(mkdir);
            string mkdirCmd = coms.Bash(mkdir);
            Console.Write(mkdirCmd);
            string username = "Ryan.Brown@csiweb.com";
            string password = "C0nt41nm3!";
            string getTwist = coms.Bash($"curl -k -u {username}:{password} --output {dirE}/twistcli https://200.0.34.100:8083/api/v1/util/twistcli");
            string chmod = coms.Bash($"chmod u+x {dirE}/twistcli");
            string lineNum = coms.Bash("docker images | head | awk 'END{print NR}'");
            Console.Write(lineNum);
            int lineInt = Int32.Parse(lineNum);
            Console.Write(lineInt);
            string imageIds = $"docker images | head -{lineInt} | awk '{{print $3}}'";
            Console.Write(imageIds);
            string dockerImageCom = coms.Bash(imageIds);
            Console.Write(dockerImageCom);
            //string command = $"docker images | head -lineInt | awk '{{print $3}}' | sed -n '3 p'";
            //Console.Write(command);
            for (int i = 2; i <= lineInt; i++)
            {
                string test = coms.Bash($"docker images | head -{lineInt} | awk '{{print $3}}' | sed -n '{i} p'");
                Console.Write(test);
                string test2 = coms.Bash($"{dirE}/twistcli images scan -u {username} -p {password} --address https://200.0.34.100:8083 --vulnerability-threshold high {test}");
                //string test2 = $".{dirE}/twistcli images scan -u Ryan.Brown @csiweb.com - p C0nt41nm3! --address https://200.0.34.100:8083 --vulnerability-threshold high {test}";
                Console.Write(test2);
            }
        }
    }
}