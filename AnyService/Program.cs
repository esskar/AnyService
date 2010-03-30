using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AnyService
{
    class Program
    {
        public enum Cmd
        {
            Install, Uninstall, Start, Stop, Help, Run
        };

        static int Main(string[] args)
        {
            if (args.Length == 0)
                args = new string[] { "help" };

            int rc = 0;

            string command = args[0].ToLowerInvariant();
            try
            {                
                Cmd cmd = (Cmd)Enum.Parse(typeof(Cmd), command, true);
                switch(cmd)
                {
                    case Cmd.Install: rc = Program.HandleInstall(args); break;
                    case Cmd.Uninstall: rc = Program.HandleUninstall(args); break;
                    case Cmd.Start: rc = Program.HandleStart(args); break;
                    case Cmd.Stop: rc = Program.HandleStop(args); break;
                    case Cmd.Run: rc = Program.HandleRun(args); break;
                    case Cmd.Help:
                    default: rc = Program.HandleHelp(null); break;
                }

            }
            catch(Exception e)
            {
                rc = Program.HandleHelp(e.Message);
            }

            return rc;
        }        

        static int HandleInstall(string[] args)
        {
            int retval = 0;

            if (args.Length < 3)
                throw new ArgumentException("Not enough arguments for 'install' command.");
            string serviceName = args[1];
            string externalCommandLine = args[args.Length - 1];
            
            try
            {

                Service s = new Service(serviceName);
                if (!s.CheckExternalImagePath(externalCommandLine))
                    throw new Exception("I do not understand your external program argument.");
                s.Install(externalCommandLine);

                Console.WriteLine(string.Format("Successfully installed an AnyService called: {0}", serviceName));                
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Unable to install service: " + e.Message);
                retval = -1;
            }
            return retval;
        }

        static int HandleUninstall(string[] args)
        {
            int retval = 0;

            if (args.Length < 2)
                throw new ArgumentException("Not enough arguments for 'uninstall' command.");
            string serviceName = args[1];
            
            try
            {
                Service s = new Service(serviceName);
                s.Uninstall();

                Console.WriteLine("Successfully uninstalled an AnyService called: " + serviceName);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Unable to uninstall service: " + e.Message);
                retval = -1;
            }
            return retval;
        }

        static int HandleRun(string[] args)
        {
            int retval = 0;

            if (args.Length < 2)
                throw new ArgumentException("Not enough arguments for 'run' command.");
            string serviceName = args[1];
            try
            {

                Service s = new Service(serviceName);
                s.Run();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Unable to install service: " + e.Message);
                retval = -1;
            }
            return retval;
        }

        static int HandleStart(string[] args)
        {
            int retval = 0;

            return retval;
        }

        static int HandleStop(string[] args)
        {
            int retval = 0;

            return retval;
        }

        static int HandleHelp(string message)
        {
            int retval = 0;

            if (!string.IsNullOrEmpty(message))
            {
                Console.WriteLine("Error handling command: " + message + Environment.NewLine);
                retval = 1;
            }
            Console.WriteLine("AnyService, Copyright © 2010, Sascha Kiefer." + Environment.NewLine);
            Console.WriteLine("Usage: AnyService.exe <command> <service name> [options] [file]" + Environment.NewLine);
            Console.WriteLine("Command:");
            Console.WriteLine("\tinstall\t\t- Installs an AnyService named <service name> with application [file]");
            Console.WriteLine("\tuninstall\t- Uninstalls an AnyService name <service name>");
            Console.WriteLine("\tstart\t\t- Tries to start an AnyService named <service name>");
            Console.WriteLine("\tstop\t\t- Tries to stop an AnyService named <service name>");
            Console.WriteLine("\thelp\t\t- Displays this help message.");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("\tAnyService.exe install MyService c:\\Path\\To\\MyNotYetAService.exe");

            return retval;
        }
    }
}
