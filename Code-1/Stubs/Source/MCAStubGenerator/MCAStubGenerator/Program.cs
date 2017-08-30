using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MCAStubGenerator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmDBUpgrade());

            var cmdArgs = new CmdArgs();

            var parser = new Parser(settings =>
            {
                settings.CaseSensitive = true;
            });

            if (parser.ParseArguments(args, cmdArgs))
            {
            }
            else
            {
                Console.WriteLine(cmdArgs.GetUsage());
            }     
        }

        private static string GetLogFilePath(string rootDir, string srvName, string dbName)
        {
            var logPath = Path.Combine(rootDir, "Logs", EnsureValidPath(srvName), EnsureValidPath(dbName));

            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            string sTimeStamp = DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" 
                              + DateTime.Now.TimeOfDay.Hours.ToString() + "_" + DateTime.Now.TimeOfDay.Minutes.ToString() + "_" + DateTime.Now.TimeOfDay.Seconds.ToString();

            logPath = Path.Combine(logPath, string.Format("{0}.txt", sTimeStamp));

            return logPath;
        }

        private static char[] invalidChars = null;

        private static string EnsureValidPath(string path)
        {
            if (invalidChars == null)
                invalidChars = Path.GetInvalidFileNameChars().Union(Path.GetInvalidPathChars()).Distinct().ToArray();

            var retVal = path;

            foreach (var chr in invalidChars)
            {
                retVal = retVal.Replace(chr, '-');
            }

            return retVal;
        }
    }

    public class CmdArgs
    {
        [Option('d', "dbName", Required = true, HelpText = "Database to upgrade.")]
        public string DbName { get; set; }

        [Option('s', Required = true, HelpText = "SQL Server name/ip.")]
        public string Server { get; set; }

        [Option('u', Required = false, HelpText = "SQL Username to connect to database server. Not needed in case of windows authentication")]
        public string UserName { get; set; }

        [Option('p', Required = false, HelpText = "SQL Password to connect to database server. Not needed in case of windows authentication")]
        public string Password { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return "Usage: UpdateDb.exe -s <sql server name or ip> -d <database name> -u <SQL login user name> -p <sql login password>";
        }
    }
}