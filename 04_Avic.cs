List<IMyPowerProducer> hydrogen;
List<IMyBatteryBlock> batteries;
List<IMySolarPanel> solarPanels;
List<IMyTerminalBlock> cargo;
List<IMyInventory> inventories;
IMyCockpit cockpit;
IMyTextSurface panel1floor, panelPower, panelCargo;
IMyInteriorLight lamp, lamp2;
int timer = 0;

string mess, messPower, messCargo;

float batteriesMax = 0f, batteriesCurrent = 0f, batteriesCurrentPrew = 0f;
float solarMax = 0f, solarCurrent = 0f, solarCurrentPrew = 0f;
float hydrogenMax = 0f, hydrogenCurrent = 0f, hydrogenCurrentPrew = 0f;
int solarCount = 0, batteriesCount = 0, hydrogenCount = 0;





public Program() { /*вызывается один раз при компиляции*/
	cockpit = GridTerminalSystem.GetBlockWithName("Главный кокпит") as IMyCockpit; 
	panel1floor = GridTerminalSystem.GetBlockWithName("Дисплей 1x1") as IMyTextSurface;
	panelPower = GridTerminalSystem.GetBlockWithName("ЖК-панель статус батареи") as IMyTextSurface;
	panelCargo = GridTerminalSystem.GetBlockWithName("ЖК-панель статус заполнение") as IMyTextSurface;

	lamp = GridTerminalSystem.GetBlockWithName("лампа кокпита 1") as IMyInteriorLight;
	lamp2 = GridTerminalSystem.GetBlockWithName("лампа кокпита 2") as IMyInteriorLight;
	lamp.Color  = new Color(255, 255, 255);
	lamp2.Color  = new Color(255, 255, 255);

	panel1floor.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
	panel1floor.FontSize = 1.36f;

	panelPower.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
	panelPower.FontSize = 1.36f;

	panelCargo.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
	panelCargo.FontSize = 1.36f;

	Refresh();
	Runtime.UpdateFrequency = UpdateFrequency.Update100; 
}
public void Refresh() {
	cargo = new List<IMyTerminalBlock>();
	batteries = new List<IMyBatteryBlock>();
	solarPanels = new List<IMySolarPanel>();
	hydrogen = new List<IMyPowerProducer>();
	inventories = new List<IMyInventory>();
	
	GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(cargo);
	GridTerminalSystem.GetBlocksOfType(hydrogen, t => t.BlockDefinition.SubtypeId.Contains("HydrogenEngine"));
	GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(batteries);
	GridTerminalSystem.GetBlocksOfType<IMySolarPanel>(solarPanels);
	
	foreach (IMyTerminalBlock i in cargo) {
		if (i.CubeGrid.CustomName == cockpit.CubeGrid.CustomName)
			inventories.Add(i.GetInventory());
	}
	
	
	
	batteriesCurrentPrew = batteriesCurrent;
	batteriesMax = batteriesCurrent = 0f;
	batteriesCount = 0;
	foreach (IMyBatteryBlock i in batteries) {
		if (i.CubeGrid.CustomName == cockpit.CubeGrid.CustomName) {
			batteriesCount++;
			batteriesMax += i.MaxStoredPower;
			batteriesCurrent += i.CurrentStoredPower;
		}
	}
	solarCurrentPrew = solarCurrent;
	solarMax = solarCurrent = 0f;
	solarCount = 0;
	foreach (IMySolarPanel i in solarPanels) {
		if (i.CubeGrid.CustomName == cockpit.CubeGrid.CustomName) {   
			solarCount++;
			solarMax += i.MaxOutput;
			solarCurrent += i.CurrentOutput;
		}
	}
	
	
	hydrogenCurrentPrew = hydrogenCurrent;
	hydrogenMax = hydrogenCurrent = 0f;
	hydrogenCount = 0;
	foreach (IMyPowerProducer i in hydrogen) {
		if (i.CubeGrid.CustomName == cockpit.CubeGrid.CustomName) {   
			hydrogenCount++;
			hydrogenMax += i.MaxOutput;
			hydrogenCurrent += i.CurrentOutput;
		}
	}
	
}

