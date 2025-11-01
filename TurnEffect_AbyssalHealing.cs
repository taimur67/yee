using System;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200034B RID: 843
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class TurnEffect_AbyssalHealing : TurnAbilityEffect
	{
		// Token: 0x1700026C RID: 620
		// (get) Token: 0x0600100C RID: 4108 RVA: 0x0003F601 File Offset: 0x0003D801
		public override TurnProcessStage HasEffectInStage
		{
			get
			{
				return TurnProcessStage.TurnModule_Healing;
			}
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0003F604 File Offset: 0x0003D804
		protected override void OnStageOfTurnIfActive(Ability ability, TurnProcessContext context, GamePiece piece)
		{
			piece.Heal(this.BaseHealing);
			foreach (HexCoord coord in context.HexBoard.GetNeighbours(piece.Location, false))
			{
				GamePiece gamePieceAt = context.CurrentTurn.GetGamePieceAt(coord);
				if (gamePieceAt != null && this.LegionRef.Id == gamePieceAt.StaticDataId)
				{
					piece.Heal(this.HealingPerAdjacentLegion);
				}
			}
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0003F698 File Offset: 0x0003D898
		public override void DeepClone(out AbilityEffect clone)
		{
			TurnEffect_AbyssalHealing turnEffect_AbyssalHealing = new TurnEffect_AbyssalHealing
			{
				BaseHealing = this.BaseHealing,
				HealingPerAdjacentLegion = this.HealingPerAdjacentLegion,
				LegionRef = this.LegionRef.DeepClone<GamePieceStaticData>()
			};
			base.DeepCloneTurnAbilityEffectParts(turnEffect_AbyssalHealing);
			clone = turnEffect_AbyssalHealing;
		}

		// Token: 0x0400076F RID: 1903
		[JsonProperty]
		public int BaseHealing;

		// Token: 0x04000770 RID: 1904
		[JsonProperty]
		public int HealingPerAdjacentLegion;

		// Token: 0x04000771 RID: 1905
		[JsonProperty]
		public ConfigRef<GamePieceStaticData> LegionRef;
	}
}
