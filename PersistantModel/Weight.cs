using Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;

namespace PersistantModel
{
    /// <summary>
    /// Class to handle bit values
    /// to identify an equation uniquely
    /// </summary>
    [Serializable]
    public class Weight : IWeight, IEqualityComparer<IWeight>
    {
        #region Fields

        /// <summary>
        /// GetHashCode list
        /// </summary>
        private static Dictionary<int, Weight> hashCodes;

        /// <summary>
        /// Value type for a constant
        /// </summary>
        public static readonly byte ConstantValueType = 1;
        /// <summary>
        /// Value type for a coefficient
        /// </summary>
        public static readonly byte CoefficientValueType = 2;
        /// <summary>
        /// Value type for an unknown term
        /// </summary>
        public static readonly byte UnknownTermValueType = 4;
        /// <summary>
        /// Value type for a string
        /// </summary>
        public static readonly byte StringValueType = 8;
        /// <summary>
        /// Value type for a double value
        /// </summary>
        public static readonly byte DoubleValueType = 16;
        /// <summary>
        /// Value type for a binary operator
        /// </summary>
        public static readonly byte BinaryOperatorValueType = 32;
        /// <summary>
        /// Value type for a unary operator
        /// </summary>
        public static readonly byte UnaryOperatorValueType = 64;
        /// <summary>
        /// Value type for a multiple operator
        /// </summary>
        public static readonly byte MultipleOperationValueType = 128;

        /// <summary>
        /// Value type of this weight
        /// </summary>
        private byte typeValue;
        /// <summary>
        /// Operator or function
        /// </summary>
        private char arithmeticOperator;
        /// <summary>
        /// Specific data support
        /// </summary>
        private dynamic value;
        /// <summary>
        /// Owner instance to keep unique element
        /// </summary>
        private IArithmetic ownerInstance;

        #endregion

        #region Constructors

        /// <summary>
        /// Compute a weight value
        /// as a computed hash integer
        /// </summary>
        /// <param name="type">value type</param>
        /// <param name="op">operator or function</param>
        /// <param name="value">value</param>
        /// <param name="owner">owner object</param>
        public Weight(byte type, char op, dynamic value, IArithmetic owner)
        {
            this.Construct(type, op, value, owner);
        }

