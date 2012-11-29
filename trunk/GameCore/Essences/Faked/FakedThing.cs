namespace GameCore.Essences.Faked
{
	public class FakedThing : Thing, IFaked
	{
		public FakedThing(Essence _essence) : base(_essence.Material)
		{
			Essence = _essence;
		}

		public override FColor LerpColor{ get{ return Essence.LerpColor; } }

		public override string Name { get { return Essence.Name; } }

		public override EMaterialType AllowedMaterialsType { get { return Essence.AllowedMaterialsType; } }

		#region IFaked Members

		public override int TileIndex
		{
			get
			{
				return Essence.TileIndex;
			}
		}

		public Essence Essence { get; private set; }

		public override ETileset Tileset
		{
			get { return Essence.Tileset; }
		}

		#endregion

		public override bool Is<T>() { return Essence is T; }

		protected override int CalcHashCode() { return Essence.GetHashCode(); }
	}
}