using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common
{
    /// <summary>
    ///  Class for storing simple hierarchical structure 
    ///  the childless nodes considered to be 'leaf' 
    /// </summary>
    public class SimpleTree
    {
        /// they say encapsulate - hide your privates 
        /// but I prefer it this way - no useless getter setter for simple field
        public SimpleTree Prev = null;
        public SimpleTree Next = null;
        public SimpleTree Parent = null;
        public SimpleTree Child = null;

        public int ID { get; set; }
        public string Text { get; set; }
        public object Item { get; set; }        

        /// <summary>
        /// Default shorthand for creating 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="id"></param>
        public SimpleTree(string text = null, int id = 0, object item=null)
        {
            ID = id;
            Text = text;
            Item = item;            
        }



        /// <summary>
        /// Get First element of the same level  
        /// </summary>
        public SimpleTree First
        {
            get
            {
                SimpleTree x = this;
                while (x.Prev != null)
                {
                    x = x.Prev;
                }
                return x;
            }
        }

        /// <summary>
        ///  Get Last Element of the same level 
        /// </summary>
        public SimpleTree Last
        {
            get
            {
                SimpleTree x = this;
                while (x.Next != null)
                {
                    x = x.Next;
                }
                return x;
            }
        }

        /// <summary>
        ///  add sibling to last node 
        /// </summary>
        /// <param name="x"></param>
        public void Add(SimpleTree x)
        {
            SimpleTree z = Last;

            if (z.Next == null)
            {
                z.Next = x;

                x.Prev = z;
                x.Parent = z.Parent;
                
            }
        }

        /// <summary>
        ///  Multiply, to reproduce, to make children
        ///  add child node
        /// </summary>
        /// <param name="x"></param>
        public void Mul(SimpleTree x)
        {
            if (Child == null)
            {
                Child = x;
                x.Parent = this;
            }
            else
            {
                Child.Add(x);
            }
            
        }

        /// <summary>
        /// Shorthand for object Adding to last node sibling on current node 
        /// </summary>
        /// <param name="text">Node Text</param>
        /// <param name="id">Node ID, if not given, generated from next available ID </param>
        /// <returns>Added Node</returns>
        public SimpleTree Add(string text, int id = 0, object item = null)
        {
            SimpleTree x = new SimpleTree(text, this.GetID(id), item);
            Add(x);
            return x;
        }

        /// <summary>
        /// Shorthand for object Multiplying
        /// </summary>
        /// <param name="text">Node Text</param>
        /// <param name="id">Node ID, when not given, generated from next available ID</param>
        /// <returns>Multiplied Node</returns>
        public SimpleTree Mul(string text, int id = 0, object item = null)
        {
            SimpleTree x = new SimpleTree(text, this.GetID(id), item);

            Mul(x);

            return x;
        }       

    } // class SimpleTree

}
