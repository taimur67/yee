using System;
using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000483 RID: 1155
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ResourceNFT : IDeepClone<ResourceNFT>
	{
		// Token: 0x17000315 RID: 789
		// (get) Token: 0x060015A8 RID: 5544 RVA: 0x00051852 File Offset: 0x0004FA52
		public static IReadOnlyCollection<ResourceTypes> ResourceKeys
		{
			get
			{
				List<ResourceTypes> result;
				if ((result = ResourceNFT._resourceKeys) == null)
				{
					result = (ResourceNFT._resourceKeys = IEnumerableExtensions.ToList<ResourceTypes>(IEnumerableExtensions.ExceptFor<ResourceTypes>(ResourceAccumulation.ResourceKeys, new ResourceTypes[]
					{
						ResourceTypes.Prestige
					})));
				}
				return result;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x060015A9 RID: 5545 RVA: 0x0005187C File Offset: 0x0004FA7C
		[JsonIgnore]
		public int ValueSum
		{
			get
			{
				return this.Values.ValueSum;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x060015AA RID: 5546 RVA: 0x00051889 File Offset: 0x0004FA89
		[JsonIgnore]
		public BigInteger ValueHash
		{
			get
			{
				return this.Values.CalculateValueHash();
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x060015AB RID: 5547 RVA: 0x00051896 File Offset: 0x0004FA96
		// (set) Token: 0x060015AC RID: 5548 RVA: 0x0005189E File Offset: 0x0004FA9E
		[JsonProperty]
		public int Id { get; private set; }

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x060015AD RID: 5549 RVA: 0x000518A7 File Offset: 0x0004FAA7
		// (set) Token: 0x060015AE RID: 5550 RVA: 0x000518AF File Offset: 0x0004FAAF
		[JsonProperty]
		public ResourceAccumulation Values { get; protected set; } = new ResourceAccumulation();

		// Token: 0x060015AF RID: 5551 RVA: 0x000518B8 File Offset: 0x0004FAB8
		[JsonConstructor]
		protected ResourceNFT()
		{
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x000518CB File Offset: 0x0004FACB
		public ResourceNFT(int id, params ResourceAccumulation[] others)
		{
			this.Id = id;
			this.Values.Accumulate(others);
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x000518F2 File Offset: 0x0004FAF2
		public ResourceNFT(int id, ResourceAccumulation values)
		{
			this.Id = id;
			this.Values = values;
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x00051913 File Offset: 0x0004FB13
		public static implicit operator ResourceAccumulation(in ResourceNFT nft)
		{
			return nft.Values;
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x0005191C File Offset: 0x0004FB1C
		public bool EqualValue(ResourceNFT other)
		{
			return this.Values.Equal(other.Values);
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x0005192F File Offset: 0x0004FB2F
		public override string ToString()
		{
			return string.Format("[{0}] {1}", this.Id, this.Values);
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x0005194C File Offset: 0x0004FB4C
		public void DeepClone(out ResourceNFT clone)
		{
			clone = new ResourceNFT
			{
				Id = this.Id,
				Values = this.Values.DeepClone<ResourceAccumulation>(),
				VisualOverrideId = this.VisualOverrideId.DeepClone()
			};
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x00051984 File Offset: 0x0004FB84
		public override bool Equals(object obj)
		{
			ResourceNFT resourceNFT = obj as ResourceNFT;
			return resourceNFT != null && resourceNFT.Id == this.Id;
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x000519AB File Offset: 0x0004FBAB
		public override int GetHashCode()
		{
			return this.Id;
		}

		// Token: 0x04000AE8 RID: 2792
		public const int MaxValueCount = 99;

		// Token: 0x04000AE9 RID: 2793
		public static readonly ResourceNFT Invalid = new ResourceNFT(-1, Array.Empty<ResourceAccumulation>());

		// Token: 0x04000AEA RID: 2794
		private static List<ResourceTypes> _resourceKeys;

		// Token: 0x04000AEB RID: 2795
		[JsonProperty]
		public string VisualOverrideId;
	}
}
