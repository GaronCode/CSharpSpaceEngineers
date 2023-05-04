
        List<string> CONTAINERS_NAME = new List<string> { "cont" };
        string COCKPIT_NAME = "cocpit";
        
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        IMyCockpit cockpit;

        List<IMyCargoContainer> containers = new List<IMyCargoContainer>();
        Dictionary<string, MyFixedPoint> allCargo;
        long currentVolume = 0, maxVolume = 0;

        List<IMyTextSurface> displayOutputs = new List<IMyTextSurface>();

        List<IMyBatteryBlock> batteries = new List<IMyBatteryBlock>();
        float batteriesMax = 0f, batteriesCurrent = 0f;


        Dictionary<string, string> translate = new Dictionary<string, string>()
        {
            ["Iron"] = "Железо",
            ["Stone"] = "Камень",
            ["Ice"] = "Лёд",
            ["Nickel"] = "Никель",
            ["Gold"] = "Золото",
            ["Silver"] = "Серебро",
            ["Cobalt"] = "Кобальт",
            /////////////////////////////////////////////
            ["BulletproofGlass"] = "Бронестекло",
            ["Computer"] = "Компьютер",
            ["Construction"] = "Стройкомпоненты",
            ["Detector"] = "Компоненты детектора",
            ["Display"] = "Экран",
            ["Girder"] = "Балка",
            ["GravityGenerator"] = "Компонент гравигена",
            ["InteriorPlate"] = "Внутренняя пластина",
            ["LargeTube"] = "Большая труба",
            ["Medical"] = "Мед. компоненты",
            ["MetalGrid"] = "Решётка",
            ["Motor"] = "Мотор",
            ["PowerCell"] = "Энергоячейка",
            ["RadioCommunication"] = "Радиокомпоненты",
            ["Reactor"] = "Компоненты реактора",
            ["SmallTube"] = "Малая трубка",
            ["SolarCell"] = "Солнечная ячейка",
            ["SteelPlate"] = "Стальная пластина",
            ["Superconductor"] = "Сверхпроводник",
            ["Thrust"] = "Деталь ионки"
        };

        public Program()
        {
            cockpit = GridTerminalSystem.GetBlockWithName(COCKPIT_NAME) as IMyCockpit;

            foreach (string item in CONTAINERS_NAME)
            {
                containers.Add(GridTerminalSystem.GetBlockWithName(item) as IMyCargoContainer);
            }

            for (int i = 0; i < 4; i++)
            {
                try
                {
                    displayOutputs.Add(cockpit.GetSurface(i));
                }
                catch (Exception e)
                {
                    break;
                }
            }


            batteriesMax = 0f;
            batteriesCurrent = 0f;
            GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(batteries);
            






            Runtime.UpdateFrequency = UpdateFrequency.Update100;
        }

        public void updateBatteries() {
            batteriesMax = 0;
            batteriesCurrent = 0;
            foreach (IMyBatteryBlock i in batteries)
            {
                if (i.CubeGrid.CustomName == cockpit.CubeGrid.CustomName)
                {
                    batteriesMax += i.MaxStoredPower;
                    batteriesCurrent += i.CurrentStoredPower;
                }
            }
        }

        public void updateCargo()
        {
            List<List<MyInventoryItem>> inventoryItems = new List<List<MyInventoryItem>>();

            currentVolume = 0;
            maxVolume = 0;

            foreach (IMyCargoContainer item in containers)
            {
                IMyInventory inventory = item.GetInventory();
                currentVolume += inventory.CurrentVolume.RawValue;
                maxVolume += inventory.MaxVolume.RawValue;

                List<MyInventoryItem> items = new List<MyInventoryItem>();
                inventory.GetItems(items);
                inventoryItems.Add(items);
            }


            allCargo = new Dictionary<string, MyFixedPoint>();
            foreach (List<MyInventoryItem> item in inventoryItems)
            {
                foreach (MyInventoryItem i in item)
                {
                    if (allCargo.ContainsKey(i.Type.SubtypeId))
                    {
                        allCargo[i.Type.SubtypeId] += i.Amount;
                    }
                    else
                    {
                        allCargo.Add(i.Type.SubtypeId, i.Amount);
                    }
                }
            }
        }

        public void Main(string arg)
        {
            ///////////////////////////////////////////////////////////////////////////////////////////////////
            updateCargo();
            string messCargo = "";
            if (allCargo.Count == 0)
            {
                messCargo = "Пусто";
            }
            else
            {
                foreach (var oneType in allCargo)
                {
                    string name, value = Math.Ceiling((float)oneType.Value).ToString();
                    if (translate.ContainsKey(oneType.Key)) name = translate[oneType.Key];
                    else name = oneType.Key;

                    messCargo += name + makeSpaces(name, value) + value + "\r\n";
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////
            string messVolume = "ОБЪЁМ\r\n"+Math.Ceiling((float)currentVolume / 1000) /1000+ " из " + Math.Ceiling((float)maxVolume / 1000) / 1000 + " л\r\n";
            double procent = CalcProcent(currentVolume, maxVolume);
            messVolume += MakeProcentString(procent) + "\r\n" + procent + "%";
            ChangeDisplayColor(displayOutputs[0], procent, true);
            ///////////////////////////////////////////////////////////////////////////////////////////////////
            updateBatteries();
            string messBattery = "ЗАРЯД\r\n" + Math.Ceiling((float)batteriesCurrent*100)/100 + " из " + Math.Ceiling((float)batteriesMax*100)/100 + " МВт\r\n";
            procent = CalcProcent(batteriesCurrent, batteriesMax);
            messBattery += MakeProcentString(procent) + "\r\n" + procent + "%";
            ChangeDisplayColor(displayOutputs[2], procent);


            displayOutputs[0].WriteText(messVolume);
            displayOutputs[1].WriteText(messCargo);
            displayOutputs[2].WriteText(messBattery);
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


        public void ChangeDisplayColor(IMyTextSurface d, double p) { 
            if (p < 10)
            {
                d.BackgroundColor = new Color(80, 0, 0);
                return;
            }
            if (p > 50)
            {
                d.BackgroundColor = new Color(0, 100, 0);
                return;
            }
            d.BackgroundColor = new Color((int)p*(-5)+250, 100, 0);
        }
        public void ChangeDisplayColor(IMyTextSurface d, double p, Boolean s) {
            ChangeDisplayColor(d, 100 - p);
        }

        public double CalcProcent(float now, float max)
        {
            return Math.Round(now / max * 100);
        }

        public string makeSpaces(string firstString,string lastString)
        {
            string s = "";
            int length = 25 - (firstString.Length + lastString.Length);
            for (int i = 0; i < length; i++)
            {
                s += " ";
            }
            return s;
        }

