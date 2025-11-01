using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;

namespace LoG
{
	// Token: 0x0200053D RID: 1341
	public static class TurnStateExtensions
	{
		// Token: 0x060019FA RID: 6650 RVA: 0x0005AC7D File Offset: 0x00058E7D
		public static bool TryAddActiveTurnModule(this TurnState turn, TurnProcessContext context, IdentifiableStaticData data)
		{
			return turn.AddActiveTurnModule(context, data) != null;
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x0005AC8C File Offset: 0x00058E8C
		public static T GetTurnModule<T>(this TurnState turn, TurnModuleInstanceId id) where T : TurnModuleInstance
		{
			return turn.ActiveModules.OfType<T>().FirstOrDefault((T x) => x.Id == id);
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x0005ACC4 File Offset: 0x00058EC4
		public static TurnModuleInstance AddActiveTurnModule(this TurnState turn, TurnProcessContext context, IdentifiableStaticData data)
		{
			TurnModuleInstance turnModuleInstance = TurnModuleInstanceFactory.CreateInstance(turn, data);
			if (turnModuleInstance == null)
			{
				return null;
			}
			turn.AddActiveTurnModule(context, turnModuleInstance);
			return turnModuleInstance;
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x0005ACE8 File Offset: 0x00058EE8
		public static void AddActiveTurnModule(this TurnState turn, TurnProcessContext context, TurnModuleInstance instance)
		{
			turn.ActiveModules.Add(instance);
			TurnModuleProcessor turnModuleProcessor = TurnModuleProcessorFactory.CreateProcessor(instance, context);
			turnModuleProcessor.OnAdded();
			context.AddModuleProcessor(turnModuleProcessor);
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x0005AD18 File Offset: 0x00058F18
		public static void RemoveActiveTurnModule(this TurnState turn, TurnProcessContext context, TurnModuleInstance instance)
		{
			turn.ActiveModules.Remove(instance);
			TurnModuleProcessor turnModuleProcessor = TurnModuleProcessorFactory.CreateProcessor(instance, context);
			turnModuleProcessor.OnRemoved();
			context.RemoveModuleProcessor(turnModuleProcessor);
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x0005AD48 File Offset: 0x00058F48
		public static IEnumerable<GamePiece> EnumerateGamePiecesCapturableBy(this TurnState turn, PlayerState player)
		{
			return from t in turn.EnumerateAllGamePieces()
			where LegionMovementProcessor.IsCapturableBy(turn, t, player)
			select t;
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x0005AD85 File Offset: 0x00058F85
		public static IEnumerable<GameEntity> EnumerateAllGameEntities(this TurnState turn)
		{
			return turn.EnumeratePlayerStates(true, true).Concat(turn.AllGameItems);
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x0005AD9C File Offset: 0x00058F9C
		public static Dictionary<HexCoord, GamePiece> GenerateHexCoordToGamePieceLookup(this TurnState turn)
		{
			Dictionary<HexCoord, GamePiece> dictionary = new Dictionary<HexCoord, GamePiece>();
			foreach (GamePiece gamePiece in turn.GetActiveGamePieces())
			{
				if (dictionary.ContainsKey(gamePiece.Location))
				{
					SimLogger logger = SimLogger.Logger;
					if (logger != null)
					{
						logger.Error(string.Format("More than 1 game piece at location {0}", gamePiece.Location));
					}
				}
				else
				{
					dictionary.Add(gamePiece.Location, gamePiece);
				}
			}
			return dictionary;
		}
	}
}
