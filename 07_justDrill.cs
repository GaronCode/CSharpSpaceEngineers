IMyCockpit cockpit;
IMyTextSurface panel, panel2;
IMyInventory drillLeft, drillRight;



    
public Program() {
	cockpit = GridTerminalSystem.GetBlockWithName("АД_Кокпит_бура") as IMyCockpit;
	drillLeft = (GridTerminalSystem.GetBlockWithName("АД_Бур_левый") as IMyTerminalBlock).GetInventory();
	drillRight = (GridTerminalSystem.GetBlockWithName("АД_Бур_правый") as IMyTerminalBlock).GetInventory();




	panel = cockpit.GetSurface(1);
	panel2 = cockpit.GetSurface(2);
	
	
	panel.ContentType = panel2.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
	panel.FontSize = panel2.FontSize = 5.5f;



	Runtime.UpdateFrequency = UpdateFrequency.Update100;
}    

public void Main(string arg) {
 

panel.WriteText(Math.Ceiling((float)drillLeft.CurrentVolume*10) +  " из 33");

panel2.WriteText(Math.Ceiling((float)drillRight.CurrentVolume*10) +  " из 33");


}