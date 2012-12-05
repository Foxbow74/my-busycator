using GameCore.AbstractLanguage;
using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;

namespace GameCore.Essences.Mechanisms
{
    public abstract class Mechanism:Thing, ISpecial
    {
        protected Mechanism(EALNouns _name, Material _material, uint _mechanismId)
			: base(_name, _material)
        {
            MechanismId = _mechanismId;
        }

        public uint MechanismId { get; protected set; }

        public override ETileset Tileset
        {
            get
            {
                return ETileset.MECHANISMS;
            }
        }
    }

	public interface IInteractiveThing
    {
		EActResults Interract(Creature _creature, LiveMapCell _liveMapCell);
    }

    public enum EMagicPlateEffect
    {
        RANDOM_MONSTER_APPEAR,
    }
}
