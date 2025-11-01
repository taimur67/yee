using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004D8 RID: 1240
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class InsultDecisionResponse : DiplomaticDecisionResponse, IGrievanceAccessor, ISelectionAccessor
	{
		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06001752 RID: 5970 RVA: 0x00054DF1 File Offset: 0x00052FF1
		// (set) Token: 0x06001753 RID: 5971 RVA: 0x00054DF9 File Offset: 0x00052FF9
		[JsonProperty]
		public GrievanceContext GrievanceResponse { get; set; }

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06001754 RID: 5972 RVA: 0x00054E02 File Offset: 0x00053002
		public bool IsAcceptInsult
		{
			get
			{
				return this.GrievanceResponse == null || this.Choice == YesNo.No;
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06001755 RID: 5973 RVA: 0x00054E17 File Offset: 0x00053017
		public bool IsRefuseInsult
		{
			get
			{
				return !this.IsAcceptInsult;
			}
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x00054E22 File Offset: 0x00053022
		protected override void Reset()
		{
			base.Reset();
			this.GrievanceResponse = null;
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x00054E31 File Offset: 0x00053031
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			foreach (ActionPhase actionPhase in base.GetActionPhaseSteps(player, turn, database))
			{
				yield return actionPhase;
			}
			IEnumerator<ActionPhase> enumerator = null;
			InsultDecisionRequest request;
			if (!player.TryGetDecisionRequest<InsultDecisionRequest>(base.DecisionId, out request))
			{
				yield break;
			}
			yield return new ActionPhase_DiplomaticResponse
			{
				Request = request,
				Response = this
			};
			yield break;
			yield break;
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x00054E56 File Offset: 0x00053056
		protected void DeepCloneInsultDecisionResponseParts(InsultDecisionResponse insultDecisionResponse)
		{
			insultDecisionResponse.GrievanceResponse = this.GrievanceResponse.DeepClone<GrievanceContext>();
			base.DeepCloneDiplomaticDecisionResponseParts(insultDecisionResponse);
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x00054E70 File Offset: 0x00053070
		public override void DeepClone(out DecisionResponse clone)
		{
			InsultDecisionResponse insultDecisionResponse = new InsultDecisionResponse();
			this.DeepCloneInsultDecisionResponseParts(insultDecisionResponse);
			clone = insultDecisionResponse;
		}
	}
}
