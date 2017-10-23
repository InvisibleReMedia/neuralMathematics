using Interfaces;
using PersistantModel;
using System;
using System.Collections.Generic;
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
            IArithmetic eq1 = new Equal(new Power(new Addition(new Coefficient("a"), new Coefficient("b")), new NumericValue(2.0d)),
                                        new Sum(new Power(new Coefficient("a"), new NumericValue(2.0d)), new Product(new NumericValue(2.0d), new Coefficient("a"), new Coefficient("b")), new Power(new Coefficient("b"), new NumericValue(2.0d))));
            IEnumerable<IArithmetic> ex = Weight.UniqueArithmeticInstances;
            s1.Add(t1);
            s1.Add(eq1);
            Answer a = new Answer("Expressions avec la formule de Newton", s1);
            Exercice e = new Exercice(1, "Formule de Newton", "Décrivez la production de la formule de newton à l'ordre 2 et 3", a);
            w.Add(e);
            FlowDocument fd = new FlowDocument();
            w.ToDocument(fd);
            return fd;
        }
    }
}
