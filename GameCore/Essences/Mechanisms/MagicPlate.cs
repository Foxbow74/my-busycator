using System;
using GameCore.Creatures;
using GameCore.Messages;
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
			var myCell = _creature[delta.X, delta.Y];

			switch (Effect)
			{
				case EMagicPlateEffect.RANDOM_MONSTER_APPEAR:
					if (myCell.Creature==null)
					{
						var monster = (Monster)EssenceHelper.GetRandomFakedCreature<Monster>(World.Rnd).ResolveFake(_creature);
						monster.Behaviour=EMonsterBehaviour.ATACK_AVATAR;
						myCell.AddCreature(monster);

						MessageManager.SendMessage(this, new SoundTextMessage("послышался всплеск"));
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}