List<IMyPowerProducer> winds;
List<IMyPowerProducer> hydrogen;
List<IMyBatteryBlock> batteries;
List<IMySolarPanel> solarPanels;
IMyCockpit cockpit;
IMyTextSurface panel1floor;
IMyTextSurface panelTopLeft;
IMyTextSurface panelTopCenter;
IMyTextSurface panelTopRight;
IMyTextSurface panelBottom;

int timer = 0;

string mess;

float batteriesMax = 0f, batteriesCurrent = 0f, batteriesCurrentPrew = 0f;
float solarMax = 0f, solarCurrent = 0f, solarCurrentPrew = 0f;
float windMax = 0f, windCurrent = 0f, windCurrentPrew = 0f;
float hydrogenMax = 0f, hydrogenCurrent = 0f, hydrogenCurrentPrew = 0f;
int solarCount = 0, batteriesCount = 0, windCount = 0, hydrogenCount = 0;





public Program() { /*вызывается один раз при компиляции*/
	cockpit = GridTerminalSystem.GetBlockWithName("Main") as IMyCockpit; 
	panel1floor = GridTerminalSystem.GetBlockWithName("Нижний этаж панель") as IMyTextSurface;
	panelTopLeft = GridTerminalSystem.GetBlockWithName("Экран лево") as IMyTextSurface;
	panelTopCenter = GridTerminalSystem.GetBlockWithName("Экран центр") as IMyTextSurface;
	panelTopRight = GridTerminalSystem.GetBlockWithName("Экран право") as IMyTextSurface;
	panelBottom = GridTerminalSystem.GetBlockWithName("Экран низ") as IMyTextSurface;


	panel1floor.ContentType = panelTopLeft.ContentType = panelTopCenter.ContentType = panelTopRight.ContentType = panelBottom.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
	panel1floor.FontSize = 1.3f;


	Refresh();
	Runtime.UpdateFrequency = UpdateFrequency.Update10; 
}
public void Refresh() {
	
	batteries = new List<IMyBatteryBlock>();
	solarPanels = new List<IMySolarPanel>();
	winds = new List<IMyPowerProducer>();
	hydrogen = new List<IMyPowerProducer>();
	
	GridTerminalSystem.GetBlocksOfType(winds, t => t.BlockDefinition.SubtypeId.Contains("WindTurbine"));
	GridTerminalSystem.GetBlocksOfType(hydrogen, t => t.BlockDefinition.SubtypeId.Contains("HydrogenEngine"));
	GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(batteries);
	GridTerminalSystem.GetBlocksOfType<IMySolarPanel>(solarPanels);
	
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
	
	windCurrentPrew = windCurrent;
	windMax = windCurrent = 0f;
	windCount = 0;
	foreach (IMyPowerProducer i in winds) {
		if (i.CubeGrid.CustomName == cockpit.CubeGrid.CustomName) {   
			windCount++;
			windMax += i.MaxOutput;
			windCurrent += i.CurrentOutput;
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
	
	mess = "АКБ: "+Convert.ToString(Math.Round(batteriesCurrent, 3))+" из "+Convert.ToString(Math.Round(batteriesMax, 3))+" кВт*ч.\r\n";

	double equals = Math.Round(procent/5);
	for (int i = 1; i < equals; i++) {
		mess += "=";	
	}
	double  dashs = 20 - equals;
	for (int i = 1; i < dashs; i++) {
		mess += "–";
	}
	mess += "\r\n";

	mess += ""+Convert.ToString(procent)+"%";
	
	


	panel1floor.WriteText(mess);
	
	panelTopLeft.WriteText("Аккумуляторных станций: "+batteriesCount+"\r\nЗаряд: "+Math.Round(batteriesCurrent, 3).ToString("0.000")+" из "+Math.Round(batteriesMax, 3).ToString("0.000")+" МВт*ч.\r\nИзменение: "+Math.Round((batteriesCurrent-batteriesCurrentPrew)*1000, 3).ToString("0.000")+" кВт");
	
	mess = "               Производство энергии\r\n";
	mess += "Солнечные батареи "+solarCount.ToString("000")+" шт: "+Math.Round(solarCurrent, 2).ToString("0.000")+ " МВт\r\n"; 
	mess += "Ветряки                  "+windCount.ToString("000")+" шт: "+Math.Round(windCurrent, 2).ToString("0.000")+ " МВт\r\n"; 
	mess += "Водородгены          "+hydrogenCount.ToString("000")+" шт: "+Math.Round(hydrogenCurrent, 2).ToString("0.000")+ " МВт\r\n"; 
	
	
	panelTopCenter.WriteText(mess);
	
	
	

	timer++;
}