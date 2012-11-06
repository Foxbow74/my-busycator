using System;
using System.Collections.Generic;
using System.Drawing;
using GameCore;
using GameCore.Storage;
using GameUi;
using ResourceWizard.StoreableVMs;

namespace ResourceWizard
{
	class Manager
	{
		readonly XResourceServer m_resourceSrv = new XResourceServer();
		readonly XClient m_resourceCli = new XClient();

		private static Manager m_instance;

		private Manager()
		{
			//XRoot.TileSets
		}

		public XResourceRootVM XRoot
		{
			get
			{
				return m_resourceCli.GetRoot<XResourceRootVM>();
			}
		}

		public void Save() { World.SaveResources(); }

		public static Manager Instance
		{
			get { return m_instance ?? (m_instance = new Manager()); }
		}

		readonly Dictionary<ETextureSet,Bitmap> m_textures = new Dictionary<ETextureSet, Bitmap>();


		public Bitmap this[ETextureSet _set]
		{
			get
			{
				Bitmap value;
				if(!m_textures.TryGetValue(_set, out value))
				{
					string fileName;
					switch (_set)
					{
						case ETextureSet.RJ:
							
							fileName = "Resources\\redjack15v.bmp";
							break;
						case ETextureSet.RB1:
							fileName = "Resources\\RantingRodent_Brick_01.bmp";
							break;
						case ETextureSet.RB2:
							fileName = "Resources\\RantingRodent_Brick_02.bmp";
							break;
						case ETextureSet.RN1:
							fileName = "Resources\\RantingRodent_Natural_01.bmp";
							break;
						case ETextureSet.RN2:
							fileName = "Resources\\RantingRodent_Natural_02.bmp";
							break;
						case ETextureSet.GP:
							fileName = "Resources\\gold_plated_16x16.bmp";
							break;
						case ETextureSet.NH:
							fileName = "Resources\\nethack.bmp";
							break;
						case ETextureSet.HM:
							fileName = "Resources\\aq.png";
							break;
						case ETextureSet.PH:
							fileName = "Resources\\Phoebus_16x16.png";
							break;
						case ETextureSet.U4:
							fileName = "Resources\\Ultima4.png";
							break;
						case ETextureSet.U5:
							fileName = "Resources\\Ultima5.png";
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					value = (Bitmap)Bitmap.FromFile(fileName);
					m_textures[_set] = value;
				}
				return value;
			}
		}
	}
}
