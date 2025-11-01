using System;

namespace LoG
{
	// Token: 0x020003A8 RID: 936
	public static class GamePieceStatUtils
	{
		// Token: 0x06001244 RID: 4676 RVA: 0x00046058 File Offset: 0x00044258
		public static ModifiableValue GetStat(this GamePiece gamePiece, GamePieceStatUtils.GamePieceVariable var)
		{
			ModifiableValue result;
			switch (var)
			{
			case GamePieceStatUtils.GamePieceVariable.Ranged:
				result = gamePiece.CombatStats.Ranged;
				break;
			case GamePieceStatUtils.GamePieceVariable.Melee:
				result = gamePiece.CombatStats.Melee;
				break;
			case GamePieceStatUtils.GamePieceVariable.Infernal:
				result = gamePiece.CombatStats.Infernal;
				break;
			case GamePieceStatUtils.GamePieceVariable.Health:
				result = gamePiece.HP;
				break;
			case GamePieceStatUtils.GamePieceVariable.Level:
				result = gamePiece.Level;
				break;
			case GamePieceStatUtils.GamePieceVariable.Movement:
				result = gamePiece.GroundMoveDistance;
				break;
			case GamePieceStatUtils.GamePieceVariable.Teleport:
				result = gamePiece.TeleportDistance;
				break;
			case GamePieceStatUtils.GamePieceVariable.Prestige:
				result = gamePiece.PassivePrestige;
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x0200093E RID: 2366
		public enum GamePieceVariable
		{
			// Token: 0x04001555 RID: 5461
			Ranged,
			// Token: 0x04001556 RID: 5462
			Melee,
			// Token: 0x04001557 RID: 5463
			Infernal,
			// Token: 0x04001558 RID: 5464
			Health,
			// Token: 0x04001559 RID: 5465
			Level,
			// Token: 0x0400155A RID: 5466
			Movement,
			// Token: 0x0400155B RID: 5467
			Teleport,
			// Token: 0x0400155C RID: 5468
			Prestige
		}
	}
}
