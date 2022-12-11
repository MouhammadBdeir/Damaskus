#region File Description

//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion File Description

#region Using Statements

using GameStateManagement.Screens.GamePlayScreenComponent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Threading;
using Tutorial009.Models;
using Tutorial009.Sprites;

#endregion Using Statements

namespace GameStateManagement
{
	/// <summary>
	/// This screen implements the actual game logic. It is just a
	/// placeholder to get the idea across: you'll probably want to
	/// put some more interesting gameplay in here!
	/// </summary>
	internal class GameplayScreen : GameScreen
	{
		#region Fields
		private ContentManager content;
		private SpriteFont gameFont;
		private List<Sprite> _sprites;
		//player object
		private Player player;
		//Laser object
		private Laser laser;
		// Enemy object
		private Enemy enemy;
		//StarField Object
		private StarField starField;


		private float pauseAlpha;

		#endregion Fields

		#region Initialization


		// Tastatur abfragen
		private KeyboardState currentKeyboardState;
		private KeyboardState previousKeyboardState;

		/// <summary>
		/// Constructor.
		/// </summary>
		public GameplayScreen()
		{
			TransitionOnTime = TimeSpan.FromSeconds(1.5);
			TransitionOffTime = TimeSpan.FromSeconds(0.5);

		}

		/// <summary>
		/// Load graphics content for the game.
		/// </summary>
		public override void LoadContent()
		{
			if (content == null)
				content = new ContentManager(ScreenManager.Game.Services, "Content");

			gameFont = content.Load<SpriteFont>("gamefont");

			// A real game would probably have more content than this sample, so
			// it would take longer to load. We simulate that by delaying for a
			// while, giving you a chance to admire the beautiful loading screen.
			Thread.Sleep(1000);
			enemy = new Enemy(content);
			enemy.LoadContent();
			var playerTexture = content.Load<Texture2D>("hero");
			player = new Player(playerTexture, enemy)
			{
				Input = new Input()
				{
					Left = Keys.A,
					Right = Keys.D,
					Up = Keys.W,
					Down = Keys.S,
				},
				Position = new Vector2(81, 81),
				Colour = Color.White,
				Speed = 3,
			};
			laser = new Laser(content, enemy, player);
			laser.LoadContent();
			createMap();
			player.loadContent(content);
			// once the load has finished, we use ResetElapsedTime to tell the game's
			// timing mechanism that we have just finished a very long frame, and that
			// it should not try to catch up.
			ScreenManager.Game.ResetElapsedTime();
			starField = new StarField(content);
			starField.LoadContent();          
		}

		/// <summary>
		/// Unload graphics content used by the game.
		/// </summary>
		public override void UnloadContent()
		{
			content.Unload();
		}
		public void createMap()
		{
			var stoneTexture = content.Load<Texture2D>("stone");

			_sprites = new List<Sprite>();
			_sprites.Add(player);

			for (int i = 0; i < 18; i++)
			{
				StoneOpject up = new StoneOpject(stoneTexture)
				{
					Position = new Vector2(81 * i, 0),
					Colour = Color.White,
					Speed = 5,
				};
				_sprites.Add(up);
				StoneOpject left = new StoneOpject(stoneTexture)
				{
					Position = new Vector2(0, 81 * i),
					Colour = Color.White,
					Speed = 5,
				};
				_sprites.Add(left);

				StoneOpject right = new StoneOpject(stoneTexture)
				{
					Position = new Vector2(81 * 17, 81 * i),
					Colour = Color.White,
					Speed = 5,
				};
				_sprites.Add(right);
				StoneOpject down = new StoneOpject(stoneTexture)
				{
					Position = new Vector2(81 * i, 81 * 13),
					Colour = Color.White,
					Speed = 5,
				};
				_sprites.Add(down);
			}
			StoneOpject shit = new StoneOpject(stoneTexture)
			{
				Position = new Vector2(81 * 6, 81 * 6),
				Colour = Color.White,
				Speed = 5,
			};
			_sprites.Add(shit);

		}
		#endregion Initialization


		//#region Input Operationen

		public bool IsNewKeyPressed(Keys key)
		{
			return currentKeyboardState.IsKeyDown(key) &&
					!previousKeyboardState.IsKeyDown(key);
		}
		#region Update and Draw

		/// <summary>
		/// Updates the state of the game. This method checks the GameScreen.IsActive
		/// property, so the game will stop updating when the pause menu is active,
		/// or if you tab away to a different application.
		/// </summary>
		public override void Update(GameTime gameTime, bool otherScreenHasFocus,
													   bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, false);

			// Gradually fade in or out depending on whether we are covered by the pause screen.
			if (coveredByOtherScreen)
			{
				pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
			}
			else
			{
				pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
			}

			if (IsActive)
			{
				currentKeyboardState = Keyboard.GetState();
				foreach (var sprite in _sprites)
					sprite.Update(gameTime, _sprites);
				player.Move(laser, currentKeyboardState, previousKeyboardState);
				enemy.UpdateEnemies();
				laser.UpdateLaserShots();
				previousKeyboardState = currentKeyboardState;

			}
		}

		/// <summary>
		/// Lets the game respond to player input. Unlike the Update method,
		/// this will only be called when the gameplay screen is active.
		/// </summary>
		public override void HandleInput(InputState input)
		{
			if (input == null)
			{
				throw new ArgumentNullException(nameof(input));
			}

			// Look up inputs for the active player profile.
			int playerIndex = (int)ControllingPlayer.Value;

			KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
			GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

			// The game pauses either if the user presses the pause button, or if
			// they unplug the active gamepad. This requires us to keep track of
			// whether a gamepad was ever plugged in, because we don't want to pause
			// on PC if they are playing with a keyboard and have no gamepad at all!
			bool gamePadDisconnected = !gamePadState.IsConnected &&
									   input.GamePadWasConnected[playerIndex];

			if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
			{
				ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
			}
			else
			{
				// Otherwise move the player position.
				Vector2 movement = Vector2.Zero;

				if (keyboardState.IsKeyDown(Keys.Left))
				{
					movement.X--;
				}

				if (keyboardState.IsKeyDown(Keys.Right))
				{
					movement.X++;
				}

				if (keyboardState.IsKeyDown(Keys.Up))
				{
					movement.Y--;
				}

				if (keyboardState.IsKeyDown(Keys.Down))
				{
					movement.Y++;
				}

				Vector2 thumbstick = gamePadState.ThumbSticks.Left;

				movement.X += thumbstick.X;
				movement.Y -= thumbstick.Y;

				if (movement.Length() > 1)
				{
					movement.Normalize();
				}
			}
		}

		/// <summary>
		/// Draws the gameplay screen.
		/// </summary>
		public override void Draw(GameTime gameTime)
		{
			// This game has a blue background. Why? Because!
			ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
											   Color.CornflowerBlue, 0, 0);

			// Our player and enemy are both actually just text strings.
			SpriteBatch _spriteBatch = ScreenManager.SpriteBatch;

			_spriteBatch.Begin();

			// Hintergrund zeichnen
			starField.DrawBackground(_spriteBatch);
			foreach (var sprite in _sprites)
				sprite.Draw(_spriteBatch);
			enemy.DrawEnemy(_spriteBatch);
			laser.DrawLaser(_spriteBatch);
			player.Draw(_spriteBatch);
			_spriteBatch.End();

			// If the game is transitioning on or off, fade it out to black.
			if (TransitionPosition > 0 || pauseAlpha > 0)
			{
				float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

				ScreenManager.FadeBackBufferToBlack(alpha);
			}
		}
		#endregion Update and Draw
	}
}