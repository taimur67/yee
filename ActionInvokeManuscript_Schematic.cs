using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020000E3 RID: 227
	public class ActionInvokeManuscript_Schematic : ActionOrderGOAPNode<InvokeManuscriptOrder>
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000352 RID: 850 RVA: 0x0000F82A File Offset: 0x0000DA2A
		public override ActionID ID
		{
			get
			{
				return ActionID.Invoke_Manuscript_Schematic;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000353 RID: 851 RVA: 0x0000F82E File Offset: 0x0000DA2E
		public override string ActionName
		{
			get
			{
				return "Invoke schematic: " + this.StaticData.Id;
			}
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000F845 File Offset: 0x0000DA45
		public ActionInvokeManuscript_Schematic(ManuscriptStaticData staticData)
		{
			this.StaticData = staticData;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000F854 File Offset: 0x0000DA54
		public override void Prepare()
		{
			base.AddPrecondition(new WPCompletedManuscript(this.StaticData, ""));
			foreach (PlayerState playerState in this.OwningPlanner.TrueTurn.EnumeratePlayerStates(false, false))
			{
				if (playerState.Id != this.OwningPlanner.PlayerId)
				{
					base.AddEffect(new WPUndermineArchfiend(playerState.Id));
				}
			}
			base.Prepare();
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000F8E8 File Offset: 0x0000DAE8
		protected override InvokeManuscriptOrder GenerateOrder()
		{
			return new ManuscriptArchfiendOrder
			{
				AbilityId = this.StaticData.ProvidedAbility.Id
			};
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000F908 File Offset: 0x0000DB08
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			Manuscript manuscript = context.CurrentTurn.GetGameItemsControlledBy<Manuscript>(playerState.Id).FirstOrDefault((Manuscript t) => t.StaticDataId == this.StaticData.Id);
			if (manuscript != null && manuscript.Id != Identifier.Invalid)
			{
				base.Order.ManuscriptId = manuscript.Id;
				Result result = base.SubmitAction(context, playerState);
				if (result.successful)
				{
					this.OwningPlanner.AIPersistentData.RecordActionUsed(ActionID.Invoke_Manuscript_Schematic, this.OwningPlanner.TrueTurn);
				}
				return result;
			}
			return Result.Failure;
		}

		// Token: 0x0400020A RID: 522
		public ManuscriptStaticData StaticData;
	}
}
