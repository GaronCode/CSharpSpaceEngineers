
List<IMyTerminalBlock> cargo;
List<IMyTerminalBlock> connectors;
List<IMyTerminalBlock> drills;
List<IMyInventory> inventories;
List<IMyBatteryBlock> batteries;
IMyCockpit cockpit;
IMyTextSurface panel;

int timer = 0;
string mess;
float batteriesMax = 0f, batteriesCurrent = 0f;

string COCKPIT_NAME = "Кокпит СИГАРА";
int COCKPIT_SURFACE = 0;


public void Refresh() {
	cargo = new List<IMyTerminalBlock>();
	connectors = new List<IMyTerminalBlock>();
	drills = new List<IMyTerminalBlock>();
	inventories = new List<IMyInventory>();
	batteries = new List<IMyBatteryBlock>();
	
	GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(cargo);
	GridTerminalSystem.GetBlocksOfType<IMyShipConnector>(connectors);
	GridTerminalSystem.GetBlocksOfType<IMyShipDrill>(drills);
	GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(batteries);
	
	
	batteriesMax = 0f;
	batteriesCurrent = 0f;
	foreach (IMyBatteryBlock i in batteries) {
		if (i.CubeGrid.CustomName == cockpit.CubeGrid.CustomName) {
			batteriesMax += i.MaxStoredPower;
			batteriesCurrent += i.CurrentStoredPower;
		}
	}
	
	
	foreach (IMyTerminalBlock i in cargo) {
		if (i.CubeGrid.CustomName == cockpit.CubeGrid.CustomName)
			inventories.Add(i.GetInventory());
	}
		
	foreach (IMyTerminalBlock i in connectors) {
		if (i.CubeGrid.CustomName == cockpit.CubeGrid.CustomName)
			inventories.Add(i.GetInventory());
	}
		
	foreach (IMyTerminalBlock i in drills) {
		if (i.CubeGrid.CustomName == cockpit.CubeGrid.CustomName)
			inventories.Add(i.GetInventory());
	}
		
}
  


public Program() { /*вызывается один раз при компиляции*/
	cockpit = GridTerminalSystem.GetBlockWithName(COCKPIT_NAME) as IMyCockpit;
	panel = cockpit.GetSurface(COCKPIT_SURFACE);
	panel.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
	panel.FontSize = 1.3f;

	Refresh();

	Runtime.UpdateFrequency = UpdateFrequency.Update10; //каждый десятый тик
}

public void Main(string arg) { /*каждый раз с частотой UpdateFrequency*/
	if (timer == 10) {
		Refresh();
		timer = 0;
	}
	
	float now = 0f;
	float max = 0f;
	foreach (IMyInventory i in inventories) {
		now += (float)i.CurrentVolume;
		max += (float)i.MaxVolume;
	}
	double procent = Math.Round(now/max * 100);
	
	
	mess = "Грузовых блоков: "+Convert.ToString(inventories.Count)+"\r\n";
	mess += "Заполнено: "+Convert.ToString(Math.Ceiling(now))+" из "+Convert.ToString(Math.Ceiling(max))+"л.\r\n";

	mess += "\r\n";
	double equals = Math.Round(procent/5);
	for (int i = 1; i < equals; i++) {
		mess += "=";
	}
	double dashs = 20 - equals;
	for (int i = 1; i < dashs; i++) {
		mess += "–";
	}
	mess += "\r\n";

	mess += ""+Convert.ToString(procent)+"%\r\n";
	
	///////////////////////////////////////////////////////////////////////////
	
	procent = Math.Round(batteriesCurrent/batteriesMax * 100);
	
	
	mess += "\r\n\r\n";
	mess += "Заряд: "+Convert.ToString(Math.Round(batteriesCurrent, 3))+" из "+Convert.ToString(Math.Round(batteriesMax, 3))+"кВт*ч.\r\n";

	mess += "\r\n";
	equals = Math.Round(procent/5);
	for (int i = 1; i < equals; i++) {
		mess += "=";	
	}
	dashs = 20 - equals;
	for (int i = 1; i < dashs; i++) {
		mess += "–";
	}
	mess += "\r\n";

	mess += ""+Convert.ToString(procent)+"%\r\n";
	
	


	mess += Convert.ToString(batteriesMax);

	panel.WriteText(mess);

	timer++;
}