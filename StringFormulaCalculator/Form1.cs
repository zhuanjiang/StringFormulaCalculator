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
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            double result = Calculate(txtStringParameter.Text);
            rtbResult.Text = result.ToString();
        }

        public static double Calculate(string sum)
        {
            // Your code starts here
            return 0.0;
        }
    }
}
