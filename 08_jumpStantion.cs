IMyCockpit cockpit;
string textHercules, textJumper, textStantion, text;
IMyTextSurface jumpDriveTextPanel;
IMyTextSurface panel, panel2, panel3;

int herculesJumpCount = 0, jumperJumpCount = 0,  stantionJumpCount = 0;

float herculesPowerMax = 0f, herculesPowerCurrent = 0f;
float jumperPowerMax = 0f, jumperPowerCurrent = 0f;
float stantionPowerMax = 0f, stantionPowerCurrent = 0f;
List<IMyJumpDrive> jumps;
    
public Program() {
	cockpit = GridTerminalSystem.GetBlockWithName("TSS - кокпит") as IMyCockpit;
	jumpDriveTextPanel = cockpit.GetSurface(0);


	panel = GridTerminalSystem.GetBlockWithName("ПрозрачнаяПанель") as IMyTextSurface;
	panel2 = GridTerminalSystem.GetBlockWithName("НижняяПанель") as IMyTextSurface;
	panel3 = GridTerminalSystem.GetBlockWithName("ВерхняяПанель") as IMyTextSurface;
	panel.ContentType = panel2.ContentType = panel3.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;


	
	jumps = new List<IMyJumpDrive>();
	
	GridTerminalSystem.GetBlocksOfType<IMyJumpDrive>(jumps);
	
	
	
	
	jumpDriveTextPanel.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
	jumpDriveTextPanel.FontSize = 1.2f;



	
	Runtime.UpdateFrequency = UpdateFrequency.Update10;
}    

public void Main(string arg) {
 textHercules = textJumper = textStantion = "";
 herculesPowerMax = jumperPowerMax = stantionPowerMax = 0f;
 herculesPowerCurrent = jumperPowerCurrent = stantionPowerCurrent = 0f;
 herculesJumpCount = jumperJumpCount = stantionJumpCount = 0;
	foreach (IMyJumpDrive i in jumps) {
		if (i.CubeGrid.CustomName == "[VEX] Hercules") {
			 herculesPowerMax += i.MaxStoredPower;
			 herculesPowerCurrent += i.CurrentStoredPower;
			 herculesJumpCount++;
		}
		else if (i.CubeGrid.CustomName == "[VEX]Jumper") {
			 jumperPowerMax += i.MaxStoredPower;
			 jumperPowerCurrent += i.CurrentStoredPower;
			 jumperJumpCount++;
		}
		else {
			 stantionPowerMax += i.MaxStoredPower;
			 stantionPowerCurrent += i.CurrentStoredPower;
			 stantionJumpCount++;
		}

	}
 
	double tempProcent = calcProcent(herculesPowerCurrent,herculesPowerMax);	
	textHercules += "Заряд Геркулеса\r\n";
	textHercules += makeProcentString(tempProcent)+"\r\n";
	textHercules += tempProcent + "%\r\n";
	
	tempProcent = calcProcent(jumperPowerCurrent,jumperPowerMax);	
	textJumper += "Заряд Попрыгунчика\r\n";
	textJumper += makeProcentString(tempProcent)+"\r\n";
	textJumper += tempProcent + "%\r\n";
 
	tempProcent = calcProcent(stantionPowerCurrent,stantionPowerMax);	
	textStantion += "Заряд Станции\r\n";
	textStantion += makeProcentString(tempProcent)+"\r\n";
	textStantion += tempProcent + "%\r\n";
	
	text = textHercules + textJumper + textStantion;
jumpDriveTextPanel.WriteText(text);
panel.WriteText(text);
panel2.WriteText(text);
panel3.WriteText(text);
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

public double calcProcent(float now,float max) {
	return Math.Round( now/max * 100);	
}