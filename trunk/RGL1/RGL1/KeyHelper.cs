using Microsoft.Xna.Framework.Input;

namespace RGL1
{
	public static class KeyHelper
	{

		private static readonly Keys[] m_keyModificators = new[]
		                                            	{
		                                            		Keys.RightShift, Keys.LeftShift, Keys.RightControl, Keys.LeftControl,
		                                            		Keys.RightAlt, Keys.LeftAlt
		                                            	};

		public static Keys[] KeyModificators
		{
			get { return m_keyModificators; }
		}
	}
}
