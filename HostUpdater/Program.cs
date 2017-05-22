using System;
using System.Diagnostics;

namespace HostsUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch swatch = new System.Diagnostics.Stopwatch();
            swatch.Restart();
            Updater updater = new Updater();
            updater.UpdateHostFile();
            swatch.Stop();
            Console.WriteLine("总计花费时间：" + swatch.ElapsedMilliseconds + "ms");
            Console.ReadKey();
        }
    }
}
