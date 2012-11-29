using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;

namespace GameCore.Essences.Mechanisms
{
	public class Button : Mechanism, IInteractiveThing
	{
		public Button(Material _material, uint _mechanismId)
			: base(_material, _mechanismId)
		{
		}

		public override string Name
		{
			get { return "кнопка"; }
		}

		public override int TileIndex
		{
			get
			{
				return 3;
			}
		}

		#region IInteractiveThing Members

		public EActResults Interract(Creature _creature, LiveMapCell _liveMapCell)
		{
			var tuple = World.TheWorld.GetRemoteActivation(MechanismId);
			tuple.Item1.RemoteActivation(_creature, tuple.Item2);
			return EActResults.DONE;
		}

		#endregion
	}
}