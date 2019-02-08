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
        /// Add an item into the menu
        /// </summary>
        /// <param name="id">button id</param>
        /// <param name="title">title</param>
        /// <param name="g">table</param>
        private static void AddItemStretchedMenu(string id, string title, TableRowGroup g)
        {
            TableRow tr = new TableRow();
            Button te = new Button();
            te.Name = id;
            te.Content = title;
            te.Click += Button_Click;
            SetButtonStyle(te);
            TableCell tc = new TableCell(new BlockUIContainer(te));
            tc.ColumnSpan = 2;
            tr.Cells.Add(tc);
            g.Rows.Add(tr);
        }

        /// <summary>
        /// Add an item into the menu
        /// </summary>
        /// <param name="id">button id</param>
        /// <param name="title">title</param>
        /// <param name="tr">table</param>
        private static void AddItemCellIntoMenu(string id, string title, TableRow tr)
        {
            Button te = new Button();
            te.Name = id;
            te.Content = title;
            te.Click += Button_Click;
            SetButtonStyle(te);
            TableCell tc = new TableCell(new BlockUIContainer(te));
            tr.Cells.Add(tc);
        }


        /// <summary>
        /// Fonction de menu
        /// </summary>
        /// <returns></returns>
        public static FlowDocument Menu()
        {
            Table t = new Table();
            TableRowGroup trg = new TableRowGroup();

            TableRow row = new TableRow();
            AddItemCellIntoMenu("P2", "Résolution du polynôme d'ordre 2", row);
            AddItemCellIntoMenu("P3", "Résolution du polynôme d'ordre 3", row);
            trg.Rows.Add(row);

            AddItemStretchedMenu("Test", "Tests", trg);
            AddItemStretchedMenu("Num", "Résolution numérique du polynôme 2", trg);

            AddItemStretchedMenu("Tab", "Tableau de calcul du polynôme 2", trg);

            row = new TableRow();
            AddItemCellIntoMenu("T2", "Test de résolution du polynôme d'ordre 2", row);
            AddItemCellIntoMenu("T3", "Test de résolution du polynôme d'ordre 3", row);
            trg.Rows.Add(row);

            row = new TableRow();
            AddItemCellIntoMenu("F", "Différence de polynômes", row);
            AddItemCellIntoMenu("TF3", "Différentielle du polynôme 3", row);
            trg.Rows.Add(row);

            AddItemStretchedMenu("Sum", "Nombre en base 10", trg);

            AddItemStretchedMenu("Close", "Quitter", trg);

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


            string[] textValues = new string[4];
            AddTextBox("textbox_b", "Coefficient B", "2", fd, (t) => { textValues[0] = t.Text; });
            AddTextBox("textbox_c", "Coefficient C", "1", fd, (t) => { textValues[1] = t.Text; });
            AddTextBox("textbox_x0", "Valeur X0", "1", fd, (t) => { textValues[2] = t.Text; });
            AddTextBox("textbox_y", "Valeur Y", "4", fd, (t) => { textValues[3] = t.Text; });

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
                function.Let("b", Convert.ToDouble(textValues[0]));
                function.Let("c", Convert.ToDouble(textValues[1]));
                y0.Let("x_0", Convert.ToDouble(textValues[2]));
                valDY.Let("y_0", y0.RightOperand);
                valDY.Let("y", Double.Parse(textValues[3]));
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


            string[] textValues = new string[3];
            AddTextBox("textbox_b", "Coefficient B", "2", fd, (t) => { textValues[0] = t.Text; });
            AddTextBox("textbox_c", "Coefficient C", "1", fd, (t) => { textValues[1] = t.Text; });
            AddTextBox("textbox_y", "Valeur Y", "4", fd, (t) => { textValues[2] = t.Text; });

            FlowDocumentScrollViewer scrollViewer = new FlowDocumentScrollViewer();
            Button btCalc = new Button();
            btCalc.Name = "btCalc";
            btCalc.Content = "Recalculer";
            btCalc.Click += new RoutedEventHandler((o, e) =>
            {
                errorText.Text = string.Empty;
                Polynome2 p = new Polynome2();
                double b, c, y, x, x2;
                b = Convert.ToDouble(textValues[0]);
                c = Convert.ToDouble(textValues[1]);
                y = Convert.ToDouble(textValues[2]);
                if (!p.searchNumerical(y, 20, out x, b, c))
                    Arithmetic.RaiseEventError(new OverflowException("Solution non trouvée"));
                function.Let("b", b);
                function.Let("c", c);
                function.Let("x", x);
                function.Let("x0", 0);
                x2 = p.Functions["Complementaire"].Converting().ToDouble();
                p.ComputeX1(y, 0);
                Wording w2 = new Wording("Application", "Modifiez les zones de saisie et cliquer sur le bouton Recalculer",
                                         new Exercice(1, "Calculs", "", new Answer("", true, new SequenceProof(
                                                    new Texte("Le point de départ de la recherche vaut {x_0 = " + ("-b/2").ToArithmetic().AsRepresented("tex") + "}", true),
                                                    new Texte("Valeur calculée numériquement {x = " + x.ToString() + "}", true),
                                                    new Texte("Preuve : {f(" + x.ToString() + ") = " + function.RightOperand.Converting().ToDouble() + "}", true),
                                                    new Texte("On sait également déterminer la seconde solution"),
                                                    new Equal("X_2".ToArithmetic(), p.Functions["Complementaire"]),
                                                    new Texte("Valeur calculée numériquement {x = " + x2.ToString() + "}", true),
                                                    new Texte("Preuve : {f(" + x2.ToString() + ") = " + p.ComputeY0(x2) + "}", true)))),
                                         new Exercice(2, "Calculs algébriques", "", new Answer("", true, new SequenceProof(
                                                    new Texte("Valeur calculée algébriquement {x = " + p.Functions["DX_1"].AsRepresented("tex") + "}", true),
                                                    new Texte("Le résultat vaut {x = " + p.Functions["DX_1"].Converting().AsRepresented("tex") + " = " + p.ComputeX1(y, 0).ToString() + "}", true),
                                                    new Texte("Preuve : {f(" + p.ComputeX1(y, 0).ToString() + ") = " + p.ComputeY0(p.ComputeX1(y, 0)) + "}", true),
                                                    new Texte("On sait également déterminer la seconde solution"),
                                                    new Texte("Le résultat vaut {x = " + p.Functions["DX_2"].Converting().AsRepresented("tex") + " = " + p.ComputeX2(y, 0).ToString() + "}", true),
                                                    new Texte("Preuve : {f(" + p.ComputeX2(y, 0).ToString() + ") = " + p.ComputeY0(p.ComputeX2(y, 0)) + "}", true)
                ))));
                FlowDocument f2 = new FlowDocument();
                scrollViewer.Document = null;
                w2.ToDocument(f2);
                scrollViewer.Document = f2;
                scrollViewer.UpdateLayout();
            });
            fd.Blocks.Add(new BlockUIContainer(btCalc));

            WrapPanel panel = new WrapPanel();
            panel.Children.Add(scrollViewer);

            fd.Blocks.Add(new BlockUIContainer(panel));


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

        /// <summary>
        /// Differential
        /// </summary>
        /// <returns>document</returns>
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
                            ),
                            new Exercice(2, "Exprimez la différence des ordonnées en fonction du taux de variation", @"Multipliez par {\mu}", true, 
                                new Answer(@"On multiplie le taux de variation {\tau} par {\mu}", true,
                                    new SequenceProof(new Equal((@"\mu * \tau(F(X),X_0,\mu)").ToArithmetic(), (@"F(X_0 + \mu) - F(X_0)").ToArithmetic()))
                                )
                            ),
                            new Exercice(3, @"Exprimez le taux de variation pour la fonction particulière {F(X)=X*H(X)+c} où {c} est un coefficient et {H(X)} une sous-fonction", "", true,
                                new Answer(@"La fonction est définie {F(X)=X*H(X)+c}", true,
                                    new SequenceProof(
                                        new Equal((@"\tau(F(X),X_0,\mu)").ToArithmetic(), (@"(F(X_0 + \mu) - F(X_0)) / \mu").ToArithmetic()),
                                        new Equal((@"\tau(F(X),X_0,\mu)").ToArithmetic(), (@"((X_0 + \mu)*H(X_0 + \mu) - X_0*H(X_0)) / \mu").ToArithmetic()),
                                        new Texte(@"Je ne connais pas la valeur numérique de {H(X_0 + \mu)}. Donc, je tente de la supprimer de l'équation", true),
                                        new Equal((@"\tau(F(X),X_0,\mu)").ToArithmetic(), (@"((X_0 + \mu)*H(X_0 + \mu) - (X_0 + \mu)*H(X_0) + (X_0 + \mu)*H(X_0) - X_0*H(X_0)) / \mu").ToArithmetic()),
                                        new Equal((@"\tau(F(X),X_0,\mu)").ToArithmetic(), (@"((X_0 + \mu)*(H(X_0 + \mu) - H(X_0)) + \mu*H(X_0)) / \mu").ToArithmetic()),
                                        new Equal((@"\tau(F(X),X_0,\mu)").ToArithmetic(), (@"(X_0 + \mu)*(H(X_0 + \mu) - H(X_0)) / \mu + H(X_0)").ToArithmetic()),
                                        new Equal((@"\tau(F(X),X_0,\mu)").ToArithmetic(), (@"(X_0 + \mu)*\tau(H(X),X_0,\mu) + H(X_0)").ToArithmetic())
                                    )
                                )
                            ),
                            new Exercice(4, "Exprimez la différence des ordonnées en fonction du taux de variation de {F}", @"Multipliez par {\mu}", true, 
                                new Answer(@"On multiplie le taux de variation de {F} par {\mu}", true,
                                    new SequenceProof(new Equal((@"F(X_0 + \mu) - F(X_0)").ToArithmetic(), (@"\mu*((X_0 + \mu)*\tau(H(X),X_0,\mu) + H(X_0))").ToArithmetic()))
                                )
                            ),
                            new Exercice(5, "Simplifiez l'équation en supprimant le taux de variation de {H}", "", true,
                                new Answer("", false,
                                    new SequenceProof(new Equal((@"F(X) - F(X_0)").ToArithmetic(), (@"X*(H(X)-H(X_0)) + \mu*H(X_0)").ToArithmetic()))
                                )
                            ),
                            new Exercice(6, @"Exprimez le taux de variation pour deux déplacements successifs {\mu_1} et {\mu_2}", "", true,
                                new Answer(@"", false,
                                    new SequenceProof(new Equal((@"\tau(F(X),X_0,\mu_1 + \mu_2)").ToArithmetic(), (@"(X_0 + \mu_1 + \mu_2)*\tau(H(X),X_0,\mu_1 + \mu_2) + H(X_0)").ToArithmetic()))
                                )
                            ),
                            new Exercice(7, @"Exprimez la somme de deux taux de variation pour deux déplacements successifs {\mu_1} et {\mu_2}", "", true,
                                new Answer(@"", false,
                                    new SequenceProof(
                                        new Equal((@"\mu_1*\tau(F(X),X_0,\mu_1) + \mu_2*\tau(F(X),X_0 + \mu_1, \mu_2)").ToArithmetic(), (@"\mu_1 * ((X_0 + \mu_1)*\tau(H(X),X_0,\mu_1) + H(X_0)) + ...").ToArithmetic()),
                                        new Texte("     {" + (@"\mu_2 * ((X_0 + \mu_1 + \mu_2)*\tau(H(X),X_0 + \mu_1, \mu_2) + H(X_0+\mu_1))").ToArithmetic().ToTex() + "}", true)
                                    )
                                )
                            ),
                            new Exercice(8, "Montrez que la somme de deux déplacements successifs est égal à la somme des déplacements", "", false,
                                new Answer("", false,
                                    new SequenceProof(
                                        new Texte("On multiplie chaque taux de variation par le déplacement"),
                                        new Equal((@"F(X) - F(X_0)").ToArithmetic(), (@"(\mu_1 + \mu_2)*\tau(F(X),X_0,\mu_1 + \mu_2)").ToArithmetic()),
                                        new Equal((@"F(X_1) - F(X_0)").ToArithmetic(), (@"\mu_1*\tau(F(X),X_0,\mu_1)").ToArithmetic()),
                                        new Equal((@"F(X) - F(X_1)").ToArithmetic(), (@"\mu_2*\tau(F(X),X_1,\mu_2)").ToArithmetic()),
                                        new Equal((@"F(X) - F(X_0)").ToArithmetic(), (@"F(X_1) - F(X_0) + F(X) - F(X_1)").ToArithmetic()),
                                        new Texte("L'égalité est vraie")
                                    )
                                )
                            ),
                            new Exercice(9, @"Isolez {X} dans l'équation obtenue à l'exercice 5 et posez {\mu = \mu_1 + \mu_2}", "", true,
                                new Answer("", false,
                                    new SequenceProof(new Equal((@"X").ToArithmetic(), (@"(F(X) - F(X_0) - (\mu_1 + \mu_2)*H(X_0)) / (H(X)-H(X_0))").ToArithmetic()))
                                )
                            ),
                            new Exercice(10, @"Remplacez la différentielle de {F} par la somme de deux déplacements successifs et simplifiez", "", true,
                                new Answer("", false,
                                    new SequenceProof(
                                        new Equal((@"X").ToArithmetic(), (@"(X_1*(H(X_1)-H(X_0)) + \mu*H(X_0) + X*(H(X)-H(X_1)) + \mu_2*H(X_1) - (\mu_1 + \mu_2)*H(X_0)) / (H(X)-H(X_0))").ToArithmetic()),
                                        new Equal((@"X").ToArithmetic(), (@"(X_1*(H(X_1)-H(X_0)) + X*(H(X)-H(X_1)) - \mu_2*(H(X_1) - H(X_0))) / (H(X)-H(X_0))").ToArithmetic()),
                                        new Equal((@"X").ToArithmetic(), (@"((X_1-\mu_2)*(H(X_1)-H(X_0)) + X*(H(X)-H(X_1))) / (H(X)-H(X_0))").ToArithmetic()),
                                        new Equal((@"X*(H(X)-H(X_0))").ToArithmetic(), (@"(X_1-\mu_2)*(H(X_1)-H(X_0)) + X*(H(X)-H(X_1))").ToArithmetic()),
                                        new Equal((@"X*(H(X)-H(X_0)) - X*(H(X)-H(X_1))").ToArithmetic(), (@"(X_1-\mu_2)*(H(X_1)-H(X_0))").ToArithmetic()),
                                        new Equal((@"X*(H(X_1)-H(X_0))").ToArithmetic(), (@"(X_1-\mu_2)*(H(X_1)-H(X_0))").ToArithmetic()),
                                        new Equal((@"X_0 + \mu_1 + \mu_2").ToArithmetic(), (@"X_0 + \mu_1 - \mu_2").ToArithmetic()),
                                        new Equal((@"\mu_2").ToArithmetic(), (@"0").ToArithmetic())

                                    )
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

        /// <summary>
        /// Differential 3
        /// </summary>
        /// <returns>document</returns>
        public static FlowDocument Differential3()
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

            Wording w = new Wording("Diffentielle du polynôme 3", "Différence entre deux points d'un même polynôme 3",
                            new Exercice(1, "Exprimez la différentielle du polynôme 3", "",
                                new Answer("On présente la différentielle du polynôme 3",
                                    new SequenceProof(new Equal((@"Y - Y_0").ToArithmetic(), (@"DX*(DX^2 + (f''(X_0)/2)*DX + f'(X_0))").ToArithmetic()))

                                )
                            ),
                            new Exercice(2, "Exprimez les autres solutions {X_1} et {X_2} pour un point {X_0} donné", "", true,
                                new Answer(@"On pose la différentielle nulle",
                                    new SequenceProof(
                                        new Equal((@"0").ToArithmetic(), (@"DX*(DX^2 + f''(X_0)/2*DX + f'(X_0))").ToArithmetic()),
                                        new Texte("On résout l'équation d'ordre 2 dans la différentielle"),
                                        new Equal(("X_1").ToArithmetic(), ("X_0 + ((f''(X_0)/4)^2 - f'(X_0))v2 - f''(X_0)/4").ToArithmetic()),
                                        new Equal(("X_2").ToArithmetic(), ("X_0 - ((f''(X_0)/4)^2 - f'(X_0))v2 - f''(X_0)/4").ToArithmetic())
                                )
                            )),
                            new Exercice(3, "Exprimez la fonction {Y} selon le produit de trois fonctions", "Les fonctions sont affines", true,
                                new Answer(@"On développe la fonction {Y}", true,
                                    new SequenceProof(
                                        new Equal(("Y").ToArithmetic(), (@"(X-X_0)*(X-X_1)*(X-X_2)").ToArithmetic()),
                                        new Texte("On développe cette équation"),
                                        new Equal(("Y").ToArithmetic(), ("X^3 - (X_0+X_1+X_2)*X^2 + (X_0*[X_1+X_2]+X_1*X_2)*X - X_0*X_1*X_2").ToArithmetic())
                                )
                            )),
                            new Exercice(4, "Remplacez dans l'exercice 3, les équations de {X_1} et {X_2}", "", true,
                                new Answer(@"",
                                    new SequenceProof(
                                        new Equal("Q".ToArithmetic(), "((X_0 - f''(X_0)/4) + ((f''(X_0)/4)^2 - f'(X_0))v2)*((X_0 - f''(X_0)/4) - ((f''(X_0)/4)^2 - f'(X_0))v2)".ToArithmetic()),
                                        new Equal("Q".ToArithmetic(), "(X_0 - f''(X_0)/4) ^2  - (f''(X_0)/4)^2 + f'(X_0)".ToArithmetic()),
                                        new Equal("Q".ToArithmetic(), "X_0^2 - X_0*f''(X_0)/2 + f'(X_0)".ToArithmetic()),
                                        new Equal(("Y").ToArithmetic(), ("X^3 - (3*X_0-f''(X_0)/2)*X^2 + (2*X_0*(X_0 - f''(X_0)/4)+Q)*X - X_0*Q").ToArithmetic()),
                                        new Equal(("Y").ToArithmetic(), ("X^3 - (3*X_0-f''(X_0)/2)*X^2 + (2*X_0*(X_0 - f''(X_0)/4)+X_0^2 - X_0*f''(X_0)/2 + f'(X_0))*X - X_0*Q").ToArithmetic()),
                                        new Equal(("Y").ToArithmetic(), ("X^3 - (3*X_0-f''(X_0)/2)*X^2 + (3*X_0^2 - X_0*f''(X_0) + f'(X_0))*X - (X_0^3 - X_0^2*f''(X_0)/2 + X_0*f'(X_0))").ToArithmetic()),
                                        new Equal(("Y").ToArithmetic(), ("X^3 + b*X^2 + c*X - (X_0^3 - X_0^2*f''(X_0)/2 + X_0*f'(X_0))").ToArithmetic())
                                )
                            ))
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

        /// <summary>
        /// Add a textbox
        /// </summary>
        /// <param name="id">textbox id</param>
        /// <param name="title">title textbox</param>
        /// <param name="initialValue">value init</param>
        /// <param name="fd">flow document</param>
        /// <param name="a">fonction callback</param>
        private static void AddTextBox(string id, string title, string initialValue, FlowDocument fd, Action<TextBox> a)
        {
            DockPanel d = new DockPanel();
            TextBlock txtb = new TextBlock();
            txtb.Text = title;
            TextBox tbox = new TextBox();
            tbox.TextChanged += new TextChangedEventHandler((o, e) => { a(tbox); });
            tbox.Name = id;
            tbox.Text = initialValue;
            d.Children.Add(txtb);
            d.Children.Add(tbox);
            fd.Blocks.Add(new BlockUIContainer(d));
        }

        /// <summary>
        /// Add an item into the menu
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="tr">table</param>
        private static void AddItemCellIntoTabular(dynamic value, TableRow tr)
        {
            TextBlock txtb = new TextBlock();
            txtb.Text = Convert.ToString(value);
            TableCell tc = new TableCell(new BlockUIContainer(txtb));
            tc.BorderBrush = new SolidColorBrush(Colors.Aqua);
            tc.BorderThickness = new Thickness(1);
            tr.Cells.Add(tc);
        }

        /// <summary>
        /// Create a tabular to study data numbers
        /// </summary>
        /// <returns>flow document</returns>
        public static FlowDocument TabularPolynome2()
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

            IArithmetic function = ("X_0^2 + b*X_0 + c").ToArithmetic();


            string[] textValues = new string[5];
            AddTextBox("textbox_b", "Coefficient B", "2", fd, (t) => { textValues[0] = t.Text; });
            AddTextBox("textbox_c", "Coefficient C", "1", fd, (t) => { textValues[1] = t.Text; });
            AddTextBox("textbox_dx", "Valeur DX", "1", fd, (t) => { textValues[3] = t.Text; });
            AddTextBox("textbox_x0", "Valeur de départ X0", "0", fd, (t) => { textValues[4] = t.Text; });

            WrapPanel panel = new WrapPanel();
            FlowDocumentScrollViewer scrollViewer = new FlowDocumentScrollViewer();

            Button btCalc = new Button();
            btCalc.Name = "btCalc";
            btCalc.Content = "Recalculer";
            btCalc.Click += new RoutedEventHandler((o, e) =>
            {
                double x0, b, c, y, y0, dx, dy, prev = 0, diff = 0;
                Table t = new Table();
                TableRowGroup trg = new TableRowGroup();

                TableRow row = new TableRow();
                AddItemCellIntoTabular("X0", row);
                AddItemCellIntoTabular("Y0 = X0^2 + b*X0 + c", row);
                AddItemCellIntoTabular("DY = DX*[DX + Y'(X0)]", row);
                AddItemCellIntoTabular("Y = Y0 + DY", row);
                AddItemCellIntoTabular("Diff DY = 2*dx^2", row);
                AddItemCellIntoTabular("Sum DY", row);
                AddItemCellIntoTabular("Y final = Sum DY + Y0", row);
                trg.Rows.Add(row);

                Polynome2 p = new Polynome2();
                x0 = Convert.ToDouble(textValues[4]);
                dx = Convert.ToDouble(textValues[3]);
                y = Convert.ToDouble(textValues[2]);
                b = Convert.ToDouble(textValues[0]);
                c = Convert.ToDouble(textValues[1]);
                double sum = 0;
                double y0init = p.ComputeY0(x0, b, c);
                for (int step = 0; step < 100; ++step)
                {

                    row = new TableRow();
                    y0 = p.ComputeY0(x0, b, c);
                    AddItemCellIntoTabular(x0, row);
                    AddItemCellIntoTabular(y0, row);
                    dy = p.DifferentialDX(dx, x0, b, c);
                    AddItemCellIntoTabular(dy, row);
                    y = y0 + dy;
                    if (step > 0)
                        diff = dy - prev;
                    prev = dy;
                    AddItemCellIntoTabular(y, row);
                    AddItemCellIntoTabular(diff, row);
                    sum = sum + dy;
                    AddItemCellIntoTabular(sum, row);
                    AddItemCellIntoTabular(sum + y0init, row);
                    trg.Rows.Add(row);
                    x0 += dx;
                }
                t.RowGroups.Add(trg);

                FlowDocument f2 = new FlowDocument(t);
                Paragraph p2 = new Paragraph();
                fd.Blocks.Add(p2);
                scrollViewer.Document = f2;
                scrollViewer.UpdateLayout();
            });
            fd.Blocks.Add(new BlockUIContainer(btCalc));

            panel.Children.Add(scrollViewer);

            fd.Blocks.Add(new BlockUIContainer(panel));

            Button but = new Button();
            but.Name = "GoBack";
            but.Content = "Retour";
            but.Click += Button_Click;
            SetButtonStyle(but);
            fd.Blocks.Add(new BlockUIContainer(but));

            return fd;
        
        }

        /// <summary>
        /// Differential 3
        /// </summary>
        /// <returns>document</returns>
        public static FlowDocument PolynômeBase10()
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

            Wording w = new Wording("Le polynôme de base X", "La valeur numérique d'un nombre",
                            new Exercice(1, "Exprimez un polynôme d'ordre 2 avec les coefficients b et c", "",
                                new Answer("Le polynôme 2 est formé d'un trinome",
                                    new SequenceProof(new Equal((@"Y").ToArithmetic(), (@"X^2 + b*X + c").ToArithmetic()))

                                )
                            ),
                            new Exercice(2, "Développez un nombre réel comme la sommation des chiffres du nombre. Concluez", "", true,
                                new Answer(@"On suppose, pour le moment, que {X} est un nombre entier", true,
                                    new SequenceProof(
                                        new Equal((@"f(10)").ToArithmetic(), (@"n_4*10^4 + n_3*10^3 + n_2*10^2 + n_1*10 + n_0").ToArithmetic()),
                                        new Texte("Un nombre peut être vu comme un polynôme où les coefficients sont les chiffres de base 10")
                                )
                            )),
                            new Exercice(3, "Dans cet exercice, il s'agit de trouver la valeur d'un chiffre", "Exprimez la valeur {Y} pour chaque valeur {n_i} de 0 à 9", true,
                                new Answer(@"On développe la fonction {Y}", true,
                                    new SequenceProof(
                                        new Equal(("p_i").ToArithmetic(), ("(n_i)^2 + b*(n_i) + c").ToArithmetic()),
                                        new Texte("Comme tous les {n_i} vont de 0 à 9, calculons la valeur de {Y} pour chaque valeur de {X} de 0 à 9", true),
                                        new Equal(("p_i").ToArithmetic(), ("0^2 + b*0 + c").ToArithmetic()),
                                        new Equal(("p_i").ToArithmetic(), ("1^2 + b*1 + c").ToArithmetic()),
                                        new Equal(("p_i").ToArithmetic(), ("2^2 + b*2 + c").ToArithmetic()),
                                        new Equal(("p_i").ToArithmetic(), ("3^2 + b*3 + c").ToArithmetic()),
                                        new Equal(("p_i").ToArithmetic(), ("4^2 + b*4 + c").ToArithmetic()),
                                        new Equal(("p_i").ToArithmetic(), ("5^2 + b*5 + c").ToArithmetic()),
                                        new Equal(("p_i").ToArithmetic(), ("6^2 + b*6 + c").ToArithmetic()),
                                        new Equal(("p_i").ToArithmetic(), ("7^2 + b*7 + c").ToArithmetic()),
                                        new Equal(("p_i").ToArithmetic(), ("8^2 + b*8 + c").ToArithmetic()),
                                        new Equal(("p_i").ToArithmetic(), ("9^2 + b*9 + c").ToArithmetic())
                                )
                            )),
                            new Exercice(4, "Expliquez l'effet quand {p_i} peut avoir deux chiffres", "Se rappeler la notion de congruence", true,
                                new Answer(@"",
                                    new SequenceProof(
                                        new Texte("{p_i} devient un nombre à deux chiffres à partir de {n_i=3}", true),
                                        new Texte("Dans ce cas, la base 10 ne convient plus et {B=f(n_i)+1}", true),
                                        new Texte("Ainsi, lorsque {p_i=3} alors {B=17}", true),
                                        new Texte("Ainsi, lorsque {p_i=4} alors {B=26}", true),
                                        new Texte("Ainsi, lorsque {p_i=5} alors {B=17}", true),
                                        new Texte("Ainsi, lorsque {p_i=3} alors {B=17}", true),
                                        new Texte("Ainsi, lorsque {p_i=3} alors {B=17}", true),
                                        new Texte("Ainsi, lorsque {p_i=3} alors {B=17}", true),
                                        new Texte("Ainsi, lorsque {p_i=3} alors {B=17}", true)
                                )
                            ))
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
