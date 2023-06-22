using Business.Abstract;
using Core.Graphify;
using Core.Graphify.Algorithms;
using Core.Result;
using DataAccess.Abstract;
using Entity.DBM;
using Entity.FTM;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Business.Service
{
    public class WorkPageService : IWorkPageService
    {
		//private List<WorkPage> WorkPages = new List<WorkPage>();

		private readonly IWorkPageDataAccessService workPageDataAccessService;
		private readonly GraphifySheet sheet;

        public WorkPageService(IWorkPageDataAccessService workPageDataAccessService, GraphifySheet sheet)
        {
			this.workPageDataAccessService = workPageDataAccessService;
			this.sheet = sheet;
        }

		private async Task UpdateFile(WorkPage page)
		{
			using (StreamWriter write = new StreamWriter(page.Path))
			{
				string data = JsonConvert.SerializeObject(page);
				await write.WriteAsync(data);
				write.Close();
			}
		}

        public async Task<Result<string?>> NewWorkPage(int ownerID, string pageName)
        {
			try
			{
				WorkPage page = new WorkPage(ownerID, Guid.NewGuid().ToString(), pageName);


				var exec = await this.workPageDataAccessService.CreateNewWorkPage(ownerID, page.Identifier, page.PageName, page.Path);
				if (exec == null || !exec.Success)
					return new Result<string?>(false, null, "New workpage could not created.");

				this.sheet.WorkPages.Add(page);

				using (StreamWriter write = new StreamWriter(page.Path))
				{
					await write.WriteAsync(JsonConvert.SerializeObject(page));
					write.Close();
				}

                return new Result<string?>(true, page.Identifier, "New workpage created successfully.");
            }
			catch (Exception)
			{
                return new Result<string?>(false, null, "Server internal error.");
            }
        }

		public async Task<Result<string?>> CreateNode(CreateNodeRequest node)
		{
			try
			{
				//Şuanlık -- Her bir kullanıcı her workpage'e erişebilir.
				//No authority.

				//Performans arttırmak için ilerde dictionary ile identifier-index key-value-pair yapılabilir.
				//Page bul.
				WorkPage? page = this.sheet.WorkPages.Find(page => page.Identifier == node.Identifier);

				if (page == null) //Page bulunamadıysa hata yolla.
					return new Result<string?>(false, null, "Workpage not found.");

				//Aynı isimde node'a izin verilmez.
				int findIndex = page.Nodes.FindIndex(n => n.Name == node.NodeName.ToLower());
				if (findIndex != -1)
					return new Result<string?>(false, node.NodeName.ToLower(), "Specified node name is already available.");

				//NodeType 1,2,3 tipten biri olabilir.
				
				//Node oluştur.
				Node newNode = new Node(node.NodeName.ToLower());
				newNode.Color = node.Color;
				newNode.Position = new Vector2D(node.PositionX, node.PositionY);
				newNode.Radius = node.Radius;
				newNode.Type = (NodeType)node.NodeType;
				newNode.LineWidth = node.LineWidth;
				newNode.LineColor = node.LineColor;
				newNode.Coordinate = new GeoCoordinatePortable.GeoCoordinate(
					((node.Latitude.HasValue) ? node.Latitude.Value : 0),
					((node.Longitude.HasValue) ? node.Longitude.Value : 0),
					((node.Altitude.HasValue) ? node.Altitude.Value : 0), 1, 1, 0, 0);

				page.Nodes.Add(newNode);

				await this.UpdateFile(page);

				return new Result<string?>(true, newNode.Name, "Node created successfully.");
			}
			catch (Exception)
			{
				return new Result<string?>(false, null, "Server internal error.");
			}
		}
		public async Task<Result<string?>> CreateEdge(CreateEdgeRequest edge)
		{
			try
			{
				WorkPage? page = this.sheet.WorkPages.Find(page => page.Identifier == edge.Identifier);

                if (page == null) //Page bulunamadıysa hata yolla.
                    return new Result<string?>(false, null, "Workpage not found.");

				//Aynı isimde edge'e izin verilemez.
				edge.Name = edge.Name.Trim().ToLower();
				int findIndex = page.Edges.FindIndex(p => p.Name == edge.Name);
				if (findIndex != -1)
					return new Result<string?>(false, edge.Name, "Specified edge name is already available.");

				//kaynak ve hedefi bul.
				Node? source = page.Nodes.Find(p => p.Name == edge.SourceNodeName);
				Node? destination = page.Nodes.Find(p => p.Name == edge.DestinationNodeName);

				if (source == null || destination == null)
					return new Result<string?>(false, null, "Source or destination node could not found.");

				//Aynı source ve destination'dan bir edge tanımlı mı?

				//Edge oluştur.
				Edge newEdge = new Edge(source, destination, edge.Name);
				newEdge.Color = edge.Color;
				newEdge.Weight = edge.Weight;
				newEdge.Width = edge.Width;

				page.Edges.Add(newEdge);
				//source.Edges.Add(newEdge);

                await this.UpdateFile(page);

                return new Result<string?>(true, newEdge.Name, "Edge created successfully.");
            }
			catch (Exception)
			{
				return new Result<string?>(false, null, "Server internal error.");
			}
		}

		public async Task<Result<List<WorkpageTemplate>?>> GetWorkpageTemplates(int ownerID)
		{
			try
			{
				List<WorkpageTemplate> templates = await this.workPageDataAccessService.GetWorkpageTemplates(ownerID);
				return new Result<List<WorkpageTemplate>?>(true, templates, "Workpages retrived successfully.");
			}
			catch (Exception)
			{
				//Log alınabilir.
				return new Result<List<WorkpageTemplate>?>(false, null, "Server internal error.");
			}
		}

		public async Task<Result<WorkPage?>> GetWorkpageByIdentifier(string identifier)
		{
			//try
			{
				WorkpageTemplate? template = await this.workPageDataAccessService.GetWorkpageTemplateByIdentifier(identifier);
				if (template == null)
					return new Result<WorkPage?>(false, null, "Workpage could not found.");

				FileInfo file = new FileInfo(template.FilePath);
				if (!file.Exists || file.Length == 0)
					return new Result<WorkPage?>(false, null, "Workpage is empty.");

				using (StreamReader read = new StreamReader(template.FilePath))
				{
					string data = await read.ReadToEndAsync();
					if (data == null)
						return new Result<WorkPage?>(false, null, "Server internal error.");

					//WorkPage? page = (WorkPage?)JsonConvert.DeserializeObject(data);
					JObject obj = JObject.Parse(data);
					WorkPage? page = obj.ToObject<WorkPage?>();

					var include = this.sheet.WorkPages.Find(p => p.Identifier == page?.Identifier);
					if (include == null)
						this.sheet.WorkPages.Add(page);
					
					return new Result<WorkPage?>(true, page, "Workpage retrived successfully.");
                }
			}
			//catch (Exception)
			{
                return new Result<WorkPage?>(false, null, "Server internal error.");
            }
		}

		public Result<List<Node>?> RunGeneticAlgorithm(string identifier)
		{
			try
			{

				WorkPage? page = this.sheet.WorkPages.Find(p => p.Identifier == identifier);
				if (page == null)
					return new Result<List<Node>?>(false, null, "Workpage could not found.");

				Node? startNode = page.Nodes.Find(p => p.Type == NodeType.ChargingStationNode);
				Node? endNode = page.Nodes.Find(p => p.Type == NodeType.DepotNode);

				if (startNode == null || endNode == null)
					return new Result<List<Node>?>(false, null, "Start/end node could not found.");

				//GeneticAlgorithm algorithm = new GeneticAlgorithm(page.Nodes, page.Edges, 100, 0.05, 1000);
				//List<Node> route = algorithm.Solve(startNode, endNode);

				DijkstraAlgorithm dijkstra = new DijkstraAlgorithm(page.Nodes, page.Edges);
				var route = dijkstra.FindShortestPath(startNode, endNode);

				return new Result<List<Node>?>(true, route, "Success");

			}
			catch (Exception)
			{
				return new Result<List<Node>?>(false, null, "Internal server error.");
			}
		}
    }
}
