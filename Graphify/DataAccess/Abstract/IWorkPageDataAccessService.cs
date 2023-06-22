using Entity.DBM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IWorkPageDataAccessService
    {
        /// <summary>
        /// Yeni work page oluşturur.
        /// </summary>
        /// <param name="ownerID">Workpage UserID.</param>
        /// <param name="identifier">Benzersiz workpage key.</param>
        /// <param name="pageName">Workpage adı.</param>
        /// <param name="path">Dosya yolu.</param>
        Task<ProcedureResult?> CreateNewWorkPage(int ownerID, string identifier, string pageName, string path);
        Task<List<WorkpageTemplate>> GetWorkpageTemplates(int ownerID);
        Task<WorkpageTemplate?> GetWorkpageTemplateByIdentifier(string identifier);
    }
}
