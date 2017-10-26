using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Class to gather all records
    /// into a graph of shared nodes
    /// </summary>
    /// <typeparam name="T">Persistent object</typeparam>
    public class Gathering<T> : Arithmetic where T : Weight
    {

        #region Fields

        /// <summary>
        /// Records
        /// </summary>
        private RecordZone<T> records;

        /// <summary>
        /// List of terms not visited and already visited
        /// </summary>
        private List<T> opened, closed;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="r">records</param>
        /// <param name="start">start</param>
        public Gathering(RecordZone<T> r, T start)
        {
            this.records = r;
            this.opened = new List<T>(r.Records);
            this.closed = new List<T>();
            this.Gather(start);
        }

        #endregion

        #region Methods

        private void Gather(T element)
        {
            this.opened.Remove(element);
            this.closed.Add(element);

            if (element.OwnerObject is BinaryOperation)
            {
                BinaryOperation bin = element.OwnerObject as BinaryOperation;
                int h = bin.LeftOperand.OwnerWeight.HashCode;
                T next = this.opened.Find(x => x.HashCode == h);
                if (next != null)
                {
                    this.Gather(next);
                    bin[leftTermName] = next.OwnerObject;
                }
                else
                {
                    next = this.closed.Find(x => x.HashCode == h);
                    if (next != null)
                        bin[leftTermName] = next.OwnerObject;
                }
                h = bin.RightOperand.OwnerWeight.HashCode;
                next = this.opened.Find(x => x.HashCode == h);
                if (next != null)
                {
                    this.Gather(next);
                    bin[rightTermName] = next.OwnerObject;
                }
                else
                {
                    next = this.closed.Find(x => x.HashCode == h);
                    if (next != null)
                        bin[rightTermName] = next.OwnerObject;
                }

            }
            else if (element.OwnerObject is UnaryOperation)
            {
                UnaryOperation u = element.OwnerObject as UnaryOperation;
                int h = u.InnerOperand.OwnerWeight.HashCode;
                T next = this.opened.Find(x => x.HashCode == h);
                if (next != null)
                {
                    this.Gather(next);
                    u[innerOperandName] = next.OwnerObject;
                }
                else
                {
                    next = this.closed.Find(x => x.HashCode == h);
                    if (next != null)
                        u[innerOperandName] = next.OwnerObject;
                }
            }
            else if (element.OwnerObject is Term)
            {
                Term t = element.OwnerObject as Term;
                int h = t.Constant.OwnerWeight.HashCode;
                T next = this.opened.Find(x => x.HashCode == h);
                if (next != null)
                {
                    this.Gather(next);
                    t[constantName] = next.OwnerObject;
                }
                else
                {
                    next = this.closed.Find(x => x.HashCode == h);
                    if (next != null)
                        t[constantName] = next.OwnerObject;
                }

                h = t.Coefficient.OwnerWeight.HashCode;
                next = this.opened.Find(x => x.HashCode == h);
                if (next != null)
                {
                    this.Gather(next);
                    t[coefName] = next.OwnerObject;
                }
                else
                {
                    next = this.closed.Find(x => x.HashCode == h);
                    if (next != null)
                        t[coefName] = next.OwnerObject;
                }

                h = t.Unknown.OwnerWeight.HashCode;
                next = this.opened.Find(x => x.HashCode == h);
                if (next != null)
                {
                    this.Gather(next);
                    t[unknownName] = next.OwnerObject;
                }
                else
                {
                    next = this.closed.Find(x => x.HashCode == h);
                    if (next != null)
                        t[unknownName] = next.OwnerObject;
                }
            }
            else if (element.OwnerObject is Sum)
            {
                Sum s = element.OwnerObject as Sum;
                for(int index = 0; index < s.Items.Count(); ++index)
                {
                    int h = s.Items.ElementAt(index).OwnerWeight.HashCode;
                    T next = this.opened.Find(x => x.HashCode == h);
                    if (next != null)
                    {
                        this.Gather(next);
                        s.Replace(s.Items.ElementAt(index), next.OwnerObject);
                    }
                    else
                    {
                        next = this.closed.Find(x => x.HashCode == h);
                        if (next != null)
                            s.Replace(s.Items.ElementAt(index), next.OwnerObject);
                    }
                }
            }
            else if (element.OwnerObject is Product)
            {
                Product p = element.OwnerObject as Product;
                for (int index = 0; index < p.Items.Count(); ++index)
                {
                    int h = p.Items.ElementAt(index).OwnerWeight.HashCode;
                    T next = this.opened.Find(x => x.HashCode == h);
                    if (next != null)
                    {
                        this.Gather(next);
                        p.Replace(p.Items.ElementAt(index), next.OwnerObject);
                    }
                    else
                    {
                        next = this.closed.Find(x => x.HashCode == h);
                        if (next != null)
                            p.Replace(p.Items.ElementAt(index), next.OwnerObject);
                    }
                }
            }
        }

        #endregion

    }
}
