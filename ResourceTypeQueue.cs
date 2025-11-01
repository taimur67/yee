using System;
using System.Collections.Generic;
using System.Linq;
using LoG.Simulation.Extensions;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000484 RID: 1156
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ResourceTypeQueue : IDeepClone<ResourceTypeQueue>
	{
		// Token: 0x060015B9 RID: 5561 RVA: 0x000519C8 File Offset: 0x0004FBC8
		public void ConfigureDefault()
		{
			this._drawPile.Clear();
			this._discardPile.Clear();
			this._discardPile.AddRange(Enumerable.Repeat<ResourceTypes>(ResourceTypes.Souls, 3));
			this._discardPile.AddRange(Enumerable.Repeat<ResourceTypes>(ResourceTypes.Ichor, 2));
			this._discardPile.AddRange(Enumerable.Repeat<ResourceTypes>(ResourceTypes.Hellfire, 2));
			this._discardPile.AddRange(Enumerable.Repeat<ResourceTypes>(ResourceTypes.Darkness, 2));
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x00051A34 File Offset: 0x0004FC34
		public void ConfigureFrom(IEnumerable<WeightedValue<ResourceTypes>> resourceWeights)
		{
			this._drawPile.Clear();
			this._discardPile.Clear();
			int num = 0;
			foreach (WeightedValue<ResourceTypes> weightedValue in resourceWeights)
			{
				int num2 = (int)weightedValue.Weight;
				this._discardPile.AddRange(Enumerable.Repeat<ResourceTypes>(weightedValue.Value, num2));
				num += num2;
				if (this._discardPile.Count > ResourceTypeQueue._maximumDeckSize)
				{
					SimLogger logger = SimLogger.Logger;
					if (logger != null)
					{
						logger.Error(string.Format("Cannot configure ResourceTypeQueue to have {0}>{1} elements, as this will cause issues with the size of save files", this._discardPile.Count, ResourceTypeQueue._maximumDeckSize));
					}
					this.ConfigureDefault();
					break;
				}
			}
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x00051B00 File Offset: 0x0004FD00
		public ResourceTypes Draw(SimulationRandom random)
		{
			if (this._drawPile.Count == 0)
			{
				if (this._discardPile.Count == 0)
				{
					SimLogger logger = SimLogger.Logger;
					if (logger != null)
					{
						logger.Warn("Attempt to access empty ResourceTypeQueue - forcing initialisation with a default spread");
					}
					this.ConfigureDefault();
				}
				this._drawPile = this._discardPile;
				this._drawPile.ShuffleContents(random);
				this._discardPile = new List<ResourceTypes>();
			}
			ResourceTypes resourceTypes = ListExtensions.PopFirst<ResourceTypes>(this._drawPile);
			this._discardPile.Add(resourceTypes);
			return resourceTypes;
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x00051B80 File Offset: 0x0004FD80
		public bool TryDrawFirst(SimulationRandom random, IEnumerable<ResourceTypes> acceptableResources, out ResourceTypes drawnType)
		{
			this._discardPile.Shuffle(random);
			this._drawPile.AddRange(this._discardPile);
			this._discardPile.Clear();
			for (int i = 0; i < this._drawPile.Count; i++)
			{
				drawnType = this._drawPile[i];
				if (IEnumerableExtensions.Contains<ResourceTypes>(acceptableResources, drawnType))
				{
					this._drawPile.RemoveAt(i);
					this._discardPile.Add(drawnType);
					return true;
				}
			}
			drawnType = ResourceTypes.Souls;
			return false;
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x00051C03 File Offset: 0x0004FE03
		public void DeepClone(out ResourceTypeQueue clone)
		{
			clone = new ResourceTypeQueue();
			clone._drawPile = this._drawPile.DeepClone();
			clone._discardPile = this._discardPile.DeepClone();
		}

		// Token: 0x04000AEE RID: 2798
		private static readonly int _maximumDeckSize = 16;

		// Token: 0x04000AEF RID: 2799
		[JsonProperty]
		private List<ResourceTypes> _drawPile = new List<ResourceTypes>();

		// Token: 0x04000AF0 RID: 2800
		[JsonProperty]
		private List<ResourceTypes> _discardPile = new List<ResourceTypes>();
	}
}
