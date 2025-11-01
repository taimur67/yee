using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003AD RID: 941
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class PraetorDuelOutcomeEvent : GameEvent
	{
		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x0600125F RID: 4703 RVA: 0x00046552 File Offset: 0x00044752
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06001260 RID: 4704 RVA: 0x00046555 File Offset: 0x00044755
		public bool ChallengerWin
		{
			get
			{
				return this.Winner != null && this.Challenger.PlayerId == this.Winner.PlayerId;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06001261 RID: 4705 RVA: 0x00046579 File Offset: 0x00044779
		public bool DefenderWin
		{
			get
			{
				return this.Winner != null && this.Defender.PlayerId == this.Winner.PlayerId;
			}
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x000465A0 File Offset: 0x000447A0
		public bool TryGetParticipantData(int playerId, out PraetorDuelParticipantData data)
		{
			data = null;
			if (this.Challenger != null && this.Challenger.PlayerId == playerId)
			{
				data = this.Challenger;
			}
			if (this.Defender != null && this.Defender.PlayerId == playerId)
			{
				data = this.Defender;
			}
			return data != null;
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x000465F1 File Offset: 0x000447F1
		[JsonConstructor]
		public PraetorDuelOutcomeEvent()
		{
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x000465FC File Offset: 0x000447FC
		public PraetorDuelOutcomeEvent(PraetorDuelData duelData)
		{
			this.Challenger = duelData.Challenger;
			this.Defender = duelData.Defender;
			this.TriggeringPlayerID = duelData.Challenger.PlayerId;
			base.AddAffectedPlayerId(duelData.Defender.PlayerId);
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x0004664C File Offset: 0x0004484C
		public override string GetDebugName(TurnContext context)
		{
			string arg = string.Empty;
			if (this.Winner != null)
			{
				arg = string.Format("{0} - {1}", this.Winner.PlayerId, this.Winner.Praetor);
			}
			return string.Format("{0} {1}", this.Result, arg);
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x000466A8 File Offset: 0x000448A8
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (this.Result == DuelResultStatus.CombatWinner)
			{
				if (this.Winner != null && forPlayerID == this.Winner.PlayerId)
				{
					return TurnLogEntryType.PraetorDuelVictory;
				}
			}
			else
			{
				if (this.Result == DuelResultStatus.DefaultWinner)
				{
					return TurnLogEntryType.PraetorDuelNoShow;
				}
				if (this.Result == DuelResultStatus.Cancelled)
				{
					return TurnLogEntryType.PraetorDuelNoShowAll;
				}
				if (this.Result == DuelResultStatus.Draw)
				{
					return TurnLogEntryType.PraetorDuelDraw;
				}
			}
			return TurnLogEntryType.PraetorDuelDefeat;
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x00046708 File Offset: 0x00044908
		public override void DeepClone(out GameEvent clone)
		{
			PraetorDuelOutcomeEvent praetorDuelOutcomeEvent = new PraetorDuelOutcomeEvent
			{
				FinalTally = this.FinalTally.DeepClone<DuelProcessDamageTally>(),
				Result = this.Result,
				WinnerVictories = this.WinnerVictories,
				Winner = this.Winner.DeepClone<PraetorDuelParticipantData>(),
				Loser = this.Loser.DeepClone<PraetorDuelParticipantData>(),
				Challenger = this.Challenger.DeepClone<PraetorDuelParticipantData>(),
				Defender = this.Defender.DeepClone<PraetorDuelParticipantData>(),
				WinnerPrestigeGain = this.WinnerPrestigeGain,
				LoserPrestigePenalty = this.LoserPrestigePenalty
			};
			base.DeepCloneGameEventParts<PraetorDuelOutcomeEvent>(praetorDuelOutcomeEvent);
			clone = praetorDuelOutcomeEvent;
		}

		// Token: 0x0400088A RID: 2186
		[JsonProperty]
		public DuelProcessDamageTally FinalTally;

		// Token: 0x0400088B RID: 2187
		[JsonProperty]
		[DefaultValue(DuelResultStatus.Cancelled)]
		public DuelResultStatus Result;

		// Token: 0x0400088C RID: 2188
		[BindableValue("win_prestige", BindingOption.None)]
		[JsonProperty]
		public int WinnerPrestigeGain;

		// Token: 0x0400088D RID: 2189
		[BindableValue("lose_prestige", BindingOption.None)]
		[JsonProperty]
		public int LoserPrestigePenalty;

		// Token: 0x0400088E RID: 2190
		[BindableValue("victories", BindingOption.None)]
		[JsonProperty]
		public int WinnerVictories;

		// Token: 0x0400088F RID: 2191
		[BindableValue("winner", BindingOption.None)]
		[JsonProperty]
		[DefaultValue(null)]
		public PraetorDuelParticipantData Winner;

		// Token: 0x04000890 RID: 2192
		[BindableValue("loser", BindingOption.None)]
		[JsonProperty]
		[DefaultValue(null)]
		public PraetorDuelParticipantData Loser;

		// Token: 0x04000891 RID: 2193
		[BindableValue("challenger", BindingOption.None)]
		[JsonProperty]
		[DefaultValue(null)]
		public PraetorDuelParticipantData Challenger;

		// Token: 0x04000892 RID: 2194
		[BindableValue("defender", BindingOption.None)]
		[JsonProperty]
		[DefaultValue(null)]
		public PraetorDuelParticipantData Defender;
	}
}
