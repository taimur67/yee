using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000746 RID: 1862
	public class SpawnLegionsProcessor : EdictEffectModuleProcessor<SpawnLegionsInstance, SpawnLegionsEffectStaticData>
	{
		// Token: 0x060022FA RID: 8954 RVA: 0x00079521 File Offset: 0x00077721
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_TurnEnd, new TurnModuleProcessor.ProcessEvent(this.OnTurnEnd));
		}

		// Token: 0x060022FB RID: 8955 RVA: 0x0007953C File Offset: 0x0007773C
		public void OnTurnEnd()
		{
			IEnumerable<Hex> enumerable = IEnumerableExtensions.ToList<Hex>(from x in base._currentTurn.HexBoard.GetHexesControlledByPlayer(base.Instance.TargetPlayerId)
			where base._currentTurn.GetGamePieceAt(x.HexCoord) == null
			select x).SelectRandom(base._random, this._staticData.SpawnCount);
			GamePieceStaticData staticData = base._database.Fetch(this._staticData.Legion);
			foreach (Hex hex in enumerable)
			{
				GamePiece gamePiece = this.TurnProcessContext.SpawnLegion(staticData, base._currentTurn.ForceMajeurePlayer, hex.HexCoord);
				IdentifiableStaticData data;
				if (this._staticData.Behaviour != null && base._database.TryFetch(this._staticData.Behaviour, out data))
				{
					NeutralForceTurnModuleInstance neutralForceTurnModuleInstance = (NeutralForceTurnModuleInstance)TurnModuleInstanceFactory.CreateInstance(base._currentTurn, data);
					base._currentTurn.AddActiveTurnModule(this.TurnProcessContext, neutralForceTurnModuleInstance);
					neutralForceTurnModuleInstance.GamePieceId = gamePiece.Id;
					LegionSpawnedEvent gameEvent = new LegionSpawnedEvent(-1, gamePiece.Id, hex.HexCoord, LegionSpawnedEvent.LegionSpawnType.Edict, gamePiece);
					base._currentTurn.AddGameEvent<LegionSpawnedEvent>(gameEvent);
				}
			}
			base.RemoveSelf();
		}
	}
}
