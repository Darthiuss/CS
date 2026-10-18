using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace LR3
{
    public partial class MainWindow : Window
    {
        private static readonly CultureInfo Ua = new CultureInfo("uk-UA");

        private readonly List<Vehicle> vehicles = new List<Vehicle>();
        private readonly TripAnalysis analysis = new TripAnalysis();

        public MainWindow()
        {
            InitializeComponent();
        }

        // Показуємо панель, що відповідає вибраному типу транспорту.
        private void VehicleTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Подія виникає під час ініціалізації, коли частина елементів ще не створена.
            if (ConsumptionLabel == null || CarPanel == null ||
                MotorcyclePanel == null || ElectricPanel == null) return;

            int type = VehicleTypeComboBox.SelectedIndex;   // 0 - авто, 1 - мотоцикл, 2 - електромобіль

            CarPanel.Visibility = type == 0 ? Visibility.Visible : Visibility.Collapsed;
            MotorcyclePanel.Visibility = type == 1 ? Visibility.Visible : Visibility.Collapsed;
            ElectricPanel.Visibility = type == 2 ? Visibility.Visible : Visibility.Collapsed;

            ConsumptionLabel.Content = type == 2 ? "Витрата (кВт·год/100 км):" : "Витрата (л/100 км):";
        }

        private void AddTripButton_Click(object sender, RoutedEventArgs e)
        {
            if (!TryParseDouble(SpeedTextBox.Text, out double speed) ||
                !TryParseDouble(TravelTimeTextBox.Text, out double time) ||
                !TryParseDouble(FuelConsumptionTextBox.Text, out double consumption))
            {
                MessageBox.Show("Швидкість, час і витрата повинні бути числами.",
                                "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Vehicle vehicle;
                switch (VehicleTypeComboBox.SelectedIndex)
                {
                    case 0:
                        vehicle = new Car(speed, time, consumption,
                                          (EngineType)EngineTypeComboBox.SelectedIndex);
                        break;

                    case 1:
                        vehicle = new Motorcycle(speed, time, consumption,
                                                 (MotorcycleClass)MotorcycleClassComboBox.SelectedIndex);
                        break;

                    default:
                        if (!TryParseDouble(BatteryCapacityTextBox.Text, out double battery))
                        {
                            MessageBox.Show("Ємність батареї повинна бути числом.",
                                            "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        vehicle = new ElectricCar(speed, time, consumption, battery);
                        break;
                }

                vehicles.Add(vehicle);
                RefreshView();

                SpeedTextBox.Clear();
                TravelTimeTextBox.Clear();
                FuelConsumptionTextBox.Clear();
                BatteryCapacityTextBox.Clear();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Змінилась ціна пального або енергії - перераховуємо список та підсумки.
        private void PriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TripsListBox == null || TotalCostTextBlock == null || MinCostTextBlock == null ||
                TotalEmissionTextBlock == null || ElectricPriceTextBox == null) return;

            RefreshView();
        }

        // Для електромобіля використовується ціна енергії, для решти - ціна пального.
        private double GetPrice(Vehicle vehicle, double fuelPrice, double electricPrice)
        {
            return vehicle is ElectricCar ? electricPrice : fuelPrice;
        }

        private void RefreshView()
        {
            TripsListBox.Items.Clear();

            if (!TryParseDouble(FuelPriceTextBox.Text, out double fuelPrice) || fuelPrice < 0 ||
                !TryParseDouble(ElectricPriceTextBox.Text, out double electricPrice) || electricPrice < 0)
            {
                TotalCostTextBlock.Text = "Введіть коректні ціни пального та енергії.";
                MinCostTextBlock.Text = "";
                TotalEmissionTextBlock.Text = "";
                return;
            }

            int number = 1;
            foreach (Vehicle vehicle in vehicles)
            {
                // Поліморфізм: GetRoute() та CalculateTripCost() виконуються
                // за реалізацією конкретного нащадка.
                double cost = vehicle.CalculateTripCost(GetPrice(vehicle, fuelPrice, electricPrice));

                string item = $"{number}. {vehicle.GetRoute()}\n" +
                              $"   Вартість поїздки: {cost.ToString("C", Ua)}";

                // Поліморфізм через інтерфейс: нас цікавить лише наявність методу
                // CalculateCO2Emission(), а не конкретний клас об'єкта.
                if (vehicle is IEmissionCalculable emission)
                {
                    item += $"\n   Викиди CO₂: {emission.CalculateCO2Emission():F1} кг";
                }

                if (vehicle is Motorcycle motorcycle)
                {
                    item += $"\n   Час розгону: {motorcycle.CalculateAccelerationTime():F1} с";
                }

                if (vehicle is ElectricCar electricCar)
                {
                    item += $"\n   Зарядок на маршруті: {electricCar.CalculateChargesCount()}";
                }

                TripsListBox.Items.Add(item);
                number++;
            }

            // Електромобілі рахуються за ціною енергії, решта - за ціною пального,
            // тому розділяємо їх на дві групи.
            List<Vehicle> fuelVehicles = vehicles.Where(v => !(v is ElectricCar)).ToList();
            List<Vehicle> electricVehicles = vehicles.Where(v => v is ElectricCar).ToList();

            // Загальна вартість: колекції передаються через інтерфейс ICostCalculable.
            double total = analysis.GetTotalTripCost(fuelVehicles.Cast<ICostCalculable>().ToList(), fuelPrice)
                         + analysis.GetTotalTripCost(electricVehicles.Cast<ICostCalculable>().ToList(), electricPrice);
            TotalCostTextBlock.Text = $"Загальна вартість поїздок: {total.ToString("C", Ua)}";

            // Мінімальна вартість поїздки (метод вашого TripAnalysis) серед обох груп.
            var minimums = new List<double>();
            if (fuelVehicles.Count > 0) minimums.Add(analysis.GetMinimalPriceTrip(fuelVehicles, fuelPrice));
            if (electricVehicles.Count > 0) minimums.Add(analysis.GetMinimalPriceTrip(electricVehicles, electricPrice));

            MinCostTextBlock.Text = minimums.Count > 0
                ? $"Мінімальна вартість поїздки: {minimums.Min().ToString("C", Ua)}"
                : "Мінімальна вартість поїздки: -";

            // Загальні викиди: колекція передається через інтерфейс IEmissionCalculable.
            double totalEmission = analysis.GetTotalEmission(vehicles.OfType<IEmissionCalculable>().ToList());
            TotalEmissionTextBlock.Text = $"Загальні викиди CO₂: {totalEmission:F1} кг";
        }

        // Приймає і кому, і крапку як десятковий роздільник.
        private static bool TryParseDouble(string text, out double value)
        {
            string normalized = (text ?? string.Empty).Trim().Replace(',', '.');
            return double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        }
    }
}