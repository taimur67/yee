using System;
using System.Collections.Generic;
using System.Linq;
using LoG.Simulation.Extensions;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200047F RID: 1151
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ManuscriptTypeQueue : IDeepClone<ManuscriptTypeQueue>
	{
		// Token: 0x0600155B RID: 5467 RVA: 0x00050AA8 File Offset: 0x0004ECA8
		public void ConfigureDefault()
		{
			this._drawPile.Clear();
			this._discardPile.Clear();
			this._discardPile.AddRange(Enumerable.Repeat<ManuscriptCategory>(ManuscriptCategory.Manual, 8));
			this._discardPile.AddRange(Enumerable.Repeat<ManuscriptCategory>(ManuscriptCategory.Primer, 6));
			this._discardPile.AddRange(Enumerable.Repeat<ManuscriptCategory>(ManuscriptCategory.Treatise, 1));
			this._discardPile.AddRange(Enumerable.Repeat<ManuscriptCategory>(ManuscriptCategory.Schematic, 1));
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x00050B14 File Offset: 0x0004ED14
		public ManuscriptCategory Draw(SimulationRandom random)
		{
			if (this._drawPile.Count == 0)
			{
				if (this._discardPile.Count == 0)
				{
					SimLogger logger = SimLogger.Logger;
					if (logger != null)
					{
						logger.Warn("Attempt to access empty ManuscriptTypeQueue - forcing initialisation with a default spread");
					}
					this.ConfigureDefault();
				}
				this._drawPile = this._discardPile;
				this._drawPile.ShuffleContents(random);
				this._discardPile = new List<ManuscriptCategory>();
			}
			ManuscriptCategory manuscriptCategory = ListExtensions.PopFirst<ManuscriptCategory>(this._drawPile);
			this._discardPile.Add(manuscriptCategory);
			return manuscriptCategory;
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x00050B92 File Offset: 0x0004ED92
		public void DeepClone(out ManuscriptTypeQueue clone)
		{
			clone = new ManuscriptTypeQueue();
			clone._drawPile = this._drawPile.DeepClone();
			clone._discardPile = this._discardPile.DeepClone();
		}

		// Token: 0x04000ADE RID: 2782
		[JsonProperty]
		private List<ManuscriptCategory> _drawPile = new List<ManuscriptCategory>();

		// Token: 0x04000ADF RID: 2783
		[JsonProperty]
		private List<ManuscriptCategory> _discardPile = new List<ManuscriptCategory>();
	}
}
