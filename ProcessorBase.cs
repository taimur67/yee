using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000652 RID: 1618
	public abstract class ProcessorBase
	{
		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06001DF2 RID: 7666 RVA: 0x00067450 File Offset: 0x00065650
		protected TurnState _currentTurn
		{
			get
			{
				return this.TurnProcessContext.CurrentTurn;
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06001DF3 RID: 7667 RVA: 0x0006745D File Offset: 0x0006565D
		protected GameDatabase _database
		{
			get
			{
				return this.TurnProcessContext.Database;
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06001DF4 RID: 7668 RVA: 0x0006746A File Offset: 0x0006566A
		protected GameRules _rules
		{
			get
			{
				return this.TurnProcessContext.Rules;
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06001DF5 RID: 7669 RVA: 0x00067477 File Offset: 0x00065677
		protected SimulationRandom _random
		{
			get
			{
				return this._currentTurn.Random;
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06001DF6 RID: 7670 RVA: 0x00067484 File Offset: 0x00065684
		protected DiplomaticTurnState _currentDiplomacy
		{
			get
			{
				return this.TurnProcessContext.Diplomacy;
			}
		}

		// Token: 0x04000CCA RID: 3274
		public TurnProcessContext TurnProcessContext;
	}
}
