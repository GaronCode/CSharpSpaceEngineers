
IMyCockpit cockpit;
IMyTextSurface panel;
List<IMyMotorSuspension> wheels;


string mess, COCKPIT_NAME = 'Главный кокпит';

int i = 0, SURFACE_ID = 0;

public Program() { 
	cockpit = GridTerminalSystem.GetBlockWithName(COCKPIT_NAME) as IMyCockpit;
	panel = cockpit.GetSurface(SURFACE_ID);
	
	wheels = new List<IMyMotorSuspension>();
	GridTerminalSystem.GetBlocksOfType<IMyMotorSuspension>(wheels);

	Runtime.UpdateFrequency = UpdateFrequency.Update10;
}

public void Main(string arg) { 
	i++;
	if (i >= 100) i = 0;	
	mess = cockpit.MoveIndicator+"\r\n";
	
	foreach (IMyMotorSuspension i in wheels) {
		if (i.CubeGrid.CustomName == cockpit.CubeGrid.CustomName) {   
			i.SteeringOverride = cockpit.MoveIndicator.X;
			mess+="+";
		}
	}
	

	panel.WriteText(i + "_" +mess);
	
}