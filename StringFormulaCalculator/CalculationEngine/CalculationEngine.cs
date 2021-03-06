﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringFormulaCalculator
{
    public class CalculationEngine
    {
        // EventHandler
        public static event EventHandler<DisplayFormulaEventArgs> FormulaUpdated;

        // Main Function to Call
        public static double Calculate(string sum)
        {
            return BracketRecognition(sum);
        }

        // String Formula Detection Function
        private static double BracketRecognition(string formula)
        {
            string removedWhiteSpaceFormula = formula.Replace(" ", string.Empty);

            OnFormulaUpdated(removedWhiteSpaceFormula);

            while (removedWhiteSpaceFormula.LastIndexOf('(') != -1)
            {
                int startIndex = removedWhiteSpaceFormula.LastIndexOf('(');
                int endIndex = removedWhiteSpaceFormula.IndexOf(')', startIndex);
                int length = endIndex - startIndex + 1;
                string toCalculateFormula = removedWhiteSpaceFormula.Substring(startIndex, length);
                double calResult = BasicMathRecognition(toCalculateFormula);
                removedWhiteSpaceFormula = removedWhiteSpaceFormula.Replace(toCalculateFormula, calResult.ToString());
                OnFormulaUpdated(removedWhiteSpaceFormula);
            }

            return BasicMathRecognition(removedWhiteSpaceFormula);
        }


        private static double BasicMathRecognition(string formula)
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
        private static void OnFormulaUpdated(string updatedFormula)
        {
            DisplayFormulaEventArgs e = new DisplayFormulaEventArgs();
            e.Formula = updatedFormula;

            EventHandler<DisplayFormulaEventArgs> handler = FormulaUpdated;
            if (handler != null)
            {
                handler(null, e);
            }
        }


        // Math handler
        private static double DivisionAndMultiplicationOrderHandler(string divAndMulFormula)
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

            for (int n = 0; n < divAndMulOrders.Count; n++)
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

            if (divAndMulOrders.Count == 0)
            {
                return double.Parse(divAndMulFormula);
            }
            else
            {
                return previousVal;
            }
        }

        private static double AdditionAndSubtractionOrderHandler(string addAndSubFormula)
        {
            List<string> addAndSubOrders = new List<string>();
            char[] addAndSub = { '+', '-' };
            string toCalculateFormula = addAndSubFormula;

            // Negative and Positive Handling
            toCalculateFormula = toCalculateFormula.Replace("+-", "-").Replace("-+", "-").Replace("--", "+");

            // First Negative Handling
            StringBuilder sb = new StringBuilder(toCalculateFormula);
            if (sb[0] == '-')
            {
                sb[0] = 'N';
            }
            toCalculateFormula = sb.ToString();

            // Collect Addition and Subtraction Order
            string getFormulaOrder = toCalculateFormula;
            while (getFormulaOrder.IndexOfAny(addAndSub) != -1)
            {
                int charIndex = getFormulaOrder.IndexOfAny(addAndSub);
                addAndSubOrders.Add(getFormulaOrder.Substring(charIndex, 1));
                getFormulaOrder = getFormulaOrder.Substring(charIndex + 1);
            }

            // Calculate based on Order
            string[] splitNumbers = toCalculateFormula.Split(addAndSub, StringSplitOptions.RemoveEmptyEntries);
            double previousVal = 0;

            for (int n = 0; n < addAndSubOrders.Count; n++)
            {
                double a, b = 0;
                if (n == 0)
                {
                    a = double.Parse(splitNumbers[n].Replace("N", "-"));
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

            if(addAndSubOrders.Count == 0)
            {
                return double.Parse(addAndSubFormula);
            }
            else
            {
                return previousVal;
            }
        }

        // Basic Math Method
        private static double Addition(double a, double b)
        {
            return a + b;
        }

        private static double Subtraction(double a, double b)
        {
            return a - b;
        }

        private static double Multiplication(double a, double b)
        {
            return a * b;
        }

        private static double Division(double a, double b)
        {
            return a / b;
        }
    }
}
