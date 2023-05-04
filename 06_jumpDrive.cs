IMyTextSurface panelForJumpA;
IMyJumpDrive jumpDrive;
string jumpDriveTextPanel;

float jumpPowerMax = 0f, jumpPowerCurrent = 0f;

    
public Program() {
	panelForJumpA = GridTerminalSystem.GetBlockWithName("КВ Дисплей Прыжок #2") as IMyTextSurface;

	jumpDrive = GridTerminalSystem.GetBlockWithName("КВ_Прыжковый") as IMyJumpDrive;
	

	panelForJumpA.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
	panelForJumpA.FontSize = 1.5f;
	


	jumpPowerMax = jumpDrive.MaxStoredPower;
	
	Runtime.UpdateFrequency = UpdateFrequency.Update10;
}    

public void Main(string arg) {
	
	jumpPowerCurrent = jumpDrive.CurrentStoredPower;
	double jumpPowerProcent = Math.Round( jumpPowerCurrent/jumpPowerMax * 100);	

	
	jumpDriveTextPanel = "Статус прыжкового двигателя\r\n";
	jumpDriveTextPanel += "Заряд\r\n";
	jumpDriveTextPanel += makeProcentString(jumpPowerProcent) + "\r\n";
	jumpDriveTextPanel += ""+jumpPowerProcent+"%\r\n";
	


	panelForJumpA.WriteText(jumpDriveTextPanel);

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