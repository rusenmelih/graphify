using Core.Graphify;
using Core.Result;
using Entity.DBM;
using Entity.FTM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IWorkPageService
    {
        Task<Result<string?>> NewWorkPage(int ownerID, string pageName);
        Task<Result<string?>> CreateNode(CreateNodeRequest node);
        Task<Result<string?>> CreateEdge(CreateEdgeRequest edge);
        Task<Result<List<WorkpageTemplate>?>> GetWorkpageTemplates(int ownerID);
        Task<Result<WorkPage?>> GetWorkpageByIdentifier(string identifier);
        Result<List<Node>?> RunGeneticAlgorithm(string identifier);
    }
}
