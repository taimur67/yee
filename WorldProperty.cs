using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200019E RID: 414
	public abstract class WorldProperty
	{
		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x000234E6 File Offset: 0x000216E6
		public GameRules GameRules
		{
			get
			{
				return this.OwningPlanner.Rules;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x000234F3 File Offset: 0x000216F3
		public GameDatabase GameDatabase
		{
			get
			{
				return this.OwningPlanner.Database;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x00023500 File Offset: 0x00021700
		public virtual bool MustBeSuccessfullFulfilledAsPrecondition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00023503 File Offset: 0x00021703
		public WorldProperty Setup(GOAPPlanner planner)
		{
			this.OwningPlanner = planner;
			this._abilityHelper = new AbilityHelper(this.OwningPlanner.AIPreviewContext, this.OwningPlanner.AIPreviewPlayerState, this.OwningPlanner.PlannedTurn);
			return this;
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0002353C File Offset: 0x0002173C
		public bool IsFulfilled(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			bool flag = this.IsFulfilledInternal(viewContext, playerState, planner);
			return this.InvertLogic != flag;
		}

		// Token: 0x06000790 RID: 1936
		internal abstract bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner);

		// Token: 0x06000791 RID: 1937 RVA: 0x0002355F File Offset: 0x0002175F
		public virtual float CalculateDynamicCostScalar(GOAPPlanner planner, IReadOnlyList<WorldProperty> effects)
		{
			return 1f;
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x00023566 File Offset: 0x00021766
		public virtual WPProvidesEffect ProvidesEffect(WorldProperty precondition)
		{
			if (base.GetType() != precondition.GetType())
			{
				return WPProvidesEffect.Cannot;
			}
			if (this.IsFulfilledInternal(this.OwningPlanner.AIPreviewContext, this.OwningPlanner.AIPreviewPlayerState, this.OwningPlanner))
			{
				return WPProvidesEffect.Redundant;
			}
			return WPProvidesEffect.Yes;
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x000235A4 File Offset: 0x000217A4
		public virtual void OnAddedAsConstraint(GOAPNode owner, TurnState turnState, PlayerState playerState)
		{
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x000235A6 File Offset: 0x000217A6
		public virtual void OnAddedAsEffect(GOAPNode owner, TurnState turnState, PlayerState playerState)
		{
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x000235A8 File Offset: 0x000217A8
		public virtual void OnAddedAsPrecondition(GOAPNode owner, TurnState turnState, PlayerState playerState)
		{
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x000235AA File Offset: 0x000217AA
		public virtual string DebugName
		{
			get
			{
				return base.GetType().Name;
			}
		}

		// Token: 0x04000376 RID: 886
		public GOAPPlanner OwningPlanner;

		// Token: 0x04000377 RID: 887
		public static int MaxWeight = 999;

		// Token: 0x04000378 RID: 888
		public bool InvertLogic;

		// Token: 0x04000379 RID: 889
		protected AbilityHelper _abilityHelper;
	}
}
