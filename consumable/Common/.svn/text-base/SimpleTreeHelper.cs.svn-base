using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common
{
    public static class SimpleTreeHelper
    {

        /// <summary>
        /// get all sibling including current node  
        /// </summary>
        /// <returns></returns>
        public static List<SimpleTree> Kin(this SimpleTree t)
        {
            SimpleTree y = t.First;
            List<SimpleTree> b = new List<SimpleTree>();
            while (y != null)
            {
                b.Add(y);
                y = y.Next;
            }
            return b;
        }

        public static int Kins(this SimpleTree t)
        {
            SimpleTree y = t.First;
            int r = 0;
            while (y != null)
            {
                r++;

                y = y.Next;
            }
            return r;

        }

        public static int Depth(this SimpleTree t)
        {
            int r = 0;
            SimpleTree a = t;
            while (a.Parent != null)
            {
                ++r;
                a = a.Parent;
            }
            return r;
        }

        public static int PreCount(this SimpleTree t)
        {
            int r = 0;
            SimpleTree a = t;
            while (a.Prev != null)
            {
                ++r;
                a = a.Prev;
            }
            return r;
        }

        public static int NextCount(this SimpleTree t)
        {
            int r = 0;
            SimpleTree a = t;
            while (a.Next != null)
            {
                ++r;
                a = a.Next;
            }
            return r;
        }


        public static int Childs(this SimpleTree t)
        {
            SimpleTree y = t.Child;
            int r = 0;
            while (y != null)
            {
                r++;

                y = y.Next;
            }
            return r;
        }

        /// <summary>
        /// get child node of current node  
        /// </summary>
        /// <returns></returns>
        public static List<SimpleTree> Children(this SimpleTree t)
        {
            List<SimpleTree> b = null;
            SimpleTree x = t.Child;
            if (x != null)
                b = new List<SimpleTree>();
            while (x != null)
            {
                b.Add(x);
                x = x.Next;
            }
            return b;
        }


        
        public static void Bore(this SimpleTree t, Chop method, int eager = 0)
        {
            Stack<SimpleTree> s = new Stack<SimpleTree>();
            Stack<SimpleTree> u = new Stack<SimpleTree>();
            s.Push(t);

            SimpleTree x = null;

            while (s.Count > 0)
            {
                x = s.Pop();
                if (eager == 1)
                    method(x);
                else
                    u.Push(x);

                if (eager == 0)
                {
                    x = x.Child;

                    while (x != null)
                    {
                        s.Push(x);
                        x = x.Next;
                    }

                }
                else
                {
                    x = x.Child;
                    if (x != null) x = x.Last;
                    while (x != null)
                    {
                        s.Push(x);
                        x = x.Prev;
                    }
                }
            }

            if (eager == 0)
            {
                while ((x = u.Pop()) != null)
                {
                    method(x);
                }
            }

        }

        public static void Walk(this SimpleTree t, Chop method)
        {
            Stack<SimpleTree> s = new Stack<SimpleTree>();
            s.Push(t.Root());

            SimpleTree x = null;

            while (s.Count > 0)
            {
                x = s.Pop();
                method(x);
                SimpleTree l;
                l = x.Next;
                if (l != null)
                    s.Push(l);
                l = x.Child;
                if (l != null)
                    s.Push(l);
            }
        }

        public static SimpleTree Root(this SimpleTree t)
        {
            while (t.Parent != null)
            {
                t = t.Parent;
            }
            return t;
        }

        public static int GetMax(this SimpleTree t)
        {
            Stack<SimpleTree> s = new Stack<SimpleTree>();
            s.Push(t.Root());

            SimpleTree x = null;
            int Max = -1;
            while (s.Count > 0)
            {
                x = s.Pop();
                if (x.ID > Max)
                    Max = x.ID;

                SimpleTree l;
                l = x.Next;
                if (l != null)
                    s.Push(l);
                l = x.Child;
                if (l != null)
                    s.Push(l);
            }
            return Max;
        }

        public static int GetID(this SimpleTree t, int id = 0)
        {
            return (id < 1) ? (t.GetMax() + 1) : id;
        }

        public static void Del(this SimpleTree t)
        {
            Bore(t, Delete);
        }

        public static void Delete(SimpleTree t)
        {
            SimpleTree r = t.Root();
            if (t == r)
                return;

            SimpleTree n = t.Prev;
            if (n != null)
                n.Next = t.Next;
        }

        /// <summary>
        /// find element by path  
        /// </summary>
        /// <param name="path">string path</param>
        /// <param name="fromRoot">traverse from root or from current node</param>
        /// <returns>node containing text in path separated by "/" slash</returns>
        public static SimpleTree Go(this SimpleTree t, string path)
        {
            string[] p = path.Split(new string[] { "/" }, StringSplitOptions.None);
            SimpleTree x = t;
            
            for (int level = 0; level <  p.Length ; level++)
            {
                string px = p[level];
                if (level == 0)
                {
                    if (px.isEmpty())
                    {
                        x = x.Root();
                    }
                    x = x.Child;
                    if (level == 0 && px.isEmpty()) continue;
                }
                bool found = false;
                while (x != null && !found)
                {
                    if (string.Compare(x.Text, px, true) != 0)
                    {
                        x = x.Next;
                    }
                    else
                    {
                        if  (level < p.Length -1)
                            x = x.Child;
                        found = true;
                    }
                }
                if (!found)
                {
                    x = null;
                }
            }
            return x;
        }

        /// <summary>
        /// find text contained in path 
        /// </summary>
        /// <param name="path">path of search value inside nodes separated by "/" slash</param>
        /// <param name="fromRoot">traverse from root whentrue or from current node when false</param>
        /// <returns>value of last leaf</returns>
        public static string GetText(this SimpleTree t, string path, bool fromRoot = false)
        {
            SimpleTree x = null;
            if (fromRoot)
                x = t.Root();
            else
                x = t;

            x = x.Go(path);

            return (x != null) ? x.Text : null;
        }
    }


    /// <summary>
    /// SimpleTree delegate executed while iterating through nested SimpleTree
    /// using SimpleTreeHelper
    /// </summary>
    /// <param name="s"></param>
    public delegate void Chop(SimpleTree s);
}
