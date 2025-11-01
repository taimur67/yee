using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200039D RID: 925
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class StatTransferModifier : Modifier<GamePiece, StatTransferModiferStaticData>, IRevealsKnowledge
	{
		// Token: 0x060011B1 RID: 4529 RVA: 0x00043D1D File Offset: 0x00041F1D
		[JsonConstructor]
		protected StatTransferModifier()
		{
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00043D25 File Offset: 0x00041F25
		public StatTransferModifier(StatTransferModiferStaticData data = null) : base(data)
		{
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x00043D30 File Offset: 0x00041F30
		public override void ApplyTo(TurnContext context, GamePiece item)
		{
			if (!this.Data.AllowedGamePieceCategories.Contains(item.SubCategory))
			{
				return;
			}
			ModifiableValue currentLevel = context.CurrentTurn.FindPlayerState(item.ControllingPlayerId, null).PowersLevels[this.Data.PowerType].CurrentLevel;
			base.ApplyStatModifier(item.CombatStats.GetStat(this.Data.TargetStat), currentLevel, this.Data.TargetType);
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x00043DB4 File Offset: 0x00041FB4
		public KnowledgeModifier GenerateKnowledgeModifier(TurnState view, PlayerState player, PlayerState targetPlayer)
		{
			PlayerPowerLevel playerPowerLevel = targetPlayer.PowersLevels[this.Data.PowerType].DeepClone<PlayerPowerLevel>();
			KnowledgeModifier result = new PowerKnowledgeModifier(targetPlayer.Id, this.Data.PowerType, playerPowerLevel);
			player.GetOrCreateKnowledgeContext(targetPlayer.Id).UpdatePower(view, this.Data.PowerType, playerPowerLevel);
			return result;
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00043E14 File Offset: 0x00042014
		public override void InstallInto(GamePiece item, TurnState turn, bool baseAdjust = false)
		{
			if (!this.Data.AllowedGamePieceCategories.Contains(item.SubCategory))
			{
				return;
			}
			ModifiableValue currentLevel = turn.FindPlayerState(item.ControllingPlayerId, null).PowersLevels[this.Data.PowerType].CurrentLevel;
			base.InstallStatModifier(item.CombatStats.GetStat(this.Data.TargetStat), currentLevel, this.Data.TargetType, false);
		}
	}
}
