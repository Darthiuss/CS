using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using LR3;

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

        private void VehicleTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EngineLabel == null || ClassLabel == null) return;

            bool isCar = VehicleTypeComboBox.SelectedIndex == 0;
            EngineLabel.Visibility = isCar ? Visibility.Visible : Visibility.Collapsed;
            EngineTypeComboBox.Visibility = isCar ? Visibility.Visible : Visibility.Collapsed;
            ClassLabel.Visibility = isCar ? Visibility.Collapsed : Visibility.Visible;
            MotorcycleClassComboBox.Visibility = isCar ? Visibility.Collapsed : Visibility.Visible;
        }

        private void AddTripButton_Click(object sender, RoutedEventArgs e)
        {
            if (!TryParseDouble(SpeedTextBox.Text, out double speed) ||
                !TryParseDouble(TravelTimeTextBox.Text, out double time) ||
                !TryParseDouble(FuelConsumptionTextBox.Text, out double consumption))
            {
                MessageBox.Show("Швидкість, час і витрата пального повинні бути числами.",
                                "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Vehicle vehicle;
                if (VehicleTypeComboBox.SelectedIndex == 0)
                {
                    vehicle = new Car(speed, time, consumption,
                                      (EngineType)EngineTypeComboBox.SelectedIndex);
                }
                else
                {
                    vehicle = new Motorcycle(speed, time, consumption,
                                             (MotorcycleClass)MotorcycleClassComboBox.SelectedIndex);
                }

                vehicles.Add(vehicle);
                RefreshView();

                SpeedTextBox.Clear();
                TravelTimeTextBox.Clear();
                FuelConsumptionTextBox.Clear();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void FuelPriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TripsListBox == null || TotalCostTextBlock == null) return;
            RefreshView();
        }

        private void RefreshView()
        {
            TripsListBox.Items.Clear();

            if (!TryParseDouble(FuelPriceTextBox.Text, out double fuelPrice) || fuelPrice < 0)
            {
                TotalCostTextBlock.Text = "Введіть коректну ціну пального.";
                return;
            }

            int number = 1;
            foreach (Vehicle vehicle in vehicles)
            {
                string item = $"{number}. {vehicle.GetRoute()}\n" +
                              $"   Вартість поїздки: {vehicle.CalculateTripCost(fuelPrice).ToString("C", Ua)}";

                if (vehicle is Motorcycle motorcycle)
                {
                    item += $"\n   Час розгону: {motorcycle.CalculateAccelerationTime():F1} с";
                }

                TripsListBox.Items.Add(item);
                number++;
            }

            double total = analysis.GetTotalTripCost(vehicles, fuelPrice);
            TotalCostTextBlock.Text = $"Загальна вартість поїздок: {total.ToString("C", Ua)}";
        }

        private static bool TryParseDouble(string text, out double value)
        {
            string normalized = (text ?? string.Empty).Trim().Replace(',', '.');
            return double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        }
    }
}
