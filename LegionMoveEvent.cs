using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000232 RID: 562
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class LegionMoveEvent : GameEvent
	{
		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000AFA RID: 2810 RVA: 0x0002F236 File Offset: 0x0002D436
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000AFB RID: 2811 RVA: 0x0002F239 File Offset: 0x0002D439
		protected override GameEventVisibilityStrippingRule GameEventVisibilityStrippingRule
		{
			get
			{
				return GameEventVisibilityStrippingRule.ForceParentChainToRemainIfVisible;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000AFC RID: 2812 RVA: 0x0002F23C File Offset: 0x0002D43C
		[JsonIgnore]
		public List<MoveStepEvent> Path
		{
			get
			{
				return IEnumerableExtensions.ToList<MoveStepEvent>(from t in base.Enumerate<MoveStepEvent>()
				where !(t is BlockedStep)
				select t);
			}
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0002F26D File Offset: 0x0002D46D
		public bool IncludesTeleportStep()
		{
			return base.Enumerate<MoveStepEvent>().Any((MoveStepEvent step) => step.PathMode == PathMode.Teleport);
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000AFE RID: 2814 RVA: 0x0002F299 File Offset: 0x0002D499
		[JsonIgnore]
		public bool HasCoords
		{
			get
			{
				return this.Path.Count > 0;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x0002F2AC File Offset: 0x0002D4AC
		[JsonIgnore]
		public Result Result
		{
			get
			{
				BlockedStep blockedStep;
				if (base.TryGet<BlockedStep>(out blockedStep))
				{
					return blockedStep.Reason;
				}
				return Result.Success;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x0002F2CF File Offset: 0x0002D4CF
		public BattleEvent MovedIntoBattleEvent
		{
			get
			{
				return base.Get<BattleEvent>();
			}
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0002F2D7 File Offset: 0x0002D4D7
		[JsonConstructor]
		private LegionMoveEvent()
		{
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x0002F2DF File Offset: 0x0002D4DF
		public LegionMoveEvent(Identifier legionId, HexCoord startingCoord, HexCoord intendedDestinationCoord, int playerId) : base(playerId)
		{
			base.AddAffectedPlayerId(playerId);
			this.LegionID = legionId;
			this.StartingCoord = startingCoord;
			this.IntendedDestinationCoord = intendedDestinationCoord;
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x0002F306 File Offset: 0x0002D506
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Legion {0} movement", this.LegionID);
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x0002F320 File Offset: 0x0002D520
		public override void DeepClone(out GameEvent clone)
		{
			LegionMoveEvent legionMoveEvent = new LegionMoveEvent
			{
				LegionID = this.LegionID,
				StartingCoord = this.StartingCoord,
				IntendedDestinationCoord = this.IntendedDestinationCoord
			};
			base.DeepCloneGameEventParts<LegionMoveEvent>(legionMoveEvent);
			clone = legionMoveEvent;
		}

		// Token: 0x04000505 RID: 1285
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier LegionID;

		// Token: 0x04000506 RID: 1286
		[JsonProperty]
		public HexCoord StartingCoord;

		// Token: 0x04000507 RID: 1287
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public HexCoord IntendedDestinationCoord;
	}
}
