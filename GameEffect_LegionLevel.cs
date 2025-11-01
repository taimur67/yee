using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200034A RID: 842
	public class GameEffect_LegionLevel : GameAbilityEffect
	{
		// Token: 0x06001009 RID: 4105 RVA: 0x0003F574 File Offset: 0x0003D774
		public override void Process(TurnProcessContext context, PlayerState player)
		{
			GamePiece gamePiece = context.CurrentTurn.FetchGameItem<GamePiece>(player.PersonalGuardId);
			GamePropertiesStaticData gamePropertiesStaticData = context.Database.FetchSingle<GamePropertiesStaticData>();
			LegionLevelTable legionLevelTable = context.Database.Fetch(gamePropertiesStaticData.LegionLevelTable);
			LevelUpData dataForLevel = legionLevelTable.GetDataForLevel(gamePiece.Level + 1);
			gamePiece.LevelExperience = dataForLevel.RequiredExperience;
			gamePiece.ProcessLevelUp(context, legionLevelTable, 1);
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0003F5DC File Offset: 0x0003D7DC
		public override void DeepClone(out AbilityEffect clone)
		{
			GameEffect_LegionLevel gameEffect_LegionLevel = new GameEffect_LegionLevel();
			base.DeepCloneAbilityEffectParts(gameEffect_LegionLevel);
			clone = gameEffect_LegionLevel;
		}
	}
}
