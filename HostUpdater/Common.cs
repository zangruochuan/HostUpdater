using Microsoft.Win32;

namespace HostsUpdater
{
    public class Common
    {
        public static string GetPath(string name)
        {
            RegistryKey folders;
            folders = OpenRegistryPath(Registry.CurrentUser, @"/software/microsoft/windows/currentversion/explorer/shell folders");
            string Path = folders.GetValue(name).ToString();
            return Path;
        }


        private static RegistryKey OpenRegistryPath(RegistryKey root, string s)
        {
            s = s.Remove(0, 1) + @"/";
            while (s.IndexOf(@"/") != -1)
            {
                root = root.OpenSubKey(s.Substring(0, s.IndexOf(@"/")));
                s = s.Remove(0, s.IndexOf(@"/") + 1);
            }
            return root;
        }
        #region 获取系统常用路径（参考）
        //            using Microsoft.Win32;
        //namespace JPGCompact
        //    {
        //        public partial class MainForm : Form
        //        {
        //            private void Test()
        //            {
        //                RegistryKey folders;
        //                folders = OpenRegistryPath(Registry.CurrentUser, @"/software/microsoft/windows/currentversion/explorer/shell folders");
        //                // Windows用户桌面路径
        //                string desktopPath = folders.GetValue("Desktop").ToString();
        //                // Windows用户字体目录路径
        //                string fontsPath = folders.GetValue("Fonts").ToString();
        //                // Windows用户网络邻居路径
        //                string nethoodPath = folders.GetValue("Nethood").ToString();
        //                // Windows用户我的文档路径
        //                string personalPath = folders.GetValue("Personal").ToString();
        //                // Windows用户开始菜单程序路径
        //                string programsPath = folders.GetValue("Programs").ToString();
        //                // Windows用户存放用户最近访问文档快捷方式的目录路径
        //                string recentPath = folders.GetValue("Recent").ToString();
        //                // Windows用户发送到目录路径
        //                string sendtoPath = folders.GetValue("Sendto").ToString();
        //                // Windows用户开始菜单目录路径
        //                string startmenuPath = folders.GetValue("Startmenu").ToString();
        //                // Windows用户开始菜单启动项目录路径
        //                string startupPath = folders.GetValue("Startup").ToString();
        //                // Windows用户收藏夹目录路径
        //                string favoritesPath = folders.GetValue("Favorites").ToString();
        //                // Windows用户网页历史目录路径
        //                string historyPath = folders.GetValue("History").ToString();
        //                // Windows用户Cookies目录路径
        //                string cookiesPath = folders.GetValue("Cookies").ToString();
        //                // Windows用户Cache目录路径
        //                string cachePath = folders.GetValue("Cache").ToString();
        //                // Windows用户应用程式数据目录路径
        //                string appdataPath = folders.GetValue("Appdata").ToString();
        //                // Windows用户打印目录路径
        //                string printhoodPath = folders.GetValue("Printhood").ToString();
        //            }

        //            private RegistryKey OpenRegistryPath(RegistryKey root, string s)
        //            {
        //                s = s.Remove(0, 1) + @"/";
        //                while (s.IndexOf(@"/") != -1)
        //                {
        //                    root = root.OpenSubKey(s.Substring(0, s.IndexOf(@"/")));
        //                    s = s.Remove(0, s.IndexOf(@"/") + 1);
        //                }
        //                return root;
        //            }
        //        }
        //    }
        //}
        #endregion
    }
}
