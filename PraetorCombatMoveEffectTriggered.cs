using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003B0 RID: 944
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorCombatMoveEffectTriggered : GameEvent
	{
		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06001274 RID: 4724 RVA: 0x000468F2 File Offset: 0x00044AF2
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06001275 RID: 4725 RVA: 0x000468F5 File Offset: 0x00044AF5
		[JsonIgnore]
		public int OwningPlayerId
		{
			get
			{
				return this.TriggeringPlayerID;
			}
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x000468FD File Offset: 0x00044AFD
		[JsonConstructor]
		protected PraetorCombatMoveEffectTriggered()
		{
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x00046905 File Offset: 0x00044B05
		public PraetorCombatMoveEffectTriggered(PraetorDuelParticipantData participant, PraetorCombatMoveEffectData effect) : this(participant.PlayerId, participant.Praetor, effect)
		{
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x0004691A File Offset: 0x00044B1A
		public PraetorCombatMoveEffectTriggered(int player, Identifier praetor, PraetorCombatMoveEffectData effect) : base(player)
		{
			this.PraetorSource = praetor;
			this.Effect = effect;
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x00046931 File Offset: 0x00044B31
		public override string GetDebugName(TurnContext context)
		{
			string str = context.Debug_GetItemName(this.PraetorSource);
			string str2 = " Triggered ";
			PraetorCombatMoveEffectData effect = this.Effect;
			return str + str2 + (((effect != null) ? effect.DebugDescription : null) ?? "None");
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x00046964 File Offset: 0x00044B64
		public override void DeepClone(out GameEvent clone)
		{
			PraetorCombatMoveEffectTriggered praetorCombatMoveEffectTriggered = new PraetorCombatMoveEffectTriggered
			{
				PraetorSource = this.PraetorSource,
				Effect = this.Effect.DeepClone(CloneFunction.FastClone)
			};
			base.DeepCloneGameEventParts<PraetorCombatMoveEffectTriggered>(praetorCombatMoveEffectTriggered);
			clone = praetorCombatMoveEffectTriggered;
		}

		// Token: 0x04000899 RID: 2201
		[JsonProperty]
		public Identifier PraetorSource;

		// Token: 0x0400089A RID: 2202
		[JsonProperty]
		public PraetorCombatMoveEffectData Effect;
	}
}
