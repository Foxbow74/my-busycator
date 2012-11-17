using GameCore.Creatures;
using GameCore.Essences.Weapons;
using GameCore.Mapping;
using GameCore.Misc;

namespace GameCore.Essences.Mechanisms
{
	public class MagicPlate : Mechanism, IRemoteActivation
	{
		public EMagicPlateEffect Effect { get; private set; }

		public MagicPlate(Material _material, uint _mechanismId, EMagicPlateEffect _effect)
			: base(_material, _mechanismId)
		{
			Effect = _effect;
		}

		public override string Name
		{
			get { return "пластина"; }
		}

		public override void Resolve(Creature _creature)
		{
		}

		public override int TileIndex
		{
			get
			{
				return 2;
			}
		}

		public void RemoteActivation(Creature _creature, Point _worldCoords)
		{
			var creatureLiveCell = _creature[0, 0];
			var delta = _worldCoords - creatureLiveCell.WorldCoords;
			_creature[delta.X,delta.Y].AddItem(EssenceHelper.GetFirstFoundedItem<Axe>());
		}
	}

	public interface IRemoteActivation
	{
		uint MechanismId { get; }
		void RemoteActivation(Creature _creature, Point _worldCoords);
	}
}