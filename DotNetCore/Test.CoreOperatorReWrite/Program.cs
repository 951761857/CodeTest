using System;
using System.Linq;

namespace Test.CoreOperatorReWrite
{
    class Program
    {
        static void Main(string[] args)
        {
            Test test = new Test() { ID = 1, Name = "jiang1" };

            //索引器
            var name1 = test["Name"];
            test["Name"] = "jiang2";

            var item1 = test[0];
            test[0] = new Test()
            {
                ID = 3,
                Name = "jiang3"
            };


            //++
            for (int i = 0; i < 5; i++)
            {
                test++;
                Console.WriteLine(test.RunState);
            }

            //--
            for (int i = 0; i < 10; i++)
            {
                test--;
                Console.WriteLine(test.RunState);
            }

            test = +test;
            test = test;
            test = new Test() { Speed = 10, Name = "jiang1" } + new Test() { Speed = 10, Name = "jiang2" };
            test = new Test() { Speed = 10, Name = "jiang2" } + new Test() { Speed = 10, Name = "jiang1" };
            test += new Test() { Speed = 30, Name = "jiang3" };


            test.Speed = 1;
            if (test)
            {
                Console.Write("test");
            }

            if (!test)
            {
                Console.Write("test");
            }

        }
    }



    public class Test
    {
        public int ID { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 后退、停止、行走、飞起
        /// </summary>
        public string RunState { get; set; }

        private int _speed { get; set; }

        public int Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
                UpdateState();
            }
        }


        public void UpdateState()
        {
            switch (this._speed)
            {
                case -1: this.RunState = "后退"; break;
                case 0: this.RunState = "停止"; break;
                case 1: this.RunState = "行走"; break;
                case 2: this.RunState = "飞起"; break;
            }
        }

        /// <summary>
        /// get：通过属性名获取值
        /// set：设置name
        /// </summary>
        /// <param name="attr"></param>
        /// <returns></returns>
        public string this[string attr]
        {
            get
            {
                return this.GetType().GetProperties().FirstOrDefault(q => q.Name == attr).GetValue(this)?.ToString();
            }
            set
            {
                this.Name = value;
            }
        }

        /// <summary>
        /// get：获取一个新对象，按索引选择单个属性赋值返回
        /// set：赋值全部属性
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Test this[int index]
        {
            get
            {
                var obj = new Test();
                var properties = typeof(Test).GetProperties()[index];
                properties.SetValue(obj, properties.GetValue(this));
                return obj;
            }
            set
            {
                foreach (var item in typeof(Test).GetProperties().Where(q => q.Name != "Item"))
                {
                    item.SetValue(this, item.GetValue(value));
                }
            }
        }

        /// <summary>
        /// ++
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public static Test operator ++(Test test)
        {
            if (test.Speed == 2)
            {
                return test;
            }
            test.Speed += 1;


            return test;
        }

        /// <summary>
        /// --
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public static Test operator --(Test test)
        {
            if (test.Speed == -1)
            {
                return test;
            }
            test.Speed -= 1;

            return test;
        }


        public static Test operator +(Test test1)
        {
            return test1;
        }

        public static Test operator +(Test test1, Test test2)
        {
            test1.Speed += test2.Speed;
            return test1;
        }

        /// <summary>
        /// 以该方法返回bool值为准
        /// </summary>
        /// <param name="test1"></param>
        /// <returns></returns>
        public static bool operator true(Test test1)
        {
            return test1.RunState == "停止";
        }

        /// <summary>
        /// 并不会执行
        /// </summary>
        /// <param name="test1"></param>
        /// <returns></returns>
        public static bool operator false(Test test1)
        {
            return test1.RunState == "停止";
        }

        public static bool operator !(Test test1)
        {
            return test1.RunState != "停止";
        }

    }
}
