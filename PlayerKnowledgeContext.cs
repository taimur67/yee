using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000300 RID: 768
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PlayerKnowledgeContext : IDeepClone<PlayerKnowledgeContext>
	{
		// Token: 0x06000EE7 RID: 3815 RVA: 0x0003AFF8 File Offset: 0x000391F8
		public void UpdatePower(TurnProcessContext context, PowerType power, PlayerPowerLevel level)
		{
			this.UpdatePower(context.CurrentTurn, power, level);
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x0003B008 File Offset: 0x00039208
		public void UpdatePower(TurnState turn, PowerType power, PlayerPowerLevel level)
		{
			this.LastRevealedPowers[power] = turn.TurnValue;
			this.KnownPowers[power] = level;
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x0003B029 File Offset: 0x00039229
		public void UpdateRelics(TurnProcessContext context, IEnumerable<Identifier> relics)
		{
			this.RelicContents = IEnumerableExtensions.ToList<Identifier>(relics);
			this.LastRevealedRelics = context.CurrentTurn.TurnValue;
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x0003B048 File Offset: 0x00039248
		public void UpdateSchemes(TurnProcessContext context, IEnumerable<Identifier> schemes)
		{
			this.SchemeContents = IEnumerableExtensions.ToList<Identifier>(schemes);
			this.LastRevealedSchemes = context.CurrentTurn.TurnValue;
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x0003B067 File Offset: 0x00039267
		public void UpdateRituals(TurnProcessContext context, IEnumerable<Identifier> rituals)
		{
			this.LastRevealedRituals = context.CurrentTurn.TurnValue;
			this.RitualContents = IEnumerableExtensions.ToList<Identifier>(rituals);
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x0003B086 File Offset: 0x00039286
		public void UpdateVaultContents(TurnProcessContext context, IEnumerable<Identifier> contents)
		{
			this.LastRevealedVault = context.CurrentTurn.TurnValue;
			this.VaultContents = IEnumerableExtensions.ToList<Identifier>(contents);
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x0003B0A5 File Offset: 0x000392A5
		public void UpdateEventContents(TurnProcessContext context, IEnumerable<Identifier> contents)
		{
			this.LastRevealedVault = context.CurrentTurn.TurnValue;
			this.EventContents = IEnumerableExtensions.ToList<Identifier>(contents);
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x0003B0C4 File Offset: 0x000392C4
		public void RemoveContent(TurnProcessContext context, Identifier id)
		{
			this.VaultContents.RemoveAll((Identifier t) => t == id);
			this.RitualContents.RemoveAll((Identifier t) => t == id);
			this.SchemeContents.RemoveAll((Identifier t) => t == id);
			this.RelicContents.RemoveAll((Identifier t) => t == id);
			this.EventContents.RemoveAll((Identifier t) => t == id);
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x0003B156 File Offset: 0x00039356
		public void UpdateResources(TurnProcessContext context, ResourceAccumulation accumulation)
		{
			this.LastRevealedResources = context.CurrentTurn.TurnValue;
			this.ResourceContents = accumulation;
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x0003B170 File Offset: 0x00039370
		public void DeepClone(out PlayerKnowledgeContext clone)
		{
			clone = new PlayerKnowledgeContext
			{
				LastRevealedVault = this.LastRevealedVault,
				LastRevealedRituals = this.LastRevealedRituals,
				LastRevealedSchemes = this.LastRevealedSchemes,
				LastRevealedRelics = this.LastRevealedRelics,
				LastRevealedResources = this.LastRevealedResources,
				LastRevealedPowers = this.LastRevealedPowers.DeepClone<PowerType>(),
				KnownPowers = this.KnownPowers.DeepClone<PowerType, PlayerPowerLevel>(),
				VaultContents = this.VaultContents.DeepClone(),
				RitualContents = this.RitualContents.DeepClone(),
				SchemeContents = this.SchemeContents.DeepClone(),
				RelicContents = this.RelicContents.DeepClone(),
				EventContents = this.EventContents.DeepClone(),
				ResourceContents = this.ResourceContents.DeepClone<ResourceAccumulation>()
			};
		}

		// Token: 0x040006CE RID: 1742
		[JsonProperty]
		[DefaultValue(-1)]
		public int LastRevealedVault = -1;

		// Token: 0x040006CF RID: 1743
		[JsonProperty]
		[DefaultValue(-1)]
		public int LastRevealedRituals = -1;

		// Token: 0x040006D0 RID: 1744
		[JsonProperty]
		[DefaultValue(-1)]
		public int LastRevealedSchemes = -1;

		// Token: 0x040006D1 RID: 1745
		[JsonProperty]
		[DefaultValue(-1)]
		public int LastRevealedRelics = -1;

		// Token: 0x040006D2 RID: 1746
		[JsonProperty]
		[DefaultValue(-1)]
		public int LastRevealedResources = -1;

		// Token: 0x040006D3 RID: 1747
		[JsonProperty]
		public Dictionary<PowerType, int> LastRevealedPowers = new Dictionary<PowerType, int>();

		// Token: 0x040006D4 RID: 1748
		[JsonProperty]
		public Dictionary<PowerType, PlayerPowerLevel> KnownPowers = new Dictionary<PowerType, PlayerPowerLevel>();

		// Token: 0x040006D5 RID: 1749
		[JsonProperty]
		public List<Identifier> VaultContents = new List<Identifier>();

		// Token: 0x040006D6 RID: 1750
		[JsonProperty]
		public List<Identifier> RitualContents = new List<Identifier>();

		// Token: 0x040006D7 RID: 1751
		[JsonProperty]
		public List<Identifier> SchemeContents = new List<Identifier>();

		// Token: 0x040006D8 RID: 1752
		[JsonProperty]
		public List<Identifier> RelicContents = new List<Identifier>();

		// Token: 0x040006D9 RID: 1753
		[JsonProperty]
		public List<Identifier> EventContents = new List<Identifier>();

		// Token: 0x040006DA RID: 1754
		[JsonProperty]
		public ResourceAccumulation ResourceContents = new ResourceAccumulation();
	}
}
