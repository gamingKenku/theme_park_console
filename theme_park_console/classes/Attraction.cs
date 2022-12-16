using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theme_park_console
{
    public abstract class Attraction
    {
        public string name;
        public double ticket_price;
        public int customers_group;
        public double session_time;
        public double price;
        public Attraction(string name, double price)
        {
            this.name = name;
            this.price = price;
        }
        public Attraction() { }
        protected abstract double GetTicketPrice();
        protected abstract int GetCustomersGroup();
        protected abstract double GetSessionTime();
        public abstract string GetInfo();
    }
    public class FerrisWheel : Attraction
    {
        private double height, rotation_period;
        private int number_of_cabins, customers_per_cabin;
        public FerrisWheel(string name, double price, double height, double rotation_period,
            int number_of_cabins, int customers_per_cabin) : base(name, price)
        {
            this.height = height; 
            this.rotation_period = rotation_period;
            this.number_of_cabins = number_of_cabins; 
            this.customers_per_cabin = customers_per_cabin;

            ticket_price = GetTicketPrice();
            customers_group = GetCustomersGroup();
            session_time = GetSessionTime();
        }
        public FerrisWheel() { }
        protected override double GetTicketPrice()
        {
            double price = height * 0.15;

            if (price > 15) return 15;
            else if (price < 5) return 5;

            return price;
        }
        protected override int GetCustomersGroup()
        {
            return customers_per_cabin * number_of_cabins;
        }
        protected override double GetSessionTime()
        {
            return rotation_period;
        }
        public override string GetInfo()
        {
            string res = "";
            res += $"Название: {name};\n";
            res += "Тип аттракциона: колесо обозрения;\n";
            res += $"Цена за билет {Math.Round(ticket_price, 2)}$;\n";
            res += $"Максимальное число посетителей: {customers_group};\n";
            res += $"Время посещения: {Math.Round(session_time, 2)} минут.";
            return res;
        }
    }
    public class RollerCoaster : Attraction
    {
        private double total_length, average_speed_km_h;
        private int number_of_vagons, customers_per_vagon;
        private double max_speed_km_h, highest_point_meters;
        public RollerCoaster(string name, double price, double total_length, double average_speed,
            int number_of_vagons, int customers_per_vagon, double max_speed, double highest_point) : base(name, price)
        {
            this.total_length = total_length;
            this.average_speed_km_h = average_speed;
            this.customers_per_vagon = customers_per_vagon;
            this.number_of_vagons = number_of_vagons;
            this.max_speed_km_h = max_speed;
            this.highest_point_meters = highest_point;

            ticket_price = GetTicketPrice();
            customers_group = GetCustomersGroup();
            session_time = GetSessionTime();
        }
        public RollerCoaster() { }
        protected override int GetCustomersGroup()
        {
            return customers_per_vagon * number_of_vagons;
        }
        protected override double GetSessionTime()
        {
            return total_length / (average_speed_km_h * 16.6667);
        }
        protected override double GetTicketPrice()
        {
            double price = max_speed_km_h * 0.15 + highest_point_meters * 0.01;
            if (price > 30.0) return 30.0;
            else if (price < 10) return 10.0;

            return price;
        }
        public override string GetInfo()
        {
            string res = "";
            res += $"Название аттракциона: {name};\n";
            res += "Тип аттракциона: американские горки;\n";
            res += $"Цена за билет {Math.Round(ticket_price, 2)}$;\n";
            res += $"Максимальное число посетителей: {customers_group};\n";
            res += $"Время посещения: {Math.Round(session_time, 2)} минут.";
            return res;
        }
    }
    public class BumpingCars : Attraction
    {
        private int number_of_cars;
        private double floor_area_m2, max_car_speed_km_h;
        public BumpingCars(string name, double price, int number_of_cars, 
            double floor_area, double max_car_speed) : base(name, price)
        {
            this.number_of_cars = number_of_cars;
            this.floor_area_m2 = floor_area;
            this.max_car_speed_km_h = max_car_speed;

            ticket_price = GetTicketPrice();
            customers_group = GetCustomersGroup();
            session_time = GetSessionTime();
        }
        public BumpingCars() { }
        protected override int GetCustomersGroup()
        {
            return number_of_cars * 2;
        }
        protected override double GetSessionTime()
        {
            return 50 / number_of_cars;
        }
        protected override double GetTicketPrice()
        {
            double price = floor_area_m2 * 0.02 + max_car_speed_km_h * 1.0;

            if (price > 30.0) return 30.0;
            else if (price < 10) return 10.0;

            return price;
        }
        public override string GetInfo()
        {
            string res = "";
            res += $"Название аттракциона: {name};\n";
            res += "Тип аттракциона: автодром;\n";
            res += $"Цена за билет {Math.Round(ticket_price, 2)}$;\n";
            res += $"Максимальное число посетителей: {customers_group};\n";
            res += $"Время посещения: {Math.Round(session_time, 2)} минут.";
            return res;
        }
    }
}
