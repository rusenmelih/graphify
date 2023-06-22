using DataAccess.Abstract;
using Entity.DBM;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Provider
{
    public class WorkPageDataAccessService : IWorkPageDataAccessService
    {
        public async Task<ProcedureResult?> CreateNewWorkPage(int ownerID, string identifier, string pageName, string path)
        {
            using (GraphifyDb db = new GraphifyDb())
                return (await db.ProcedureResults.FromSqlRaw("EXECUTE CreateWorkPage @ownerID, @identifier, @pageName, @path",
                    new SqlParameter("ownerID", ownerID),
                    new SqlParameter("identifier", identifier),
                    new SqlParameter("pageName", pageName),
                    new SqlParameter("path", path)).ToListAsync()).FirstOrDefault();
        }

        public async Task<List<WorkpageTemplate>> GetWorkpageTemplates(int ownerID)
        {
            using (GraphifyDb db = new GraphifyDb())
                return (await db.WorkpageTemplates.FromSqlRaw("EXECUTE GetWorkPages @ownerID",
                    new SqlParameter("ownerID", ownerID)).ToListAsync());
        }

        public async Task<WorkpageTemplate?> GetWorkpageTemplateByIdentifier(string identifier)
        {
            using (GraphifyDb db = new GraphifyDb())
                return (await db.WorkpageTemplates.FromSqlRaw("EXECUTE GetWorkpage @identifier",
                    new SqlParameter("identifier", identifier)).ToListAsync()).FirstOrDefault();
        }
    }
}
