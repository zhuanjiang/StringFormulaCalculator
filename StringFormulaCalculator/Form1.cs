using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StringFormulaCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CalculationEngine.FormulaUpdated += onFormulaUpdate;
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            rtbResult.Text = string.Empty;
            double result = CalculationEngine.Calculate(txtStringParameter.Text);
            rtbResult.AppendText("Answer is " + result.ToString());
        }

        private void onFormulaUpdate(object sender, DisplayFormulaEventArgs e)
        {
            rtbResult.AppendText(e.Formula + "\n");
        }
    }
}
