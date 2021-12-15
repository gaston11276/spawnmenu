using System.Threading.Tasks;
using Gaston11276.Fivemui;

namespace Gaston11276.Spawnmenu.Client
{
	public class EntrySpawnLocation
	{
		public Textbox uiLabel = new Textbox();
		public Textbox uiName = new Textbox();
		public Textbox uiIndex = new Textbox();
		public Textbox btnDecrease = new Textbox();
		public Textbox btnIncrease = new Textbox();

		public delegate void fpSetInt(int index);
		public fpSetInt SetIndex;

		public delegate int fpGetInt();
		public fpGetInt GetIndex;
		public fpGetInt GetIndexMax;

		public delegate string fpGetString();
		public fpGetString GetName;

		public EntrySpawnLocation()
		{ }

		public async Task SetUi()
		{
			uiIndex.SetText($"{GetIndex()}");
			uiName.SetText($"{GetName()}");
			await WindowManager.Delay(WindowManager.delayMs);
		}

		public void Increase()
		{
			int index = GetIndex();
			index++;

			if (index > GetIndexMax() - 1)
			{
				index = GetIndexMax() - 1;
			}

			uiIndex.SetText($"{index}");
			SetIndex(index);

			string spawnName = GetName();
			uiName.SetText($"{spawnName}");
		}

		public void Decrease()
		{
			int index = GetIndex();
			index--;

			if (index < 01)
			{
				index = 0;
			}

			uiIndex.SetText($"{index}");
			SetIndex(index);

			string spawnName = GetName();
			uiName.SetText($"{spawnName}");
		}
	}
}
