using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020001AA RID: 426
	public class Pathfinder<TNode, TAgent> : Pathfinder where TNode : PFNode where TAgent : PFAgent
	{
		// Token: 0x060007DF RID: 2015 RVA: 0x00023EFC File Offset: 0x000220FC
		public List<TNode> FindPath(TNode start, TNode destination, TAgent agentData)
		{
			List<TNode> result;
			this.TryFindPath(start, destination, agentData, out result, null);
			return result;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00023F18 File Offset: 0x00022118
		public bool TryFindPath(TNode start, TNode destination, TAgent agentData, out List<TNode> path, InfluenceMap influenceMap = null)
		{
			path = new List<TNode>();
			this._destination = destination;
			List<TNode> list = new List<TNode>();
			foreach (TNode tnode in this.Map)
			{
				tnode.Reset();
			}
			list.Add(start);
			int num = 0;
			while (list.Count > 0 && num <= this.MaxIterationsTimeout)
			{
				num++;
				list.Sort((TNode x, TNode y) => x.GetTotalCost().CompareTo(y.GetTotalCost()));
				TNode tnode2 = list[0];
				list.Remove(tnode2);
				if (this.IsDestination(tnode2))
				{
					while (tnode2 != null)
					{
						path.Add(tnode2);
						if (tnode2 == start)
						{
							break;
						}
						tnode2 = (tnode2.PFParent as TNode);
						if (path.Contains(tnode2))
						{
							path.Clear();
							break;
						}
					}
					if (path.Count != 0)
					{
						break;
					}
				}
				else
				{
					foreach (TNode tnode3 in this.GetNeighbours(tnode2, destination, agentData))
					{
						if (tnode3 != null && Pathfinder<TNode, TAgent>.TryFillNeighbourCosts(this, tnode3, tnode2, (destination != null) ? destination : start, agentData))
						{
							list.Add(tnode3);
						}
					}
				}
			}
			path.Reverse();
			return path.Count > 0;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x000240D0 File Offset: 0x000222D0
		public static bool TryFillNeighbourCosts(Pathfinder pathfinder, PFNode neighbour, PFNode currentNode, PFNode heuristicTarget, TAgent agentData)
		{
			float num = neighbour.GetEdgeCost(currentNode);
			if (agentData != null)
			{
				num += agentData.GenerateCostModifierForNode(pathfinder, neighbour);
			}
			if (num < 0f)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					string format = "Pathfinding edge cost is: ({0}) cannot be lower than 0. Clamping node name: ({1})";
					object arg = num;
					GOAPNode goapnode = neighbour as GOAPNode;
					logger.Error(string.Format(format, arg, ((goapnode != null) ? goapnode.ActionName : null) ?? "Unnamed"));
				}
				num = 0f;
			}
			float num2 = currentNode.TraversalCost + num;
			if (neighbour.HasBeenAssignedCosts && neighbour.TraversalCost <= num2)
			{
				return false;
			}
			if (heuristicTarget != null)
			{
				float heuristic = neighbour.GetHeuristic(heuristicTarget);
				neighbour.HeuristicCost = heuristic;
			}
			neighbour.TraversalCost = num2;
			neighbour.PFParent = currentNode;
			neighbour.HasBeenAssignedCosts = true;
			return true;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0002418C File Offset: 0x0002238C
		public bool TryFindPathToAny(TNode start, IPathDestinationChecker<TNode> destinationChecker, TAgent agentData, out List<TNode> path)
		{
			foreach (TNode tnode in this.Map)
			{
				tnode.Reset();
			}
			path = new List<TNode>();
			List<TNode> list = new List<TNode>();
			list.Add(start);
			int num = 0;
			while (list.Count > 0 && num <= this.MaxIterationsTimeout)
			{
				num++;
				list.Sort((TNode x, TNode y) => x.TraversalCost.CompareTo(y.TraversalCost));
				TNode tnode2 = list[0];
				list.Remove(tnode2);
				if (destinationChecker.IsDestination(tnode2))
				{
					while (tnode2 != null)
					{
						path.Add(tnode2);
						if (tnode2 == start)
						{
							break;
						}
						tnode2 = (tnode2.PFParent as TNode);
						if (path.Contains(tnode2))
						{
							path.Clear();
							break;
						}
					}
					if (path.Count != 0)
					{
						break;
					}
				}
				else
				{
					foreach (TNode tnode3 in this.GetNeighbours(tnode2, agentData))
					{
						if (tnode3 != null && Pathfinder<TNode, TAgent>.TryFillNeighbourCosts(this, tnode3, tnode2, null, agentData))
						{
							list.Add(tnode3);
						}
					}
				}
			}
			path.Reverse();
			return path.Count > 0;
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00024324 File Offset: 0x00022524
		public virtual IEnumerable<TNode> GetNeighbours(TNode currentNode, TNode destination, TAgent agent)
		{
			return currentNode.SelectValidNeighbours(agent, destination).OfType<TNode>();
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00024342 File Offset: 0x00022542
		public virtual IEnumerable<TNode> GetNeighbours(TNode currentNode, TAgent agent)
		{
			return currentNode.SelectValidNeighbours(agent).OfType<TNode>();
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0002435A File Offset: 0x0002255A
		public virtual bool IsDestination(PFNode node)
		{
			return this._destination != null && this._destination == node;
		}

		// Token: 0x0400039B RID: 923
		public List<TNode> Map = new List<TNode>();

		// Token: 0x0400039C RID: 924
		public int MaxIterationsTimeout = 10000;

		// Token: 0x0400039D RID: 925
		private TNode _destination;
	}
}
