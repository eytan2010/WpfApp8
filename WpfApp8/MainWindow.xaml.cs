using System;
using System.Text;
using System.Windows;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HotelBookingApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RoomType_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb && rb.IsChecked == true)
            {
                string roomName = rb.Content.ToString();
                string infoMessage = roomName switch
                {
                    "חדר יחיד" => "חדר יחיד.",
                    "חדר זוגי" => "חדר זוגי.",
                    "סוויטה" => "סוויטה.",
                    _ => ""
                };

                if (!string.IsNullOrEmpty(infoMessage))
                {
                    MessageBox.Show(infoMessage, "מידע על החדר", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                CalculatePrice();
            }
        }

        private void OnDatesOrRoomChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculatePrice();
        }

        private void CalculatePrice()
        {
            if (DpArrival == null || DpDeparture == null || TxtTotalPrice == null || TxtBreakdown == null)
                return;

            if (!DpArrival.SelectedDate.HasValue || !DpDeparture.SelectedDate.HasValue)
            {
                TxtBreakdown.Text = "יש לבחור תאריך הגעה ותאריך עזיבה";
                TxtTotalPrice.Text = "סה'כ לתשלום: 0 ₪";
                return;
            }

            DateTime arrival = DpArrival.SelectedDate.Value;
            DateTime departure = DpDeparture.SelectedDate.Value;

            if (departure <= arrival)
            {
                TxtBreakdown.Text = "תאריך עזיבה חייב להיות מאוחר מתאריך ההגעה";
                TxtTotalPrice.Text = "סה'כ לתשלום: 0 ₪";
                return;
            }

            int weekdayRate = 400;
            int weekendRate = 600;

            if (RbDouble.IsChecked == true)
            {
                weekdayRate = 600;
                weekendRate = 850;
            }
            else if (RbSuite.IsChecked == true)
            {
                weekdayRate = 1000;
                weekendRate = 1400;
            }

            int weekdayNights = 0;
            int weekendNights = 0;

            for (DateTime date = arrival; date < departure; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Friday || date.DayOfWeek == DayOfWeek.Saturday)
                {
                    weekendNights++;
                }
                else
                {
                    weekdayNights++;
                }
            }

            int totalCost = (weekdayNights * weekdayRate) + (weekendNights * weekendRate);

            TxtBreakdown.Text = $"לילות אמצע שבוע: {weekdayNights} ({weekdayRate} ₪/לילה) | לילות סופ'ש: {weekendNights} ({weekendRate} ₪/לילה)";
            TxtTotalPrice.Text = $"סה'כ לתשלום: {totalCost:N0} ₪";
        }
    }
}