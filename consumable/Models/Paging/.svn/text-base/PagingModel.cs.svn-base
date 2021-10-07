using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models
{
    public class PagingModel
    {

        private static readonly int MAX_JUMP = 3;
        private int page = 0, rows = 0, count = 0, pages = 0, jumps = MAX_JUMP;
        private List<int> jump = new List<int>();

        private void CountPages()
        {
            if (rows > 0)
                pages = ((count / rows) + (((count % rows) > 0) ? 1 : 0));
            else
                pages = 1;
        }

        private void MakeJumps()
        {
            if (pages == 0)
                CountPages();

            jumps = Math.Min(MAX_JUMP, pages);
            jump.Clear();

            if (count <= MAX_JUMP)
            {
                for (int i = 0; i < jumps; i++)
                {
                    jump.Add((i + 1));
                }
            }
            else
            {
                int p1 = 0, p2 = 0;
                int mid = (MAX_JUMP / 2);

                if (page <= mid)
                {
                    p1 = 1;
                    p2 = Math.Min(MAX_JUMP, pages);
                }
                else
                {
                    p1 = Math.Max(Math.Min(pages - MAX_JUMP, page - mid), 1);
                    p2 = Math.Min(p1 + MAX_JUMP - 1, pages);
                }

                jumps = p2 - p1 + 1;
                for (int i = 0; i < jumps; i++)
                {
                    jump.Add((p1 + i));
                }
            }
        }


        public int CountData
        {
            get
            {
                return count;
            }

            set
            {
                count = value;
                CountPages();
            }

        }
        public int StartData
        {
            get
            {
                return (page - 1) * rows + 1;
            }
        }
        public int EndData
        {
            get
            {
                int e = StartData + rows - 1;
                if (e > count) e = count;
                return e;
            }
        }

        public int Page
        {
            get
            {
                return page;
            }

            set
            {
                page = value;
            }
        }

        public int RowsPerPage
        {
            get
            {
                return rows;
            }
        }

        public int Count
        {
            get
            {
                return count;
            }
        }

        public int PositionPage
        {
            get
            {
                return page;
            }

            set
            {
                page = value;
            }
        }

        public int Pages
        {
            get
            {
                return pages;
            }
        }

        public List<int> Indexes
        {
            get
            {
                return jump;
            }
        }

        public List<int> ListIndex
        {
            get
            {
                int jml = 0;
                if (rows > 0)
                    jml = ((count - 1) / rows) + 1;
                else
                    jml = 0;

                List<int> list = new List<int>();

                for (int i = 0; i < jml; i++)
                {
                    list.Add(i);
                }

                return list;

            }
        }

        public PagingModel(int countdata,
            int positionpage, int dataperpage)
        {
            page = positionpage;
            rows = dataperpage;
            count = countdata;

            CountPages();
            MakeJumps();
        }
    }
}
