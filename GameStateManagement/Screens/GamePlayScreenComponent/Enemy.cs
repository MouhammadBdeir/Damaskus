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
	public class Enemy
	{
		private ContentManager content;
		private int Hp;
		//Enemy position
		private Vector2 enemyPosition = new Vector2(100, 100);
		private Rectangle VectorSizeWithPosition;
		// Sprites
		private Texture2D EnemyTexture;

		// Gegner Variablen
		public readonly List<Vector2> enemyPositions = new List<Vector2>();
		public readonly List<Enemy> enemies = new List<Enemy>();
		private Vector2 enemyStartPosition = new Vector2(100, 100);
		private float enemyRadius;
		private float enemySpeed = 1f;
		private Color enemyColor;
		// Zufallszahlen
		private Random random = new Random();

		public Enemy(ContentManager content)
		{
			this.content = content;
			Hp = 100;
		}
		public Rectangle getVectorSizeWithPosition()
		{
			return VectorSizeWithPosition;
		}
		public float getEnemyRadius()
		{
			return enemyRadius;
		}
		public void LoadContent()
		{
			// Texturen laden
			EnemyTexture = content.Load<Texture2D>("Zombie");

			// Radius der Feinde festlegen
			if (EnemyTexture != null)
			{
				enemyRadius = 60;

				// Gegner erzeugen
				CreateEnemies();
			}
		}

		public void CreateEnemies()
		{
			// Feinde erzeugen
			Vector2 position = enemyStartPosition;
			position.X -= EnemyTexture.Width / 2;

			// Eine Zufallszahl zwischen 3 und 10 ermitteln
			int count = random.Next(3, 11);

			// Gegener erzeugen
			enemyPositions.Add(new Vector2(81 * 4, 81 * 6));
			enemies.Add(new Enemy(content));
			enemyPositions.Add(new Vector2(81 * 3, 81 * 6));
			enemies.Add(new Enemy(content));
			enemyPositions.Add(new Vector2(81 * 4, 81 * 7));
			enemies.Add(new Enemy(content));
			//	enemyPositions.Add(new Vector2(81 * 2, 81 * 5));
			//	enemyPositions.Add(new Vector2(81 * 5, 81 * 5));
			enemyPositions.Add(new Vector2(81 * 6, 81 * 4));
			enemies.Add(new Enemy(content));
			for (int i = 0; i < count; i++)
			{
				enemyPositions.Add(position);
				enemies.Add(new Enemy(content));
				position.X += 81;
			}

			// Farbwert ändern
			switch (count)
			{
				case 3:
					enemyColor = Color.Red;
					break;
				case 4:
					enemyColor = Color.Green;
					break;
				case 5:
					enemyColor = Color.Yellow;
					break;
				case 6:
					enemyColor = Color.Blue;
					break;
				case 7:
					enemyColor = Color.Magenta;
					break;
				case 8:
					enemyColor = Color.Yellow;
					break;
				case 9:
					enemyColor = Color.White;
					break;
				case 10:
					enemyColor = Color.DarkGreen;
					break;
				default:
					break;
			}
		}

		public void UpdateEnemies()
		{
			// Startposition verändern
			enemyStartPosition.X += enemySpeed;

			// Bewegungsrichtung umkehren
			if (enemyStartPosition.X > 250)
			{
				enemySpeed *= -1;
			}
			else if (enemyStartPosition.X < 100f)
			{
				enemySpeed *= -1;
			}

			// Alle Feinde abgeschossen? Dann Neue Gegener
			if (enemyPositions.Count == 0 && EnemyTexture != null)
			{
				CreateEnemies();
			}

			// Aktualisieren
			for (int i = 0; i < enemyPositions.Count; i++)
			{
				Vector2 position = enemyPositions[i];
				position.X += enemySpeed;
				position.Y += enemySpeed;
				enemyPositions[i] = position;
			}
		}

		public void DrawEnemy(SpriteBatch _spriteBatch)
		{
			// TODO
			// Die Liste mit allen Gegnern (enemyPositions) durchlaufen
			// und alle Feinde (EnemyTexture) zeichnen
			foreach (Vector2 v in enemyPositions)
			{
				VectorSizeWithPosition = new Rectangle((int)v.X, (int)v.Y, 80, 80);
				_spriteBatch.Draw(EnemyTexture, VectorSizeWithPosition, enemyColor);
			}
		}
		public int getHp()
		{
			return Hp;
		}
		public void setHp(int damage)
		{
			Hp -= damage;
		}
	}
}
