using Interfaces;
using PersistantModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Maths;

namespace NeuralMathematics
{
    /// <summary>
    /// Fonctions qui implémentent les documents (FlowDocument)
    /// </summary>
    public static class Applicatif
    {
        /// <summary>
        /// Evenement sur click des boutons
        /// </summary>
        private static event EventHandler onButtonClick;

        /// <summary>
        /// Event handler for clicked button
        /// </summary>
        public static event EventHandler ButtonClicked
        {
            add
            {
                onButtonClick += value;
            }
            remove
            {
                onButtonClick -= value;
            }
        }


        /// <summary>
        /// Fonction de menu
        /// </summary>
        /// <returns></returns>
        public static FlowDocument Menu()
        {
            Table t = new Table();
            TableRowGroup trg = new TableRowGroup();
            TableRow tr = new TableRow();
            Button b1 = new Button();
            b1.Name = "P2";
            b1.Content = "Résolution du polynôme d'ordre 2";
            b1.Click += Button_Click;
            SetButtonStyle(b1);
            Button b2 = new Button();
            b2.Name = "P3";
            b2.Content = "Résolution du polynôme d'ordre 3";
            b2.Click += Button_Click;
            SetButtonStyle(b2);
            TableCell tc = new TableCell(new BlockUIContainer(b1));
            tr.Cells.Add(tc);
            tc = new TableCell(new BlockUIContainer(b2));
            tr.Cells.Add(tc);
            trg.Rows.Add(tr);
            tr = new TableRow();
            Button b5 = new Button();
            b5.Name = "Close";
            b5.Content = "Quitter";
            b5.Click += Button_Click;
            SetButtonStyle(b5);
            tc = new TableCell(new BlockUIContainer(b5));
            tc.ColumnSpan = 2;
            tr.Cells.Add(tc);
            trg.Rows.Add(tr);
            t.RowGroups.Add(trg);

            FlowDocument fd = new FlowDocument(t);
            Paragraph p = new Paragraph();
            fd.Blocks.Add(p);
            return fd;
        }

        /// <summary>
        /// When click on button 1
        /// </summary>
        /// <param name="sender">source window</param>
        /// <param name="e">args</param>
        private static void Button_Click(object sender, RoutedEventArgs e)
        {
            onButtonClick(sender, e);
        }

        /// <summary>
        /// Set the button style
        /// </summary>
        /// <param name="b">button</param>
        private static void SetButtonStyle(Button b)
        {
            b.Height = 50.0d;
            b.Background = Brushes.AliceBlue;
            b.Foreground = Brushes.Black;
        }

        /// <summary>
        /// Converts double into Arithmetic
        /// </summary>
        /// <param name="d">d</param>
        /// <returns>pbject</returns>
        private static Arithmetic C(double d)
        {
            return new NumericValue(d);
        }

        /// <summary>
        /// Converts term into Arithmetic
        /// </summary>
        /// <param name="s">term</param>
        /// <returns>pbject</returns>
        private static Arithmetic C(string s)
        {
            return new UnknownTerm(s);
        }

        /// <summary>
        /// Converts coefficient into Arithmetic
        /// </summary>
        /// <param name="c">coefficient</param>
        /// <returns>pbject</returns>
        private static Arithmetic C(char c)
        {
            return new Coefficient(c.ToString());
        }

