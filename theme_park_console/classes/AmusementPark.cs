using System;

namespace theme_park_console
{
    enum AttractionTypes
    {
        FerrisWheel,
        RollerCoaster,
        BumpingCars,
        Undefined
    }
    class AmusementPark
    {
        public string Name { get; set; }
        public double Budget { get; set; }

        private Logger logger;

        public AttractionLinkedList<Attraction> attractions;
        public AmusementPark(string name, double budget)
        {
            Name = name;
            Budget = budget;
            attractions = new AttractionLinkedList<Attraction>();
            logger = new Logger();

            logger.LogEvent += LoggerMethods.LogInConsole;
            logger.LogEvent += LoggerMethods.LogInFile;
        }
        public static AttractionTypes GetAttractionType(Attraction attraction)
        {
            if (attraction.GetType() == typeof(FerrisWheel))
                return AttractionTypes.FerrisWheel;
            if (attraction.GetType() == typeof(RollerCoaster))
                return AttractionTypes.RollerCoaster;
            if (attraction.GetType() == typeof(BumpingCars))
                return AttractionTypes.BumpingCars;

            return AttractionTypes.Undefined;
        }
        public bool ConsoleCreateAttraction(AttractionTypes type, string name, double price)
        {
            string[] data;
            Attraction attraction = null;

            switch (type)
            {
                case AttractionTypes.FerrisWheel:
                    Console.WriteLine("Введите данные в следующем формате через пробел:\n" +
                        "высота_колеса_в_метрах период_одного_поворота_в_минутах количество_кабин вместимость_кабины");
                    data = Console.ReadLine().Split(' ');
                    double height, period;
                    int cabins, wheel_passengers;

                    try
                    {
                        if (data.Length != 4)
                        {
                            throw new FormatException();
                        }

                        if (!double.TryParse(data[0], out height) || !double.TryParse(data[1], out period) ||
                            !int.TryParse(data[2], out cabins) || !int.TryParse(data[3], out wheel_passengers))
                        {
                            throw new AttractionException("Нельзя создать аттракцион с такими параметрами!");
                        }

                        if (height <= 0 || period <= 0 || cabins <= 0 || wheel_passengers <= 0)
                        {
                            throw new AttractionException("Нельзя создать аттракцион с такими параметрами!");
                        }

                        attraction = new FerrisWheel(name, price, height, period, cabins, wheel_passengers);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Строка имеет неверный формат.");
                        return false;
                    }
                    catch (AttractionException)
                    {
                        Console.WriteLine("Нельзя создать аттракцион с такими параметрами!");
                        return false;
                    }

                    break;

                case AttractionTypes.RollerCoaster:
                    Console.WriteLine("Введите данные в следующем формате через пробел:\n" +
                        "общая_длина_в_метрах средняя_скорость(км/ч) количество_вагонов вместимость_вагона\n" +
                        "максимальная_скорость_вагона(км/ч) самая_высокая_точка_в_метрах");
                    data = Console.ReadLine().Split(' ');
                    double length, average_speed, max_speed, highest_point;
                    int wagons, wagon_passengers;

                    try
                    {
                        if (data.Length != 6)
                        {
                            throw new FormatException();
                        }
                        if (!double.TryParse(data[0], out length) || !double.TryParse(data[1], out average_speed) ||
                            !int.TryParse(data[2], out wagons) || !int.TryParse(data[3], out wagon_passengers) ||
                            !double.TryParse(data[4], out max_speed) || !double.TryParse(data[5], out highest_point))
                        {
                            throw new AttractionException("Нельзя создать аттракцион с такими параметрами!");
                        }
                        if (length <= 0 || average_speed <= 0 || max_speed <= 0 || highest_point <= 0 ||
                            wagons <= 0 || wagon_passengers <= 0)
                        {
                            throw new AttractionException("Нельзя создать аттракцион с такими параметрами!");
                        }

                        attraction = new RollerCoaster(name, price, length, average_speed, wagons, wagon_passengers,
                            max_speed, highest_point);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Строка имеет неверный формат");
                        return false;
                    }
                    catch (AttractionException)
                    {
                        Console.WriteLine("Нельзя создать аттракцион с такими параметрами!");
                        return false;
                    }

                    break;

                case AttractionTypes.BumpingCars:
                    Console.WriteLine("Введите данные в следующем формате через пробел:\n" +
                        "количество_машин общая_площадь_аттракциона(кв.метр) максимальная_скорость_машины(км/ч)");
                    data = Console.ReadLine().Split(' ');
                    int cars;
                    double area, car_max_speed;

                    try
                    {
                        if (data.Length != 3)
                        {
                            throw new FormatException();
                        }
                        if (!int.TryParse(data[0], out cars) || !double.TryParse(data[1], out area) ||
                            !double.TryParse(data[2], out car_max_speed))
                        {
                            throw new AttractionException("Нельзя создать аттракцион с такими параметрами!");
                        }
                        if (cars <= 0 || area <= 0 || car_max_speed <= 0)
                        {
                            throw new AttractionException("Нельзя создать аттракцион с такими параметрами!");
                        }

                        attraction = new BumpingCars(name, price, cars, area, car_max_speed);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Строка имеет неверный формат");
                        return false;
                    }
                    catch (AttractionException)
                    {
                        Console.WriteLine("Нельзя создать аттракцион с такими параметрами!");
                        return false;
                    }

                    break;
            }

            if (attraction != null)
                attractions.Add(attraction);

            return true;
        }
        public bool CreateFerrisWheel(string name, double price, double height, double rotation_period,
            int number_of_cabins, int customers_per_cabin)
        {
            Attraction attraction = new FerrisWheel(name,
                        price,
                        height,
                        rotation_period,
                        number_of_cabins,
                        customers_per_cabin
                        );
            attractions.Add(attraction);
            return true;
        }
        public bool CreateRollerCoater(string name, double price, double total_lenght, double average_speed,
            int number_of_vagons, int customers_per_vagon, double max_speed, double highest_point)
        {
            Attraction attraction = new RollerCoaster(name,
                        price,
                        total_lenght,
                        average_speed,
                        number_of_vagons,
                        customers_per_vagon,
                        max_speed,
                        highest_point
                        );

            attractions.Add(attraction);
            return true;
        }
        public bool CreateBumpingCars(string name, double price, int number_of_cars, double floor_area, double max_car_speed)
        {
            Attraction attraction = new BumpingCars(name,
                        price,
                        number_of_cars,
                        floor_area,
                        max_car_speed
                        );

            attractions.Add(attraction);
            return true;
        }
        public void SaveListToXML()
        {
            XMLSerialiser serialiser = new XMLSerialiser();
            serialiser.Serialise(attractions);

            logger.InvokeLogEvent("Список аттракционов был сохранен в файл XML.");
        }
        public void LoadListFromXML()
        {
            logger.InvokeLogEvent("Пользователь начал процесс загрузки списка аттракционов из файла XML.");

            XMLSerialiser serialiser = new XMLSerialiser();
            attractions = serialiser.Deserialise();

            logger.InvokeLogEvent("Список аттракционов был загружен из файла XML.");
        }
        public void SaveListToJSON()
        {
            JSONSerialiser serialiser = new JSONSerialiser();
            serialiser.Serialise(attractions);

            logger.InvokeLogEvent("Список аттракционов был сохранен в файл JSON.");
        }
        public void LoadListFromJSON()
        {
            logger.InvokeLogEvent("Пользователь начал процесс загрузки списка аттракционов из файла JSON.");

            JSONSerialiser serialiser = new JSONSerialiser();
            attractions = serialiser.Deserialise();

            logger.InvokeLogEvent("Список аттракционов был загружен из файла JSON.");
        }
    }
}
