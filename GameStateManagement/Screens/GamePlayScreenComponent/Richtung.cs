using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Screens.GamePlayScreenComponent
{

	internal class Richtung
	{
		public Richtung(Vector2 vector2, char dirction)
		{
			this.vector2 = vector2;
			this.dirction = dirction;
		}
		private Vector2 vector2;
		private char dirction;

		public Vector2 getVector2()
		{
			return vector2;
		}
		public void setVector2(Vector2 vector)
		{
			this.vector2 = vector;
		}
		public char getDirction()
		{
			return dirction;
		}
		public void setDirction(char d)
		{
			this.dirction = d;
		}
	}

}
