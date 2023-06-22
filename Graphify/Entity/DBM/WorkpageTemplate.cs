using System;
using System.ComponentModel.DataAnnotations;

namespace Entity.DBM
{
    public class WorkpageTemplate : IEntity
    {
        [Key]
        public int PageID { get; set; }
        public int OwnerID { get; set; }
        public string PageName { get; set; }
        public string Identifier { get; set; }
        public string FilePath { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
