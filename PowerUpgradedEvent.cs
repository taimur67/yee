using System;
using System.Collections.Generic;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200025B RID: 603
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class PowerUpgradedEvent : GameEvent
	{
		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000BCC RID: 3020 RVA: 0x00030422 File Offset: 0x0002E622
		// (set) Token: 0x06000BCD RID: 3021 RVA: 0x0003042A File Offset: 0x0002E62A
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public PowerType PowerType { get; private set; }

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000BCE RID: 3022 RVA: 0x00030433 File Offset: 0x0002E633
		// (set) Token: 0x06000BCF RID: 3023 RVA: 0x0003043B File Offset: 0x0002E63B
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int Level { get; private set; }

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00030444 File Offset: 0x0002E644
		[JsonConstructor]
		private PowerUpgradedEvent()
		{
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x0003044C File Offset: 0x0002E64C
		public PowerUpgradedEvent(int playerId, PowerType powerType, int level) : base(playerId)
		{
			this.PowerType = powerType;
			this.Level = level;
			base.AddAffectedPlayerId(playerId);
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x0003046A File Offset: 0x0002E66A
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Power {0} for Player {1} upgraded to level {2}", this.PowerType, this.TriggeringPlayerID, this.Level);
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00030497 File Offset: 0x0002E697
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.PowerUpgraded;
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x0003049C File Offset: 0x0002E69C
		public override void DeepClone(out GameEvent clone)
		{
			PowerUpgradedEvent powerUpgradedEvent = new PowerUpgradedEvent
			{
				PowerType = this.PowerType,
				Level = this.Level,
				Unlocks = this.Unlocks.DeepClone(CloneFunction.FastClone),
				ConfigRef = this.ConfigRef.DeepClone()
			};
			base.DeepCloneGameEventParts<PowerUpgradedEvent>(powerUpgradedEvent);
			clone = powerUpgradedEvent;
		}

		// Token: 0x0400052B RID: 1323
		[BindableValue(null, BindingOption.StaticDataId)]
		[JsonProperty]
		public List<ConfigRef> Unlocks;

		// Token: 0x0400052C RID: 1324
		[JsonProperty]
		public ConfigRef ConfigRef;
	}
}
