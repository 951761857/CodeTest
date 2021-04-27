using System;
using System.IO;
using System.Text;

namespace Test.CoreAppLifeTime.LifeTimes
{
    public class TestSingleton : IDisposable
    {
        public DateTime CreateTime { get; set; } = DateTime.Now;

        public bool IsWrite { get; set; }

        public string WriteMessage { get; set; }

        public static int Count { get; set; } = 0;

        public int Number { get; set; } = 0;

        public TestSingleton()
        {
            Count++;
            Number = Count;
        }

        public void Dispose()
        {
            if (IsWrite)
            {
                string paht = "Singleton生命周期测试.txt";
                if (!File.Exists(paht))
                {
                    File.Create(paht);
                }
                Console.WriteLine($"序号：{Number},{WriteMessage}  \r\n");
                File.AppendAllText(paht, $"序号：{Count},{WriteMessage}  \r\n", Encoding.UTF8);
            }
        }
    }
}
