IMyCockpit cockpit;
IMyTextSurface panel;
IMyMotorStator sharnir1;
IMyMotorStator sharnir2;
IMyMotorStator sharnir3;
IMyMotorStator sharnir4;
string mess;


public Program() { 

	sharnir1 = GridTerminalSystem.GetBlockWithName("Стрела вся") as IMyMotorStator;
	sharnir2 = GridTerminalSystem.GetBlockWithName("Середина стрелы низ") as IMyMotorStator;
	sharnir3 = GridTerminalSystem.GetBlockWithName("Середина стрелы верх") as IMyMotorStator;
	sharnir4 = GridTerminalSystem.GetBlockWithName("Поворот бура") as IMyMotorStator;

	cockpit = GridTerminalSystem.GetBlockWithName("Кокпит бура") as IMyCockpit;
	panel = cockpit.GetSurface(0);
		panel.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
		panel.FontSize = 1.3f;
		Runtime.UpdateFrequency = UpdateFrequency.Update10;
	}

	public void Main(string arg) { 

	mess = "Стрела вся: "+ Math.Round(sharnir1.Angle*180/3.14) + "\r\n";
	mess += "Середина стрелы низ: "+ Math.Round(sharnir2.Angle*180/Math.Pi) + "\r\n";
	mess += "Середина стрелы верх: "+ Math.Round(sharnir3.Angle*180/Math.Pi) + "\r\n";
	mess += "Поворот бура: "+ Math.Round(sharnir4.Angle*180/Math.Pi) + "\r\n";



	panel.WriteText(mess);
}