        /// <summary>
        /// Computes image size
        /// </summary>
        /// <returns></returns>
        public static FlowDocument SolvePolynome2()
        {
            FlowDocument fd = new FlowDocument();

            Equal function = new Equal(C("y"), (C("x") ^ 2.0d) + C('b') * "x" + 'c');

            Wording w = new Wording("Résolution du polynôme d'ordre 2", "Calcul de la réciproque");
            w.Content.Add(new Texte("Soit l'équation d'un polynôme d'ordre 2 en fonction de l'inconnu {x}", true),
                          function);

            function.Let("b", 2.0d);
            function.Let("c", 1.0d);
            function.Let("x", 0.0d);
            string res = function.RightOperand.Calculate(true);
            w.Add(new Exercice(1, "Calculez {y} pour {x=0}", "Choisissez {b=2} et {c=1}",
                               true, new Answer("Si {x=0} alors {y=c}", true, new SequenceProof(new Equal(C("y"), (C(0.0d) ^ 2.0d) + C(2.0d) * 0.0d + 1.0d),
                                                                                                new Texte("{y=" + res + "}", true)))));

            function.Let("x", 1.0d);
            res = function.RightOperand.Calculate(true);
            w.Add(new Exercice(2, "Calculez {y} pour {x=1}", "Choisissez {b=2} et {c=1}",
                               true, new Answer("Si {x=1} alors {" + new Equal(C("y"), (C(1.0d) ^ 2.0d) + C('b') * 1.0d + 'c').ToTex() + "}", true,
                                                new SequenceProof(new Equal(C("y"), (C(1.0d) ^ 2.0d) + C(2.0d) * 1.0d + 1.0d),
                                                                  new Texte("{y=" + res + "}", true)))));

            function.Let("b", 3.0d);
            function.Let("c", 2.0d);
            res = function.RightOperand.Calculate(true);
            w.Add(new Exercice(3, "Calculez {y} pour {x=1}", "Choisissez {b=3} et {c=2}",
                               true, new Answer("Si {x=1} alors {" + new Equal(C("y"), (C(1.0d) ^ 2.0d) + C('b') * 1.0d + 'c').ToTex() + "}", true,
                                                new SequenceProof(new Equal(C("y"), (C(1.0d) ^ 2.0d) + C(3.0d) * 1.0d + 2.0d),
                                                                  new Texte("{y=" + res + "}", true)))));

            Graphique g = Graphique.Create(() => {
                Coordinates[] bornes = new Coordinates[2];
                bornes[0] = new Coordinates(-5.0d, -5.0d);
                bornes[1] = new Coordinates(5.0d, 5.0d);
                Maths.Vector v = new Maths.Vector(bornes[0], bornes[1]);
                Coordinates s = new Coordinates(0.1d, 0.1d);
                MovingCoordinates mc = new MovingCoordinates(v, s);
                DistributedTracer2D d = new DistributedTracer2D(mc, 4, 4, 6, new Size(1.0d, 1.0d));
                Arithmetic f = new Sum(C("x") ^ 2.0d, C(3.0d) * "x", C(2.0d)); 
                d.SetFunction(f);
                return d;
            });

            w.Add(new Exercice(4, "Représentez graphiquement la courbe parabolique du polynôme d'ordre 2", "Choisissez {b=3} et {c=2}",
                               true, new Answer("Appuyez sur le bouton pour voir la courbe", new SequenceProof(g))));

            w.Add(new Exercice(5, "Exprimer la différence entre {y} et {y_0}", "Obtenir un produit", true,
                               new Answer("Expression de la différentielle {y}", true,
                                          new SequenceProof(new Equal(C("y") - C("y_0"),
                                                                      new Soustraction(new Sum(C("x") ^ 2.0d, C(3.0d) * "x", C(2.0d)),
                                                                                       new Sum(C("x_0") ^ 2.0d, C(3.0d) * "x_0", C(2.0d)))),
                                                            new Texte("Factorisation de {x - x_0}", true),
                                                            new Equal(C("y") - C("y_0"),
                                                                      new Product(C("x") - "x_0", C("x") + "x_0" + 3.0d))
                                                            ))
                                         ));

            w.ToDocument(fd);
            Button but = new Button();
            but.Name = "GoBack";
            but.Content = "Retour";
            but.Click += Button_Click;
            SetButtonStyle(but);
            fd.Blocks.Add(new BlockUIContainer(but));

            return fd;
        }
    }
}
