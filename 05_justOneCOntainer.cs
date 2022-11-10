IMyCockpit cockpit;
IMyTextSurface panel;
IMyTerminalBlock sklad;
IMyInventory inventory;
string mess;

public Program()
{
	sklad = GridTerminalSystem.GetBlockWithName("И01 Большой контейнер") as IMyTerminalBlock;
	inventory = sklad.GetInventory();

	cockpit = GridTerminalSystem.GetBlockWithName("И01 Главный кокпит") as IMyCockpit;
	panel = cockpit.GetSurface(3);
	panel.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
	panel.FontSize = 1.3f;

Runtime.UpdateFrequency = UpdateFrequency.Update10;

}


public void Main(string arg)
{

	mess = "Контейнер: " + Math.Ceiling((float)inventory.CurrentVolume) +  " m3 из " + Math.Ceiling((float)inventory.MaxVolume) + " m3";
	panel.WriteText(mess);



}
