//создаётся список с типом данных IMyTerminalBlock
List<IMyTerminalBlock> lamps = new List<IMyTerminalBlock>();
// создаётся переменная с типом данных IMyCockpit
IMyCockpit cockpit;

// поиск в списке по имени 
GridTerminalSystem.SearchBlocksOаName("содержит_этот_текст", lamps);

// перебор списка
foreach (IMyTerminalBlock i in lamps) {
	i.SetValueColor("Color", Color.Orange);
}


panel.FontSize = 1.3f;
panel.FontColor = Color.Orange;
