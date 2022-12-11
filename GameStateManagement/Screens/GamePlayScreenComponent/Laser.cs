using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tutorial009.Sprites;

namespace GameStateManagement.Screens.GamePlayScreenComponent
{
	public class Laser
	{
		private ContentManager content;

		private Enemy enemy;

		private Player player;
		private Rectangle VectorSizeWithPosition;
		// Sprites
		private Texture2D LaserTexture;

		// Laser Variablen
		private List<Richtung> laserShots = new List<Richtung>();

		private float laserSpeed = 10f;

		// Sound Effekte
		private SoundEffect laserSound;
		private SoundEffect explosionSound;

		public Laser(ContentManager content, Enemy enemy, Player player)
		{
			this.content = content;
			this.enemy = enemy;
			this.player = player;
		}

		public void LoadContent()
		{

			// Texturen laden
			LaserTexture = content.Load<Texture2D>("bullet");


			// TODO
			// Sounds laden
			explosionSound = content.Load<SoundEffect>("explosion");
			laserSound = content.Load<SoundEffect>("laserfire");

		}





		public void FireLaser()
		{
			float X = player.Position.X + 35;
			float Y = player.Position.Y + 35;
			// Position in der Liste speichern
			laserShots.Add(new Richtung(new Vector2(X, Y), player.getDirction()));
			PlayLaserSound();
		}

		public void UpdateLaserShots()
		{
			int laserIndex = 0;

			while (laserIndex < laserShots.Count)
			{
				// hat der Schuss den Bildschirm verlassen?
				if (laserShots[laserIndex].getVector2().Y < 0)
				{
					laserShots.RemoveAt(laserIndex);
				}
				else
				{
					// Position des Schusses aktualiesieren
					Vector2 pos = laserShots[laserIndex].getVector2();
					switch (laserShots[laserIndex].getDirction())
					{
						case 'u':
							pos.Y -= laserSpeed;

							break;
						case 'd':
							pos.Y += laserSpeed;
							break;
						case 'l':
							pos.X -= laserSpeed;
							break;
						case 'r':
							pos.X += laserSpeed;
							break;
						default:
							pos.X += laserSpeed;
							break;
					}
					laserShots[laserIndex].setVector2(pos);

					// Überprüfen ob ein Treffer vorliegt
					int enemyIndex = 0;

					while (enemyIndex < enemy.enemyPositions.Count)
					{
						// Abstand zwischen Feind-Position und Schuss-Position ermitteln
						float distance = Vector2.Distance(enemy.enemyPositions[enemyIndex], laserShots[laserIndex].getVector2());

						// Treffer?

						if (distance < enemy.getEnemyRadius())
						{

							// Schuss entfernen
							laserShots.RemoveAt(laserIndex);
							// Feind entfernen
							if (enemy.enemies[enemyIndex].getHp()<=0) {
								enemy.enemyPositions.RemoveAt(enemyIndex);
							}
							else
							{
								enemy.enemies[enemyIndex].setHp(player.gatDamage());
							}
							// Punkte erhöhen
							player.playerScore++;

							PlayExplosionSound();

							// Schleife verlassen
							break;
						}
						else
						{
							enemyIndex++;
						}
					}
					laserIndex++;
				}
			}
		}


		public void PlayExplosionSound()
		{
			// TODO
			// Explosions WAV abspielen
			explosionSound.Play();
		}

		public void PlayLaserSound()
		{
			// TODO
			// Laserschuss WAV abspielen
			laserSound.Play();
		}

		public void DrawLaser(SpriteBatch _spriteBatch)
		{
			// TODO
			// Die Liste mit den Laser-Schüssen (laserShots) durchlaufen
			// und alle Schüsse (LaserTexture) zeichnen
			foreach (Richtung v in laserShots)
			{
				VectorSizeWithPosition = new Rectangle((int)v.getVector2().X, (int)v.getVector2().Y, 20, 20);
				_spriteBatch.Draw(LaserTexture, VectorSizeWithPosition, Color.White);
			}

		}
	}
}
