using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HostsUpdater
{
    public class Config
    {
        public static bool HasConfigFile
        {
            get
            {
                FileInfo file = new FileInfo("config");
                if (File.Exists(file.FullName))
                {
                    return true;
                }
                return false;
            }
        }
        public static string GetConfig(string Key)
        {
            try
            {
                string value = string.Empty;
                FileInfo file = new FileInfo("config");
                if (File.Exists(file.FullName))
                {
                    StreamReader sr = new StreamReader(file.FullName);
                    Dictionary<string, string> Dic = new Dictionary<string, string>();
                    Dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(sr.ReadToEnd());
                    if (Dic.ContainsKey(Key))
                    {
                        value = Dic[Key];
                        Console.WriteLine("从config获取"+Key+"为"+value);
                    }
                }
                return value;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return string.Empty;
        }
    }
}
