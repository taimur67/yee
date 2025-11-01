using System;
using System.Collections.Generic;
using System.ComponentModel;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200060B RID: 1547
	public class PromoteGamePieceEventOrder : PlayGrandEventOrder
	{
		// Token: 0x06001CC4 RID: 7364 RVA: 0x0006349E File Offset: 0x0006169E
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			yield return new ActionPhase_TargetGamePiece(new Action<GamePiece>(this.SetGamePiece), new ActionPhase_Target<GamePiece>.IsValidFunc(this.IsValidGamePiece), new ActionPhase_TargetGamePiece.IsValidArchfiendFunc(this.IsValidArchfiend), new ActionPhase_TargetGamePiece.IsSelectableGamePieceFunc(base.FilterGamePiece), 1, ActionPhaseType.None);
			yield break;
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x000634AE File Offset: 0x000616AE
		private void SetGamePiece(GamePiece target)
		{
			this.TargetGamePiece = target;
		}

		// Token: 0x04000C80 RID: 3200
		[BindableValue("target", BindingOption.None)]
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier TargetGamePiece = Identifier.Invalid;
	}
}
