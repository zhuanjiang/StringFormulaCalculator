using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringFormulaCalculator
{
    public class CalculationEngine
    {
        // EventHandler
        public event EventHandler<DisplayFormulaEventArgs> FormulaUpdated;

        // Main Function to Call
        public double StartCalculate(string formula)
        {
            return BracketRecognition(formula);
        }

        // String Formula Detection Function
        private double BracketRecognition(string formula)
        {
            string removedWhiteSpaceFormula = formula.Replace(" ", string.Empty);

            OnFormulaUpdated(removedWhiteSpaceFormula);

            while (removedWhiteSpaceFormula.LastIndexOf('(') != -1)
            {
                int startIndex = removedWhiteSpaceFormula.LastIndexOf('(');
                int endIndex = removedWhiteSpaceFormula.IndexOf(')');
                int length = endIndex - startIndex + 1;
                string toCalculateFormula = removedWhiteSpaceFormula.Substring(startIndex, length);
                double calResult = BasicMathRecognition(toCalculateFormula);
                removedWhiteSpaceFormula = removedWhiteSpaceFormula.Replace(toCalculateFormula, calResult.ToString());
                OnFormulaUpdated(removedWhiteSpaceFormula);
            }

            return BasicMathRecognition(removedWhiteSpaceFormula);
        }


        private double BasicMathRecognition(string formula)
        {
            string removedWhiteSpaceFormula = formula.Replace(" ", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty);

            // Handle Division and Multiplication
            char[] handleDivAndMulSeparator = { '+', '-' };
            string[] splitByOtherThanDivAndMul = removedWhiteSpaceFormula.Split(handleDivAndMulSeparator, StringSplitOptions.RemoveEmptyEntries);
            
            foreach(var toCal in splitByOtherThanDivAndMul)
            {
                if (toCal.Contains('/') || toCal.Contains('*'))
                {
                    double divResult = DivisionAndMultiplicationOrderHandler(toCal);
                    removedWhiteSpaceFormula = removedWhiteSpaceFormula.Replace(toCal, divResult.ToString());
                }
            }

            // Handle Addition and Subtraction
            char[] handleAddAndSubSeparator = { '/', '*' };
            string[] splitByOtherThanAddAndSub = removedWhiteSpaceFormula.Split(handleAddAndSubSeparator, StringSplitOptions.RemoveEmptyEntries);

            foreach (var toCal in splitByOtherThanAddAndSub)
            {
                if (toCal.Contains('+') || toCal.Contains('-'))
                {
                    double divResult = AdditionAndSubtractionOrderHandler(toCal);
                    removedWhiteSpaceFormula = removedWhiteSpaceFormula.Replace(toCal, divResult.ToString());
                }
            }

            return double.Parse(removedWhiteSpaceFormula);
        }


        // Event
        protected virtual void OnFormulaUpdated(string updatedFormula)
        {
            DisplayFormulaEventArgs e = new DisplayFormulaEventArgs();
            e.Formula = updatedFormula;

            FormulaUpdated?.Invoke(this, e);
        }


        // Math handler
        private double DivisionAndMultiplicationOrderHandler(string divAndMulFormula)
        {
            List<string> divAndMulOrders = new List<string>();
            char[] divAndMul = { '/', '*' };
            string toCalculateFormula = divAndMulFormula;

            // Collect Division and Multiplication Order
            while (toCalculateFormula.IndexOfAny(divAndMul) != -1)
            {
                int charIndex = toCalculateFormula.IndexOfAny(divAndMul);
                divAndMulOrders.Add(toCalculateFormula.Substring(charIndex, 1));
                toCalculateFormula = toCalculateFormula.Substring(charIndex + 1);
            }

            // Calculate based on Order
            string[] splitNumbers = divAndMulFormula.Split(divAndMul, StringSplitOptions.RemoveEmptyEntries);
            double previousVal = 0;

            for(int n = 0; n < divAndMulOrders.Count; n++)
            {
                double a, b = 0;
                if (n == 0)
                {
                    a = double.Parse(splitNumbers[n]);
                    b = double.Parse(splitNumbers[n + 1]);
                }
                else
                {
                    a = previousVal;
                    b = double.Parse(splitNumbers[n + 1]);
                }

                if (divAndMulOrders[n].Equals("/"))
                {
                    previousVal = Division(a, b);
                }
                else if (divAndMulOrders[n].Equals("*"))
                {
                    previousVal = Multiplication(a, b);
                }
            }

            return previousVal;
        }

        private double AdditionAndSubtractionOrderHandler(string addAndSubFormula)
        {
            List<string> addAndSubOrders = new List<string>();
            char[] addAndSub = { '+', '-' };
            string toCalculateFormula = addAndSubFormula;

            // Collect Addition and Subtraction Order
            while (toCalculateFormula.IndexOfAny(addAndSub) != -1)
            {
                int charIndex = toCalculateFormula.IndexOfAny(addAndSub);
                addAndSubOrders.Add(toCalculateFormula.Substring(charIndex, 1));
                toCalculateFormula = toCalculateFormula.Substring(charIndex + 1);
            }

            // Calculate based on Order
            string[] splitNumbers = addAndSubFormula.Split(addAndSub, StringSplitOptions.RemoveEmptyEntries);
            double previousVal = 0;

            for (int n = 0; n < addAndSubOrders.Count; n++)
            {
                double a, b = 0;
                if (n == 0)
                {
                    a = double.Parse(splitNumbers[n]);
                    b = double.Parse(splitNumbers[n + 1]);
                }
                else
                {
                    a = previousVal;
                    b = double.Parse(splitNumbers[n + 1]);
                }

                if (addAndSubOrders[n].Equals("+"))
                {
                    previousVal = Addition(a, b);
                }
                else if (addAndSubOrders[n].Equals("-"))
                {
                    previousVal = Subtraction(a, b);
                }
            }

            return previousVal;
        }

        // Basic Math Method
        private double Addition(double a, double b)
        {
            return a + b;
        }

        private double Subtraction(double a, double b)
        {
            return a - b;
        }

        private double Multiplication(double a, double b)
        {
            return a * b;
        }

        private double Division(double a, double b)
        {
            return a / b;
        }

        // Math Function
        private double Absolute(double a)
        {
            return Math.Abs(a);
        }

        private double BankerRounding(double a, int toDigit)
        {
            return Math.Round(a, toDigit);
        }

        private double NormalRounding(double a, int toDigit)
        {
            return Math.Round(a, toDigit, MidpointRounding.AwayFromZero);
        }
    }
}
