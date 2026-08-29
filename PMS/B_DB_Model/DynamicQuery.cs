using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class DynamicQuery
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? AccessUsers { get; set; }
        public string? PrintTitle { get; set; }
        public string Title { get; set; }
        public string SqlQuery { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public List<QueryParam> Params { get; set; }
    }

    public class QueryParam
    {
        public int Id { get; set; }
        public int QueryId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public int SortOrder { get; set; }

        public string? DataSourceQuery { get; set; }

        public List<QueryParamOption> Options { get; set; } = new();
    }


    public class QueryParamOption
    {
        public int Id { get; set; }
        public int ParamId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
