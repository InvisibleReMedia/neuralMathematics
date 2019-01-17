using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using System.Windows.Documents;
using WpfMath.Controls;
using System.Reflection;

namespace PersistantModel
{
    /// <summary>
    /// Classe permettant
    /// d'enregistrer des éléments
    /// arithmetiques
    /// </summary>
    [Serializable]
    public class Arithmetic : IArithmetic, IEquation, IDocument, ICloneable, IEqualityComparer<Arithmetic>
    {

        #region Fields

        /// <summary>
        /// Index name to store the equation object
        /// </summary>
        protected static readonly string equationName = "equation";
        /// <summary>
        /// nom pour le champ lettre
        /// </summary>
        protected static readonly string letterName = "letter";
        /// <summary>
        /// Index name to store a boolean value of having an existing value
        /// </summary>
        protected static readonly string hasValueName = "hasValue";
        /// <summary>
        /// nom pour le champ valeur
        /// </summary>
        protected static readonly string valueName = "value";
        /// <summary>
        /// Index name to store operator name
        /// </summary>
        protected static readonly string operatorName = "operator";
        /// <summary>
        /// Index name to store value
        /// </summary>
        protected static readonly string innerOperandName = "inner";
        /// <summary>
        /// Index name to store left value
        /// </summary>
        protected static readonly string leftTermName = "left";
        /// <summary>
        /// Index name to store right value
        /// </summary>
        protected static readonly string rightTermName = "right";
        /// <summary>
        /// Index name to store a constant
        /// </summary>
        protected static readonly string constantName = "constant";
        /// <summary>
        /// Index name to store a coefficient
        /// </summary>
        protected static readonly string coefName = "coefficient";
        /// <summary>
        /// Index name to store an unknown term
        /// </summary>
        protected static readonly string unknownName = "unknown";
        /// <summary>
        /// Boolean to act equation as calculable
        /// </summary>
        protected static readonly string isCalculableName = "isCalculable";
        /// <summary>
        /// Value that acts as a calculated result
        /// </summary>
        protected static readonly string calculatedValueName = "calculated";
        /// <summary>
        /// String element that acts as an un-calculable result
        /// </summary>
        protected static readonly string uncalculatedValueName = "uncalculated";
        /// <summary>
        /// Index name to store list
        /// </summary>
        protected static readonly string listName = "list";
        /// <summary>
        /// Index name for tex mode
        /// </summary>
        protected static readonly string texModeName = "texMode";

        /// <summary>
        /// Computed weight
        /// </summary>
        protected Weight weight;

        /// <summary>
        /// mutualized record zone
        /// </summary>
        protected RecordZone<Weight> recordZone;

        /// <summary>
        /// Inner data informations
        /// </summary>
        protected Dictionary<string, dynamic> persistentData;

        protected static event EventHandler<KeyValuePair<string, IArithmetic>> addVariable;

        protected static Func<string, IArithmetic> getVariable;

