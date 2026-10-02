using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static ConsoleApp52.SecuteDevice;

namespace ConsoleApp52
{
    internal class Program
    {    //учился работать с интерфейсами
        static void Main(string[] args)
        {
            Console.WriteLine("=== CИСТЕМА УМНЫЙ ДОМ ЗАПУЩЕНА ===\n");
            List<IDevice> smarthouse = new List<IDevice>
            {
                new SecuteDevice ("Cветоидиодная лампа в спальне", 10.5),
                new SecuteDevice("Мощный прожектор во дворе", 50.0),
                new SmartLock ("Входная дверь","admin123"),
                new SmartTV ("Телевизор в гостинной",120.0)
            };
            Console.WriteLine($"----Включение вмех устройств ----");
            foreach(var device in smarthouse) 
            {
                if (device is ISecuteDevice secuteDevice) 
                {
                    Console.WriteLine($"\n Попытка открыть{device.Name}...");
                    secuteDevice.Authenticate("wrong_pass");
                    if (secuteDevice.Authenticate("admin123"))
                    {
                        device.TurnOff();
                    }
                    Console.WriteLine();
                }
                else 
                {
                    device.Turnon();
                }
            }
            Console.WriteLine("\n --- Расчет энергопотребления ---");
            double totalPower = 0;
            foreach(var device in smarthouse) 
            {
                if (device is IEnergyCinsuming energyDevice) 
                {
                    energyDevice.DisplayEcoStatus();
                    totalPower += energyDevice.PowerConsumpt;
                    
                }
            }
            Console.WriteLine($"\n [ИТОГ] Общее энеропотреления дома: {totalPower}");
            Console.ReadLine();
        }
    }
    interface IDevice 
    {
        string Name { get; }
        bool IsOn { get; }
        void Turnon();
        void TurnOff();
    }
    interface IEnergyCinsuming 
    {
        double PowerConsumpt { get; }
        void DisplayEcoStatus();


    }
    interface ISecuteDevice 
    {
        bool Authenticate(string password);
   }
    public class SecuteDevice : IDevice, IEnergyCinsuming
    {
        public string Name { get; set; }
        public bool IsOn { get; set; }
        public double PowerConsumpt { get; set; }

        public int Brightness { get; set; }
        public SecuteDevice(string name, double powerconsumption)
        {
            Name = name;
            PowerConsumpt = powerconsumption;
        }
        public void Turnon()
        {
            IsOn = true;
            Console.WriteLine($"{Name} включена, Яркость {Brightness}  ");

        }
        public void TurnOff()
        {
            IsOn = false;
            Console.WriteLine($"{Name} Выключена");
        }
        public void DisplayEcoStatus() 
        {
            if (PowerConsumpt < 15)
            {
                Console.WriteLine($"|{Name}| Статус:Энергосберегающая лампа {PowerConsumpt} Вт.");
            }
            else
            {


                Console.WriteLine($"|{Name}|Статус : Высококе потребление энергии {PowerConsumpt} Вт ");
                }
        }
        public class SmartLock : IDevice, ISecuteDevice 
        {
            private readonly string _secondPassword;
            private bool _isUnlocked = false;
            public string Name { get; }
            public bool IsOn {  set; get; }
            public SmartLock (string name, string passord) 
            {
                Name = name;
                _secondPassword = passord;
                IsOn = true;
            }
            public bool Authenticate(string password) 
            {
                if (_secondPassword == password) 
                {
                    _isUnlocked = true;
                    Console.WriteLine($"{Name} Авторизация успешна доступ разрешен.");
                    return true;
                }
                else { Console.WriteLine($"|{Name}| Отказано в доступе! Неверный пароль ."); }
                return false;
            }
            public void Turnon() 
            {
                if (!_isUnlocked) 
                {
                    Console.WriteLine($"|{Name}|Отказано в доступе сперва пройдите авторизацию.");
                    return;
                }
               
                    IsOn = true;
                    Console.WriteLine($"|{Name}|Замок успешно заблокирован");
                _isUnlocked= false;

                
            }
            public void TurnOff() 
            {
                if (!_isUnlocked)
                {
                    Console.WriteLine($"|{Name}|Отказано в доступе сперва пройдите авторизацию.");
                    return;
                }
                IsOn = false;
                Console.WriteLine($"|{Name}| Замок успешно Разблокирован (открыт).");
            }

        }
        public class SmartTV:IDevice,IEnergyCinsuming 
        {
            public string Name { get; }
            public bool IsOn {  get; private set; }
            public double PowerConsumpt { get; }
            public int CurrentChannel { get; set; } = 1;
            public SmartTV(string name, double powerConsumpt) 
            {
                Name    = name;
                PowerConsumpt = powerConsumpt;
            }
            public void Turnon() 
            {
                IsOn = true;
                Console.WriteLine($"|{Name}| Включен. Тукщий канал {CurrentChannel}.");


            }
            public void TurnOff() 
            {
                IsOn = false;
                Console.WriteLine($"|{Name}| Выключен.");
            }
            public void DisplayEcoStatus() 
            {
                Console.WriteLine($"|{Name}|Потребление в рабочем режиме:{PowerConsumpt}вт.");
            }


        }

    }

}
