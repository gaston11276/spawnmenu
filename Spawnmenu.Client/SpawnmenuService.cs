using System.Threading.Tasks;
using JetBrains.Annotations;
using CitizenFX.Core.Native;
using NFive.SDK.Core.Models.Player;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Input;
using NFive.SDK.Client.Commands;
using NFive.SDK.Client.Communications;
using NFive.SDK.Client.Events;
using NFive.SDK.Client.Interface;
using NFive.SDK.Client.Services;
using Gaston11276.Fivemui;

namespace Gaston11276.Spawnmenu.Client
{
	[PublicAPI]
	public class SpawnmenuService : Service
	{
		WindowSpawnLocation windowSpawnLocation = new WindowSpawnLocation();
		bool firstSpawn = true;

		public SpawnmenuService(ILogger logger, ITickManager ticks, ICommunicationManager comms, ICommandManager commands, IOverlayManager overlay, User user) : base(logger, ticks, comms, commands, overlay, user) { }

		public override async Task Started()
		{
			WindowManager.AddWindow(windowSpawnLocation);
			windowSpawnLocation.CreateUi();
			windowSpawnLocation.SetUi();
			windowSpawnLocation.SetHotkey(InputControl.SelectCharacterMichael);
			windowSpawnLocation.RegisterOnSpawnCallback(OnSpawn);

			//windowSpawnLocation.Open();
			await Delay(0);
		}

		void OnSpawn()
		{
			if (firstSpawn)
			{
				API.ShutdownLoadingScreenNui();
				firstSpawn = false;
			}
		}
	}
}
