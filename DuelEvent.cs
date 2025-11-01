using System;
using System.Collections.Generic;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003AA RID: 938
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class DuelEvent : GameEvent
	{
		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x0600124E RID: 4686 RVA: 0x00046381 File Offset: 0x00044581
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x00046384 File Offset: 0x00044584
		[JsonConstructor]
		public DuelEvent()
		{
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x0004638C File Offset: 0x0004458C
		public DuelEvent(PraetorDuelData data) : base(data.Challenger.PlayerId)
		{
			this.DuelData = data;
			base.AddAffectedPlayerId(data.Defender.PlayerId);
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06001251 RID: 4689 RVA: 0x000463B7 File Offset: 0x000445B7
		private PraetorDuelOutcomeEvent Outcome
		{
			get
			{
				return base.Get<PraetorDuelOutcomeEvent>();
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06001252 RID: 4690 RVA: 0x000463BF File Offset: 0x000445BF
		private IEnumerable<PraetorDuelPhaseEvent> Phases
		{
			get
			{
				return base.Enumerate<PraetorDuelPhaseEvent>();
			}
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x000463C8 File Offset: 0x000445C8
		public override string GetDebugName(TurnContext context)
		{
			string empty = string.Empty;
			string str = context.Debug_GetItemName(this.DuelData.Challenger.Praetor);
			string str2 = "  ";
			PraetorDuelParticipantData challenger = this.DuelData.Challenger;
			string str3;
			if (challenger == null)
			{
				str3 = null;
			}
			else
			{
				ConfigRef combatMove = challenger.GetCombatMove(context.CurrentTurn);
				str3 = ((combatMove != null) ? combatMove.Id : null);
			}
			string str4 = empty + str + str2 + str3 + " \nvs\n ";
			string str5 = context.Debug_GetItemName(this.DuelData.Defender.Praetor);
			string str6 = "  ";
			PraetorDuelParticipantData defender = this.DuelData.Defender;
			string str7;
			if (defender == null)
			{
				str7 = null;
			}
			else
			{
				ConfigRef combatMove2 = defender.GetCombatMove(context.CurrentTurn);
				str7 = ((combatMove2 != null) ? combatMove2.Id : null);
			}
			return str4 + str5 + str6 + str7;
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x00046478 File Offset: 0x00044678
		public override void DeepClone(out GameEvent clone)
		{
			DuelEvent duelEvent = new DuelEvent
			{
				DuelData = this.DuelData.DeepClone<PraetorDuelData>()
			};
			base.DeepCloneGameEventParts<DuelEvent>(duelEvent);
			clone = duelEvent;
		}

		// Token: 0x04000889 RID: 2185
		[JsonProperty]
		public PraetorDuelData DuelData;
	}
}
