
List<IMyTerminalBlock> cargo;
List<IMyTerminalBlock> connectors;
List<IMyTerminalBlock> drills;
List<IMyInventory> inventories;
List<IMyBatteryBlock> batteries;

float batteriesMax = 0f, batteriesCurrent = 0f;
string COCKPIT_NAME = "Кокпит СИГАРА";
int COCKPIT_SURFACE = 0;

List<string> displayNames = new List<string>() { "РК-2 ЖК-панель 1", "РК-2 ЖК-панель 2", "РК-2 ЖК-панель 3" };
List<string> displayTexts = new List<string>(3);
List<IMyTextSurface> displayOutputs = new List<IMyTextSurface>(3);

Dictionary<string, string> translate = new Dictionary<string, string>()
{
    ["Iron"] = "Железо",
    ["Stone"] = "Камень",
    ["Ice"] = "Лёд",
    ["Nickel"] = "Никель",
    ["Gold"] = "Золото",
    ["Silver"] = "Серебро",
    ["Cobalt"] = "Кобальт",
};

public Program() {
    container = GridTerminalSystem.GetBlockWithName(CONTAINER_NAME) as IMyCargoContainer;

    int s = 0;
    foreach (string displayName in displayNames) {
        IMyTextSurface display = GridTerminalSystem.GetBlockWithName(displayName) as IMyTextSurface;
        display.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
        displayOutputs[s] = display;
        displayTexts[s] = "";
    }



//   panel1 = GridTerminalSystem.GetBlockWithName("РК-2 ЖК-панель 1") as IMyTextSurface;
//   panel2 = GridTerminalSystem.GetBlockWithName("РК-2 ЖК-панель 2") as IMyTextSurface;
//   panel3 = GridTerminalSystem.GetBlockWithName("РК-2 ЖК-панель 3") as IMyTextSurface;
//   panel1.ContentType = panel2.ContentType = panel3.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
  

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

    // int i = 0;
    // foreach (var item in displayTexts) {
    //     displayTexts[i] = "";
    //     i++;
    // }



    foreach (var oneType in allCargo) {
        if (translate.ContainsKey(oneType.Key)) displayTexts[1] = translate[oneType.Key];
        else displayTexts[1] = oneType.Key;
        displayTexts[2] = "" + Math.Ceiling((float)oneType.Value);
        displayTexts[1] += "\r\n";
        displayTexts[2] += "\r\n";
    }

    displayTexts[3] = Math.Ceiling((float)inventory.CurrentVolume) +  " m3 \r\n/\r\n " + Math.Ceiling((float)inventory.MaxVolume) + " m3";


    int u = 0;
    foreach (IMyTextSurface display in displayOutputs)
    {
        display.WriteText(displayTexts[u]);
        u++;
    }

//   panel3.WriteText();
//   panel1.WriteText(mess1);
//     panel2.WriteText(mess2);
 }


