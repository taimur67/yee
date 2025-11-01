using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002E3 RID: 739
	public class ModifyGameItemRitualResistanceEvent : GameEvent
	{
		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000E6F RID: 3695 RVA: 0x00039861 File Offset: 0x00037A61
		[JsonIgnore]
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00039864 File Offset: 0x00037A64
		[JsonConstructor]
		private ModifyGameItemRitualResistanceEvent()
		{
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x00039873 File Offset: 0x00037A73
		public ModifyGameItemRitualResistanceEvent(int triggeringPlayerID, int affectedItemOwner, GameItem affectedGameItem, int resistanceOffset) : base(triggeringPlayerID)
		{
			this.Target = affectedGameItem.Id;
			this.ResistanceOffset = resistanceOffset;
			base.AddAffectedPlayerId(affectedItemOwner);
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x0003989E File Offset: 0x00037A9E
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("GameItem {0} has had their resistance against {1}'s rituals offset by {2}", this.Target, this.TriggeringPlayerID, this.ResistanceOffset);
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x000398CC File Offset: 0x00037ACC
		public override void DeepClone(out GameEvent clone)
		{
			ModifyGameItemRitualResistanceEvent modifyGameItemRitualResistanceEvent = new ModifyGameItemRitualResistanceEvent
			{
				Target = this.Target,
				ResistanceOffset = this.ResistanceOffset
			};
			base.DeepCloneGameEventParts<ModifyGameItemRitualResistanceEvent>(modifyGameItemRitualResistanceEvent);
			clone = modifyGameItemRitualResistanceEvent;
		}

		// Token: 0x0400065F RID: 1631
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier Target = Identifier.Invalid;

		// Token: 0x04000660 RID: 1632
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int ResistanceOffset;
	}
}
