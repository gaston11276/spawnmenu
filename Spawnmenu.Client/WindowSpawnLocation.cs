using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Gaston11276.SimpleUi;
using Gaston11276.Fivemui;

namespace Gaston11276.Spawnmenu.Client
{
	public class WindowSpawnLocation : Window
	{
		PanelSpawnLocation panelSpawnLocation = new PanelSpawnLocation();
		List<fpVoid> onSpawnCallbacks = new List<fpVoid>();

		public WindowSpawnLocation()
		{
			defaultPadding = 0.0025f;
		}

		public void RegisterOnSpawnCallback(fpVoid OnRespawn)
		{
			onSpawnCallbacks.Add(OnRespawn);
		}

		protected override void OnOpen()
		{
			Game.Player.CanControlCharacter = false;
			//API.FreezePedCameraRotation(Game.PlayerPed.Handle);
			base.OnOpen();
		}

		protected override void OnClose()
		{
			Game.Player.CanControlCharacter = true;
			base.OnClose();
		}

		public async void SetUi()
		{
			await panelSpawnLocation.SetUi();
		}

		protected void CreateColumn(UiElementFiveM uiPanel, HGravity gravity, UiElementFiveM uiColumn, string label = null)
		{
			uiColumn.SetOrientation(Orientation.Vertical);
			uiColumn.SetPadding(new UiRectangle(defaultPadding));
			uiColumn.SetGravity(gravity);
			uiColumn.SetFlags(UiElement.TRANSPARENT);
			uiPanel.AddElement(uiColumn);

			if (label != null)
			{
				Textbox header = new Textbox();
				header.SetPadding(new UiRectangle(defaultPadding));
				header.SetText(label);
				header.SetFlags(UiElement.TRANSPARENT);
				if (label.Length == 0)
				{
					header.SetTextFlags(UiElement.TRANSPARENT);
				}
				uiColumn.AddElement(header);
			}
		}

		public override void CreateUi()
		{
			base.CreateUi();

			SetFlags(HIDDEN);
			SetPadding(new UiRectangle(defaultPadding));
			SetAlignment(HAlignment.Right);
			SetProperties(FLOATING | MOVABLE | COLLISION_PARENT);
			SetOrientation(Orientation.Vertical);			

			Textbox header = new Textbox();
			header.SetText("Spawn Location");
			header.SetFlags(TRANSPARENT);
			header.SetPadding(new UiRectangle(defaultPadding));
			AddElement(header);

			panelSpawnLocation.CreateUi();
			AddElement(panelSpawnLocation);

			UiElementFiveM panelButtons = new UiElementFiveM();
			panelButtons.SetHDimension(Dimension.Fill);
			panelButtons.SetPadding(new UiRectangle(0f));
			panelButtons.SetFlags(TRANSPARENT);
			panelButtons.SetMargin(new UiRectangle(0f, -defaultPadding, 0f, 0f));
			AddElement(panelButtons);

			UiElementFiveM panelButtonsLeft = new UiElementFiveM();
			panelButtonsLeft.SetGravity(HGravity.Left);
			panelButtonsLeft.SetPadding(new UiRectangle(0f));
			panelButtonsLeft.SetFlags(TRANSPARENT);
			panelButtonsLeft.SetHDimension(Dimension.Fill);
			panelButtons.AddElement(panelButtonsLeft);

			UiElementFiveM panelButtonsRight = new UiElementFiveM();
			panelButtonsRight.SetPadding(new UiRectangle(0f));
			panelButtonsRight.SetFlags(TRANSPARENT);
			panelButtonsRight.SetGravity(HGravity.Right);
			panelButtonsRight.SetHDimension(Dimension.Fill);
			panelButtons.AddElement(panelButtonsRight);

			Textbox uiButtonClose= new Textbox();
			uiButtonClose.SetText("Close");
			uiButtonClose.SetPadding(new UiRectangle(defaultPadding));
			uiButtonClose.SetProperties(UiElement.CANFOCUS);
			uiButtonClose.RegisterOnLMBRelease(Close);
			WindowManager.RegisterOnMouseButtonCallback(uiButtonClose.OnMouseButton);
			WindowManager.RegisterOnMouseMoveCallback(uiButtonClose.OnMouseMove);
			panelButtonsLeft.AddElement(uiButtonClose);

			Textbox uiButtonSpawn = new Textbox();
			uiButtonSpawn.SetText("Spawn");
			uiButtonSpawn.SetPadding(new UiRectangle(defaultPadding));
			uiButtonSpawn.SetProperties(UiElement.CANFOCUS);
			uiButtonSpawn.RegisterOnLMBRelease(Respawn);
			WindowManager.RegisterOnMouseButtonCallback(uiButtonSpawn.OnMouseButton);
			WindowManager.RegisterOnMouseMoveCallback(uiButtonSpawn.OnMouseMove);
			panelButtonsRight.AddElement(uiButtonSpawn);

			Refresh();
		}

		private async void Respawn()
		{
			API.DoScreenFadeOut(500);
			while (API.IsScreenFadingOut()) await WindowManager.Delay(10);

			Game.Player.Character.IsPositionFrozen = true;
			Vector3 pos = panelSpawnLocation.GetSpawnLocation();

			foreach (fpVoid OnRespawn in onSpawnCallbacks)
			{
				OnRespawn();
			}

			Game.Player.Character.Health = 200;

			API.RequestCollisionAtCoord(pos.X, pos.Y, pos.Z);
			API.SetEntityCoordsNoOffset(Game.PlayerPed.Handle, pos.X, pos.Y, pos.Z, false, false, false);
			while (!API.HasCollisionLoadedAroundEntity(Game.PlayerPed.Handle)) await WindowManager.Delay(100);
			
			Game.Player.Character.IsPositionFrozen = false;

			API.DoScreenFadeIn(500);
			while (API.IsScreenFadingIn()) await WindowManager.Delay(10);

			UiCamera.SetMode(CameraMode.Game);
		}
	}
}
