using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000684 RID: 1668
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ModifyGamePieceKnowledgeRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001EAA RID: 7850 RVA: 0x00069AA4 File Offset: 0x00067CA4
		public ModifyGamePieceKnowledgeRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x00069AB1 File Offset: 0x00067CB1
		public ModifyGamePieceKnowledgeRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x00069ABA File Offset: 0x00067CBA
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			yield return new ActionPhase_TargetGamePiece(delegate(GamePiece x)
			{
				this.TargetContext.SetTargetGamePiece(x);
			}, new ActionPhase_Target<GamePiece>.IsValidFunc(this.IsValidGamePiece), new ActionPhase_TargetGamePiece.IsValidArchfiendFunc(this.IsValidArchfiend), new ActionPhase_TargetGamePiece.IsSelectableGamePieceFunc(base.FilterGamePiece), 1, ActionPhaseType.None);
			yield return new ActionPhase_SelectCombatStat(delegate(List<CombatStatType> x)
			{
				this.TargetStatType = IEnumerableExtensions.FirstOrDefault<CombatStatType>(x);
			}, (TurnContext _, List<List<CombatStatType>> _, List<CombatStatType> _, int _) => Result.Success, 1);
			yield return new ActionPhase_SelectStrongerOrWeaker(delegate(StrongerWeaker x)
			{
				this.StrongerOrWeaker = x;
			});
			yield break;
		}

		// Token: 0x04000CE0 RID: 3296
		[JsonProperty]
		public CombatStatType TargetStatType;

		// Token: 0x04000CE1 RID: 3297
		[JsonProperty]
		public StrongerWeaker StrongerOrWeaker;
	}
}
