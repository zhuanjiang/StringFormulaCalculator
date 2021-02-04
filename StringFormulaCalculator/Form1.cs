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
         static CalculationEngine calEngine;

        public Form1()
        {
            InitializeComponent();
            calEngine = new CalculationEngine();
            calEngine.FormulaUpdated += onFormulaUpdate;
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            rtbResult.Text = string.Empty;
            double result = Calculate(txtStringParameter.Text);
            rtbResult.AppendText("Answer is " + result.ToString());
        }

        public static double Calculate(string sum)
        {
            return calEngine.StartCalculate(sum);
        }

        private void onFormulaUpdate(object sender, DisplayFormulaEventArgs e)
        {
            rtbResult.AppendText(e.Formula + "\n");
        }
    }
}