        /// <summary>
        /// Compute a weight value
        /// as a computed hash integer
        /// </summary>
        /// <param name="type">value type</param>
        /// <param name="value">value</param>
        /// <param name="owner">owner object</param>
        public Weight(byte type, dynamic value, IArithmetic owner)
        {
            this.Construct(type, char.MinValue, value, owner);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets all hashcodes required 
        /// </summary>
        public static Dictionary<int, Weight> HashCodes
        {
            get
            {
                return hashCodes;
            }
            set
            {
                hashCodes = new Dictionary<int, Weight>(value);
            }
        }

        /// <summary>
        /// Gets the owner object instance
        /// </summary>
        public IArithmetic OwnerObject
        {
            get
            {
                return this.ownerInstance;
            }
        }

        /// <summary>
        /// Gets unique arithmetic instance of object
        /// </summary>
        public static IEnumerable<IArithmetic> UniqueArithmeticInstances
        {
            get
            {
                return hashCodes.Values.Select(x => x.OwnerObject);
            }
        }

        /// <summary>
        /// Gets the hash code number
        /// </summary>
        public int HashCode
        {
            get
            {
                return this.GetHashCode();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Construction of elements
        /// with a dynamic element (not working in constructor's function)
        /// </summary>
        /// <param name="type">value type</param>
        /// <param name="op">operator</param>
        /// <param name="value">value</param>
        /// <param name="owner">owner object</param>
        protected void Construct(byte type, char op, dynamic value, IArithmetic owner)
        {
            this.typeValue = type;
            this.arithmeticOperator = op;
            this.value = value;
            int h = this.GetHashCode();
            foreach (Weight w in hashCodes.Values)
            {
                if (w == this)
                {
                    h = w.GetHashCode();
                    break;
                }
            }
            if (!hashCodes.ContainsKey(h))
            {
                this.ownerInstance = owner;
                hashCodes.Add(h, this);
            }
            else
                if (hashCodes[h] != this)
                throw new InvalidCastException(String.Format("Weight {0} is not equals to Weight {1}", hashCodes[h].ToString(), this.ToString()));
            else
                this.ownerInstance = hashCodes[h].OwnerObject;
        }

        /// <summary>
        /// Initialize hash codes
        /// </summary>
        public static void Initialize()
        {
            hashCodes = new Dictionary<int, Weight>();
        }

        /// <summary>
        /// Empty hash codes list
        /// </summary>
        public static void Clear()
        {
            if (hashCodes != null)
                hashCodes.Clear();
            else
                hashCodes = new Dictionary<int, Weight>();
        }

        /// <summary>
        /// Computes a weight from a numeric value
        /// </summary>
        /// <param name="nv">numeric value object</param>
        /// <returns>weight of this object</returns>
        public static Weight ComputeWeight(NumericValue nv)
        {
            return new Weight(ConstantValueType, nv.Value, nv);
        }

        /// <summary>
        /// Computes a weight from a coefficient object
        /// </summary>
        /// <param name="c">coefficient object</param>
        /// <returns>weight of this object</returns>
        public static Weight ComputeWeight(Coefficient c)
        {
            return new Weight(CoefficientValueType, c.Name, c);
        }

        /// <summary>
        /// Computes a weight from an unknown term
        /// </summary>
        /// <param name="ut">unknown term object</param>
        /// <returns>weight of this object</returns>
        public static Weight ComputeWeight(UnknownTerm ut)
        {
            return new Weight(UnknownTermValueType, ut.Name, ut);
        }

        /// <summary>
        /// Computes a weight from a binary operator
        /// </summary>
        /// <param name="ut">binary term object</param>
        /// <returns>weight of this object</returns>
        public static Weight ComputeWeight(BinaryOperation op)
        {
            return new Weight(BinaryOperatorValueType, op.Operator, new Weight[] { op.LeftOperand.OwnerWeight as Weight, op.RightOperand.OwnerWeight as Weight }, op);
        }

        /// <summary>
        /// Computes a weight from a unary operator
        /// </summary>
        /// <param name="ut">unary term object</param>
        /// <returns>weight of this object</returns>
        public static Weight ComputeWeight(UnaryOperation op)
        {
            return new Weight(UnaryOperatorValueType, op.Operator, new Weight[] { op.InnerOperand.OwnerWeight as Weight }, op);
        }

        /// <summary>
        /// Computes a weight from a multiple sum
        /// </summary>
        /// <param name="ut">sum equation object</param>
        /// <returns>weight of this object</returns>
        public static Weight ComputeWeight(Sum op)
        {
            return new Weight(MultipleOperationValueType, op.Operator, op.Items.Select(x => x.OwnerWeight).Cast<Weight>().ToArray(), op);
        }

        /// <summary>
        /// Computes a weight from a multiple sum
        /// </summary>
        /// <param name="ut">product equation object</param>
        /// <returns>weight of this object</returns>
        public static Weight ComputeWeight(Product op)
        {
            return new Weight(MultipleOperationValueType, op.Operator, op.Items.Select(x => x.OwnerWeight).Cast<Weight>().ToArray(), op);
        }

        /// <summary>
        /// Equality Operator
        /// </summary>
        /// <param name="x">weight left</param>
        /// <param name="y">weight right</param>
        /// <returns>true if equals</returns>
        public static bool operator ==(Weight x, Weight y)
        {
            if (x.typeValue == y.typeValue)
            {
                if (x.typeValue == BinaryOperatorValueType || x.typeValue == UnaryOperatorValueType || x.typeValue == MultipleOperationValueType)
                {
                    if (x.arithmeticOperator == y.arithmeticOperator)
                    {
                        if (x.value == y.value)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (x.value == y.value)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Different Operator
        /// </summary>
        /// <param name="x">weight left</param>
        /// <param name="y">weight right</param>
        /// <returns>true if at least different</returns>
        public static bool operator !=(Weight x, Weight y)
        {
            if (x.typeValue == y.typeValue)
            {
                if (x.typeValue == BinaryOperatorValueType || x.typeValue == UnaryOperatorValueType || x.typeValue == MultipleOperationValueType)
                {
                    if (x.arithmeticOperator == y.arithmeticOperator)
                    {
                        if (x.value == y.value)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    if (x.value == y.value)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Equals overriden method
        /// </summary>
        /// <param name="obj">obj to compare</param>
        /// <returns>true or false</returns>
        public override bool Equals(object obj)
        {
            if (obj is Weight)
            {
                Weight w = obj as Weight;

                if (this.typeValue == w.typeValue)
                {
                    if (w.typeValue == BinaryOperatorValueType || w.typeValue == UnaryOperatorValueType || w.typeValue == MultipleOperationValueType)
                    {
                        if (this.arithmeticOperator == w.arithmeticOperator)
                        {
                            if (this.value == w.value)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (this.value == w.value)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            } 
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Get hash code
        /// </summary>
        /// <returns>hash code</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Equals two weights
        /// </summary>
        /// <param name="x">weight left</param>
        /// <param name="y">weight right</param>
        /// <returns>true if equals false elsewhere</returns>
        public bool Equals(IWeight x, IWeight y)
        {
            int hx, hy;
            hx = this.GetHashCode(x);
            hy = this.GetHashCode(y);
            if (hx == hy)
            {
                if (hashCodes.ContainsKey(hx) && hashCodes.ContainsKey(hy))
                {
                    return hashCodes[hx] == hashCodes[hy];
                }
                return true;

            }
            else return false;
        }

        /// <summary>
        /// Get hash Code from obj
        /// </summary>
        /// <param name="obj">weight</param>
        /// <returns>prior hash code</returns>
        public int GetHashCode(IWeight obj)
        {
            return obj.GetHashCode();
        }

        /// <summary>
        /// Obtain the equation function as string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.OwnerObject.ToString();
        }

        #endregion
    }
}
