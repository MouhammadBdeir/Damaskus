using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using GameStateManagement.GameContent;
using GameStateManagement.Screens.GamePlayScreenComponent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tutorial009.Models;

namespace Tutorial009.Sprites
{

	public class Player : Sprite
	{
		private Enemy enemy;
		public Input Input;
		private Texture2D HpTexture;
		private Texture2D ManaTexture;
		private SpriteFont einFont;
		private SoundEffect explosionSound;
		private float Radius;
		public int playerScore;
		private int Hp;
		private int Mana;
		private char Dirction;
		private int damage;
		public Player(Texture2D texture, Enemy enemy)
		  : base(texture)
		{
			this.enemy = enemy;
			damage = 50;
			Hp = 100;
			Mana= 100;
		}
		public int gatDamage()
		{
			return damage;	
		}
		public void setDamage(int damage)
		{
			this.damage += damage;
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
					Hp-=30;
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
		public void loadContent(ContentManager content)
		{
			einFont = content.Load<SpriteFont>("Arial");
			HpTexture = content.Load<Texture2D>("heart");
			ManaTexture = content.Load<Texture2D>("mana");
		}
		public void Move(Laser laser, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState)
		{
			if (Keyboard.GetState().IsKeyDown(Input.Left))
			{
				Velocity.X = -Speed;
				Dirction = 'l';
			}
			else if (Keyboard.GetState().IsKeyDown(Input.Right))
			{
				Velocity.X = Speed;
				Dirction = 'r';
			}
			if (Keyboard.GetState().IsKeyDown(Input.Up))
			{
				Velocity.Y = -Speed;
				Dirction = 'u';
			}	
			else if (Keyboard.GetState().IsKeyDown(Input.Down))
			{
				Velocity.Y = Speed;
				Dirction = 'd';
			}
			if (IsNewKeyPressed(Keys.Space, currentKeyboardState, previousKeyboardState))
			{
				//Ersetzt
				laser.FireLaser();
			}
		}
		public void Draw(SpriteBatch _spriteBatch)
		{
			_spriteBatch.DrawString(einFont, Hp.ToString() + "%", new Vector2(60, 18), Color.Red);
			_spriteBatch.DrawString(einFont, Mana.ToString() + "%", new Vector2(DisplaySetting.Display_Width - 70, 18), Color.Red);
			_spriteBatch.Draw(HpTexture, new Vector2(0, 0), Color.White);
			_spriteBatch.Draw(ManaTexture, new Vector2(DisplaySetting.Display_Width - 120, 0), Color.White);
		}
		public bool IsNewKeyPressed(Keys key, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState)
		{
			return currentKeyboardState.IsKeyDown(key) &&
					!previousKeyboardState.IsKeyDown(key);
		}
		internal char getDirction()
		{
			return Dirction;
		}
	}
}
