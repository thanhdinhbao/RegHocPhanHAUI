using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegHocPhanHAUI
{
    public class ClassByModules
    {
        public class Datum
        {
            public int IndependentClassID { get; set; }
            public string ModulesName { get; set; }
            public string ModulesCode { get; set; }
            public string ClassCode { get; set; }
            public string ClassName { get; set; }
            public int MaxStudent { get; set; }
            public int CountS { get; set; }
            public int Status { get; set; }
            public int IsLock { get; set; }
            public string GiaoVien { get; set; }
            public int Costs { get; set; }
            public string StartDate { get; set; }
            public string ListDate { get; set; }
            public string Credits { get; set; }
            public string BranchName { get; set; }
            public string Description { get; set; }
        }

        public class Root
        {
            public int err { get; set; }
            public string msg { get; set; }
            public int current { get; set; }
            public List<Datum> data { get; set; }
        }

    }
}
