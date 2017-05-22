using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace HostsUpdater
{
    public class Updater
    {
        // 资源路径
        private string resourceUrl;
        // 备份本地保存路径
        private string savePath;
        // Hosts路径
        private string systemHostsPath;

        public string ResourceUrl { get => resourceUrl; set => resourceUrl = value; }
        public string SavePath { get => savePath; set => savePath = value; }
        public string SystemHostsPath { get => systemHostsPath; set => systemHostsPath = value; }

        public Updater()
        {
            if (Config.HasConfigFile)
            {
                ResourceUrl = Config.GetConfig("ResourceUrl");
                SavePath = Config.GetConfig("SavePath");
                SystemHostsPath = Config.GetConfig("SystemHostsPath");
            }
            else
            {
                Console.WriteLine("未找到配置文件，使用默认配置");
            }
        }
        private void FailUpdateHostFile(Exception ex)
        {
            Console.WriteLine("HOSTS更新失败");
            Console.WriteLine("错误描述:" + ex.Message);
            Retry();
        }

        private void Retry()
        {
            Console.WriteLine("重试(Y/N)?");
            string IsRetry = Console.ReadLine();
            if (IsRetry.ToUpper() == "Y")
            {
                UpdateHostFile();
            }
            else if (IsRetry.ToUpper() == "N")
            {

            }
            else
            {
                Retry();
            }
        }

        public void UpdateHostFile()
        {
            try
            {
                string url = string.IsNullOrWhiteSpace(ResourceUrl) ? "https://raw.githubusercontent.com/racaljk/hosts/master/hosts" : ResourceUrl;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                string html = reader.ReadToEnd();
                                Regex reg = new Regex("Last updated: [0-9]{4}-[01][0-9]-[0-3][0-9]");
                                string updateDate = reg.Match(html, 0, 100).ToString().Substring(14);
                                string DesktopPath = Common.GetPath("Desktop");
                                string uri = string.IsNullOrWhiteSpace(SavePath) ? DesktopPath + "/一键替换HOSTS/" : SavePath;

                                string path = uri + updateDate;
                                string filename = "HOSTS";
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                    using (FileStream fs = new FileStream(path + "/" + filename, FileMode.Create))
                                    {
                                        using (StreamWriter sw = new StreamWriter(fs))
                                        {
                                            sw.WriteLine(html);
                                            sw.Flush();
                                        }
                                    }
                                    string HostsPath = string.IsNullOrWhiteSpace(SystemHostsPath) ? @"C:\\Windows\System32\drivers\etc\hosts" : SystemHostsPath;
                                    using (FileStream fs = new FileStream(HostsPath, FileMode.Create))
                                    {
                                        using (StreamWriter sw = new StreamWriter(fs))
                                        {
                                            sw.WriteLine(html);
                                            sw.Flush();
                                        }
                                    }
                                    Process p = new Process();
                                    p.StartInfo.FileName = "cmd.exe";
                                    p.StartInfo.UseShellExecute = false;
                                    p.StartInfo.RedirectStandardInput = true;
                                    p.StartInfo.RedirectStandardOutput = true;
                                    p.StartInfo.RedirectStandardError = true;
                                    p.StartInfo.CreateNoWindow = true;
                                    p.Start();

                                    p.StandardInput.WriteLine("ipconfig/flusedns" + "&exit");
                                    Console.WriteLine("HOSTS更新成功");
                                }
                                else
                                {
                                    Console.WriteLine("DNS文件已经是最新的");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FailUpdateHostFile(ex);
            }
        }
    }
}
