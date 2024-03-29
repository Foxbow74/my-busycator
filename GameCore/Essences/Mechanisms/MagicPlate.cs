using System;
using GameCore.AbstractLanguage;
using GameCore.Creatures;
using GameCore.Messages;
using GameCore.Misc;

namespace GameCore.Essences.Mechanisms
{
	public class MagicPlate : Mechanism, IRemoteActivation
	{
		public MagicPlate(Material _material, uint _mechanismId, EMagicPlateEffect _effect)
			: base(EALNouns.MagicPlate, _material, _mechanismId)
		{
			Effect = _effect;
		}

		public EMagicPlateEffect Effect { get; private set; }

		public override int TileIndex
		{
			get
			{
				return 2;
			}
		}

		#region IRemoteActivation Members

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
						var monster = (AbstractMonster)EssenceHelper.GetRandomFakedCreature<AbstractMonster>(World.Rnd).Essence.Clone(_creature);
						monster.Behaviour=EMonsterBehaviour.IDLE;
						World.TheWorld.CreatureManager.AddCreature(monster, myCell.WorldCoords, myCell.LiveCoords);

						MessageManager.SendMessage(this, new SoundTextMessage("���������� �������"));
					}
					else
					{
						MessageManager.SendMessage(this, new SoundTextMessage("���������� �����"));
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		#endregion
	}
}