using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStateManagement.Screens.GamePlayScreenComponent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tutorial009.Models;

namespace Tutorial009.Sprites
{

	public class Player : Sprite
	{
		private Enemy enemy;
		public	Input Input;
		private Texture2D HpTexture;
		private Texture2D ManaTexture;
		private SpriteFont einFont;
		private SoundEffect explosionSound;
		private float Radius;
		public int playerScore;
		private int Hp;
		private int Mana;
		private char Dirction;
		public Player(Texture2D texture, Enemy enemy)
		  : base(texture)
		{
			this.enemy = enemy;	
		}
		public Texture2D getHpTexture()
		{
			return HpTexture;
		}
		public Texture2D getManaTexture()
		{
			return ManaTexture;
		}
		public float getRadius()
		{
			return Radius;
		}
		public int getMana()
		{
			return Mana;
		}
		public int getHp()
		{
			return Hp;
		}

		public override void Update(GameTime gameTime, List<Sprite> sprites)
		{
			Move();
			int enemyIndex = 0;
			while (enemyIndex < enemy.enemyPositions.Count)
			{
				// Abstand zwischen Feind-Position und Schuss-Position ermitteln
				float distance = Vector2.Distance(enemy.enemyPositions[enemyIndex], Position);

				// Treffer?

				if (distance < enemy.getEnemyRadius())
				{
					// Feind entfernen
					enemy.enemyPositions.RemoveAt(enemyIndex);
					// Punkte erhöhen
					Hp--;
					// Schleife verlassen
					break;
				}
				else
				{
					enemyIndex++;
				}
			}

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
		private void Move()
		{
			if (Keyboard.GetState().IsKeyDown(Input.Left))
				Velocity.X = -Speed;
			else if (Keyboard.GetState().IsKeyDown(Input.Right))
				Velocity.X = Speed;
			if (Keyboard.GetState().IsKeyDown(Input.Up))
				Velocity.Y = -Speed;
			else if (Keyboard.GetState().IsKeyDown(Input.Down))
				Velocity.Y = Speed;
		}
	}
}
