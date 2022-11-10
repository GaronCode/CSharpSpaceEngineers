
//создание переменных со своими типами данных
//IMyTextSurface, 
IMyTextSurface panel;
IMyTerminalBlock sklad1;
IMyInventory inventory1;

string mess;
double nowVolume = 0,  maxVolume = 0;

string CONTAINER_NAME = "Большой контейнер фуры 1";
string DISPLAY_NAME = "Дисплей фуры";

public Program() {
  sklad1 = GridTerminalSystem.GetBlockWithName(CONTAINER_NAME) as IMyTerminalBlock;
  inventory1 = sklad1.GetInventory();

  maxVolume = Math.Ceiling((float)inventory1.MaxVolume);

  panel = GridTerminalSystem.GetBlockWithName(CONTAINER_NAME) as IMyTextSurface;
  panel.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
  panel.FontSize = 3.3f;

  Runtime.UpdateFrequency = UpdateFrequency.Update100;
}

public void Main(string arg) {

  nowVolume = Math.Ceiling((float)inventory1.CurrentVolume);
  mess = "Заполненно:\r\n";
  mess += nowVolume + "/" + maxVolume;

  panel.WriteText(mess);

 }
