using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020001A5 RID: 421
	public abstract class PFNode
	{
		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x00023CEB File Offset: 0x00021EEB
		// (set) Token: 0x060007C4 RID: 1988 RVA: 0x00023CF3 File Offset: 0x00021EF3
		public float BaseCost { get; protected set; } = 1f;

		// Token: 0x060007C5 RID: 1989 RVA: 0x00023CFC File Offset: 0x00021EFC
		public void Disable(string reason)
		{
			this.DisabledContext = reason;
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00023D05 File Offset: 0x00021F05
		public void Enable()
		{
			this.DisabledContext = null;
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x00023D0E File Offset: 0x00021F0E
		public virtual bool IsDisabled()
		{
			return this.DisabledContext != null;
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00023D19 File Offset: 0x00021F19
		public virtual bool ShouldIncludeInPath()
		{
			return true;
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00023D1C File Offset: 0x00021F1C
		public void Reset()
		{
			this.PFParent = null;
			this.HasBeenAssignedCosts = false;
			this.HeuristicCost = 0f;
			this.TraversalCost = 0f;
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00023D42 File Offset: 0x00021F42
		public void AddNeighbour(PFNode neighbour)
		{
			this.Neighbours.Add(neighbour);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00023D51 File Offset: 0x00021F51
		public float GetHeuristic(PFNode destination)
		{
			return this.Heuristic(destination);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x00023D5A File Offset: 0x00021F5A
		public float GetTotalCost()
		{
			return this.TraversalCost + this.HeuristicCost;
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00023D69 File Offset: 0x00021F69
		public void AddRawScalarCostModifier(PFCostModifier modifier, float amount)
		{
			this.CostModifiers[(int)modifier] += amount;
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x00023D7C File Offset: 0x00021F7C
		public virtual float GetEdgeCost(PFNode fromNode = null)
		{
			float num = 0f;
			int num2 = 0;
			for (int i = 0; i < 4; i++)
			{
				float num3 = this.CostModifiers[i];
				if (Math.Abs(num3) > 0f)
				{
					num += num3;
					num2++;
				}
			}
			float num4 = 1f;
			if (num2 > 0)
			{
				num /= (float)num2;
				num4 = Math.Max(0f, num4 + num);
			}
			return this.BaseCost * num4;
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00023DE3 File Offset: 0x00021FE3
		public void ClearAllModifiers()
		{
			ArrayExtensions.Clear<float>(this.CostModifiers);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00023DF0 File Offset: 0x00021FF0
		public virtual IEnumerable<PFNode> GetNeighbours()
		{
			return this.Neighbours;
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x00023DF8 File Offset: 0x00021FF8
		public virtual IEnumerable<PFNode> SelectValidNeighbours(PFAgent agentData, PFNode finalDestination)
		{
			return from t in this.GetNeighbours()
			where t.IsValidNeighbour(agentData, this)
			select t;
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x00023E30 File Offset: 0x00022030
		public virtual IEnumerable<PFNode> SelectValidNeighbours(PFAgent agentData)
		{
			return from t in this.GetNeighbours()
			where t.IsValidNeighbour(agentData, this)
			select t;
		}

		// Token: 0x060007D3 RID: 2003
		public abstract float Heuristic(PFNode destination);

		// Token: 0x060007D4 RID: 2004
		public abstract bool IsValidNeighbour(PFAgent agent, PFNode callingNode);

		// Token: 0x04000390 RID: 912
		private const int CostModifierCount = 4;

		// Token: 0x04000392 RID: 914
		public readonly float[] CostModifiers = new float[4];

		// Token: 0x04000393 RID: 915
		public PFNode PFParent;

		// Token: 0x04000394 RID: 916
		public bool HasBeenAssignedCosts;

		// Token: 0x04000395 RID: 917
		public float HeuristicCost;

		// Token: 0x04000396 RID: 918
		public float TraversalCost;

		// Token: 0x04000397 RID: 919
		public HashSet<PFNode> Neighbours = new HashSet<PFNode>();

		// Token: 0x04000398 RID: 920
		public string DisabledContext;
	}
}
