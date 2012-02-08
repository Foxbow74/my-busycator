using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.Acts;
using GameCore.Messages;
using GameCore.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks.Items
{
	internal class BackpackUiBlock : ItemsSelectorUiBlock
	{
		private readonly IEnumerable<EThingCategory> m_allowedCategories;

		public BackpackUiBlock(Rectangle _rectangle)
			: base(_rectangle, ESelectItemDialogBehavior.ALLOW_CHANGE_FILTER, null, World.TheWorld.Avatar.GetBackPackItems().OrderBy(_thingDescriptor => _thingDescriptor.Thing.Name))
		{
			m_allowedCategories = new EThingCategory[0];
		}

		public BackpackUiBlock(Rectangle _rectangle, ESelectItemDialogBehavior _behavior, IEnumerable<EThingCategory> _allowedCategory, Act _act)
			: base(_rectangle, _behavior, _act, World.TheWorld.Avatar.GetBackPackItems().OrderBy(_thingDescriptor => _thingDescriptor.Thing.Name))
		{
			m_allowedCategories = _allowedCategory ?? new EThingCategory[0];
		}

		protected override IEnumerable<EThingCategory> AllowedCategories
		{
			get { return m_allowedCategories; }
		}

		protected override int HeaderTakesLine
		{
			get { return 4; }
		}

		protected override void DrawHeader(SpriteBatch _spriteBatch)
		{
			DrawLine("СОДЕРЖИМОЕ РЮКЗАКА", Color.White, _spriteBatch, 0, 0, EAlignment.CENTER);
			DrawLine("ВЕС:", Color, _spriteBatch, 2, 0, EAlignment.LEFT);
			DrawLine("ДОСТУПНЫЙ ВЕС:", Color, _spriteBatch, 2, 0, EAlignment.RIGHT);
		}
	}
}