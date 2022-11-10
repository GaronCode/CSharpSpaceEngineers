IMyTextSurface panel;
IMyCargoContainer container;

;

string mess;

string CONTAINER_NAME = "КВ_Контейнер_извлекательный";
string DISPLAY_NAME = "КВ Дисплей левая каюта";



public Program() {
  container = GridTerminalSystem.GetBlockWithName(CONTAINER_NAME) as IMyCargoContainer;
  panel = GridTerminalSystem.GetBlockWithName(DISPLAY_NAME) as IMyTextSurface;
  panel.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
  panel.FontSize = 1f;

  Runtime.UpdateFrequency = UpdateFrequency.Update100;
}

public void Main(string arg) {

  IMyInventory inventory = container.GetInventory();
  List<MyInventoryItem> items = new List<MyInventoryItem>();

  inventory.GetItems(items);

  var allCargo = new Dictionary<string, MyFixedPoint>();
  foreach (MyInventoryItem i in items) {

    if (allCargo.ContainsKey(i.Type.SubtypeId)) {
      allCargo[i.Type.SubtypeId] += i.Amount;
    }
    else {
        allCargo.Add(i.Type.SubtypeId, i.Amount);
    }
  }

  mess = '';
  foreach (var oneType in allCargo) {
    mess += oneType.Key + ": " + oneType.Value + "\r\n";
  }


  panel.WriteText(mess);
 }
