using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200047E RID: 1150
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class Cost : ResourceAccumulation, IDeepClone<Cost>
	{
		// Token: 0x17000309 RID: 777
		// (get) Token: 0x0600154D RID: 5453 RVA: 0x00050833 File Offset: 0x0004EA33
		[JsonIgnore]
		public static Cost None
		{
			get
			{
				return new Cost();
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x0600154E RID: 5454 RVA: 0x0005083A File Offset: 0x0004EA3A
		[JsonIgnore]
		public override bool IsZero
		{
			get
			{
				return this.RequiredTokenCount == 0 && base.IsZero;
			}
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x0005084C File Offset: 0x0004EA4C
		public Cost()
		{
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x00050854 File Offset: 0x0004EA54
		public Cost(params ResourceAccumulation[] others) : base(others)
		{
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x0005085D File Offset: 0x0004EA5D
		public Cost(int requiredTokenCount)
		{
			this.RequiredTokenCount = requiredTokenCount;
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x0005086C File Offset: 0x0004EA6C
		public Cost(CostStaticData data)
		{
			if (data != null)
			{
				base[ResourceTypes.Souls] = data.Soul;
				base[ResourceTypes.Ichor] = data.Ichor;
				base[ResourceTypes.Hellfire] = data.Hellfire;
				base[ResourceTypes.Darkness] = data.Darkness;
				base[ResourceTypes.Prestige] = data.Prestige;
			}
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x000508C9 File Offset: 0x0004EAC9
		public static implicit operator Cost(in CostStaticData data)
		{
			return new Cost(data);
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x000508D4 File Offset: 0x0004EAD4
		public static Cost operator +(Cost a, Cost b)
		{
			Cost cost = new Cost();
			cost[ResourceTypes.Souls] = a[ResourceTypes.Souls] + b[ResourceTypes.Souls];
			cost[ResourceTypes.Ichor] = a[ResourceTypes.Ichor] + b[ResourceTypes.Ichor];
			cost[ResourceTypes.Hellfire] = a[ResourceTypes.Hellfire] + b[ResourceTypes.Hellfire];
			cost[ResourceTypes.Darkness] = a[ResourceTypes.Darkness] + b[ResourceTypes.Darkness];
			cost[ResourceTypes.Prestige] = a[ResourceTypes.Prestige] + b[ResourceTypes.Prestige];
			return cost;
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x00050954 File Offset: 0x0004EB54
		public static Cost operator -(Cost a, Cost b)
		{
			Cost cost = new Cost();
			cost[ResourceTypes.Souls] = a[ResourceTypes.Souls] - b[ResourceTypes.Souls];
			cost[ResourceTypes.Ichor] = a[ResourceTypes.Ichor] - b[ResourceTypes.Ichor];
			cost[ResourceTypes.Hellfire] = a[ResourceTypes.Hellfire] - b[ResourceTypes.Hellfire];
			cost[ResourceTypes.Darkness] = a[ResourceTypes.Darkness] - b[ResourceTypes.Darkness];
			cost[ResourceTypes.Prestige] = a[ResourceTypes.Prestige] - b[ResourceTypes.Prestige];
			return cost;
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x000509D4 File Offset: 0x0004EBD4
		public static Cost operator *(Cost a, int b)
		{
			Cost cost = new Cost();
			cost[ResourceTypes.Souls] = a[ResourceTypes.Souls] * b;
			cost[ResourceTypes.Ichor] = a[ResourceTypes.Ichor] * b;
			cost[ResourceTypes.Hellfire] = a[ResourceTypes.Hellfire] * b;
			cost[ResourceTypes.Darkness] = a[ResourceTypes.Darkness] * b;
			cost[ResourceTypes.Prestige] = a[ResourceTypes.Prestige] * b;
			return cost;
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x00050A36 File Offset: 0x0004EC36
		public Cost Set(ResourceTypes resource, int value)
		{
			base[resource] = value;
			return this;
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x00050A41 File Offset: 0x0004EC41
		public bool IsPrestigeOnly()
		{
			return base[ResourceTypes.Prestige] != 0 && base[ResourceTypes.Souls] == 0 && base[ResourceTypes.Ichor] == 0 && base[ResourceTypes.Hellfire] == 0 && base[ResourceTypes.Darkness] == 0;
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x00050A73 File Offset: 0x0004EC73
		public void DeepClone(out Cost clone)
		{
			clone = new Cost
			{
				RequiredTokenCount = this.RequiredTokenCount
			};
			base.DeepCloneResourceAccumulationParts(clone);
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x00050A90 File Offset: 0x0004EC90
		public override void DeepClone(out ResourceAccumulation clone)
		{
			Cost cost;
			this.DeepClone(out cost);
			clone = cost;
		}

		// Token: 0x04000ADD RID: 2781
		[JsonProperty]
		public int RequiredTokenCount;
	}
}
