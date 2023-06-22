using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Graphify
{
    public class GraphifySheet
    {
        public List<WorkPage> WorkPages { get; set; }

        public GraphifySheet()
        {
            this.WorkPages = new List<WorkPage>();
        }

        //private static GraphifySheet instance;
        //private List<WorkPage> workpages;

        //private GraphifySheet()
        //{
        //    this.workpages = new List<WorkPage>();
        //}

        //public static GraphifySheet Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new GraphifySheet();
        //        }
        //        return instance;
        //    }
        //}

        //public List<WorkPage> WorkPages
        //{
        //    get { return workpages; }
        //}
    }
}
