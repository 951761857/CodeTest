using System;
using System.Linq.Expressions;

namespace Test.Application
{

    

    class Program
    {
        public delegate string Jiang(string jiang);

        public Func<ClassB, ClassB> UpdateFunc { get; set; }

        public Func<ClassB, ClassB, ClassB> UpdateFunc1 { get; set; }

        static void Main(string[] args)
        {

            ClassB.UpdateClasAC((q,i)=>q.Name == "jiang" && i.ID == 1);

            ClassB.UpdateClasA(q => { return false;});
        }


        public static void Test(Jiang handle)
        {

        }
    }

    public class ClassB
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime NowTime { get; set; }

        public static ClassB  UpdateClas(Action<ClassB> func)
        {
            var item = new ClassB()
            {
                ID = 1,
                Name = "jiang",
                NowTime = DateTime.Now,
            };

            

            func.Invoke(item);

            return item;
        }

        public static bool UpdateClasA(Func<ClassB,bool> func)
        {
            var item = new ClassB()
            {
                ID = 2,
                Name = "yi",
                NowTime = DateTime.Now,
            };


            UpdateClas(q => { q.Name = item.Name;
                q.ID = item.ID;
            });

            var res = func.Invoke(item);

            return res;
        }

        public static bool UpdateClasAC(Expression<Func<ClassB,ClassB, bool>> func)
        {
            var item = new ClassB()
            {
                ID = 2,
                Name = "yi",
                NowTime = DateTime.Now,
            };


            UpdateClas(q => {
                q.Name = item.Name;
                q.ID = item.ID;
            });



            var res = func.Compile()(new ClassB(), new ClassB() );

            return res;
        }
    }

    public  abstract class ClassA
    {
        public abstract void Test1();
    }




}
