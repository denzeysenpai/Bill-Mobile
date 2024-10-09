namespace SplitBillApp;

public partial class MainPage : ContentPage
{
    decimal bill;
    decimal tip;
    int noPerson = 1;
    public MainPage()
    {
        InitializeComponent();
    }

    private void txtBill_Completed(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtBill.Text))
        {
            txtBill.Text = "0.00";
            CalculateTotal();
        }
        try
        {
            bill = decimal.Parse(txtBill.Text);
        }
        catch (Exception)
        {
            bill = 0;
            txtBill.Text = "0.00";
        }
        CalculateTotal();
    }

    private void lblNoPerson_Completed(object sender, EventArgs e)
    {
        int newVal = 0;
        if (string.IsNullOrEmpty(lblNoPerson.Text)) CalculateTotal();
        else
        {
            try
            {
                newVal = int.Parse(lblNoPerson.Text);
                if (newVal > 0) noPerson = newVal;
                else noPerson = 1;
            }
            catch (Exception)
            {
                noPerson = 1;
            }

        }
        CalculateTotal();
    }
    private void txtCustomTip_Completed(object sender, EventArgs e)
    {
        if (decimal.TryParse(txtCustomTip.Text, out decimal customTip))
        {
            lblTip.Text = customTip.ToString("F2");
            tip = (customTip / bill) * 100;
            lblTipPercentage.Text = $"Tip: {customTip.ToString("F2")}";
            CalculateTotal();
        }
        else
        {
            DisplayAlert("Invalid input", "Please enter a valid numeric value for the custom tip.", "OK");
            txtCustomTip.Text = string.Empty;
        }
    }

    private void CalculateTotal()
    {
        // Total Tip
        var totalTip = Convert.ToDecimal((bill * tip) / 100) * 1.00m;

        // Tip by Person
        var tipByPerson = Convert.ToDecimal((totalTip / noPerson)) * 1.00m;
        lblTip.Text = $"{tipByPerson:F2}";

        // sub total
        var subTotal = Convert.ToDecimal((bill / noPerson)) * 1.00m;
        lblSubTotal.Text = $"{subTotal:F2}";

        //total
        var totalByPerson = Convert.ToDecimal((bill + totalTip) / noPerson) * 1.00m;
        lblTotal.Text = $"{totalByPerson:F2}";
    }
    private void Button_Clicked(object sender, EventArgs e)
    {
        if (sender is Button)
        {
            var btn = (Button)sender;
            var percentage = int.Parse(btn.Text.Replace("%", ""));
            sldTip.Value = percentage;
        }
    }

    private void sldTip_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        tip = (int)sldTip.Value;
        lblTipPercentage.Text = $"Tip: {tip}%";
        CalculateTotal();
    }

    private void btnMinus_Clicked(object sender, EventArgs e)
    {
        if (noPerson > 1)
        {
            noPerson--;
            lblNoPerson.Text = noPerson.ToString();
            CalculateTotal();
        }
    }

    private void btnPlus_Clicked(object sender, EventArgs e)
    {
        noPerson++;
        lblNoPerson.Text = noPerson.ToString();
        CalculateTotal();
    }

    private void BtnSlider_Clicked(object sender, EventArgs e)
    {
        ShowTipSection("Slider");
    }

    private void BtnTipOptions_Clicked(object sender, EventArgs e)
    {
        ShowTipSection("Tip Options");
    }

    private void BtnCustomTip_Clicked(object sender, EventArgs e)
    {
        ShowTipSection("Custom Tip");
    }

    private void ShowTipSection(string selectedMethod)
    {
        txtCustomTip.Text = string.Empty;
        sldTip.Value = 0;
        lblTipPercentage.Text = "Tip: 0%";

        tipOptionsLayout.IsVisible = selectedMethod == "Tip Options";
        sldTip.IsVisible = selectedMethod == "Slider";
        txtCustomTip.IsVisible = selectedMethod == "Custom Tip";
    }
}