public void Main(string arg) { /*каждый раз с частотой UpdateFrequency*/
	if (timer == 10) {
		Refresh();
		timer = 0;
	}

	
	///////////////////////////////////////////////////////////////////////////
	double procent = Math.Round(batteriesCurrent/batteriesMax * 100);
	


	mess = "АКБ ("+batteriesCount+"): "+Math.Round(batteriesCurrent, 3).ToString("0.000")+" из "+Math.Round(batteriesMax, 3).ToString("0.000")+" МВт*ч.\r\n";
	messPower = "СИЛОВАЯ ЦЕПЬ\r\n";
	messCargo = "ГРУЗ\r\n";

	string powerProcent = "";
	double equals = Math.Round(procent/5);
	for (int i = 1; i < equals; i++) {
		powerProcent += "=";	
	}
	double  dashs = 20 - equals;
	for (int i = 1; i < dashs; i++) {
		powerProcent += "–";
	}
	messPower += powerProcent;

	double powerChange = Math.Round((batteriesCurrent-batteriesCurrentPrew)*1000, 3);
	powerProcent += "("+powerChange.ToString("0.000")+")\r\n";
	
	mess += powerProcent+"                ";


	powerProcent=Convert.ToString(procent)+"%\r\n";

	mess += powerProcent;
	messPower += "\r\n"+powerProcent;
	

	if (procent > 50) {lamp.Color = lamp2.Color = new Color(220, 255, 220);}
	else if (procent > 25) {lamp.Color = lamp2.Color = new Color(255, 135, 72);}
	else if (procent > 10) {
		lamp.Color = lamp2.Color = new Color(160, 170, 115);
		messPower += "Низкий уровень заряда!%\r\n"; 
		panelPower.BackgroundColor = new Color(160, 0, 0);
	}
	else {
		lamp.Color = lamp2.Color = new Color(255, 55, 55); 
		messPower += "КРИТИЧЕСКИЙ УРОВЕНЬ ЗАРЯДА!!!\r\n";
		panelPower.BackgroundColor = new Color(230, 0, 0);
	}


	if (hydrogenCurrent > 1) messPower += "Генераторы запущены\r\n";
	else messPower += "Генераторы отключены\r\n";
		
	if (powerChange > 0) messPower += "Батареи заряжаются\r\n";
	else messPower += "Батареи разряжаются\r\n";


	mess += "    Производство энергии\r\n";
	mess += "Солнечные батареи      "+solarCount.ToString("000")+" шт\r\n "+Math.Round(solarCurrent, 2).ToString("0.000")+ " МВт\r\n"; 
	mess += "Водородгены                "+hydrogenCount.ToString("000")+" шт\r\n "+Math.Round(hydrogenCurrent, 2).ToString("0.000")+ " МВт\r\n"; 
	
	
	
	
	
	float nowCap = 0f;
	float maxCap = 0f;
	float now = 0, max = 0;
	foreach (IMyInventory i in inventories) {
		now += (float)i.CurrentVolume;
		max += (float)i.MaxVolume;
	}
	double procentCap = Math.Round(nowCap/maxCap * 100);	
	
	messCargo += makeProcentString(procentCap) + "-\r\n";
	

	
	
	panel1floor.WriteText(mess);
	panelPower.WriteText(messPower);
	panelCargo.WriteText(messCargo);
	timer++;
}


public string makeProcentString(double procent) {
	string s = "";
	double equals = Math.Round(procent/5);
	for (int i = 1; i < equals; i++) {
		s += "=";	
	}
	double  dashs = 20 - equals;
	for (int i = 1; i < dashs; i++) {
		s += "–";
	}
	return s;
}
    