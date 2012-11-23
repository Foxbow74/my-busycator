using System;
using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;

namespace GameCore.Essences.Mechanisms
{
	public class Lever : Mechanism, IInteractiveThing
	{
		public Lever(Material _material, uint _mechanismId, bool _state)
			: base(_material, _mechanismId)
		{
			State = _state;
		}

		public bool State { get; private set; }

		public override string Name
		{
			get { return "�����"; }
		}

		public override int TileIndex
		{
			get
			{
				return State?0:1;
			}
		}

		#region IInteractiveThing Members

		public EActResults Interract(Creature _creature, LiveMapCell _liveMapCell)
		{
			throw new NotImplementedException();
		}

		#endregion

		public override void Resolve(Creature _creature)
		{
		}
	}
}