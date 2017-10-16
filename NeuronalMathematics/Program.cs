using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronalMathematics
{
    class Program
    {
        static void Main(string[] args)
        {
            PersistantModel.Term t = new PersistantModel.Term(1.0, "a", "x");
            FileInfo fi = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.bin"));
            PersistantModel.PersistentDataObject.Save(fi, t);

            PersistantModel.PersistentDataObject o = t as PersistantModel.PersistentDataObject;
            PersistantModel.PersistentDataObject.Load(fi, out o);

            Console.WriteLine(o.ToString());

            Console.ReadKey();
        }
    }
}
