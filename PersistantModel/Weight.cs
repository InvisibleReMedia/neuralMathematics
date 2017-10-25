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
    public class Weight : PersistentDataObject, IWeight, IEqualityComparer<Weight>
    {

        #region Fields

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
        /// Index name of value type of this weight
        /// </summary>
        private static readonly string typeValueName = "typeValue";
        /// <summary>
        /// Index name for operator or function
        /// </summary>
        private static readonly string arithmeticOperatorName = "arithmeticOperator";
        /// <summary>
        /// Index name for specific data support
        /// </summary>
        private static readonly string valueName = "value";
        /// <summary>
        /// Index value for hash code
        /// </summary>
        private static readonly string hashCodeName = "hash";

        /// <summary>
        /// The current object that's correspond to itself
        /// </summary>
        private IArithmetic ownerObject;

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
        /// Gets the recorded object
        /// </summary>
        public IArithmetic OwnerObject
        {
            get
            {
                return this.ownerObject;
            }
        }

        /// <summary>
        /// Gets the hash code number
        /// </summary>
        public int HashCode
        {
            get
            {
                return this[hashCodeName];
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

        #endregion

        #region Methods

        /// <summary>
        /// Activate record zone
        /// </summary>
        /// <param name="sender">origin</param>
        /// <param name="e">args</param>
        private void Owner_Fetch(object sender, EventArgs e)
        {
            RecordZone<Weight> recordZone = sender as RecordZone<Weight>;
            int h = this[hashCodeName];
            foreach(Weight w in recordZone.Records)
            {
                if (w == this)
                {
                    h = w.GetHashCode();
                    break;
                }
            }
            if (!recordZone.Exists(h))
            {
                recordZone.Add(h, this);
            }
            else
            {
                Weight recorded = recordZone.Ask(h);
                if (recorded != this)
                    throw new InvalidCastException(String.Format("Weight {0} is not equals to Weight {1}", recorded.ToString(), this.ToString()));
                else
                {
                    this.ownerObject = recorded.ownerObject;
                    this[hashCodeName] = h;
                }
            }
        }

        /// <summary>
        /// Deactivate record zone
        /// </summary>
        /// <param name="sender">origin</param>
        /// <param name="e">args</param>
        private void Owner_Unfetch(object sender, EventArgs e)
        {
        }

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
            this[typeValueName] = type;
            this[arithmeticOperatorName] = op;
            this[valueName] = value;
            this[hashCodeName] = owner.GetHashCode();
            this.ownerObject = owner;
            owner.Fetch += Owner_Fetch;
            owner.Unfetch += Owner_Unfetch;

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
            if (!Object.Equals(x, null) && !Object.Equals(y, null))
            {
                if (x[typeValueName] == y[typeValueName])
                {
                    if (x[typeValueName] == BinaryOperatorValueType || x[typeValueName] == UnaryOperatorValueType || x[typeValueName] == MultipleOperationValueType)
                    {
                        if (x[arithmeticOperatorName] == y[arithmeticOperatorName])
                        {
                            if (x[valueName] == y[valueName])
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
                        if (x[valueName] == y[valueName])
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
            else if (Object.Equals(x, null) && Object.Equals(y, null))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Different Operator
        /// </summary>
        /// <param name="x">weight left</param>
        /// <param name="y">weight right</param>
        /// <returns>true if at least different</returns>
        public static bool operator !=(Weight x, Weight y)
        {
            if (!Object.Equals(x, null) && !Object.Equals(y, null))
            {
                if (x[typeValueName] == y[typeValueName])
                {
                    if (x[typeValueName] == BinaryOperatorValueType || x[typeValueName] == UnaryOperatorValueType || x[typeValueName] == MultipleOperationValueType)
                    {
                        if (x[arithmeticOperatorName] == y[arithmeticOperatorName])
                        {
                            if (x[valueName] == y[valueName])
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
                        if (x[valueName] == y[valueName])
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
            else if (Object.Equals(x, null) && Object.Equals(y, null))
                return false;
            else
                return true;
        }

        /// <summary>
        /// Equals overriden method
        /// </summary>
        /// <param name="obj">obj to compare</param>
        /// <returns>true or false</returns>
        public override bool Equals(object obj)
        {
            if (obj != null && obj is Weight)
            {
                Weight w = obj as Weight;

                if (this[typeValueName] == w[typeValueName])
                {
                    if (w[typeValueName] == BinaryOperatorValueType || w[typeValueName] == UnaryOperatorValueType || w[typeValueName] == MultipleOperationValueType)
                    {
                        if (this[arithmeticOperatorName] == w[arithmeticOperatorName])
                        {
                            if (this[valueName] == w[valueName])
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
                        if (this[valueName] == w[valueName])
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
            return this[hashCodeName];
        }

        /// <summary>
        /// Equals two weights
        /// </summary>
        /// <param name="x">weight left</param>
        /// <param name="y">weight right</param>
        /// <returns>true if equals false elsewhere</returns>
        public bool Equals(Weight x, Weight y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Get hash Code from obj
        /// </summary>
        /// <param name="obj">weight</param>
        /// <returns>prior hash code</returns>
        public int GetHashCode(Weight obj)
        {
            return obj.GetHashCode();
        }

        /// <summary>
        /// Obtain the equation function as string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ownerObject.ToString();
        }

        #endregion

    }
}
