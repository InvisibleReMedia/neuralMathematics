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
            Button te = new Button();
            te.Name = "Test";
            te.Content = "Tests";
            te.Click += Button_Click;
            SetButtonStyle(te);
            tc = new TableCell(new BlockUIContainer(te));
            tc.ColumnSpan = 2;
            tr.Cells.Add(tc);
            trg.Rows.Add(tr);

            tr = new TableRow();
            Button tn = new Button();
            tn.Name = "Num";
            tn.Content = "Résolution numérique du polynôme 2";
            tn.Click += Button_Click;
            SetButtonStyle(tn);
            tc = new TableCell(new BlockUIContainer(tn));
            tc.ColumnSpan = 2;
            tr.Cells.Add(tc);
            trg.Rows.Add(tr);

            tr = new TableRow();
            Button t1 = new Button();
            t1.Name = "T2";
            t1.Content = "Test de résolution du polynôme d'ordre 2";
            t1.Click += Button_Click;
            SetButtonStyle(t1);
            Button t2 = new Button();
            t2.Name = "T3";
            t2.Content = "Test de résolution du polynôme d'ordre 3";
            t2.Click += Button_Click;
            SetButtonStyle(t2);
            tc = new TableCell(new BlockUIContainer(t1));
            tr.Cells.Add(tc);
            tc = new TableCell(new BlockUIContainer(t2));
            tr.Cells.Add(tc);
            trg.Rows.Add(tr);

            tr = new TableRow();
            Button tf = new Button();
            tf.Name = "F";
            tf.Content = "Différence de polynômes";
            tf.Click += Button_Click;
            SetButtonStyle(tf);
            tc = new TableCell(new BlockUIContainer(tf));
            tc.ColumnSpan = 2;
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

            TextBlock errorText = new TextBlock();
            errorText.Foreground = new SolidColorBrush(Colors.Red);
            fd.Blocks.Add(new BlockUIContainer(errorText));

            // variables
            Dictionary<string, IArithmetic> variables = new Dictionary<string, IArithmetic>();

            Arithmetic.EventAddVariable += new EventHandler<KeyValuePair<string, IArithmetic>>((o, e) =>
            {
                if (e.Value != null)
                {
                    if (variables.ContainsKey(e.Key))
                    {
                        variables[e.Key] = e.Value;
                    }
                    else
                    {
                        variables.Add(e.Key, e.Value);
                    }
                }
                else
                {
                    if (variables.ContainsKey(e.Key))
                        variables.Remove(e.Key);
                }
            });
            Arithmetic.EventGetVariable = new Func<string, IArithmetic>((s) =>
            {
                if (variables.ContainsKey(s)) return variables[s];
                else return null;
            });

            Arithmetic.EventError += new EventHandler<OverflowException>((o, e) =>
            {
                errorText.Text = e.Message;
            });


            Equal function = new Equal(C("y"), (C("x") ^ 2.0d) + C('b') * "x" + 'c');
            Wording w = new Wording("Résolution du polynôme d'ordre 2", "Calcul de la réciproque");
            w.Content.Add(new Texte("Soit l'équation d'un polynôme d'ordre 2 en fonction de l'inconnu {x}", true),
                          function);

            {


                function.Let("b", 2.0d);
                function.Let("c", 1.0d);
                function.Let("x", C(0.0d));
                double res = function.RightOperand.Converting().Compute().ToDouble();
                w.Add(new Exercice(1, "Calculez {y} pour {x=0}", "Choisissez {b=2} et {c=1}",
                                   true, new Answer("Si {x=0} alors {y=c}", true, new SequenceProof(new Equal(C("y"), (C(0.0d) ^ 2.0d) + C(2.0d) * 0.0d + 1.0d),
                                                                                                    new Texte("{y=" + res + "}", true)))));

                function.Let("x", C(1.0d));
                res = function.RightOperand.Converting().Compute().ToDouble();
                w.Add(new Exercice(2, "Calculez {y} pour {x=1}", "Choisissez {b=2} et {c=1}",
                                   true, new Answer("Si {x=1} alors {" + new Equal(C("y"), (C(1.0d) ^ 2.0d) + C('b') * 1.0d + 'c').ToTex() + "}", true,
                                                    new SequenceProof(new Equal(C("y"), (C(1.0d) ^ 2.0d) + C(2.0d) * 1.0d + 1.0d),
                                                                      new Texte("{y=" + res + "}", true)))));

                function.Let("b", 3.0d);
                function.Let("c", 2.0d);
                res = function.RightOperand.Converting().Compute().ToDouble();
                w.Add(new Exercice(3, "Calculez {y} pour {x=1}", "Choisissez {b=3} et {c=2}",
                                   true, new Answer("Si {x=1} alors {" + new Equal(C("y"), (C(1.0d) ^ 2.0d) + C('b') * 1.0d + 'c').ToTex() + "}", true,
                                                    new SequenceProof(new Equal(C("y"), (C(1.0d) ^ 2.0d) + C(3.0d) * 1.0d + 2.0d),
                                                                      new Texte("{y=" + res + "}", true)))));

            }

            Graphique g = Graphique.Create(() =>
            {
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

            Arithmetic yPrime = new Equal(C("y'"), C(2.0d) * C("x_0") + C('b'));

            {
                w.Add(new Exercice(4, "Représentez graphiquement la courbe parabolique du polynôme d'ordre 2", "Choisissez {b=3} et {c=2}",
                                   true, new Answer("Appuyez sur le bouton pour voir la courbe", new SequenceProof(g))));

                Arithmetic diffY = new Equal(C("y") - C("y_0"),
                                             new Product(C("x") - "x_0", (C("x") - "x_0") + C("y'")));
                Arithmetic diffY2 = new Equal(C("y") - C("y_0"),
                                             new Product(C("x") - "x_0", new Sum(C("x") - "x_0", C(2.0d) * "x_0", C('b'))));
                Arithmetic diffY3 = new Equal(C("y") - C("y_0"),
                                             new Product(C("dx"), new Sum(C("dx"), C(2.0d) * "x_0", C('b'))));
                Arithmetic diffY4 = new Equal(C("dy"),
                                             new Product(C("dx"), new Sum(C("dx"), C("y'"))));

                w.Add(new Exercice(5, "Exprimer la différence entre {y} et {y_0} quelque soit les coefficients {b} et {c}", "Obtenir un produit et utiliser la dérivée {y'=2x+b}", true,
                                   new Answer("Expression de la différentielle {y}", true,
                                              new SequenceProof(new Equal(C("y") - C("y_0"),
                                                                          new Soustraction(new Sum(C("x") ^ 2.0d, C('b') * "x", C('c')),
                                                                                           new Sum(C("x_0") ^ 2.0d, C('b') * "x_0", C('c')))),
                                                                new Texte("Factorisation de {x - x_0}", true),
                                                                diffY,
                                                                new Texte("Obtenir {dx}", true),
                                                                diffY2,
                                                                new Texte("Exprimez {dx} dans la différentielle", true),
                                                                diffY3,
                                                                new Texte("Identifiez la fonction dérivée {y' = 2 * x_0 + b}", true),
                                                                diffY4
                                                                ))
                                             ));

            }

            Arithmetic dx1 = new Equal(C("dx_+"), C("x_1") - C("x_0"));
            Arithmetic dx2 = new Equal(C("dx_-"), C("x_2") - C("x_0"));
            Arithmetic x0 = new Equal(C("x_0"), C(1.0));
            Arithmetic y0 = new Equal(C("y_0"), new Sum(C("x_0") ^ 2.0, C('b') * C("x_0"), C('c')));
            Arithmetic dy = new Equal(C("dy"), new Multiplication(C("dx"), new Addition(C("dx"), C("y'"))));
            Arithmetic diffy = new Equal(C("dy"), C("y") - C("y_0"));

            y0.Let("b", 2.0d);
            y0.Let("c", 1.0d);
            x0.Let("x_0", 1.0d);
            y0.Let("y_0", y0.RightOperand);
            dy.Let("y", 4.0);
            dy.Let("dy", diffy.RightOperand);
            yPrime.Let("y'", yPrime.RightOperand);

            {


                w.Add(new Exercice(6, "Calculez {dy} en fonction de {dx} quand {x_0=1} et exprimez {dx + y'}. Conclure", "Choisissez {b=2} et {c=1}", true,
                                   new Answer("Vérification de l'équation différentielle",
                                              new SequenceProof(new Texte("Le résultat est {dy = " + dy.RightOperand.Converting().AsRepresented("tex") + "}", true, '(', ')'),
                                                                new Texte("Lorsque {y - y_0 = 0} alors {x=x_0=1} ou {dx+y'=x+4}", true),
                                                                new Texte(@"Lorsque {y - y_0 \not= 0} alors {dx} et {dx+y'} sont non nuls", true),
                                                                new Texte("Si l'équation différentielle est nulle alors, elle permet de déterminer l'autre solution connaissant l'une", true)))));

                w.Add(new Exercice(7, @"Ajoutez un point médian d'abscisse {\bar{x}}", "Obtenir deux distances", true,
                                   new Answer("Ajout d'un point médian",
                                              new SequenceProof(new Equal(C("dx"), new Equal(C("x") - "x_0", C("x") - @"\bar{x}" + @"\bar{x}" - "x_0")),
                                                                new Texte("Le point intermédiaire donne la somme de deux distances {dx_1} et {dx_2}", true)
                                              ))));

                w.Add(new Exercice(8, "Incorporez l'équation de {dx} dans l'équation différentielle", @"Cherchez une équation de {\bar{x}}", true,
                                   new Answer("Calculs algébriques",
                                              new SequenceProof(new Equal(C("dy"), new Product(C("dx"), new Sum(C("dx"), C("y'")))),
                                                                new Equal(C("dy"), new Sum(C("dx") ^ 2, C("dx") * C("y'"))),
                                                                new Equal(C("dy"), new Sum((C("dx_1") + "dx_2") ^ 2, (C("dx_1") + "dx_2") * C("y'"))),
                                                                new Equal(C("dy"), new Sum((C("dx_1") ^ 2.0d), C(2.0d) * C("dx_1") * C("dx_2"), C("dx_2") ^ 2.0d, C("dx_1") * C("y'"), C("dx_2") * C("y'"))),
                                                                new Texte("Le terme {dx_2} est un terme constant", true),
                                                                new Equal(C("dy"), new Sum((C("dx_1") ^ 2.0d), C(2.0d) * C("dx_1") * C("dx_2"), C("dx_2") ^ 2.0d, C("dx_1") * C("y'"), C("dx_2") * C("y'"))),
                                                                new Texte("Factorisons {dx_1}", true),
                                                                new Equal(C("dy"), new Sum((C("dx_1") ^ 2.0d), (C(2.0d) * C("dx_2") + C("y'")) * "dx_1", C("dx_2") ^ 2.0d, C("dx_2") * C("y'"))),
                                                                new Texte("Si {" + (C(2.0d) * C("dx_2") + C("y'")).ToTex() + " = 0} alors le terme {dx_1^2} est isolé", true),
                                                                new Equal(C("dx_2"), new Negative(C("y'") / C(2.0d))),
                                                                new Texte("Il en résulte l'équation algébrique solution comportant une racine carrée entièrement déterminée"),
                                                                new Equal(C("dx_1"), new Root(C("dy") - new Addition(C("dx_2") ^ 2.0d, C("dx_2") * C("y'")), C(2.0d))),
                                                                new Texte("Ce qui se simplifie par {" + new Equal(C("dx_1"), new Root(new Addition(C("dy"), new Division(new Power(C("y'"), C(2.0d)), C(4.0d))), C(2.0d))).ToTex() + "}", true),
                                                                new Texte("Conclusion : {dx = dx_1 + dx_2} = {" + new Soustraction(new Root(new Addition(C("dy"), new Division(new Power(C("y'"), C(2.0d)), C(4.0d))), C(2.0d)), C("y'") / C(2.0d)).ToTex() + "}", true)

                                              ))));

            }


            Arithmetic solEqDX = new Soustraction(new Root(new Addition(C("dy"), new Division(new Power(C("y'"), C(2.0d)), C(4.0d))), C(2.0d)), C("y'") / C(2.0d));

            w.Add(new Exercice(9, "Démontrez que l'équation {dx} ci-dessus est correcte.", "Choisissez des valeurs pour {x_0} et {y}", true,
                               new Answer("Calculs", new SequenceProof(
                                            new Texte("Valeurs {b=2}, {c=1}", true),
                                            new Texte("Valeur {x_0=1}", true),
                                            new Texte("Calcul de {y_0}", true),
                                            new Texte("{" + y0.RightOperand.Converting().Compute().AsRepresented("tex") + "}", true),
                                            new Texte("Résultat pour {dx} = {" + solEqDX.ConvertingOne().AsRepresented("tex") + "} = {" + solEqDX.Converting().AsRepresented("tex") + "}", true),
                                            new Texte("D'où les valeurs de {dx} = {" + solEqDX.Converting().Compute().AsRepresented("tex") + "} pour {y=4}", true)
                                    ))));


            w.ToDocument(fd);
            Button but = new Button();
            but.Name = "GoBack";
            but.Content = "Retour";
            but.Click += Button_Click;
            SetButtonStyle(but);
            fd.Blocks.Add(new BlockUIContainer(but));

            return fd;
        }

        /// <summary>
        /// Test resolution polynôme 2
        /// </summary>
        /// <returns>document</returns>
        public static FlowDocument TestPolynome2()
        {
            FlowDocument fd = new FlowDocument();

            TextBlock errorText = new TextBlock();
            errorText.Foreground = new SolidColorBrush(Colors.Red);
            fd.Blocks.Add(new BlockUIContainer(errorText));

            // variables
            Dictionary<string, IArithmetic> variables = new Dictionary<string, IArithmetic>();

            Arithmetic.EventAddVariable += new EventHandler<KeyValuePair<string, IArithmetic>>((o, e) =>
            {
                if (e.Value != null)
                {
                    if (variables.ContainsKey(e.Key))
                    {
                        variables[e.Key] = e.Value;
                    }
                    else
                    {
                        variables.Add(e.Key, e.Value);
                    }
                }
                else
                {
                    if (variables.ContainsKey(e.Key))
                        variables.Remove(e.Key);
                }
            });
            Arithmetic.EventGetVariable = new Func<string, IArithmetic>((s) =>
            {
                if (variables.ContainsKey(s)) return variables[s];
                else return null;
            });

            Arithmetic.EventError += new EventHandler<OverflowException>((o, e) =>
            {
                errorText.Text = e.Message;
            });

            Equal function = new Equal(C("y"), (C("x") ^ 2.0d) + C('b') * "x" + 'c');
            Wording w = new Wording("Résolution du polynôme d'ordre 2", "Calcul de la réciproque");
            w.Content.Add(new Texte("Soit l'équation d'un polynôme d'ordre 2 en fonction de l'inconnu {x}", true),
                          function);

            Arithmetic yPrime = new Equal(C("y'"), C(2.0d) * C("x_0") + C('b'));
            Arithmetic dx1 = new Equal(C("dx_+"), C("x_1") - C("x_0"));
            Arithmetic dx2 = new Equal(C("dx_-"), C("x_2") - C("x_0"));
            Arithmetic x0 = new Equal(C("x_0"), C(1.0));
            Arithmetic y0 = new Equal(C("y_0"), new Sum(C("x_0") ^ 2.0, C('b') * C("x_0"), C('c')));
            Arithmetic dy = new Equal(C("dy"), C("dx") * (C("dy") + C("y'")));
            Arithmetic diffy = new Equal(C("dy"), C("y") - C("y_0"));

            y0.Let("b", 2.0d);
            y0.Let("c", 1.0d);
            x0.Let("x_0", 1.0d);
            y0.Let("y_0", y0.RightOperand);
            dy.Let("y", 4.0);
            dy.Let("dy", diffy.RightOperand);
            yPrime.Let("y'", yPrime.RightOperand);


            DockPanel spB = new DockPanel();
            TextBlock tbB = new TextBlock();
            tbB.Text = "Coefficient B";
            TextBox tCoeffB = new TextBox();
            tCoeffB.Name = "textBox_b";
            tCoeffB.Text = "2";
            spB.Children.Add(tbB);
            spB.Children.Add(tCoeffB);
            fd.Blocks.Add(new BlockUIContainer(spB));

            DockPanel spC = new DockPanel();
            TextBlock tbC = new TextBlock();
            tbC.Text = "Coefficient C";
            TextBox tCoeffC = new TextBox();
            tCoeffC.Name = "textBox_c";
            tCoeffC.Text = "1";
            spC.Children.Add(tbC);
            spC.Children.Add(tCoeffC);
            fd.Blocks.Add(new BlockUIContainer(spC));

            DockPanel spx0 = new DockPanel();
            TextBlock tbx0 = new TextBlock();
            tbx0.Text = "Valeur X0";
            TextBox tCoeffx0 = new TextBox();
            tCoeffx0.Name = "textBox_x0";
            tCoeffx0.Text = "1";
            spx0.Children.Add(tbx0);
            spx0.Children.Add(tCoeffx0);
            fd.Blocks.Add(new BlockUIContainer(spx0));

            DockPanel spy = new DockPanel();
            TextBlock tby = new TextBlock();
            tby.Text = "Valeur Y";
            TextBox tCoeffY = new TextBox();
            tCoeffY.Name = "textBox_y";
            tCoeffY.Text = "4";
            spy.Children.Add(tby);
            spy.Children.Add(tCoeffY);
            fd.Blocks.Add(new BlockUIContainer(spy));

            WrapPanel panel = new WrapPanel();
            FlowDocumentScrollViewer scrollViewer = new FlowDocumentScrollViewer();
            panel.Children.Add(scrollViewer);

            fd.Blocks.Add(new BlockUIContainer(panel));

            Button btCalc = new Button();
            btCalc.Name = "btCalc";
            btCalc.Content = "Recalculer";
            btCalc.Click += new RoutedEventHandler((o, e) =>
            {
                Arithmetic solEqDXPlus = new Soustraction(new Root(new Addition(C("dy"), new Division(new Power(C("y'"), C(2.0d)), C(4.0d))), C(2.0d)), C("y'") / C(2.0d));
                Arithmetic solEqDXMoins = new Soustraction(new Negative(new Root(new Addition(C("dy"), new Division(new Power(C("y'"), C(2.0d)), C(4.0d))), C(2.0d))), C("y'") / C(2.0d));

                Arithmetic solEqx1 = new Addition(C("x_0"), solEqDXPlus);
                Arithmetic solEqx2 = new Addition(C("x_0"), solEqDXMoins);
                Arithmetic valDY = new Soustraction(C("y"), C("y_0"));
                errorText.Text = ""; // reinitialisation du texte d'erreur
                function.Let("b", Convert.ToDouble(tCoeffB.Text));
                function.Let("c", Convert.ToDouble(tCoeffC.Text));
                y0.Let("x_0", Convert.ToDouble(tCoeffx0.Text));
                valDY.Let("y_0", y0.RightOperand);
                valDY.Let("y", Double.Parse(tCoeffY.Text));
                solEqDXPlus.Let("dy", valDY);
                Wording w2 = new Wording("Application", "Modifiez les zones de saisie et cliquer sur le bouton Recalculer",
                                         new Exercice(1, "Calculs", "", new Answer("", true, new SequenceProof(
                                             new Texte("{dx_+} = {" + solEqDXPlus.ConvertingOne().AsRepresented("tex") + "}", true),
                                             new Texte("{dx_-} = {" + solEqDXMoins.ConvertingOne().AsRepresented("tex") + "}", true),
                                             new Texte("{x_1} = {" + solEqx1.Converting().Compute().ToDouble() + "}", true),
                                             new Texte("{x_2} = {" + solEqx2.Converting().Compute().ToDouble() + "}", true)
                ))));
                FlowDocument f2 = new FlowDocument();
                scrollViewer.Document = null;
                w2.ToDocument(f2);
                scrollViewer.Document = f2;
                scrollViewer.UpdateLayout();
            });
            fd.Blocks.Add(new BlockUIContainer(btCalc));

            w.ToDocument(fd);
            Button but = new Button();
            but.Name = "GoBack";
            but.Content = "Retour";
            but.Click += Button_Click;
            SetButtonStyle(but);
            fd.Blocks.Add(new BlockUIContainer(but));

            return fd;
        }

        /// <summary>
        /// Test resolution numérique polynôme 2
        /// </summary>
        /// <returns>document</returns>
        public static FlowDocument TestNumericalPolynome2()
        {
            FlowDocument fd = new FlowDocument();

            TextBlock errorText = new TextBlock();
            errorText.Foreground = new SolidColorBrush(Colors.Red);
            fd.Blocks.Add(new BlockUIContainer(errorText));

            // variables
            Dictionary<string, IArithmetic> variables = new Dictionary<string, IArithmetic>();

            Arithmetic.EventAddVariable += new EventHandler<KeyValuePair<string, IArithmetic>>((o, e) =>
            {
                if (e.Value != null)
                {
                    if (variables.ContainsKey(e.Key))
                    {
                        variables[e.Key] = e.Value;
                    }
                    else
                    {
                        variables.Add(e.Key, e.Value);
                    }
                }
                else
                {
                    if (variables.ContainsKey(e.Key))
                        variables.Remove(e.Key);
                }
            });
            Arithmetic.EventGetVariable = new Func<string, IArithmetic>((s) =>
            {
                if (variables.ContainsKey(s)) return variables[s];
                else return null;
            });

            Arithmetic.EventError += new EventHandler<OverflowException>((o, e) =>
            {
                errorText.Text = e.Message;
            });

            Equal function = new Equal(C("y"), (C("x") ^ 2.0d) + C('b') * "x" + 'c');
            Wording w = new Wording("Résolution du polynôme d'ordre 2", "Calcul de la réciproque");
            w.Content.Add(new Texte("Soit l'équation d'un polynôme d'ordre 2 en fonction de l'inconnu {x}", true),
                          function);

            Arithmetic yPrime = new Equal(C("y'"), C(2.0d) * C("x_0") + C('b'));
            Arithmetic dx1 = new Equal(C("dx_+"), C("x_1") - C("x_0"));
            Arithmetic dx2 = new Equal(C("dx_-"), C("x_2") - C("x_0"));
            Arithmetic x0 = new Equal(C("x_0"), C(1.0));
            Arithmetic y0 = new Equal(C("y_0"), new Sum(C("x_0") ^ 2.0, C('b') * C("x_0"), C('c')));
            Arithmetic dy = new Equal(C("dy"), C("dx") * (C("dy") + C("y'")));
            Arithmetic diffy = new Equal(C("dy"), C("y") - C("y_0"));

            y0.Let("b", 2.0d);
            y0.Let("c", 1.0d);
            x0.Let("x_0", 1.0d);
            y0.Let("y_0", y0.RightOperand);
            dy.Let("y", 4.0);
            dy.Let("dy", diffy.RightOperand);
            yPrime.Let("y'", yPrime.RightOperand);


            DockPanel spB = new DockPanel();
            TextBlock tbB = new TextBlock();
            tbB.Text = "Coefficient B";
            TextBox tCoeffB = new TextBox();
            tCoeffB.Name = "textBox_b";
            tCoeffB.Text = "2";
            spB.Children.Add(tbB);
            spB.Children.Add(tCoeffB);
            fd.Blocks.Add(new BlockUIContainer(spB));

            DockPanel spC = new DockPanel();
            TextBlock tbC = new TextBlock();
            tbC.Text = "Coefficient C";
            TextBox tCoeffC = new TextBox();
            tCoeffC.Name = "textBox_c";
            tCoeffC.Text = "1";
            spC.Children.Add(tbC);
            spC.Children.Add(tCoeffC);
            fd.Blocks.Add(new BlockUIContainer(spC));

            DockPanel spy = new DockPanel();
            TextBlock tby = new TextBlock();
            tby.Text = "Valeur Y";
            TextBox tCoeffY = new TextBox();
            tCoeffY.Name = "textBox_y";
            tCoeffY.Text = "4";
            spy.Children.Add(tby);
            spy.Children.Add(tCoeffY);
            fd.Blocks.Add(new BlockUIContainer(spy));

            WrapPanel panel = new WrapPanel();
            FlowDocumentScrollViewer scrollViewer = new FlowDocumentScrollViewer();
            panel.Children.Add(scrollViewer);

            fd.Blocks.Add(new BlockUIContainer(panel));

            Button btCalc = new Button();
            btCalc.Name = "btCalc";
            btCalc.Content = "Recalculer";
            btCalc.Click += new RoutedEventHandler((o, e) =>
            {
                Polynome2 p = new Polynome2();
                double b, c, y, x;
                b = Convert.ToDouble(tCoeffB.Text);
                c = Convert.ToDouble(tCoeffC.Text);
                y = Convert.ToDouble(tCoeffY.Text);
                p.searchNumerical(b, c, y, 17, out x);
                function.Let("b", b);
                function.Let("c", c);
                function.Let("x", x);
                Wording w2 = new Wording("Application", "Modifiez les zones de saisie et cliquer sur le bouton Recalculer",
                                         new Exercice(1, "Calculs", "", new Answer("", true, new SequenceProof(
                                                    new Texte("Valeur calculée numériquement {x = " + x.ToString() + "}", true),
                                                    new Texte("Preuve : {f(" + x.ToString() + ") = " + function.RightOperand.Converting().ToDouble() + "}", true)
                ))));
                FlowDocument f2 = new FlowDocument();
                scrollViewer.Document = null;
                w2.ToDocument(f2);
                scrollViewer.Document = f2;
                scrollViewer.UpdateLayout();
            });
            fd.Blocks.Add(new BlockUIContainer(btCalc));

            w.ToDocument(fd);
            Button but = new Button();
            but.Name = "GoBack";
            but.Content = "Retour";
            but.Click += Button_Click;
            SetButtonStyle(but);
            fd.Blocks.Add(new BlockUIContainer(but));

            return fd;
        }

        /// <summary>
        /// Solve polynôme 3
        /// </summary>
        /// <returns>document</returns>
        public static FlowDocument SolvePolynome3()
        {
            FlowDocument fd = new FlowDocument();

            Equal function = new Equal(C("y"), (C("x") ^ 3.0d) + C('b') * (C("x") ^ 2.0d) + C('c') * "x" + C('d'));

            Wording w = new Wording("Résolution du polynôme d'ordre 3", "Calcul de la réciproque");
            w.Content.Add(new Texte("Soit l'équation d'un polynôme d'ordre 3 en fonction de l'inconnu {x}", true),
                          function);

            function.Let("b", 3.0d);
            function.Let("c", 3.0d);
            function.Let("d", 1.0d);
            function.Let("x", 0.0d);
            double res = function.RightOperand.Compute().ToDouble();
            w.Add(new Exercice(1, "Calculez {y} pour {x=0}", "Choisissez {b=3}, {c=3} et {d=1}",
                               true, new Answer("Si {x=0} alors {y=d}", true, new SequenceProof(new Equal(C("y"), (C(0.0d) ^ 3.0d) + C('b') * (C(0.0d) ^ 2.0d) + C('c') * 0.0d + C('d')),
                                                                                                new Texte("{y=" + res + "}", true)))));

            function.Let("x", 1.0d);
            res = function.RightOperand.Compute().ToDouble();
            w.Add(new Exercice(2, "Calculez {y} pour {x=1}", "Choisissez {b=3}, {c=3} et {d=1}",
                               true, new Answer("Si {x=1} alors {" + new Equal(C("y"), (C(1.0d) ^ 3.0d) + C('b') * (C(1.0d) ^ 2.0d) + C('c') * 1.0d + C('d')).ToTex() + "}", true,
                                                new SequenceProof(new Equal(C("y"), (C(1.0d) ^ 3.0d) + 3 * (C(1.0d) ^ 2.0d) + 3 * 1.0d + 1),
                                                                  new Texte("{y=" + res + "}", true)))));

            Graphique g = Graphique.Create(() =>
            {
                Coordinates[] bornes = new Coordinates[2];
                bornes[0] = new Coordinates(-100.0d, -100.0d);
                bornes[1] = new Coordinates(100.0d, 100.0d);
                Maths.Vector v = new Maths.Vector(bornes[0], bornes[1]);
                Coordinates s = new Coordinates(0.01d, 0.01d);
                MovingCoordinates mc = new MovingCoordinates(v, s);
                DistributedTracer2D d = new DistributedTracer2D(mc, 4, 4, 6, new Size(0.002d, 0.002d)); // (distance aux bornes - 20) * ratio points (0.01)
                Arithmetic f = new Sum(C("x") ^ 3.0d, C(3.0d) * C("x") ^ 2.0, C(2.0d) * "x", C(1.0d));
                d.SetFunction(f);
                return d;
            });

            w.Add(new Exercice(4, "Représentez graphiquement la courbe parabolique du polynôme d'ordre 3", "Choisissez {b=3}, {c=2} et {d=1}",
                               true, new Answer("Appuyez sur le bouton pour voir la courbe", new SequenceProof(g))));

            w.Add(new Exercice(5, "Déterminez les solutions de {x} pour l'équation suivante {" + new Equal(C(0.0d), new Multiplication(new Power(C("x"),2.0d) + C('u')*C("x"), C("x") + C('v'))).ToTex() + "}", "Choisissez {u=2} et {v=1}", true,
                               new Answer("Il suffit de résoudre les deux équations",
                                          new SequenceProof(new Equal(C("x"), new Addition(new Power(C("x"),2.0d), C('u')*C("x"))),
                                                            new Equal(C("x"), new Equal(new Soustraction(new Root(new Division(new Power(C('u'), C(2.0d)), C(4.0d)), C(2.0d)), C('u') / 2), C(0.0d))),
                                                            new Equal(C("x"), new Equal(new Soustraction(new Negative(new Root(new Division(new Power(C('u'), C(2.0d)), C(4.0d)), C(2.0d))), C('u') / 2), new Negative(C('u')))),
                                                            new Equal(C("x"), new Addition(C("x"), C('v'))),
                                                            new Equal(C("x"), new Negative(C('v')))))));

            w.Add(new Exercice(6, "Déterminez les solutions de {u} et {v} pour l'équation suivante {" + new Equal(new Sum(C("x") ^ C(3.0d), C('b') * C("x")^2, C('c')*C("x")), new Multiplication(new Power(C("x"), 2.0d) + C('u') * C("x"), C("x") + C('v'))).ToTex() + "}", "Isolez un système d'équations à deux inconnues {u} et {v}", true,
                               new Answer("Il suffit de résoudre le système d'équations",
                                          new SequenceProof(new Equal(C('u') + C('v'), C('b')),
                                                            new Equal(C('u') * C('v'), C('c')),
                                                            new Texte("Pour résoudre, il faut exprimer {v} en fonction de {u}", true),
                                                            new Equal(C('v'), C('c') / C('u')),
                                                            new Equal(C('u') + C('c')/C('u'), C('b')),
                                                            new Equal(new Addition(new Soustraction(C('u') ^ C(2.0d), C('b') * C('u')), C('c')), C(0.0d)),
                                                            new Equal(C('u'), new Addition(new Root(new Soustraction(new Division(new Power(C('b'), C(2.0d)), C(4.0d)), C('c')), C(2.0d)), C('b') / C(2.0d))),
                                                            new Equal(C('u'), new Addition(new Negative(new Root(new Soustraction(new Division(new Power(C('b'), C(2.0d)), C(4.0d)), C('c')), C(2.0d))), C('b') / C(2.0d))),
                                                            new Equal(C('v'), new Division(C('c'), new Addition(new Root(new Soustraction(new Division(new Power(C('b'), C(2.0d)), C(4.0d)), C('c')), C(2.0d)), C('b') / C(2.0d)))),
                                                            new Equal(C('v'), new Division(C('c'), new Addition(new Negative(new Root(new Soustraction(new Division(new Power(C('b'), C(2.0d)), C(4.0d)), C('c')), C(2.0d))), C('b') / C(2.0d)))),
                                                            new Texte("Au total, {u} et {v} possèdent deux valeurs possibles. Mais pour chaque valeur de {u}, il existe une unique valeur correspondante pour {v}", true),
                                                            new Texte("L'ensemble forme finalement deux solutions dont les deux autres sont commutatives (Exemple: {(2,1), (1,2)} sont deux solutions commutatives)"),
                                                            new Texte("Si l'on prouve qu'il existe des cas non commutatifs alors on aura prouvé qu'il existe plus que 3 solutions dans un polynôme d'ordre 3")
                                          ))));
                                                            

            w.ToDocument(fd);
            Button but = new Button();
            but.Name = "GoBack";
            but.Content = "Retour";
            but.Click += Button_Click;
            SetButtonStyle(but);
            fd.Blocks.Add(new BlockUIContainer(but));

            return fd;
        }

        /// <summary>
        /// Tests
        /// </summary>
        /// <returns></returns>
        public static FlowDocument Tests()
        {
            FlowDocument fd = new FlowDocument();

            TextBlock errorText = new TextBlock();
            errorText.Foreground = new SolidColorBrush(Colors.Red);
            fd.Blocks.Add(new BlockUIContainer(errorText));

            // variables
            Dictionary<string, IArithmetic> variables = new Dictionary<string, IArithmetic>();

            Arithmetic.EventAddVariable += new EventHandler<KeyValuePair<string, IArithmetic>>((o, e) =>
            {
                if (e.Value != null)
                {
                    if (variables.ContainsKey(e.Key))
                    {
                        variables[e.Key] = e.Value;
                    }
                    else
                    {
                        variables.Add(e.Key, e.Value);
                    }
                }
                else
                {
                    if (variables.ContainsKey(e.Key))
                        variables.Remove(e.Key);
                }
            });
            Arithmetic.EventGetVariable = new Func<string, IArithmetic>((s) =>
            {
                if (variables.ContainsKey(s)) return variables[s];
                else return null;
            });
            Arithmetic.EventError += new EventHandler<OverflowException>((o, e) =>
            {
                errorText.Text = e.Message;
            });


            Polynome2 p = new Polynome2();

            string txt = string.Empty;
            foreach (string key in p.Functions.Keys)
            {
                txt += p.Functions[key].AsRepresented("tex") + "\n";
            }

            foreach (IArithmetic a in p.Formulas["Factorisation 1"])
            {
                txt += a.AsRepresented("tex") + "\n";
            }

            foreach (IArithmetic a in p.Formulas["Factorisation 2"])
            {
                txt += a.AsRepresented("tex") + "\n";
            }

            Polynome3 p3 = new Polynome3();

            // ensure variables
            p3.ComputeR1();

            txt += "{A=" + p3.Functions["A"].AsRepresented("tex") + "}";

            txt += "\nA=" + p3.ComputeA(27, 2, 2, 36, 3, 3, 1).ToString();

            txt += "\n{A=" + p3.Functions["A"].Converting().Compute().AsRepresented("tex") + "}";

            txt += "\n{U=" + p3.Functions["U"].AsRepresented("tex") + "}";

            txt += "\n{U=" + p3.Functions["U"].Converting().AsRepresented("tex") + "}";

            txt += "\n{R_1=" + p3.Functions["R_1"].AsRepresented("tex") + "}";

            txt += "\n{R_1=" + p3.Functions["R_1"].Converting().AsRepresented("tex") + "}";

            txt += "\nR_1=" + p3.ComputeR1();

            //txt += "\n{Sum=" + p3.Sum(0, 10, 1).AsRepresented("tex") + "}";

            //txt += "\n{Sum=" + p3.Sum(0, 10, 3).Compute().ToDouble() + "}";

            Polynome2 p2 = new Polynome2();

            txt += "\n{Sum=" + p2.Sum2(2, 2, 30) + "}";

            txt += "\n{Sum=" + p2.Sum3(2, 2, 63) + "}";

            Wording w = new Wording("Tests", 
                                    txt, true);

            w.ToDocument(fd);
            Button but = new Button();
            but.Name = "GoBack";
            but.Content = "Retour";
            but.Click += Button_Click;
            SetButtonStyle(but);
            fd.Blocks.Add(new BlockUIContainer(but));

            return fd;
        }

        public static FlowDocument Differential()
        {

            FlowDocument fd = new FlowDocument();

            TextBlock errorText = new TextBlock();
            errorText.Foreground = new SolidColorBrush(Colors.Red);
            fd.Blocks.Add(new BlockUIContainer(errorText));

            // variables
            Dictionary<string, IArithmetic> variables = new Dictionary<string, IArithmetic>();

            Arithmetic.EventAddVariable += new EventHandler<KeyValuePair<string, IArithmetic>>((o, e) =>
            {
                if (e.Value != null)
                {
                    if (variables.ContainsKey(e.Key))
                    {
                        variables[e.Key] = e.Value;
                    }
                    else
                    {
                        variables.Add(e.Key, e.Value);
                    }
                }
                else
                {
                    if (variables.ContainsKey(e.Key))
                        variables.Remove(e.Key);
                }
            });
            Arithmetic.EventGetVariable = new Func<string, IArithmetic>((s) =>
            {
                if (variables.ContainsKey(s)) return variables[s];
                else return null;
            });
            Arithmetic.EventError += new EventHandler<OverflowException>((o, e) =>
            {
                errorText.Text = e.Message;
            });

            Wording w = new Wording("Diffentielle des polynômes", "Différence entre des polynômes d'ordre différents",
                            new Exercice(1, "Exprimez le taux de variation d'une fonction quelconque", "Notion d'analyse",
                                new Answer("On calcule le rapport des différences en ordonnée et en abscisse",
                                    new SequenceProof(new Equal((@"\tau(F(X),X_0,\mu)").ToArithmetic(), (@"(F(X_0 + \mu) - F(X_0)) / \mu").ToArithmetic()))
                                                      
                                )
                            )
            );

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
