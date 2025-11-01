using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005F2 RID: 1522
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class CreateFissureEventOrder : PlayGrandEventOrder
	{
		// Token: 0x06001C83 RID: 7299 RVA: 0x00062518 File Offset: 0x00060718
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			List<HexCoord> hexTargets = this.HexTargets;
			if (hexTargets != null)
			{
				hexTargets.Clear();
			}
			yield return new ActionPhase_TargetHex(new Action<HexCoord>(this.AddHex), new ActionPhase_Target<HexCoord>.IsValidFunc(this.IsValidConnectedHex), 2);
			yield break;
		}

		// Token: 0x06001C84 RID: 7300 RVA: 0x00062528 File Offset: 0x00060728
		private void AddHex(HexCoord hex)
		{
			if (this.HexTargets == null)
			{
				this.HexTargets = new List<HexCoord>();
			}
			this.HexTargets.Add(hex);
		}

		// Token: 0x06001C85 RID: 7301 RVA: 0x0006254C File Offset: 0x0006074C
		private Result IsValidConnectedHex(TurnContext context, List<HexCoord> selected, HexCoord target, int castingPlayerId)
		{
			Problem problem = this.IsValidHex(context, selected, target, castingPlayerId) as Problem;
			if (problem != null)
			{
				return problem;
			}
			if (selected == null || selected.Count <= 0)
			{
				return Result.Success;
			}
			if (!context.HexBoard.EnumerateNeighbours(target).Any(new Func<HexCoord, bool>(selected.Contains)))
			{
				return Result.Failure;
			}
			return Result.Success;
		}

		// Token: 0x04000C7D RID: 3197
		[JsonProperty]
		public List<HexCoord> HexTargets = new List<HexCoord>();
	}
}
