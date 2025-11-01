using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000507 RID: 1287
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class BloodVassalageState : DiplomaticState
	{
		// Token: 0x1700036F RID: 879
		// (get) Token: 0x060018B3 RID: 6323 RVA: 0x00058639 File Offset: 0x00056839
		[JsonIgnore]
		public override DiplomaticStateValue Type
		{
			get
			{
				return DiplomaticStateValue.BloodVassalage;
			}
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x0005863C File Offset: 0x0005683C
		public override bool AllowMovementIntoTerritory(DiplomaticTurnState diplomacy, int requestingPlayerId, int targetPlayer)
		{
			return true;
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x060018B5 RID: 6325 RVA: 0x0005863F File Offset: 0x0005683F
		[JsonIgnore]
		public override bool AllowFriendlyDiplomacy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x060018B6 RID: 6326 RVA: 0x00058642 File Offset: 0x00056842
		[JsonIgnore]
		public override bool AllowSupport
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060018B7 RID: 6327 RVA: 0x00058645 File Offset: 0x00056845
		public override bool AllowNearbyHealingProvidedBy(int providingPlayerId)
		{
			return true;
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x060018B8 RID: 6328 RVA: 0x00058648 File Offset: 0x00056848
		[JsonIgnore]
		public int BloodLordId
		{
			get
			{
				return this._bloodLordId;
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x060018B9 RID: 6329 RVA: 0x00058650 File Offset: 0x00056850
		[JsonIgnore]
		public int VassalId
		{
			get
			{
				return this._vassalId;
			}
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x00058658 File Offset: 0x00056858
		[JsonConstructor]
		public BloodVassalageState()
		{
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x00058660 File Offset: 0x00056860
		public BloodVassalageState(int bloodLordId, int vassalId)
		{
			this._bloodLordId = bloodLordId;
			this._vassalId = vassalId;
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x00058678 File Offset: 0x00056878
		public void Swap()
		{
			int bloodLordId = this._bloodLordId;
			int vassalId = this._vassalId;
			this._vassalId = bloodLordId;
			this._bloodLordId = vassalId;
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x000586A4 File Offset: 0x000568A4
		public override void OnStateEntered(TurnProcessContext context, DiplomaticPairStatus pair, DiplomaticState prevState, bool interrupt)
		{
			foreach (DiplomaticPairStatus diplomaticPairStatus in context.Diplomacy.GetAllDiplomaticStatesOfPlayer(this._vassalId))
			{
				if (!diplomaticPairStatus.PlayerPair.Equals(pair.PlayerPair) && !(diplomaticPairStatus.DiplomaticState is EliminatedState))
				{
					diplomaticPairStatus.SetVassalised(context, this._vassalId, this._bloodLordId);
				}
			}
		}

		// Token: 0x060018BE RID: 6334 RVA: 0x0005872C File Offset: 0x0005692C
		public override void OnStateExited(TurnProcessContext context, DiplomaticPairStatus pair, DiplomaticState nextState, bool interrupt)
		{
			foreach (DiplomaticPairStatus diplomaticPairStatus in context.Diplomacy.GetAllDiplomaticStatesOfPlayer(this._vassalId))
			{
				if (!diplomaticPairStatus.PlayerPair.Equals(pair.PlayerPair) && diplomaticPairStatus.DiplomaticState is VassalisedState)
				{
					diplomaticPairStatus.SetNeutral(context, false);
				}
			}
		}

		// Token: 0x060018BF RID: 6335 RVA: 0x000587A8 File Offset: 0x000569A8
		public override void DeepClone(out DiplomaticState clone)
		{
			clone = new BloodVassalageState
			{
				_bloodLordId = this._bloodLordId,
				_vassalId = this._vassalId
			};
		}

		// Token: 0x04000B87 RID: 2951
		[JsonProperty]
		private int _bloodLordId;

		// Token: 0x04000B88 RID: 2952
		[JsonProperty]
		private int _vassalId;
	}
}
