using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Microsoft.ServiceBus
{
    internal class RelayEnvironment
    {
        internal interface IEnvironment
        {
            string RelayHostRootName { get; }

            int RelayHttpPort { get; }

            int RelayHttpsPort { get; }

            string RelayPathPrefix { get; }

            string StsHostName { get; }

            bool StsEnabled { get; }

            int StsHttpPort { get; }

            int StsHttpsPort { get; }

            int RelayNmfPort { get; }
        }

        private class MutableEnvironment : IEnvironment
        {
            private string relayHostRootName;

            private int relayHttpPort;

            private int relayHttpsPort;

            private string relayPathPrefix;

            private string stsHostName;

            private bool stsEnabled;

            private int stsHttpPort;

            private int stsHttpsPort;

            private int relayNmfPort;

            public string RelayHostRootName
            {
                get
                {
                    return relayHostRootName;
                }
                set
                {
                    relayHostRootName = value;
                }
            }

            public int RelayHttpPort => relayHttpPort;

            public int RelayHttpsPort => relayHttpsPort;

            public string RelayPathPrefix => relayPathPrefix;

            public string StsHostName => stsHostName;

            public bool StsEnabled
            {
                get
                {
                    return stsEnabled;
                }
                set
                {
                    stsEnabled = value;
                }
            }

            public int StsHttpPort => stsHttpPort;

            public int StsHttpsPort => stsHttpsPort;

            public int RelayNmfPort => relayNmfPort;

            public MutableEnvironment(IEnvironment environment)
            {
                relayHostRootName = environment.RelayHostRootName;
                relayHttpPort = environment.RelayHttpPort;
                relayHttpsPort = environment.RelayHttpsPort;
                relayPathPrefix = environment.RelayPathPrefix;
                stsHostName = environment.StsHostName;
                stsEnabled = environment.StsEnabled;
                stsHttpPort = environment.StsHttpPort;
                stsHttpsPort = environment.StsHttpsPort;
                relayNmfPort = environment.RelayNmfPort;
            }
        }

        private abstract class EnvironmentBase : IEnvironment
        {
            public abstract string RelayHostRootName { get; }

            public virtual int RelayHttpPort => 80;

            public virtual int RelayHttpsPort => 443;

            public virtual string RelayPathPrefix => string.Empty;

            public abstract string StsHostName { get; }

            public virtual bool StsEnabled => true;

            public virtual int StsHttpPort => 80;

            public virtual int StsHttpsPort => 443;

            public int RelayNmfPort => 9354;
        }

        private class LiveEnvironment : EnvironmentBase
        {
            public override string RelayHostRootName => "servicebus.windows.net";

            public override string StsHostName => "accesscontrol.windows.net";
        }

        private class PpeEnvironment : EnvironmentBase
        {
            public override string RelayHostRootName => "servicebus.windows-ppe.net";

            public override string StsHostName => "accesscontrol.windows-ppe.net";
        }

        private class IntEnvironment : EnvironmentBase
        {
            public override string RelayHostRootName => "servicebus.windows-int.net";

            public override string StsHostName => "accesscontrol.windows-ppe.net";
        }

        private class LocalEnvironment : EnvironmentBase
        {
            public override string RelayHostRootName => "servicebus.onebox.windows-int.net";

            public override string StsHostName => "servicebus.onebox.windows-int.net";

            public override bool StsEnabled => false;
        }

        private class CustomEnvironment : IEnvironment
        {
            private const string RelayHostEnvironmentVariable = "RELAYHOST";

            private const string RelayHttpPortEnvironmentVariable = "RELAYHTTPPORT";

            private const string RelayHttpsPortEnvironmentVariable = "RELAYHTTPSPORT";

            private const string RelayNmfPortEnvironmentVariable = "RELAYNMFPORT";

            private const string RelayPathPrefixEnvironmentVariable = "RELAYPATHPREFIX";

            private const string StsHostEnvironmentVariable = "STSHOST";

            private const string StsHttpPortEnvironmentVariable = "STSHTTPPORT";

            private const string StsHttpsPortEnvironmentVariable = "STSHTTPSPORT";

            private string relayHostRootName = string.Empty;

            private int relayHttpPort;

            private int relayHttpsPort;

            private string relayPathPrefix = string.Empty;

            private string stsHostName = string.Empty;

            private bool stsEnabled;

            private int stsHttpPort;

            private int stsHttpsPort;

            private int relayNmfPort;

            public string RelayHostRootName => relayHostRootName;

            public int RelayHttpPort => relayHttpPort;

            public int RelayHttpsPort => relayHttpsPort;

            public string RelayPathPrefix => relayPathPrefix;

            public string StsHostName => stsHostName;

            public bool StsEnabled => stsEnabled;

            public int StsHttpPort => stsHttpPort;

            public int StsHttpsPort => stsHttpsPort;

            public int RelayNmfPort => relayNmfPort;

            public CustomEnvironment()
            {
                relayHostRootName = System.Environment.GetEnvironmentVariable("RELAYHOST") ?? string.Empty;
                relayHttpPort = GetEnvironmentVariable("RELAYHTTPPORT", 80);
                relayHttpsPort = GetEnvironmentVariable("RELAYHTTPSPORT", 443);
                relayPathPrefix = System.Environment.GetEnvironmentVariable("RELAYPATHPREFIX") ?? string.Empty;
                stsHostName = System.Environment.GetEnvironmentVariable("STSHOST") ?? string.Empty;
                stsEnabled = true;
                stsHttpPort = GetEnvironmentVariable("STSHTTPPORT", 80);
                stsHttpsPort = GetEnvironmentVariable("STSHTTPSPORT", 443);
                relayNmfPort = GetEnvironmentVariable("RELAYNMFPORT", 9354);
            }
        }

        internal class ConfigSettings : IEnvironment
        {
            private const string RelayHostNameElement = "relayHostName";

            private const string RelayHttpPortNameElement = "relayHttpPort";

            private const string RelayHttpsPortNameElement = "relayHttpsPort";

            private const string RelayNmfPortNameElement = "relayNmfPort";

            private const string RelayPathPrefixElement = "relayPathPrefix";

            private const string StsHostNameElement = "stsHostName";

            private const string StsEnabledElement = "stsEnabled";

            private const string StsHttpPortNameElement = "stsHttpPort";

            private const string StsHttpsPortNameElement = "stsHttpsPort";

            private const string V1ConfigFileName = "servicebus.config";

            private const string WebRootPath = "approot\\";

            private readonly string configFileName;

            private bool haveSettings;

            private string relayHostName = string.Empty;

            private int relayHttpPort;

            private int relayHttpsPort;

            private int relayNmfPort;

            private string relayPathPrefix = string.Empty;

            private string stsHostName = string.Empty;

            private bool stsEnabled;

            private int stsHttpPort;

            private int stsHttpsPort;

            public bool HaveSettings => haveSettings;

            public string RelayHostRootName => relayHostName;

            public int RelayHttpPort => relayHttpPort;

            public int RelayHttpsPort => relayHttpsPort;

            public int RelayNmfPort => relayNmfPort;

            public string RelayPathPrefix => relayPathPrefix;

            public string StsHostName => stsHostName;

            public bool StsEnabled => stsEnabled;

            public int StsHttpPort => stsHttpPort;

            public int StsHttpsPort => stsHttpsPort;

            public ConfigSettings()
            {
                configFileName = "servicebus.config";
                ReadConfigSettings();
            }

            private void ReadConfigSettings()
            {
                haveSettings = false;
                string location = Assembly.GetExecutingAssembly().Location ?? string.Empty;
                string text = string.Empty;
                if (!string.IsNullOrWhiteSpace(location))
                {
                    string directoryName = Path.GetDirectoryName(location) ?? string.Empty;
                    text = Path.Combine(directoryName, configFileName);
                }

                string text2 = configFileName;
                string text3 = Path.Combine("approot\\", configFileName);
                string path;
                if (File.Exists(text2))
                {
                    path = text2;
                }
                else if (File.Exists(text3))
                {
                    path = text3;
                }
                else if (!string.IsNullOrEmpty(text) && File.Exists(text))
                {
                    path = text ?? string.Empty;
                }
                else
                {
                    string filePath = ConfigurationManager.OpenMachineConfiguration().FilePath ?? string.Empty;
                    string directoryName2 = Path.GetDirectoryName(filePath) ?? string.Empty;
                    path = Path.Combine(directoryName2, configFileName);
                }

                LiveEnvironment liveEnvironment = new LiveEnvironment();
                relayHostName = liveEnvironment.RelayHostRootName;
                relayHttpPort = liveEnvironment.RelayHttpPort;
                relayHttpsPort = liveEnvironment.RelayHttpsPort;
                relayNmfPort = liveEnvironment.RelayNmfPort;
                relayPathPrefix = liveEnvironment.RelayPathPrefix;
                stsHostName = liveEnvironment.StsHostName;
                stsEnabled = liveEnvironment.StsEnabled;
                stsHttpPort = liveEnvironment.StsHttpPort;
                stsHttpsPort = liveEnvironment.StsHttpsPort;
                if (!File.Exists(path))
                {
                    return;
                }

                Stream stream = File.OpenRead(path);
                XmlReader xmlReader = XmlReader.Create(stream);
                xmlReader.ReadStartElement("configuration");
                xmlReader.ReadStartElement("Microsoft.ServiceBus");
                while (xmlReader.IsStartElement())
                {
                    string name = xmlReader.Name;
                    string s = xmlReader.ReadElementString();
                    switch (name)
                    {
                        case "relayHostName":
                            relayHostName = s;
                            break;
                        case "relayHttpPort":
                            relayHttpPort = int.Parse(s, CultureInfo.InvariantCulture);
                            break;
                        case "relayHttpsPort":
                            relayHttpsPort = int.Parse(s, CultureInfo.InvariantCulture);
                            break;
                        case "relayNmfPort":
                            relayNmfPort = int.Parse(s, CultureInfo.InvariantCulture);
                            break;
                        case "relayPathPrefix":
                            relayPathPrefix = s;
                            if (!relayPathPrefix.StartsWith("/", StringComparison.OrdinalIgnoreCase))
                            {
                                relayPathPrefix = "/" + relayPathPrefix;
                            }

                            if (relayPathPrefix.EndsWith("/", StringComparison.Ordinal))
                            {
                                relayPathPrefix = relayPathPrefix.Substring(0, relayPathPrefix.Length - 1);
                            }

                            break;
                        case "stsHostName":
                            stsHostName = s;
                            break;
                        case "stsHttpPort":
                            stsHttpPort = int.Parse(s, CultureInfo.InvariantCulture);
                            break;
                        case "stsHttpsPort":
                            stsHttpsPort = int.Parse(s, CultureInfo.InvariantCulture);
                            break;
                    }
                }

                xmlReader.ReadEndElement();
                xmlReader.ReadEndElement();
                stream.Close();
                haveSettings = true;
            }
        }

        public const string RelayEnvEnvironmentVariable = "RELAYENV";

        public const string StsEnabledEnvironmentVariable = "RELAYSTSENABLED";

        public const string AcsVersionVariable = "ACSVERSION";

        private const int DefaultHttpPort = 80;

        private const int DefaultHttpsPort = 443;

        private const int DefaultNmfPort = 9354;

        public static string RelayHostRootName
        {
            get
            {
                return Environment?.RelayHostRootName ?? string.Empty;
            }
            set
            {
                if (Environment != null)
                    Environment.RelayHostRootName = value;
            }
        }

        public static int RelayHttpPort => Environment?.RelayHttpPort ?? 0;

        public static int RelayHttpsPort => Environment?.RelayHttpsPort ?? 0;

        public static string StsHostName => Environment?.StsHostName ?? string.Empty;

        public static bool StsEnabled
        {
            get
            {
                return Environment?.StsEnabled ?? false;
            }
            set
            {
                if (Environment != null)
                    Environment.StsEnabled = value;
            }
        }

        public static int StsHttpPort => Environment?.StsHttpPort ?? 0;

        public static int StsHttpsPort => Environment?.StsHttpsPort ?? 0;

        public static int RelayNmfPort => Environment?.RelayNmfPort ?? 0;

        internal static string RelayPathPrefix => Environment?.RelayPathPrefix ?? string.Empty;

        private static MutableEnvironment? Environment { get; set; }

        static RelayEnvironment()
        {
            if (!TryInitializeBasedOnEnvironmentVariable())
            {
                ConfigSettings configSettings = new ConfigSettings();
                if (configSettings.HaveSettings)
                {
                    Environment = new MutableEnvironment(configSettings);
                }
                else
                {
                    Environment = new MutableEnvironment(new LiveEnvironment());
                }
            }
        }

        public static bool GetEnvironmentVariable(string variable, bool defaultValue)
        {
            string environmentVariable = System.Environment.GetEnvironmentVariable(variable) ?? string.Empty;
            if (environmentVariable != null && bool.TryParse(environmentVariable, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        public static int GetEnvironmentVariable(string variable, int defaultValue)
        {
            string environmentVariable = System.Environment.GetEnvironmentVariable(variable) ?? string.Empty;
            if (!string.IsNullOrEmpty(environmentVariable) && int.TryParse(environmentVariable, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        private static bool TryInitializeBasedOnEnvironmentVariable()
        {
            string environmentVariable = System.Environment.GetEnvironmentVariable("RELAYENV") ?? string.Empty;
            if (environmentVariable != null)
            {
                switch (environmentVariable.ToUpperInvariant())
                {
                    case "LIVE":
                        Environment = new MutableEnvironment(new LiveEnvironment());
                        break;
                    case "PPE":
                        Environment = new MutableEnvironment(new PpeEnvironment());
                        break;
                    case "INT":
                        Environment = new MutableEnvironment(new IntEnvironment());
                        break;
                    case "LOCAL":
                        Environment = new MutableEnvironment(new LocalEnvironment());
                        break;
                    case "CUSTOM":
                        Environment = new MutableEnvironment(new CustomEnvironment());
                        break;
                    default:
                        {
                            Environment = new MutableEnvironment(new LiveEnvironment());
#if NETFRAMEWORK
                            string message = string.Format(CultureInfo.InvariantCulture, "Invalid RELAYENV value: {0}, valid values = LIVE, PPE, INT", environmentVariable);
                            EventLog.WriteEntry("MSCSH", message, EventLogEntryType.Error, 0);
#endif
                            break;
                        }
                }

                return true;
            }

            return false;
        }
    }
}