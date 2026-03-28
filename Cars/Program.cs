using System;

namespace CarFactory
{
    public enum CarType
    {
        Tesla,
        BMW,
        Toyota,
        Ford,
        Mercedes
    }

    public interface ICar
    {
        string GetDescription();
    }

    public interface IElectric
    {
        string GetEnergyType();
    }

    public interface IMechanical
    {
        string GetEnergyType();
    }

    public interface IAutomatic
    {
        string GetTransmission();
    }

    public interface IManual
    {
        string GetTransmission();
    }

    public abstract class ACar : ICar
    {
        public string? Brand;
        public int Seats;
        public string? InfoSystem;

        public abstract string GetEnergyType();
        public abstract string GetTransmission();

        public string GetDescription()
        {
            return Brand + ": " + GetEnergyType() + " с " + GetTransmission() +
                   ", " + Seats + " местами, " + InfoSystem + " на борту";
        }
    }

    public abstract class ElectricAutomaticCar : ACar, IElectric, IAutomatic
    {
        public override string GetEnergyType()
        {
            return "electrical car";
        }

        public override string GetTransmission()
        {
            return "автоматической коробкой передач";
        }
    }

    public abstract class MechanicalAutomaticCar : ACar, IMechanical, IAutomatic
    {
        public override string GetEnergyType()
        {
            return "mechanical car";
        }

        public override string GetTransmission()
        {
            return "автоматической коробкой передач";
        }
    }

    public abstract class MechanicalManualCar : ACar, IMechanical, IManual
    {
        public override string GetEnergyType()
        {
            return "mechanical car";
        }

        public override string GetTransmission()
        {
            return "механической коробкой передач";
        }
    }

    public class Tesla : ElectricAutomaticCar
    {
        public Tesla()
        {
            Brand = "Tesla";
            Seats = 5;
            InfoSystem = "Андроид";
        }
    }

    public class BMW : MechanicalAutomaticCar
    {
        public BMW()
        {
            Brand = "BMW";
            Seats = 5;
            InfoSystem = "iDrive";
        }
    }

    public class Toyota : MechanicalAutomaticCar
    {
        public Toyota()
        {
            Brand = "Toyota";
            Seats = 5;
            InfoSystem = "Андроид";
        }
    }

    public class Ford : MechanicalManualCar
    {
        public Ford()
        {
            Brand = "Ford";
            Seats = 5;
            InfoSystem = "SYNC";
        }
    }

    public class Mercedes : MechanicalAutomaticCar
    {
        public Mercedes()
        {
            Brand = "Mercedes";
            Seats = 5;
            InfoSystem = "MBUX";
        }
    }

    public static class CarFactory
    {
        public static ICar Create(CarType type)
        {
            switch (type)
            {
                case CarType.Tesla:    return new Tesla();
                case CarType.BMW:      return new BMW();
                case CarType.Toyota:   return new Toyota();
                case CarType.Ford:     return new Ford();
                case CarType.Mercedes: return new Mercedes();
                default: throw new ArgumentException("Неизвестный тип: " + type);
            }
        }
    }

    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.Write("Введите марку автомобиля или done для остановки ввода: ");
                string? input = Console.ReadLine();

                if (input == null || input.Trim() == "")
                    continue;

                input = input.Trim();

                if (input.ToLower() == "done")
                    break;

                CarType carType;
                if (Enum.TryParse(input, true, out carType))
                {
                    ICar car = CarFactory.Create(carType);
                    Console.WriteLine("«" + car.GetDescription() + "»");
                }
                else
                {
                    Console.WriteLine("Марка \"" + input + "\" не найдена.");
                }
            }
        }
    }
}