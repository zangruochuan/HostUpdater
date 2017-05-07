using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace HostsUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch swatch = new System.Diagnostics.Stopwatch();
            swatch.Restart();
            UpdateHostFile();
            swatch.Stop();
            Console.WriteLine("总计花费时间：" + swatch.ElapsedMilliseconds + "ms");
            Console.ReadKey();
        }

        private static void FailUpdateHostFile(Exception ex)
        {
            Console.WriteLine("HOSTS更新失败");
            Console.WriteLine("错误描述:" + ex.Message);
            Retry();
        }

        private static void Retry()
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

        private static void UpdateHostFile()
        {
            try
            {
                string url = "https://raw.githubusercontent.com/racaljk/hosts/master/hosts";
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
                                Uri uri = new Uri("C://Users/Administrator/desktop/");

                                string path = "C://Users/Administrator/desktop/一键替换HOSTS/" + updateDate;
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
                                    string systemHostsPath = @"C:\\Windows\System32\drivers\etc\hosts";
                                    using (FileStream fs = new FileStream(systemHostsPath, FileMode.Create))
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
