using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000222 RID: 546
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class ActionFailedEvent : GameEvent
	{
		// Token: 0x06000AA8 RID: 2728 RVA: 0x0002EADF File Offset: 0x0002CCDF
		[JsonConstructor]
		private ActionFailedEvent()
		{
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x0002EAE7 File Offset: 0x0002CCE7
		public ActionFailedEvent(ActionableOrder action, Result result, int playerID) : base(playerID)
		{
			this.Action = action;
			this.Result = result;
			base.AddAffectedPlayerId(playerID);
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0002EB05 File Offset: 0x0002CD05
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Action Failed - {0}, {1}", this.Action, this.Result.DebugString);
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0002EB22 File Offset: 0x0002CD22
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.OrderProblem;
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0002EB28 File Offset: 0x0002CD28
		public override void DeepClone(out GameEvent clone)
		{
			ActionFailedEvent actionFailedEvent = new ActionFailedEvent
			{
				Action = this.Action.DeepClone(CloneFunction.FastClone),
				Result = this.Result.DeepClone(CloneFunction.FastClone)
			};
			base.DeepCloneGameEventParts<ActionFailedEvent>(actionFailedEvent);
			clone = actionFailedEvent;
		}

		// Token: 0x040004EB RID: 1259
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public ActionableOrder Action;

		// Token: 0x040004EC RID: 1260
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Result Result;
	}
}
