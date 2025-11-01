using System;
using System.ComponentModel;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200050B RID: 1291
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class DiplomaticState_ChainsOfAvarice : ArmisticeState
	{
		// Token: 0x1700037B RID: 891
		// (get) Token: 0x060018D6 RID: 6358 RVA: 0x00058A85 File Offset: 0x00056C85
		[JsonIgnore]
		public override DiplomaticStateValue Type
		{
			get
			{
				return DiplomaticStateValue.ChainsOfAvarice;
			}
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x00058A89 File Offset: 0x00056C89
		[JsonConstructor]
		protected DiplomaticState_ChainsOfAvarice()
		{
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x00058AA7 File Offset: 0x00056CA7
		public DiplomaticState_ChainsOfAvarice(int actorId, int length) : base(length)
		{
			this.ActorId = actorId;
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x00058AD0 File Offset: 0x00056CD0
		public override void OnStateEntered(TurnProcessContext context, DiplomaticPairStatus pair, DiplomaticState prevState, bool interrupt)
		{
			base.OnStateEntered(context, pair, prevState, interrupt);
			DiplomaticAbility_ChainsOfAvarice diplomaticAbility_ChainsOfAvarice;
			if (!context.Database.TryFetchSingle(out diplomaticAbility_ChainsOfAvarice))
			{
				return;
			}
			ArchfiendModifierStaticData data;
			if (!diplomaticAbility_ChainsOfAvarice.TryGetComponent<ArchfiendModifierStaticData>(out data))
			{
				return;
			}
			int num;
			if (!pair.TryGetOtherPlayer(this.ActorId, out num))
			{
				return;
			}
			IModifier modifier = ModifierExtensions.CreateModifier(new DiplomaticStateContext(this.Type, this.ActorId, num), data);
			this._installedModifier = context.CurrentTurn.PushGlobalModifier(new PlayerTargetGroup(modifier, new int[]
			{
				num
			}));
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x00058B4E File Offset: 0x00056D4E
		public override void OnStateExited(TurnProcessContext context, DiplomaticPairStatus pair, DiplomaticState nextState, bool interrupt)
		{
			base.OnStateExited(context, pair, nextState, interrupt);
			if (this._installedModifier != Guid.Empty)
			{
				context.CurrentTurn.GlobalModifierStack.Pop(this._installedModifier);
			}
		}

		// Token: 0x060018DB RID: 6363 RVA: 0x00058B83 File Offset: 0x00056D83
		protected DiplomaticState_ChainsOfAvarice DeepCloneParts(DiplomaticState_ChainsOfAvarice state)
		{
			base.DeepCloneParts(state);
			state._installedModifier = this._installedModifier;
			state.ActorId = this.ActorId;
			return state;
		}

		// Token: 0x060018DC RID: 6364 RVA: 0x00058BA6 File Offset: 0x00056DA6
		public override void DeepClone(out DiplomaticState clone)
		{
			clone = this.DeepCloneParts(new DiplomaticState_ChainsOfAvarice());
		}

		// Token: 0x04000B92 RID: 2962
		[JsonProperty]
		[DefaultValue(-2147483648)]
		public int ActorId = int.MinValue;

		// Token: 0x04000B93 RID: 2963
		[JsonProperty]
		private Guid _installedModifier = Guid.Empty;
	}
}
