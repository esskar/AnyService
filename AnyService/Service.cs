using System;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

using AnyService.Win32;

namespace AnyService
{
    public class Service : Win32.WinService
    {
        private RegistryHelper m_registry = null;
        private Process m_externalProgram = null;

        public Service(string name)
            : base(name)
        {
            m_registry = new RegistryHelper(Microsoft.Win32.Registry.LocalMachine, @"SYSTEM\CurrentControlSet\Services\" + name);
        }

        public string ExternalImagePath
        {
            get { return m_registry.LoadString("ExternalImagePath", "", true); }
            set 
            {
                if (value == null) value = "";
                value = value.Replace('\'', '"');
                m_registry.SaveString("ExternalImagePath", value, true); 
            }
        }

        public bool CheckExternalImagePath(string externalImagePath)
        {
            string t1, t2;
            return this.CheckExternalImagePath(externalImagePath, out t1, out t2);
        }

        private bool CheckExternalImagePath(string externalImagePath, out string imagePath, out string arguments)
        {
            bool retval = false;
            imagePath = arguments = null;

            int pos = externalImagePath.IndexOf(".exe", StringComparison.InvariantCultureIgnoreCase);
            if (pos > 0)
            {
                imagePath = externalImagePath.Substring(0, pos + 4);
                arguments = externalImagePath.Substring(pos + 4).Trim();
                retval = true;
            }

            return retval;
        }

        protected override void OnStart(string[] args)
        {            
            string externalImagePath = this.ExternalImagePath;
            // cleanup and devide into imagepath and arguments
            string imagePath = null, arguments = null;
            if (this.CheckExternalImagePath(this.ExternalImagePath, out imagePath, out arguments))
            {
                ProcessStartInfo psi = new ProcessStartInfo(imagePath, arguments);
                psi.WorkingDirectory = Path.GetDirectoryName(imagePath);
                
                m_externalProgram = new Process();
                m_externalProgram.Exited += new EventHandler(HandleProcessExited);
                m_externalProgram.StartInfo = psi;
                if(m_externalProgram.Start())
                    base.OnStart(args);
            }
        }

        protected override void OnStop()
        {
            if (m_externalProgram != null)
            {
                m_externalProgram.Exited -= new EventHandler(HandleProcessExited);
                
                int count = 4;
                while (count < 8)
                {
                    try { m_externalProgram.Kill(); break; }
                    catch (Win32Exception) { count++; Thread.Sleep(count * 50); }
                    catch { break; }
                }
                m_externalProgram.Close();
                m_externalProgram = null;
            }

            base.OnStop();
        }

        private void HandleProcessExited(object sender, EventArgs e)
        {            
            // External Program died or stopped
            // so exit as well            
            int exitCode = -1;
            try { exitCode = m_externalProgram.ExitCode; }
            catch { }
            Environment.Exit(exitCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_registry != null)
                {
                    m_registry.Dispose();
                    m_registry = null;
                }
            }
            base.Dispose(disposing);
        }

        public new void Install(string externalCommandLine)
        {
            string serviceNameArgument = this.ServiceName;
            if (serviceNameArgument.Contains(" "))
                serviceNameArgument = "\"" + serviceNameArgument + "\"";
           
            base.Install("run " + serviceNameArgument);
            this.ExternalImagePath = externalCommandLine;
        }
    }
}
