using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Class to handle a function and
    /// parameters separated by a ,
    /// </summary>
    public class Function : Arithmetic, IVariable
    {

        #region Fields

        /// <summary>
        /// Index name for function
        /// </summary>
        private static readonly string functionName = "function";
        /// <summary>
        /// Index name for parameter list
        /// </summary>
        private static readonly string parametersName = "paramList";

        #endregion

        #region Constructor

        /// <summary>
        /// Non empty creatable class
        /// </summary>
        protected Function()
        {
            this.Set(functionName, "f");
            this.Set(parametersName, new Sequence());
        }

        /// <summary>
        /// Constructor completed
        /// </summary>
        /// <param name="f">function name</param>
        /// <param name="pars">parameters</param>
        public Function(string f, params IArithmetic[] pars) {
            
            this.Set(functionName, f);
            List<IArithmetic> list = pars.ToList();
            list.ForEach(x => x = x.Clone() as IArithmetic);
            this.Set(parametersName, new Sequence(list));
        }

        /// <summary>
        /// Constructor completed
        /// </summary>
        /// <param name="f">function name</param>
        /// <param name="list">parameters</param>
        public Function(string f, List<IArithmetic> list)
        {

            this.Set(functionName, f);
            list.ForEach(x => x = x.Clone() as IArithmetic);
            this.Set(parametersName, new Sequence(list));
        }

        /// <summary>
        /// Constructor completed
        /// </summary>
        /// <param name="f">function name</param>
        /// <param name="s">parameters</param>
        public Function(string f, Sequence s)
        {

            this.Set(functionName, f);
            this.Set(parametersName, s.Clone() as IArithmetic);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of this coefficient
        /// </summary>
        public string Name
        {
            get
            {
                return this[functionName];
            }
        }

        /// <summary>
        /// Gets the operator ID
        /// </summary>
        public override char Operator
        {
            get
            {
                return this[operatorName];
            }
        }

        /// <summary>
        /// Gets binary switch test
        /// </summary>
        public override bool IsBinaryOperator
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets unary switch test
        /// </summary>
        public override bool IsUnaryOperator
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets multiple switch test
        /// </summary>
        public override bool IsMultipleOperator
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets true if it's not an operator
        /// </summary>
        public override bool IsNotOperator
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the function name
        /// </summary>
        public string FunctionName
        {
            get { return this.Get(functionName, "f"); }
            set { this.Set(functionName, value); }
        }

        /// <summary>
        /// Gets all parameters
        /// </summary>
        public List<IArithmetic> Parameters
        {
            get { return this.Get(parametersName, new List<IArithmetic>()); }
        }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public dynamic Value
        {
            get
            {
                return this.Get(rightTermName);
            }
            set
            {
                this.Set(rightTermName, value);
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Converts all sub-variables into an equation
        /// or into its value
        /// </summary>
        /// <returns>output new equation</returns>
        public override IArithmetic Converting()
        {
            if (this.IsVariableExists(this.Name))
            {
                IArithmetic f = this.GetVariable(this.Name);
                if (f is Function)
                {
                    Function fe = f as Function;
                    for (int index = 0; index < fe.Parameters.Count; ++index)
                    {
                        if (fe.Parameters[index] is Coefficient)
                        {
                            Coefficient coeff = fe.Parameters[index] as Coefficient;
                            this.AddVariable(coeff.Name, this.Parameters[index].Converting());
                        }
                        if (fe.Parameters[index] is UnknownTerm)
                        {
                            UnknownTerm xt = fe.Parameters[index] as UnknownTerm;
                            this.AddVariable(xt.Name, this.Parameters[index].Converting());
                        }
                        if (fe.Parameters[index] is Function)
                        {
                            Function fun = fe.Parameters[index] as Function;
                            this.AddVariable(fun.Name, this.Parameters[index].Converting());
                        }
                    }
                    return fe.RightOperand.Converting();
                }
            }
            return this;
        }

        /// <summary>
        /// Transforms equation object into a tex representation
        /// </summary>
        /// <returns>tex representation</returns>
        public override string ToTex()
        {
            return this.FunctionName + @"\left(" + (this[parametersName] as Sequence).ToTex() + @"\right)";
        }

        /// <summary>
        /// Transforms equation object into a string representation
        /// </summary>
        /// <returns>string representation</returns>
        public override string ToString()
        {
            return this.FunctionName + "(" + (this[parametersName] as Sequence).ToString() + ")";
        }


        /// <summary>
        /// Create a new arithmetic class
        /// </summary>
        protected override IArithmetic Create()
        {
            return new Function();
        }
        #endregion

    }
}
