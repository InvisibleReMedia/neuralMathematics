using Interfaces;
using PersistantModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Maths
{
    public static class Applicatifs
    {
        public static FlowDocument Newton()
        {
            Wording w = new Wording("Résolution des polynômes", "La résolution des polynômes est un sujet encore mathématiquement non élucidé");
            SequenceProof s1 = new SequenceProof();
            Texte t1 = new Texte("Degré 2");
            Weight.Initialize();
            Equal eq1 = new Equal(new Power(new Addition(new Coefficient("a"), new Coefficient("b")), new NumericValue(2.0d)),
                                        new Sum(new Power(new Coefficient("a"), new NumericValue(2.0d)), new Product(new NumericValue(2.0d), new Coefficient("a"), new Coefficient("b")), new Power(new Coefficient("b"), new NumericValue(2.0d))));
            eq1.MakeUnique();

            Verify v = new Verify(eq1);
            foreach(IArithmetic va in v.Variables)
            {
                Console.WriteLine(va.ToString());
            }

            FileInfo fi = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "eq1.bin"));
            TopLevelArithmeticModel t = TopLevelArithmeticModel.Create("test-eq1");
            t.EquationList.Add(eq1);
            t.Save(fi);
            s1.Add(t1);
            s1.Add(eq1);
            Answer a = new Answer("Expressions avec la formule de Newton", s1);
            Exercice e = new Exercice(1, "Formule de Newton", "Décrivez la production de la formule de newton à l'ordre 2 et 3", a);
            w.Add(e);
            FlowDocument fd = new FlowDocument();
            w.ToDocument(fd);
            return fd;
        }

        public static FlowDocument ReloadNewton()
        {
            Wording w = new Wording("Résolution des polynômes", "La résolution des polynômes est un sujet encore mathématiquement non élucidé");
            SequenceProof s1 = new SequenceProof();
            Texte t1 = new Texte("Degré 2");

            FileInfo fi = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "eq1.bin"));
            TopLevelArithmeticModel t = TopLevelArithmeticModel.Load(fi);
            s1.Add(t1);
            s1.Add(t.EquationList[0]);
            Answer a = new Answer("Expressions avec la formule de Newton", s1);
            Exercice e = new Exercice(1, "Formule de Newton", "Décrivez la production de la formule de newton à l'ordre 2 et 3", a);
            w.Add(e);
            FlowDocument fd = new FlowDocument();
            w.ToDocument(fd);
            return fd;
        }
    }
}
