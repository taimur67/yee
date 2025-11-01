using System;
using System.ComponentModel;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003B4 RID: 948
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorDuelParticipantData : IDeepClone<PraetorDuelParticipantData>
	{
		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06001289 RID: 4745 RVA: 0x00046ADE File Offset: 0x00044CDE
		// (set) Token: 0x0600128A RID: 4746 RVA: 0x00046AE6 File Offset: 0x00044CE6
		[JsonIgnore]
		public int CombatMoveSlot
		{
			get
			{
				return this._combatMoveSlot;
			}
			set
			{
				this._combatMoveSlot = value;
			}
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x00046AEF File Offset: 0x00044CEF
		public void SetPraetor(Praetor praetor, int combatMoveSlot = -1, ConfigRef combatMove = null)
		{
			this.SetPraetor((praetor != null) ? praetor.Id : Identifier.Invalid, combatMoveSlot, combatMove);
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x00046B05 File Offset: 0x00044D05
		public void SetPraetor(Identifier praetor, int combatMoveSlot = -1, ConfigRef combatMove = null)
		{
			this.Praetor = praetor;
			this.CombatMoveSlot = combatMoveSlot;
			if (combatMove != null)
			{
				this.CombatMove = combatMove.DeepClone();
			}
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x00046B24 File Offset: 0x00044D24
		public void DeepClone(out PraetorDuelParticipantData clone)
		{
			clone = new PraetorDuelParticipantData
			{
				PlayerId = this.PlayerId,
				Praetor = this.Praetor,
				MoveCountered = this.MoveCountered,
				_combatMoveSlot = this._combatMoveSlot,
				CombatMove = this.CombatMove.DeepClone(),
				Bribe = this.Bribe.DeepClone<Payment>()
			};
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x00046B8C File Offset: 0x00044D8C
		public ConfigRef GetCombatMove(TurnState turnState)
		{
			if (this._combatMoveSlot != -1)
			{
				Praetor praetor = turnState.FetchGameItem<Praetor>(this.Praetor);
				PraetorCombatMoveInstance praetorCombatMoveInstance;
				if (praetor != null && praetor.TryGetTechniqueAtSlot(this._combatMoveSlot, out praetorCombatMoveInstance))
				{
					return praetorCombatMoveInstance.CombatMoveReference;
				}
			}
			return this.CombatMove;
		}

		// Token: 0x0400089F RID: 2207
		[JsonProperty]
		[DefaultValue(-2147483648)]
		public int PlayerId = int.MinValue;

		// Token: 0x040008A0 RID: 2208
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier Praetor = Identifier.Invalid;

		// Token: 0x040008A1 RID: 2209
		[JsonProperty]
		public ConfigRef CombatMove;

		// Token: 0x040008A2 RID: 2210
		[JsonProperty]
		public bool MoveCountered;

		// Token: 0x040008A3 RID: 2211
		[JsonProperty]
		public Payment Bribe;

		// Token: 0x040008A4 RID: 2212
		[JsonProperty]
		[DefaultValue(-1)]
		private int _combatMoveSlot = -1;
	}
}