        /// <summary>
        /// Events
        /// </summary>
        private event EventHandler eventFetch, eventUnfetch;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        protected Arithmetic()
        {
            this.persistentData = new Dictionary<string, dynamic>();     
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets all records
        /// </summary>
        public RecordZone<Weight> Records
        {
            get
            {
                return this.recordZone;
            }
        }

        /// <summary>
        /// Gets the weight content
        /// </summary>
        public IWeight OwnerWeight
        {
            get
            {
                return this.weight;
            }
        }

        /// <summary>
        /// Gets or sets a value from
        /// persistent data
        /// </summary>
        /// <param name="name">name</param>
        /// <returns>value</returns>
        public dynamic this[string name]
        {
            get
            {
                return this.Get(name);
            }
            set
            {
                this.Set(name, value);
            }
        }

        /// <summary>
        /// Event to fetch all data into
        /// a unique list of unique records
        /// </summary>
        public event EventHandler Fetch
        {
            add
            {
                this.eventFetch += value;
            }
            remove
            {
                this.eventFetch -= value;
            }
        }

        /// <summary>
        /// Event to unfetch all data into
        /// a unique list of unique records
        /// </summary>
        public event EventHandler Unfetch
        {
            add
            {
                this.eventUnfetch += value;
            }
            remove
            {
                this.eventUnfetch -= value;
            }
        }

        /// <summary>
        /// Gets the operator ID
        /// </summary>
        public virtual char Operator
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets binary switch test
        /// </summary>
        public virtual bool IsBinaryOperator
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets unary switch test
        /// </summary>
        public virtual bool IsUnaryOperator
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets multiple switch test
        /// </summary>
        public virtual bool IsMultipleOperator
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets true if it's not an operator
        /// </summary>
        public virtual bool IsNotOperator
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the inner operand
        /// </summary>
        public virtual IArithmetic InnerOperand
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the left operand
        /// </summary>
        public virtual IArithmetic LeftOperand
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the right operand
        /// </summary>
        public virtual IArithmetic RightOperand
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets or sets the tex mode
        /// </summary>
        public bool IsTexMode
        {
            get
            {
                return this.Get(texModeName);
            }
            set
            {
                this.Set(texModeName, value);
            }
        }

        /// <summary>
        /// Gets all unknown terms
        /// </summary>
        protected IEnumerable<IArithmetic> SelectUnknownTerms
        {
            get
            {
                if (this.IsBinaryOperator)
                {
                    if (this[leftTermName] != null)
                        foreach (IArithmetic e in this[leftTermName].SelectUnknownTerms) yield return e;
                    if (this[rightTermName] != null)
                        foreach (IArithmetic e in this[rightTermName].SelectUnknownTerms) yield return e;
                }
                else if (this.IsUnaryOperator)
                {
                    if (this[innerOperandName] != null)
                        foreach (IArithmetic e in this[innerOperandName].SelectUnknownTerms) yield return e;
                }
                else
                {
                    if (this is Term)
                        foreach (IArithmetic e in this[unknownName].SelectUnknownTerms) yield return e;
                    else if (this is UnknownTerm)
                        yield return this;
                    else if (this is Sum)
                    {
                        Sum s = this as Sum;
                        foreach (Arithmetic a in s.Items)
                        {
                            foreach (IArithmetic e in a.SelectUnknownTerms) yield return e;
                        }
                    }
                    else if (this is Product)
                    {
                        Product p = this as Product;
                        foreach (Arithmetic a in p.Items)
                        {
                            foreach (IArithmetic e in a.SelectUnknownTerms) yield return e;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets all unknown terms
        /// </summary>
        public void FindUnknownTerms(Action<Tuple<IArithmetic, dynamic, IArithmetic>> f)
        {
            if (this.IsBinaryOperator)
            {
                if (this[leftTermName] != null && (this.LeftOperand is UnknownTerm))
                {
                    if (this.IsVariableExists((this.LeftOperand as UnknownTerm).Name))
                        f(new Tuple<IArithmetic, dynamic, IArithmetic>(this, "left", this.GetVariable((this.LeftOperand as UnknownTerm).Name)));
                }
                else
                {
                    // sous arbre gauche
                    this[leftTermName].FindUnknownTerms(f);
                }
                if (this[rightTermName] != null && (this.RightOperand is UnknownTerm))
                {
                    if (this.IsVariableExists((this.LeftOperand as UnknownTerm).Name))
                        f(new Tuple<IArithmetic, dynamic, IArithmetic>(this, "right", this.GetVariable((this.LeftOperand as UnknownTerm).Name)));
                }
                else
                {
                    // sous arbre droit
                    this[rightTermName].FindUnknownTerms(f);
                }
                    
            }
            else if (this.IsUnaryOperator)
            {
                if (this[innerOperandName] != null && (this.InnerOperand is UnknownTerm))
                {
                    if (this.IsVariableExists((this.LeftOperand as UnknownTerm).Name))
                        f(new Tuple<IArithmetic, dynamic, IArithmetic>(this, "inner", this.GetVariable((this.LeftOperand as UnknownTerm).Name)));
                }
                else
                {
                    this[innerOperandName].FindUnknownTerms(f);
                }
            }
            else if (this is Sum)
            {
                Sum s = this as Sum;
                uint index = 0;
                foreach (Arithmetic a in s.Items)
                {
                    if (a is UnknownTerm)
                    {
                        if (this.IsVariableExists((this.LeftOperand as UnknownTerm).Name))
                            f(new Tuple<IArithmetic, dynamic, IArithmetic>(this, index, this.GetVariable((this.LeftOperand as UnknownTerm).Name)));
                    }
                    else
                    {
                        a.FindUnknownTerms(f);
                    }
                    ++index;
                }
            }
            else if (this is Product)
            {
                Product p = this as Product;
                uint index = 0;
                foreach (Arithmetic a in p.Items)
                {
                    if (a is UnknownTerm)
                    {
                        if (this.IsVariableExists((this.LeftOperand as UnknownTerm).Name))
                            f(new Tuple<IArithmetic, dynamic, IArithmetic>(this, index, this.GetVariable((this.LeftOperand as UnknownTerm).Name)));
                    }
                    else
                    {
                        a.FindUnknownTerms(f);
                    }
                    ++index;
                }
            }
        }

        /// <summary>
        /// Gets all unknown terms
        /// </summary>
        public void FindCoefficients(Action<Tuple<IArithmetic, dynamic, IArithmetic>> f)
        {
            if (this.IsBinaryOperator)
            {
                if (this[leftTermName] != null && (this.LeftOperand is Coefficient))
                {
                    if ((this.LeftOperand as Coefficient).Value.HasValue)
                        f(new Tuple<IArithmetic, dynamic, IArithmetic>(this, "left", this.LeftOperand));
                }
                else
                {
                    this[leftTermName].FindCoefficients(f);
                }
                if (this[rightTermName] != null && (this.RightOperand is Coefficient))
                {
                    if ((this.RightOperand as Coefficient).Value.HasValue)
                        f(new Tuple<IArithmetic, dynamic, IArithmetic>(this, "right", this.RightOperand));
                }
                else
                {
                    this[rightTermName].FindCoefficients(f);
                }

            }
            else if (this.IsUnaryOperator)
            {
                if (this[innerOperandName] != null && (this.InnerOperand is Coefficient))
                {
                    if ((this.InnerOperand as Coefficient).Value.HasValue)
                        f(new Tuple<IArithmetic, dynamic, IArithmetic>(this, "inner", this.InnerOperand));
                }
                else
                {
                    this[innerOperandName].FindCoefficients(f);
                }
            }
            else if (this is Sum)
            {
                Sum s = this as Sum;
                uint index = 0;
                foreach (Arithmetic a in s.Items)
                {
                    if (a is Coefficient)
                    {
                        if ((a as Coefficient).Value.HasValue)
                            f(new Tuple<IArithmetic, dynamic, IArithmetic>(this, index, a));
                    }
                    else
                    {
                        a.FindCoefficients(f);
                    }
                    ++index;
                }
            }
            else if (this is Product)
            {
                Product p = this as Product;
                uint index = 0;
                foreach (Arithmetic a in p.Items)
                {
                    if (a is Coefficient)
                    {
                        if ((a as Coefficient).Value.HasValue)
                            f(new Tuple<IArithmetic, dynamic, IArithmetic>(this, index, a));
                    }
                    else
                    {
                        a.FindCoefficients(f);
                    }
                    ++index;
                }
            }
        }

        /// <summary>
        /// Gets all unknown terms
        /// </summary>
        public IDictionary<string, IArithmetic> UnknownTerms
        {
            get
            {
                Dictionary<string, IArithmetic> d = new Dictionary<string,IArithmetic>();
                IEnumerable<UnknownTerm> terms = this.SelectUnknownTerms.Cast<UnknownTerm>();
                foreach(string name in terms.Select(x => x.Name).Distinct()) {
                    d.Add(name, terms.First(x => x.Name == name));
                }
                return d;
            }
        }

        /// <summary>
        /// Gets all coefficients
        /// </summary>
        protected IEnumerable<IArithmetic> SelectCoefficients
        {
            get
            {
                if (this.IsBinaryOperator)
                {
                    if (this[leftTermName] != null)
                        foreach (IArithmetic e in this[leftTermName].SelectCoefficients) yield return e;
                    if (this[rightTermName] != null)
                        foreach (IArithmetic e in this[rightTermName].SelectCoefficients) yield return e;
                }
                else if (this.IsUnaryOperator)
                {
                    if (this[innerOperandName] != null)
                        foreach (IArithmetic e in this[innerOperandName].SelectCoefficients) yield return e;
                }
                else
                {
                    if (this is Term)
                        foreach (IArithmetic e in this[unknownName].Coefficient.SelectCoefficients) yield return e;
                    else if (this is Coefficient)
                        yield return this;
                    else if (this is Sum)
                    {
                        Sum s = this as Sum;
                        foreach (Arithmetic a in s.Items)
                        {
                            foreach (IArithmetic e in a.SelectCoefficients) yield return e;
                        }
                    }
                    else if (this is Product)
                    {
                        Product p = this as Product;
                        foreach (Arithmetic a in p.Items)
                        {
                            foreach (IArithmetic e in a.SelectCoefficients) yield return e;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets all coefficients
        /// </summary>
        public IDictionary<string, IArithmetic> Coefficients
        {
            get
            {
                Dictionary<string, IArithmetic> d = new Dictionary<string, IArithmetic>();
                IEnumerable<Coefficient> terms = this.SelectCoefficients.Cast<Coefficient>();
                foreach (string name in terms.Select(x => x.Name).Distinct())
                {
                    d.Add(name, terms.First(x => x.Name == name));
                }
                return d;
            }
        }

        /// <summary>
        /// Gets all constants
        /// </summary>
        public IEnumerable<IArithmetic> Constants
        {
            get
            {
                if (this.IsBinaryOperator)
                {
                    if (this.LeftOperand != null)
                        foreach (IArithmetic e in this.LeftOperand.Constants) yield return e;
                    if (this.RightOperand != null)
                        foreach (IArithmetic e in this.RightOperand.Constants) yield return e;
                }
                else if (this.IsUnaryOperator)
                {
                    if (this.InnerOperand != null)
                        foreach (IArithmetic e in this.InnerOperand.Constants) yield return e;
                }
                else
                {
                    if (this is Term)
                        foreach (IArithmetic e in (this as Term).Constant.Constants) yield return e;
                    else if (this is NumericValue)
                        yield return this;
                    else if (this is Sum)
                    {
                        Sum s = this as Sum;
                        foreach (Arithmetic a in s.Items)
                        {
                            foreach (IArithmetic e in a.Constants) yield return e;
                        }
                    }
                    else if (this is Product)
                    {
                        Product p = this as Product;
                        foreach (Arithmetic a in p.Items)
                        {
                            foreach (IArithmetic e in a.Constants) yield return e;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets equation
        /// </summary>
        public IArithmetic Equation
        {
            get
            {
                return this;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Static event AddVariable
        /// </summary>
        public static event EventHandler<KeyValuePair<string, IArithmetic>> EventAddVariable
        {
            add { Arithmetic.addVariable += value; }
            remove { Arithmetic.addVariable -= value; }
        }

        /// <summary>
        /// Static event GetVariable
        /// </summary>
        public static Func<string, IArithmetic> EventGetVariable
        {
            get { return Arithmetic.getVariable; }
            set { Arithmetic.getVariable = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raise the event fetch
        /// </summary>
        /// <param name="records">records</param>
        protected void OnFetch(RecordZone<Weight> records)
        {
            this.eventFetch(records, new EventArgs());
        }

        /// <summary>
        /// Raise the event unfetch
        /// </summary>
        /// <param name="records">records</param>
        protected void OnUnfetch(RecordZone<Weight> records)
        {
            this.eventUnfetch(records, new EventArgs());
        }

        /// <summary>
        /// Sets a value into the dictionary
        /// </summary>
        /// <param name="name">name of the field</param>
        /// <param name="value">value of field</param>
        protected void Set(string name, dynamic value)
        {
            if (this.persistentData.ContainsKey(name))
            {
                this.persistentData[name] = value;
            }
            else
            {
                this.persistentData.Add(name, value);
            }
        }

        /// <summary>
        /// Gets a value from the dictionary
        /// </summary>
        /// <param name="name">name of the field</param>
        /// <param name="init">default value</param>
        /// <returns>value</returns>
        protected dynamic Get(string name, dynamic init = null)
        {
            if (!this.persistentData.ContainsKey(name))
            {
                this.persistentData.Add(name, init);
            }
            return this.persistentData[name];
        }

        /// <summary>
        /// Computes the unique weight
        /// for this object
        /// </summary>
        /// <returns>weight</returns>
        protected virtual Weight ComputeOwnerWeight()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Verify when a positive equation
        /// may contains multiple inner positive or negative
        /// equation
        /// </summary>
        /// <param name="e">equation to test</param>
        /// <returns>correct equation</returns>
        public static IArithmetic EnsureSign(IArithmetic e)
        {
            if (e is Negative || e is Positive)
            {
                return EnsureSign((e as UnaryOperation).InnerOperand);
            }
            else
            {
                return e;
            }
        }

        /// <summary>
        /// Verify when an inverse equation
        /// may contains multiple inner inverse
        /// equation
        /// </summary>
        /// <param name="e">equation to test</param>
        /// <returns>correct equation</returns>
        public static IArithmetic EnsureInverse(IArithmetic e)
        {
            if (e is Inverse)
            {
                return EnsureInverse((e as Inverse).InnerOperand);
            }
            else
            {
                return e;
            }
        }

        /// <summary>
        /// Verify when an equation
        /// may contains multiple inner add or sub equations
        /// </summary>
        /// <param name="e">equation to test</param>
        /// <param name="direction">sommation or difference</param>
        /// <returns>correct equation</returns>
        public static Sum EnsureSum(IArithmetic e, int direction)
        {
            if (e is Addition)
            {
                Sum s1 = EnsureSum((e as BinaryOperation).LeftOperand, direction);
                Sum s2 = EnsureSum((e as BinaryOperation).RightOperand, direction);
                return new Sum(s1.Items.Concat(s2.Items).ToArray());
            }
            else if (e is Soustraction)
            {
                Sum s1 = EnsureSum((e as BinaryOperation).LeftOperand, direction);
                Sum s2 = EnsureSum((e as BinaryOperation).RightOperand, -direction);
                return new Sum(s1.Items.Concat(s2.Items).ToArray());
            }
            else
            {
                if (direction > 0)
                {
                    IArithmetic eTemp = EnsureSign(e);
                    return new Sum(e);
                }
                else
                {
                    IArithmetic eTemp = EnsureSign(e);
                    return new Sum(new Negative(e));
                }
            }
        }

        /// <summary>
        /// Verify when an equation
        /// may contains multiple inner multiplication or division
        /// equation
        /// </summary>
        /// <param name="e">equation to test</param>
        /// <param name="direction">sommation or difference</param>
        /// <returns>correct equation</returns>
        public static Product EnsureProduct(IArithmetic e, int direction)
        {
            if (e is Multiplication)
            {
                Product s1 = EnsureProduct((e as BinaryOperation).LeftOperand, direction);
                Product s2 = EnsureProduct((e as BinaryOperation).RightOperand, direction);
                return new Product(s1.Items.Concat(s2.Items).ToArray());
            }
            else if (e is Division)
            {
                Product s1 = EnsureProduct((e as BinaryOperation).LeftOperand, direction);
                Product s2 = EnsureProduct((e as BinaryOperation).RightOperand, -direction);
                return new Product(s1.Items.Concat(s2.Items).ToArray());
            }
            else
            {
                if (direction > 0)
                {
                    return new Product(e);
                }
                else
                {
                    IArithmetic eTemp = EnsureInverse(e);
                    return new Product(new Inverse(e));
                }
            }
        }

        /// <summary>
        /// Create a new arithmetic class
        /// </summary>
        protected virtual IArithmetic Create()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Transforms an addition into a multiplication
        /// </summary>
        /// <returns>a list of possibles equation</returns>
        protected virtual IEnumerable<IArithmetic> MakeTransform()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// When an equation can be calculable then
        /// the result is a number else, it's an arithmetic expression
        /// </summary>
        /// <returns>result</returns>
        public virtual IArithmetic Compute()
        {
            return this;
        }

        /// <summary>
        /// Converts all sub-variables into an equation
        /// or into its value
        /// </summary>
        /// <returns>output new equation</returns>
        public IArithmetic Converting()
        {
            Arithmetic output = this.Clone() as Arithmetic;
            output.FindUnknownTerms((e) =>
            {
                if (e.Item1 is BinaryOperation)
                {
                    if (e.Item2 == "left")
                    {
                        (e.Item1 as Arithmetic)[leftTermName] = e.Item3;
                    }
                    else if (e.Item2 == "right")
                    {
                        (e.Item1 as Arithmetic)[rightTermName] = e.Item3;
                    }
                }
                else if (e.Item1 is UnaryOperation)
                {
                    (e.Item1 as Arithmetic)[innerOperandName] = e.Item3;
                }
                else if (e.Item1 is Sum || e.Item1 is Product)
                {
                    (e.Item1 as Sum)[listName][e.Item2] = e.Item3;
                }
            });


            output.FindCoefficients((e) => {
                if (e.Item1 is BinaryOperation)
                {
                    if (e.Item2 == "left")
                    {
                        (e.Item1 as Arithmetic)[leftTermName] = new NumericValue((e.Item3 as Coefficient).Value.Value);
                    }
                    else if (e.Item2 == "right")
                    {
                        (e.Item1 as Arithmetic)[rightTermName] = new NumericValue((e.Item3 as Coefficient).Value.Value);
                    }
                }
                else if (e.Item1 is UnaryOperation)
                {
                    (e.Item1 as Arithmetic)[innerOperandName] = new NumericValue((e.Item3 as Coefficient).Value.Value);
                }
                else if (e.Item1 is Sum || e.Item1 is Product)
                {
                    (e.Item1 as Sum)[listName][e.Item2] = new NumericValue((e.Item3 as Coefficient).Value.Value);
                }
            });
            return output;
        }


        /// <summary>
        /// Generates a new arithmetic object
        /// that's handle by a unique record zone
        /// This is a protected virtual method
        /// to be used by inherited classes
        /// </summary>
        protected virtual void MakeUnique(Arithmetic parent)
        {
            if (parent == null)
                this.recordZone = new RecordZone<Weight>();
            else
                this.recordZone = parent.Records;

            if (this.IsBinaryOperator)
            {
                if (this[leftTermName] != null)
                    this[leftTermName].MakeUnique(this);
                if (this[rightTermName] != null)
                    this[rightTermName].MakeUnique(this);
            }
            else if (this.IsUnaryOperator)
            {
                if (this[innerOperandName] != null)
                    this[innerOperandName].MakeUnique(this);
            }
            else
            {
                if (this is Term)
                {
                    this[constantName].MakeUnique(this);
                    this[coefName].MakeUnique(this);
                    this[unknownName].MakeUnique(this);
                }
                else if (this is Sum)
                {
                    Sum s = this as Sum;
                    foreach(Arithmetic a in s.Items)
                    {
                        a.MakeUnique(this);
                    }
                }
                else if (this is Product)
                {
                    Product p = this as Product;
                    foreach (Arithmetic a in p.Items)
                    {
                        a.MakeUnique(this);
                    }
                }
            }

            this.weight = this.ComputeOwnerWeight();
            this.eventFetch(this.recordZone, new EventArgs());
            this.eventUnfetch(this.recordZone, new EventArgs());
        }

        /// <summary>
        /// Clone this arithmetic object
        /// </summary>
        /// <returns>new arithmetic object</returns>
        public object Clone()
        {
            IArithmetic p = this.Create();
            foreach (string key in this.persistentData.Keys)
            {
                p[key] = this.persistentData[key];
            }
            return p;
        }

        /// <summary>
        /// Transforms equation object into a tex representation
        /// </summary>
        /// <returns>tex representation</returns>
        public virtual string ToTex()
        {
            string output = string.Empty;

            if (this.IsBinaryOperator)
            {
                string left = string.Empty, right = string.Empty;
                if (this.LeftOperand != null)
                    left = this.LeftOperand.ToTex();
                if (this.RightOperand != null)
                    right = this.RightOperand.ToTex();
                if (this.Operator == '+' || this.Operator == '-')
                {
                    output = @"{" + left + " " + this.Operator;
                    if (this.Operator == '-' && !(this.RightOperand is Coefficient || this.RightOperand is UnknownTerm || this.RightOperand is Term || this.RightOperand is NumericValue))
                        output += @" \left[" + right + @"\right]}";
                    else
                        output += @" " + right + @"}";
                }
                else if (this.Operator == '*' || this.Operator == '/' || this.Operator == 'v' || this.Operator == '^')
                {
                    if (this.LeftOperand is Addition || this.LeftOperand is Sum)
                        output = @"{\left(" + left + @"\right) " + this.Operator;
                    else
                        output = "{" + left + " " + this.Operator;
                    if (this.Operator == '/')
                        output += "{ " + right + " }}";
                    else
                    {
                        if (this.RightOperand is Addition || this.RightOperand is Sum)
                            output += @"\left(" + right + @"\right)}";
                        else
                            output += @" " + right + @"}";
                    }
                }
            }
            else if (this.IsUnaryOperator)
            {
                if (this.InnerOperand != null)
                    output = this.Operator + @"{\left[" + this.InnerOperand.ToTex() + @"\right]}";
            }
            else
            {
                if (this is Term)
                {
                    Term t = this as Term;
                    output = "{" + t.Constant.ToTex() + "} * {" + t.Coefficient.ToTex() + "} * {" + t.Unknown.ToTex() + "}";
                }
                else if (this is NumericValue)
                    output = (this as NumericValue).Value.ToString();
                else if (this is Coefficient)
                    output = (this as Coefficient).Name;
                else if (this is UnknownTerm)
                    output = (this as UnknownTerm).Name;
                else
                    throw new InvalidCastException();
            }
            return output;
        }

        /// <summary>
        /// Transforms equation object into a string representation
        /// </summary>
        /// <returns>string representation</returns>
        public override string ToString()
        {
            string output = string.Empty;

            if (this.IsBinaryOperator)
            {
                string left = string.Empty, right = string.Empty;
                if (this.LeftOperand != null)
                    left = this.LeftOperand.ToString();
                if (this.RightOperand != null)
                    right = this.RightOperand.ToString();
                if (this.Operator == '+' || this.Operator == '-')
                {
                    output = left + " " + this.Operator;
                    if (this.Operator == '-' && !(this.RightOperand is Coefficient || this.RightOperand is UnknownTerm || this.RightOperand is Term || this.RightOperand is NumericValue))
                        output += " [" + right + "]";
                    else
                        output += " " + right;
                }
                else if (this.Operator == '*' || this.Operator == '/' || this.Operator == 'v' || this.Operator == '^')
                {
                    output = "(" + left + ") " + this.Operator;
                    if (this.Operator == '/')
                        output += "(" + right + ")";
                    output += " " + right;
                }
            }
            else if (this.IsUnaryOperator)
            {
                if (this.InnerOperand != null)
                    output = this.Operator + "(" + this.InnerOperand.ToString() + ")";
            }
            else
            {
                if (this is Term)
                {
                    Term t = this as Term;
                    output = "(" + t.Constant.ToString() + ") * (" + t.Coefficient.ToString() + ") * (" + t.Unknown.ToString() + ")";
                }
                else if (this is NumericValue)
                    output = (this as NumericValue).Value.ToString();
                else if (this is Coefficient)
                    output = (this as Coefficient).Name;
                else if (this is UnknownTerm)
                    output = (this as UnknownTerm).Name;
                else
                    throw new InvalidCastException();
            }
            return output;
        }

        /// <summary>
        /// Generates a new arithmetic object
        /// that's handle by a unique record zone
        /// </summary>
        /// <returns>arithmetic object</returns>
        public IArithmetic MakeUnique()
        {
            this.MakeUnique(null);
            Gathering<Weight> g = new Gathering<Weight>(this.recordZone, this.OwnerWeight as Weight);
            return this;
        }

        /// <summary>
        /// Test if a variable exists
        /// </summary>
        /// <param name="letter">variable name</param>
        /// <returns>true if exists</returns>
        public bool IsVariableExists(string letter)
        {
            if (Arithmetic.getVariable != null)
            {
                IArithmetic ret = Arithmetic.getVariable(letter);
                return ret != null;
            }
            else
                return false;
        }

        /// <summary>
        /// Gets a variable object
        /// </summary>
        /// <param name="letter">the letter name</param>
        /// <returns>a variable object</returns>
        public IArithmetic GetVariable(string letter)
        {
            if (Arithmetic.getVariable != null)
            {
                IArithmetic ret = Arithmetic.getVariable(letter);
                return ret;
            }
            else
                return null;
        }

        /// <summary>
        /// Add an existing or no variable
        /// </summary>
        /// <param name="letter">variable name</param>
        /// <param name="e">element</param>
        public void AddVariable(string letter, IArithmetic e)
        {
            if (Arithmetic.addVariable != null)
                Arithmetic.addVariable(this, new KeyValuePair<string,IArithmetic>(letter, e));
        }

        /// <summary>
        /// Let a letter as a value
        /// given a letter and its value
        /// </summary>
        /// <param name="letter">letter value</param>
        /// <param name="value">numeric value</param>
        public void Let(string letter, double value)
        {
            foreach(Coefficient c in this.SelectCoefficients)
            {
                if (c.Name == letter) c.Value = value;
                this.AddVariable(letter, new NumericValue(value));
            }
            foreach(UnknownTerm x in this.SelectUnknownTerms)
            {
                if (x.Name == letter) (x as IVariable).Value = new NumericValue(value);
                this.AddVariable(letter, new NumericValue(value));
            }
        }

        /// <summary>
        /// Let a letter as an equation
        /// given a letter and its equation
        /// </summary>
        /// <param name="letter">letter value</param>
        /// <param name="e">equation object</param>
        public void Let(string letter, IArithmetic e)
        {
            foreach (Coefficient c in this.Coefficients.Values)
            {
                IArithmetic input = e.Compute();
                if (c.Name == letter && input is NumericValue)
                {
                    this.AddVariable(letter, input);
                    c.Value = (input as NumericValue).Value;
                }
            }
            foreach (UnknownTerm x in this.UnknownTerms.Values)
            {
                if (x.Name == letter) (x as IVariable).Value = (e as ICloneable).Clone() as IArithmetic;
                this.AddVariable(letter, e);
            }
        }

        /// <summary>
        /// Unlet a letter as an equation
        /// </summary>
        /// <param name="letter">letter value</param>
        public void Unlet(string letter)
        {
            foreach (Coefficient c in this.Coefficients.Values)
            {
                if (c.Name == letter)
                {
                    (c as IVariable).Value = null;
                    this.AddVariable(letter, null);
                }
            }
            foreach (UnknownTerm x in this.UnknownTerms.Values)
            {
                if (x.Name == letter) (x as IVariable).Value = null;
                this.AddVariable(letter, null);
            }
        }

        /// <summary>
        /// String representation of the algebraic equation
        /// </summary>
        /// <param name="type">type of representation; string or tex</param>
        /// <returns>string text</returns>
        public string AsRepresented(string type)
        {
            if (type == "string")
            {
                return this.ToString();
            }
            else if (type == "tex")
            {
                return this.ToTex();
            }
            else
                return "";
        }

        /// <summary>
        /// Insert text elements into a list
        /// </summary>
        /// <param name="list"></param>
        public void InsertIntoDocument(List list)
        {
            ListItem li = new ListItem();
            li.TextAlignment = System.Windows.TextAlignment.Center;
            FormulaControl fc = new FormulaControl();
            fc.Formula = this.ToTex();
            li.Blocks.Add(new BlockUIContainer(fc));
            list.ListItems.Add(li);
        }

        /// <summary>
        /// Transform the current equation to an
        /// another equation
        /// </summary>
        /// <returns>transformed equation</returns>
        public IEnumerable<IArithmetic> Transform()
        {
            return this.MakeTransform();
        }

        /// <summary>
        /// Factorization of an equation
        /// works only on a current equation as an addition
        /// </summary>
        /// <returns>factorized equation</returns>
        public IEquation Factorize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Develop equation
        /// works only on a current equation as a product
        /// </summary>
        /// <returns>developed equation</returns>
        public IEquation Develop()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Compare two object
        /// </summary>
        /// <param name="x">object left</param>
        /// <param name="y">object right</param>
        /// <returns></returns>
        public virtual bool Equals(Arithmetic x, Arithmetic y)
        {
            return x.OwnerWeight == y.OwnerWeight;
        }

        /// <summary>
        /// Hash code of this object
        /// replacement with the weight hash code
        /// </summary>
        /// <param name="obj">obj</param>
        /// <returns>hash code</returns>
        public int GetHashCode(Arithmetic obj)
        {
            return obj.GetHashCode();
        }

        /// <summary>
        /// Convert an IArithmetic object to a double
        /// </summary>
        /// <returns>double value</returns>
        public double ToDouble()
        {
            Interfaces.IArithmetic c = this.Compute();
            if (c is NumericValue)
                return (c as NumericValue).Value;
            else
                return 0.0d;
        }

        #region Operators

        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>addition</returns>
        public static Arithmetic operator +(Arithmetic a, Arithmetic b)
        {
            return new Addition(a, b);
        }

        /// <summary>
        /// Soustraction
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>soustractio</returns>
        public static Arithmetic operator -(Arithmetic a, Arithmetic b)
        {
            return new Soustraction(a, b);
        }

        /// <summary>
        /// Multiplication
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>multiplication</returns>
        public static Arithmetic operator *(Arithmetic a, Arithmetic b)
        {
            return new Multiplication(a, b);
        }

        /// <summary>
        /// Division
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>division</returns>
        public static Arithmetic operator /(Arithmetic a, Arithmetic b)
        {
            return new Division(a,b);
        }

        /// <summary>
        /// Power
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>power</returns>
        public static Arithmetic operator ^(Arithmetic a, Arithmetic b)
        {
            return new Power(a, b);
        }

        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>addition</returns>
        public static Arithmetic operator +(Arithmetic a, double b)
        {
            return new Addition(a, new NumericValue(b));
        }

        /// <summary>
        /// Soustraction
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>soustractio</returns>
        public static Arithmetic operator -(Arithmetic a, double b)
        {
            return new Soustraction(a, new NumericValue(b));
        }

        /// <summary>
        /// Multiplication
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>multiplication</returns>
        public static Arithmetic operator *(Arithmetic a, double b)
        {
            return new Multiplication(a, new NumericValue(b));
        }

        /// <summary>
        /// Division
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>division</returns>
        public static Arithmetic operator /(Arithmetic a, double b)
        {
            return new Division(a, new NumericValue(b));
        }

        /// <summary>
        /// Power
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>power</returns>
        public static Arithmetic operator ^(Arithmetic a, double b)
        {
            return new Power(a, new NumericValue(b));
        }

        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>addition</returns>
        public static Arithmetic operator +(double a, Arithmetic b)
        {
            return new Addition(b, new NumericValue(a));
        }

        /// <summary>
        /// Soustraction
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>soustractio</returns>
        public static Arithmetic operator -(double a, Arithmetic b)
        {
            return new Soustraction(b, new NumericValue(a));
        }

        /// <summary>
        /// Multiplication
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>multiplication</returns>
        public static Arithmetic operator *(double a, Arithmetic b)
        {
            return new Multiplication(b, new NumericValue(a));
        }

        /// <summary>
        /// Division
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>division</returns>
        public static Arithmetic operator /(double a, Arithmetic b)
        {
            return new Division(b, new NumericValue(a));
        }

        /// <summary>
        /// Power
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>power</returns>
        public static Arithmetic operator ^(double a, Arithmetic b)
        {
            return new Power(b, new NumericValue(a));
        }

        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>addition</returns>
        public static Arithmetic operator +(Arithmetic a, string b)
        {
            return new Addition(a, new UnknownTerm(b));
        }

        /// <summary>
        /// Soustraction
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>soustractio</returns>
        public static Arithmetic operator -(Arithmetic a, string b)
        {
            return new Soustraction(a, new UnknownTerm(b));
        }

        /// <summary>
        /// Multiplication
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>multiplication</returns>
        public static Arithmetic operator *(Arithmetic a, string b)
        {
            return new Multiplication(a, new UnknownTerm(b));
        }

        /// <summary>
        /// Division
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>division</returns>
        public static Arithmetic operator /(Arithmetic a, string b)
        {
            return new Division(a, new UnknownTerm(b));
        }

        /// <summary>
        /// Power
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>power</returns>
        public static Arithmetic operator ^(Arithmetic a, string b)
        {
            return new Power(a, new UnknownTerm(b));
        }

        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>addition</returns>
        public static Arithmetic operator +(string a, Arithmetic b)
        {
            return new Addition(b, new UnknownTerm(a));
        }

        /// <summary>
        /// Soustraction
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>soustractio</returns>
        public static Arithmetic operator -(string a, Arithmetic b)
        {
            return new Soustraction(b, new UnknownTerm(a));
        }

        /// <summary>
        /// Multiplication
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>multiplication</returns>
        public static Arithmetic operator *(string a, Arithmetic b)
        {
            return new Multiplication(b, new UnknownTerm(a));
        }

        /// <summary>
        /// Division
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>division</returns>
        public static Arithmetic operator /(string a, Arithmetic b)
        {
            return new Division(b, new UnknownTerm(a));
        }

        /// <summary>
        /// Power
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>power</returns>
        public static Arithmetic operator ^(string a, Arithmetic b)
        {
            return new Power(b, new UnknownTerm(a));
        }

        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>addition</returns>
        public static Arithmetic operator +(Arithmetic a, char b)
        {
            return new Addition(a, new Coefficient(b.ToString()));
        }

        /// <summary>
        /// Soustraction
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>soustractio</returns>
        public static Arithmetic operator -(Arithmetic a, char b)
        {
            return new Soustraction(a, new Coefficient(b.ToString()));
        }

        /// <summary>
        /// Multiplication
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>multiplication</returns>
        public static Arithmetic operator *(Arithmetic a, char b)
        {
            return new Multiplication(a, new Coefficient(b.ToString()));
        }

        /// <summary>
        /// Division
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>division</returns>
        public static Arithmetic operator /(Arithmetic a, char b)
        {
            return new Division(a, new Coefficient(b.ToString()));
        }

        /// <summary>
        /// Power
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>power</returns>
        public static Arithmetic operator ^(Arithmetic a, char b)
        {
            return new Power(a, new Coefficient(b.ToString()));
        }

        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>addition</returns>
        public static Arithmetic operator +(char a, Arithmetic b)
        {
            return new Addition(b, new Coefficient(a.ToString()));
        }

        /// <summary>
        /// Soustraction
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>soustractio</returns>
        public static Arithmetic operator -(char a, Arithmetic b)
        {
            return new Soustraction(b, new Coefficient(a.ToString()));
        }

        /// <summary>
        /// Multiplication
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>multiplication</returns>
        public static Arithmetic operator *(char a, Arithmetic b)
        {
            return new Multiplication(b, new Coefficient(a.ToString()));
        }

        /// <summary>
        /// Division
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>division</returns>
        public static Arithmetic operator /(char a, Arithmetic b)
        {
            return new Division(b, new Coefficient(a.ToString()));
        }

        /// <summary>
        /// Power
        /// </summary>
        /// <param name="a">left</param>
        /// <param name="b">right</param>
        /// <returns>power</returns>
        public static Arithmetic operator ^(char a, Arithmetic b)
        {
            return new Power(b, new Coefficient(a.ToString()));
        }

        #endregion

        #endregion

    }
}