using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002D3 RID: 723
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class CloneGamePieceActiveRitual : ActiveRitual
	{
		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000E15 RID: 3605 RVA: 0x00037AED File Offset: 0x00035CED
		// (set) Token: 0x06000E16 RID: 3606 RVA: 0x00037AF5 File Offset: 0x00035CF5
		[JsonProperty]
		public Identifier TargetGamePiece { get; set; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000E17 RID: 3607 RVA: 0x00037AFE File Offset: 0x00035CFE
		// (set) Token: 0x06000E18 RID: 3608 RVA: 0x00037B06 File Offset: 0x00035D06
		[JsonProperty]
		public List<HexCoord> CloneHexes { get; set; } = new List<HexCoord>();

		// Token: 0x06000E19 RID: 3609 RVA: 0x00037B10 File Offset: 0x00035D10
		public override Result StartRitual(TurnProcessContext context, PlayerState player, RitualCastEvent ritualCastEvent)
		{
			TurnState currentTurn = context.CurrentTurn;
			GameDatabase database = context.Database;
			CloneGamePieceRitualData data = database.Fetch<CloneGamePieceRitualData>(base.StaticDataId);
			CloneCountEntry cloneCountEntry = data.CloneCount.Find((CloneCountEntry x) => x.PowerLevel == player.PowersLevels[data.Category]);
			GamePiece source = currentTurn.FetchGameItem<GamePiece>(this.TargetGamePiece);
			for (int i = 0; i < cloneCountEntry.CloneCount; i++)
			{
				HexCoord location = this.CloneHexes[i];
				GamePiece item = currentTurn.CloneGamePiece(source, location);
				this.Clones.Add(item);
			}
			return Result.Success;
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x00037BBC File Offset: 0x00035DBC
		public override Result EndRitual(TurnProcessContext context, PlayerState player, ItemBanishedEvent banishedEvent)
		{
			TurnState currentTurn = context.CurrentTurn;
			foreach (Identifier id in this.Clones)
			{
				GamePiece gamePiece = currentTurn.FetchGameItem<GamePiece>(id);
				context.KillGamePiece(gamePiece, -1);
			}
			return Result.Success;
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x00037C28 File Offset: 0x00035E28
		public sealed override void DeepClone(out GameItem gameItem)
		{
			CloneGamePieceActiveRitual cloneGamePieceActiveRitual = new CloneGamePieceActiveRitual();
			base.DeepCloneActiveRitualParts(cloneGamePieceActiveRitual);
			cloneGamePieceActiveRitual.TargetGamePiece = this.TargetGamePiece;
			cloneGamePieceActiveRitual.CloneHexes = this.CloneHexes.DeepClone();
			cloneGamePieceActiveRitual.Clones = this.Clones.DeepClone();
			gameItem = cloneGamePieceActiveRitual;
		}

		// Token: 0x04000641 RID: 1601
		[JsonProperty]
		public List<Identifier> Clones = new List<Identifier>();
	}
}
