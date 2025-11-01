using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000480 RID: 1152
	[Serializable]
	public class Payment : IDeepClone<Payment>
	{
		// Token: 0x0600155F RID: 5471 RVA: 0x00050BDD File Offset: 0x0004EDDD
		public static Payment FromPrestige(int prestige)
		{
			return new Payment(Enumerable.Empty<ResourceNFT>(), prestige);
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x00050BEA File Offset: 0x0004EDEA
		[JsonConstructor]
		public Payment()
		{
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x00050BFD File Offset: 0x0004EDFD
		public Payment(Payment other)
		{
			this.Resources = IEnumerableExtensions.ToList<ResourceNFT>(other.Resources);
			this.Prestige = other.Prestige;
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x00050C2D File Offset: 0x0004EE2D
		public Payment(params ResourceNFT[] resources) : this(resources.AsEnumerable<ResourceNFT>(), 0)
		{
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x00050C3C File Offset: 0x0004EE3C
		public Payment(IEnumerable<ResourceNFT> resources, int prestige = 0)
		{
			this.AddResources(resources);
			this.Prestige = prestige;
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x00050C5E File Offset: 0x0004EE5E
		public void ClearPayment()
		{
			this.Prestige = 0;
			this.Resources.Clear();
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x00050C74 File Offset: 0x0004EE74
		public Payment AddResources(IEnumerable<ResourceNFT> resources)
		{
			foreach (ResourceNFT item in resources)
			{
				this.Resources.Add(item);
			}
			return this;
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x00050CC4 File Offset: 0x0004EEC4
		public bool Contains(ResourceTypes type)
		{
			return this.Total[type] > 0;
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x00050CD8 File Offset: 0x0004EED8
		public bool ContainsExact(ResourceNFT card)
		{
			return this.Resources.Any((ResourceNFT t) => card.Id == t.Id);
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x00050D09 File Offset: 0x0004EF09
		public Payment AddResources(params ResourceNFT[] nfts)
		{
			return this.AddResources(nfts.AsEnumerable<ResourceNFT>());
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x00050D17 File Offset: 0x0004EF17
		public Payment AddPrestige(int value)
		{
			this.Prestige += value;
			return this;
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x00050D28 File Offset: 0x0004EF28
		public bool RemoveResource(BigInteger hash)
		{
			ResourceNFT resourceNFT = this.Resources.FirstOrDefault((ResourceNFT t) => t.ValueHash == hash);
			return resourceNFT != null && this.Resources.Remove(resourceNFT);
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x00050D6C File Offset: 0x0004EF6C
		public bool RemoveResourceById(int resourceId)
		{
			return this.Resources.RemoveAll((ResourceNFT t) => t.Id == resourceId) > 0;
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x00050DA0 File Offset: 0x0004EFA0
		public bool RemoveResourceById(ResourceNFT resource)
		{
			return this.RemoveResourceById(resource.Id);
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x00050DAE File Offset: 0x0004EFAE
		public bool RemoveResource(ResourceNFT resource)
		{
			return this.RemoveResource(resource.ValueHash);
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x0600156E RID: 5486 RVA: 0x00050DBC File Offset: 0x0004EFBC
		public bool IsEmpty
		{
			get
			{
				return this.Total.IsZero;
			}
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x00050DC9 File Offset: 0x0004EFC9
		public void DeepClone(out Payment clone)
		{
			clone = new Payment
			{
				Resources = this.Resources.DeepClone<ResourceNFT>(),
				Prestige = this.Prestige
			};
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x00050DEF File Offset: 0x0004EFEF
		public override string ToString()
		{
			return this.Total.ToString();
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06001571 RID: 5489 RVA: 0x00050DFC File Offset: 0x0004EFFC
		public static Payment Empty
		{
			get
			{
				return new Payment();
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06001572 RID: 5490 RVA: 0x00050E04 File Offset: 0x0004F004
		[JsonIgnore]
		public ResourceAccumulation Total
		{
			get
			{
				ResourceAccumulation resourceAccumulation;
				ResourceAccumulation result = resourceAccumulation = this.Resources.Total();
				resourceAccumulation[ResourceTypes.Prestige] = resourceAccumulation[ResourceTypes.Prestige] + this.Prestige;
				return result;
			}
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x00050E34 File Offset: 0x0004F034
		public ResourceNFT Find(int resourceId)
		{
			return this.Resources.First((ResourceNFT t) => t.ValueHash == (long)resourceId);
		}

		// Token: 0x04000AE0 RID: 2784
		public List<ResourceNFT> Resources = new List<ResourceNFT>();

		// Token: 0x04000AE1 RID: 2785
		public int Prestige;
	}
}
