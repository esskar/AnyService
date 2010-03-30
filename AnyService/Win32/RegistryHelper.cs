using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace AnyService.Win32
{
    public class RegistryHelper : Disposable
    {
        public RegistryHelper(RegistryKey registryKey, string registryPath)
        {
            this.RegistryKey = registryKey;
            this.RegistryPath = registryPath;
        }

        protected override void Release()
        {
            if (this.RegistryKey != null)
            {
                this.RegistryKey.Close();
                this.RegistryKey = null;
            }
            base.Release();
        }

        protected RegistryKey ConfigurationStore
        {
            get { return this.RegistryKey.CreateSubKey(this.RegistryPath); }
        }

        public RegistryKey RegistryKey { get; set; }

        public string RegistryPath { get; set; }

        public string LoadString(string name, string def, bool create)
        {
            return this.LoadString(name, def, create, false);
        }

        public string LoadString(string name, string def, bool create, bool expandable)
        {
            using (RegistryKey key = this.ConfigurationStore)
            {
                try{ return key.GetValue(name, def).ToString(); }
                finally
                {
                    if (create && !this.HasValue(name))
                        this.SaveString(name, def, expandable);             
                }
            }
        }

        public string[] LoadStringArray(string name, string[] def, bool create)
        {
            return this.LoadStringArray(name, def, ",", create);
        }

        public string[] LoadStringArray(string name, string[] def, string sep, bool create)
        {
            string s = this.LoadString(name, def != null ? string.Join(sep, def) : "", create);
            return s.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);
        }

        public void SaveString(string name, string val, bool expandable)
        {
            using(RegistryKey key = this.ConfigurationStore)
                key.SetValue(name, val, expandable ? RegistryValueKind.ExpandString : RegistryValueKind.String);            
        }

        public int LoadInteger(string name, int def, bool create)
        {
            using (RegistryKey key = this.ConfigurationStore)
            {
                try
                {
                    return (int)key.GetValue(name, def);
                }
                finally
                {
                    if (create && !this.HasValue(name))
                        this.SaveInteger(name, def);             
                }
            }
        }

        public uint LoadDword(string name, uint def, bool create)
        {
            using (RegistryKey key = this.ConfigurationStore)
            {
                try
                {
                    return (uint)key.GetValue(name, def);
                }
                finally
                {
                    if (create && !HasValue(name))
                        this.SaveDword(name, def);             
                }
            }
        }

        public void SaveDword(string name, uint val)
        {
            using(RegistryKey key = this.ConfigurationStore)
                key.SetValue(name, val, RegistryValueKind.DWord);            
        }

        public void SaveInteger(string name, int val)
        {
            using(RegistryKey key = this.ConfigurationStore)
                key.SetValue(name, val, RegistryValueKind.DWord);            
        }

        public Type GetValueType(string name)
        {
            using (RegistryKey key = this.ConfigurationStore)
            {
                try
                {
                    RegistryValueKind kind = key.GetValueKind(name);
                    if (kind == RegistryValueKind.DWord)
                        return typeof(int);
                    else if (kind == RegistryValueKind.String)
                        return typeof(string);
                    else
                        return typeof(object);
                }
                catch (System.IO.IOException) { return null; }             
            }
        }

        public bool HasValue(string name)
        {
            using(RegistryKey key = this.ConfigurationStore)
                return key.GetValue(name) != null;            
        }

        public void DeleteValue(string Name)
        {
            using(RegistryKey key = this.ConfigurationStore)
                key.DeleteValue(Name, false);            
        }

        public void DeleteKey(string name, bool recursiv)
        {
            using(RegistryKey key = this.ConfigurationStore)
                if (recursiv) key.DeleteSubKeyTree(name);
                else key.DeleteSubKey(name, false);            
        }
    }
}
