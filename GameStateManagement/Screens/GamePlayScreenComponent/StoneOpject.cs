using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tutorial009.Models;

namespace Tutorial009.Sprites
{

	public class StoneOpject : Sprite
	{
		public StoneOpject(Texture2D texture)
		  : base(texture)
		{

		}

		public override void Update(GameTime gameTime, List<Sprite> sprites)
		{

			foreach (var sprite in sprites)
			{
				if (sprite == this)
					continue;

				if ((this.Velocity.X > 0 && this.IsTouchingLeft(sprite)) ||
					(this.Velocity.X < 0 & this.IsTouchingRight(sprite)))
					this.Velocity.X = 0;

				if ((this.Velocity.Y > 0 && this.IsTouchingTop(sprite)) ||
					(this.Velocity.Y < 0 & this.IsTouchingBottom(sprite)))
					this.Velocity.Y = 0;
			}

			Position += Velocity;

			Velocity = Vector2.Zero;
		}

	}
}
