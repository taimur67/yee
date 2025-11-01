using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000668 RID: 1640
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class DebuffGamePieceRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001E43 RID: 7747 RVA: 0x000685C0 File Offset: 0x000667C0
		public DebuffGamePieceRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x000685CD File Offset: 0x000667CD
		public DebuffGamePieceRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x000685E1 File Offset: 0x000667E1
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			List<CombatStatType> targetStats = this.TargetStats;
			if (targetStats != null)
			{
				targetStats.Clear();
			}
			yield return new ActionPhase_TargetGamePiece(delegate(GamePiece x)
			{
				this.TargetContext.SetTargetGamePiece(x);
			}, new ActionPhase_Target<GamePiece>.IsValidFunc(this.IsValidGamePiece), new ActionPhase_TargetGamePiece.IsValidArchfiendFunc(this.IsValidArchfiend), new ActionPhase_TargetGamePiece.IsSelectableGamePieceFunc(base.FilterGamePiece), 1, ActionPhaseType.None);
			DebuffGamePieceRitualData dataForRequest = database.GetDataForRequest(this);
			yield return new ActionPhase_SelectCombatStat(new Action<List<CombatStatType>>(this.SetStats), (TurnContext _, List<List<CombatStatType>> _, List<CombatStatType> _, int _) => Result.Success, dataForRequest.AttributeCount);
			yield break;
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x000685F8 File Offset: 0x000667F8
		public bool TryAddStat(GameDatabase database, CombatStatType stat)
		{
			DebuffGamePieceRitualData dataForRequest = database.GetDataForRequest(this);
			if (this.TargetStats.Count >= dataForRequest.AttributeCount)
			{
				return false;
			}
			this.AddStat(stat);
			return true;
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x0006862A File Offset: 0x0006682A
		private void AddStat(CombatStatType stat)
		{
			this.TargetStats.Add(stat);
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x00068638 File Offset: 0x00066838
		private void SetStats(IEnumerable<CombatStatType> stats)
		{
			this.TargetStats = IEnumerableExtensions.ToList<CombatStatType>(stats);
		}

		// Token: 0x04000CD0 RID: 3280
		[JsonProperty]
		public List<CombatStatType> TargetStats = new List<CombatStatType>();
	}
}
