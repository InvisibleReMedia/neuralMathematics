using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

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
        /// Gets true if equation is calculable
        /// </summary>
        public bool IsCalculable
        {
            get
            {
                if (this.persistentData.ContainsKey(isCalculableName))
                {
                    return this[isCalculableName];
                }
                else
                {
                    return false;
                }

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
        public IDictionary<string, IArithmetic> UnknownTerms
        {
            get
            {
                return this.SelectUnknownTerms.Cast<UnknownTerm>().ToDictionary(x => x.Name, x => x as IArithmetic);
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
                return this.SelectCoefficients.Cast<Coefficient>().ToDictionary(x => x.Name, x => x as IArithmetic);
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
        /// <param name="clean">true if calculate again</param>
        /// <returns>result</returns>
        protected virtual string Compute(bool clean)
        {
            string output = string.Empty;

            if (this.IsBinaryOperator)
            {
                string left = string.Empty, right = string.Empty;
                if (this.LeftOperand != null)
                    left = this.LeftOperand.Calculate(clean);
                if (this.RightOperand != null)
                    right = this.RightOperand.Calculate(clean);
                if (this.LeftOperand.IsCalculable && this.RightOperand.IsCalculable)
                {
                    this[isCalculableName] = true;
                    switch(this.Operator)
                    {
                        case '+':
                            output = (Convert.ToDouble(left) + Convert.ToDouble(right)).ToString();
                            break;
                        case '-':
                            output = (Convert.ToDouble(left) - Convert.ToDouble(right)).ToString();
                            break;
                        case '*':
                            output = (Convert.ToDouble(left) * Convert.ToDouble(right)).ToString();
                            break;
                        case '/':
                            double d = Convert.ToDouble(right);
                            if (d != 0.0d)
                                output = (Convert.ToDouble(left) / d).ToString();
                            else
                                output = Double.NaN.ToString();
                            break;
                        case '^':
                            output = Math.Pow(Convert.ToDouble(left),Convert.ToDouble(right)).ToString();
                            break;
                        case 'v':
                            double r = Convert.ToDouble(right);
                            if (r != 0.0d)
                                output = Math.Pow(Convert.ToDouble(left), 1 / r).ToString();
                            else
                                output = Double.NaN.ToString();
                            break;
                        case '=':
                            output = (this.LeftOperand[calculatedValueName] == this.RightOperand[calculatedValueName]).ToString();
                            break;
                    }
                }
                else
                {
                    this[isCalculableName] = false;
                    output = "(" + left + " " + this.Operator + " " + right + ")";
                }
            }
            else if (this.IsUnaryOperator)
            {
                if (this.InnerOperand != null)
                {
                    string inner = this.InnerOperand.Calculate(clean);
                    if (this.InnerOperand.IsCalculable)
                    {
                        this[isCalculableName] = true;
                        switch (this.Operator)
                        {
                            case 'p':
                                output = Convert.ToDouble(inner).ToString();
                                break;
                            case 'n':
                                output = (-Convert.ToDouble(inner)).ToString();
                                break;
                            case '\\':
                                double d = Convert.ToDouble(inner);
                                if (d != 0.0d)
                                    output = (1 / d).ToString();
                                else
                                    output = Double.NaN.ToString();
                                break;
                        }

                    }
                    else
                    {
                        this[isCalculableName] = false;
                        output = this.Operator + "(" + this.InnerOperand.ToString() + ")";
                    }
                }
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
                    output = (this as Coefficient).Compute(clean);
                else if (this is UnknownTerm)
                    output = (this as UnknownTerm).Compute(clean);
                else
                    throw new InvalidCastException();
            }
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
                if (this.persistentData[key] is IArithmetic)
                    p[key] = this.persistentData[key].Clone();
                else if (this.persistentData[key] is string)
                    p[key] = this.persistentData[key].Clone();
                else
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
                else if (this.Operator == '*' || this.Operator == '/')
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
                else if (this.Operator == '*' || this.Operator == '/')
                {
                    if (this.LeftOperand is Addition || this.LeftOperand is Sum)
                        output = "(" + left + ") " + this.Operator;
                    else
                        output = left + " " + this.Operator;
                    if (this.Operator == '/')
                        output += "(" + right + ")";
                    else
                    {
                        if (this.RightOperand is Addition || this.RightOperand is Sum)
                            output += " (" + right + ")";
                        else
                            output += " " + right;
                    }
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
        /// Let a letter as a value
        /// given a letter and its value
        /// </summary>
        /// <param name="letter">letter value</param>
        /// <param name="value">numeric value</param>
        public void Let(string letter, double value)
        {
            foreach(Coefficient c in this.Coefficients.Values)
            {
                if (c.Name == letter) c.Value = value;
            }
            foreach(UnknownTerm x in this.UnknownTerms.Values)
            {
                if (x.Name == letter) (x as IVariable).Value = new NumericValue(value);
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
                double d;
                if (c.Name == letter && Double.TryParse(e.Calculate(true), out d))
                {
                    c.Value = d;
                }
            }
            foreach (UnknownTerm x in this.UnknownTerms.Values)
            {
                if (x.Name == letter) (x as IVariable).Value = (e as ICloneable).Clone() as IArithmetic;
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
        /// Calculate the result of this equation
        /// terms that are valued are operated with its numeric value
        /// </summary>
        /// <param name="clean">true if calculate again</param>
        /// <returns>string representation number or algebraic</returns>
        public string Calculate(bool clean)
        {
            if (!clean && this.persistentData.ContainsKey(isCalculableName))
            {
                if (this[isCalculableName])
                {
                    return this[calculatedValueName].ToString();
                }
                else
                {
                    return this[uncalculatedValueName];
                }
            }
            else
            {
                this[isCalculableName] = false;
                string res = this.Compute(clean);
                if (this[isCalculableName])
                {
                    if (this is Equal)
                    {
                        this[calculatedValueName] = 1;
                    }
                    else
                    {
                        this[calculatedValueName] = Convert.ToDouble(res);
                    }
                }
                else
                {
                    this[uncalculatedValueName] = res;
                }
                return res;
            }
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

        #endregion
    }
}