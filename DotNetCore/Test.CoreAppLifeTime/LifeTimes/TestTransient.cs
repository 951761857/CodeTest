using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Test.CoreAppLifeTime.LifeTimes
{
    public class TestTransient : IDisposable
    {
        public DateTime CreateTime { get; set; } = DateTime.Now;

        public bool IsWrite { get; set; }

        public string WriteMessage { get; set; }

        public static int Count { get; set; } = 0;

        public int Number { get; set; } = 0;

        public TestTransient()
        {
            Count++;
            Number = Count;
        }

        public void Dispose()
        {
            if (IsWrite)
            {
                string paht = "Transient生命周期测试.txt";
                if (!File.Exists(paht))
                {
                    File.Create(paht);
                }

                Debugger.Log(4,"test", $"序号：{Number},{WriteMessage}  \r\n");
                Console.WriteLine($"序号：{Number},{WriteMessage}  \r\n");
                File.AppendAllTextAsync(paht, $"序号：{Number},{WriteMessage}  \r\n", Encoding.UTF8);
            }
        }
    }
}
