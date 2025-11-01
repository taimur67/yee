using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020001CF RID: 463
	[Serializable]
	public class BattleResult : IDeepClone<BattleResult>
	{
		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x0600089F RID: 2207 RVA: 0x00029B06 File Offset: 0x00027D06
		public int AttackingPlayerId
		{
			get
			{
				GamePiece attacker_StartState = this.Attacker_StartState;
				if (attacker_StartState == null)
				{
					return int.MinValue;
				}
				return attacker_StartState.ControllingPlayerId;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x00029B1D File Offset: 0x00027D1D
		public int DefendingPlayerId
		{
			get
			{
				GamePiece defender_StartState = this.Defender_StartState;
				if (defender_StartState == null)
				{
					return int.MinValue;
				}
				return defender_StartState.ControllingPlayerId;
			}
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x00029B34 File Offset: 0x00027D34
		public bool InvolvesGamePiece(Identifier id)
		{
			return this.AttackerId == id || this.DefenderId == id;
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x00029B4A File Offset: 0x00027D4A
		public bool InvolvesPlayer(int playerId)
		{
			return this.AttackingPlayerId == playerId || this.DefendingPlayerId == playerId;
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00029B60 File Offset: 0x00027D60
		private BattleResult()
		{
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x00029B80 File Offset: 0x00027D80
		public BattleResult(GamePiece defender, GamePiece attacker, HexCoord hex)
		{
			this.Defender_StartState = defender.DeepClone<GamePiece>();
			this.Attacker_StartState = attacker.DeepClone<GamePiece>();
			this.AttackerId = ((attacker != null) ? attacker.Id : Identifier.Invalid);
			this.DefenderId = ((defender != null) ? defender.Id : Identifier.Invalid);
			this.Hex = hex;
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x00029BEC File Offset: 0x00027DEC
		public bool TryGetSupportingPiecesForPlayer(int playerId, out IReadOnlyList<Identifier> ourSupport, out IReadOnlyList<Identifier> theirSupport)
		{
			if (playerId == this.Attacker_StartState.ControllingPlayerId)
			{
				ourSupport = this.AttackerSupportingPieces;
				theirSupport = this.DefenderSupportingPieces;
				return true;
			}
			if (playerId == this.Defender_StartState.ControllingPlayerId)
			{
				ourSupport = this.DefenderSupportingPieces;
				theirSupport = this.AttackerSupportingPieces;
				return true;
			}
			IReadOnlyList<Identifier> readOnlyList;
			theirSupport = (readOnlyList = null);
			ourSupport = readOnlyList;
			return false;
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00029C44 File Offset: 0x00027E44
		public bool TryGetPiecesForPlayer(int playerId, bool startState, out GamePiece ourPiece, out GamePiece theirPiece)
		{
			ourPiece = null;
			theirPiece = null;
			GamePiece gamePiece = startState ? this.Attacker_StartState : this.Attacker_EndState;
			GamePiece gamePiece2 = startState ? this.Defender_StartState : this.Defender_EndState;
			if (playerId == this.Attacker_StartState.ControllingPlayerId)
			{
				ourPiece = gamePiece;
				theirPiece = gamePiece2;
			}
			else if (playerId == this.Defender_StartState.ControllingPlayerId)
			{
				ourPiece = gamePiece2;
				theirPiece = gamePiece;
			}
			return ourPiece != null && theirPiece != null;
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x00029CB4 File Offset: 0x00027EB4
		public bool GetOriginalControllingPlayer(Identifier id, out int playerId)
		{
			playerId = int.MinValue;
			if (this.Attacker_StartState.Id == id)
			{
				playerId = this.AttackingPlayerId;
			}
			else if (this.Defender_StartState.Id == id)
			{
				playerId = this.DefendingPlayerId;
			}
			return playerId != int.MinValue;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x00029D02 File Offset: 0x00027F02
		public bool IsBetween(Identifier participant, Identifier other)
		{
			return this.InvolvesGamePiece(participant) && this.InvolvesGamePiece(other);
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00029D18 File Offset: 0x00027F18
		public bool DidWin(Identifier id)
		{
			BattleOutcome outcome = this.Outcome;
			bool result;
			if (outcome != BattleOutcome.Victory_Attacker)
			{
				result = (outcome == BattleOutcome.Victory_Defender && id == this.DefenderId);
			}
			else
			{
				result = (id == this.AttackerId);
			}
			return result;
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x00029D54 File Offset: 0x00027F54
		public bool PlayerHasWon(int playerID)
		{
			BattleOutcome outcome = this.Outcome;
			bool result;
			if (outcome != BattleOutcome.Victory_Attacker)
			{
				result = (outcome == BattleOutcome.Victory_Defender && playerID == this.DefendingPlayerId);
			}
			else
			{
				result = (playerID == this.AttackingPlayerId);
			}
			return result;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x00029D90 File Offset: 0x00027F90
		public bool TryGetLosingPiece_EndState(out GamePiece loser)
		{
			BattleOutcome outcome = this.Outcome;
			GamePiece gamePiece;
			if (outcome != BattleOutcome.Victory_Attacker)
			{
				if (outcome != BattleOutcome.Victory_Defender)
				{
					gamePiece = null;
				}
				else
				{
					gamePiece = this.Attacker_EndState;
				}
			}
			else
			{
				gamePiece = this.Defender_EndState;
			}
			loser = gamePiece;
			return loser != null;
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00029DCC File Offset: 0x00027FCC
		public bool TryGetWinningPiece_EndState(out GamePiece winner)
		{
			BattleOutcome outcome = this.Outcome;
			GamePiece gamePiece;
			if (outcome != BattleOutcome.Victory_Attacker)
			{
				if (outcome != BattleOutcome.Victory_Defender)
				{
					gamePiece = null;
				}
				else
				{
					gamePiece = this.Defender_EndState;
				}
			}
			else
			{
				gamePiece = this.Attacker_EndState;
			}
			winner = gamePiece;
			return winner != null;
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00029E08 File Offset: 0x00028008
		public void DeepClone(out BattleResult clone)
		{
			clone = new BattleResult
			{
				Hex = this.Hex,
				Outcome = this.Outcome,
				AttackerId = this.AttackerId,
				DefenderId = this.DefenderId,
				Defender_StartState = this.Defender_StartState.DeepClone<GamePiece>(),
				Attacker_StartState = this.Attacker_StartState.DeepClone<GamePiece>(),
				Defender_EndState = this.Defender_EndState.DeepClone<GamePiece>(),
				Attacker_EndState = this.Attacker_EndState.DeepClone<GamePiece>(),
				AttackerSupportingPieces = this.AttackerSupportingPieces.DeepClone(),
				DefenderSupportingPieces = this.DefenderSupportingPieces.DeepClone()
			};
		}

		// Token: 0x04000448 RID: 1096
		public HexCoord Hex;

		// Token: 0x04000449 RID: 1097
		public BattleOutcome Outcome;

		// Token: 0x0400044A RID: 1098
		public Identifier AttackerId;

		// Token: 0x0400044B RID: 1099
		public Identifier DefenderId;

		// Token: 0x0400044C RID: 1100
		public GamePiece Defender_StartState;

		// Token: 0x0400044D RID: 1101
		public GamePiece Attacker_StartState;

		// Token: 0x0400044E RID: 1102
		public GamePiece Defender_EndState;

		// Token: 0x0400044F RID: 1103
		public GamePiece Attacker_EndState;

		// Token: 0x04000450 RID: 1104
		public List<Identifier> AttackerSupportingPieces = new List<Identifier>();

		// Token: 0x04000451 RID: 1105
		public List<Identifier> DefenderSupportingPieces = new List<Identifier>();
	}
}
