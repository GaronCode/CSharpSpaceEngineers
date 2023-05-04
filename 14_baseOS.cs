
IMyCockpit cockpit;
Dictionary<string, IMyTextSurface> panels;

List<IMyInventory> inventoryList;
long invMax, invCurrent; 


public Program()
{
    panels = new Dictionary<string, IMyTextSurface>();
    cockpit = GridTerminalSystem.GetBlockWithName("Кресло пилота 2") as IMyCockpit;

    RegistratePanel(cockpit.GetSurface(0),"Панель кокпита");
    RegistratePanel((GridTerminalSystem.GetBlockWithName("Main Display") as IMyTextSurface),"Главная панель");

    inventoryList = new List<IMyInventory>();
    RegistrateInventoryBlock("Большой контейнер");
    CalcInventoryStatus();


    Runtime.UpdateFrequency = UpdateFrequency.Update100;
}



public void Main(string argument, UpdateType updateSource)
{
    RefreshCurrentVolume();
    double procents = CalcProcent(invCurrent, invMax);
    Print("Панель кокпита", "Заполненность: "+invCurrent+" из "+invMax+"\n"+ procents + "\n" + MakeProcentString(procents));
}
public string MakeProcentString(double procent)
{
    string s = "";
    double equals = Math.Round(procent / 5);
    for (int i = 1; i < equals; i++)
    {
        s += "=";
    }
    double dashs = 20 - equals;
    for (int i = 1; i < dashs; i++)
    {
        s += "–";
    }
    return s;
}


public double CalcProcent(double now, double max)
{
    return Math.Round(now / max * 100);
}
public void RegistratePanel(IMyTextSurface panel, string panelName)
{
    if (!panels.ContainsKey(panelName))
    {
        panel.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
        panels.Add(panelName, panel);
    }

}
public void Print(string panelName, string text)
{
    panels[panelName].WriteText(text);
}

public void RegistrateInventoryBlock(string blockName)
{
    inventoryList.Add((GridTerminalSystem.GetBlockWithName(blockName) as IMyTerminalBlock).GetInventory());
}
public void CalcInventoryStatus()
{
    invMax = 0;
    invCurrent = 0;
    foreach (IMyInventory i in inventoryList)
    {
        invMax = (long)i.MaxVolume;
        invCurrent = (long)i.CurrentVolume;
    }
}
public void RefreshCurrentVolume()
{
    foreach (IMyInventory i in inventoryList)
    {
        MyFixedPoint current = i.CurrentVolume;
    }
}


