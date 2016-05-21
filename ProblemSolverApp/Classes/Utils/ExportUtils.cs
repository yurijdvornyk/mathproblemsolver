﻿using ProblemDevelopmentKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolverApp.Classes.Utils
{
    public class ExportUtils
    {
        public static string WatermarkText { get { return "Generated by MathProblemSolver application"; } }

        public static string getTexMarkupForProblem(IProblem problem, bool showLegend)
        {
            StringBuilder fileContent = new StringBuilder();
            writeTexHeader(fileContent, showLegend);

            fileContent.AppendLine(@"\definecolor{light-gray}{gray}{0.5}\pagenumbering{gobble}");

            fileContent.AppendLine(@"\begin{document}");
            fileContent.AppendLine(@"\begin{center}");

            writeHeader(fileContent, problem);
            writeInputData(fileContent, problem);
            writeEmptyEquation(fileContent);
            writePlot(problem, fileContent, showLegend);
            writeEmptyEquation(fileContent);
            writeWatermark(fileContent);
            fileContent.AppendLine(@"\end{center}");
            fileContent.AppendLine(@"\end{document}");
            return fileContent.ToString();
        }

        private static void writeTexHeader(StringBuilder text, bool showLegend)
        {
            if (text == null)
            {
                text = new StringBuilder();
            }

            text.AppendLine(@"\documentclass[a4paper]{article}");
            text.AppendLine(@"\usepackage[utf8]{inputenc}");
            text.AppendLine(@"\usepackage[left = 3cm, right = 3cm, top = 3cm, bottom = 3cm]{geometry}");
            text.AppendLine(@"\usepackage[english]{babel}");
            text.AppendLine(@"\usepackage[T1,T2A]{fontenc}");
            text.AppendLine(@"\usepackage{amssymb,amsmath}");
            text.AppendLine(@"\usepackage{tikz}");
            text.AppendLine(@"\usepackage{pgfplots}");
            text.AppendLine(@"\usepackage{color}");
        }

        private static void writeHeader(StringBuilder text, IProblem problem)
        {
            text.AppendLine(@"\begin{LARGE}" + "\n" + problem.Name + "\n" + @"\end{LARGE}" + "\n");
            text.AppendLine(@"\begin{equation*}" + problem.Equation + @"\end{equation*}");
        }

        private static void writeInputData(StringBuilder text, IProblem problem)
        {
            text.AppendLine("\n" + @"\begin{large}Input data:\end{large}");
            text.AppendLine("\n" + @"\begin{tabular}{l l l}" + "\nParameter & Data Type & Value" + @" \\ \hline");
            foreach (var item in problem.InputData)
            {
                text.AppendLine(@"\texttt{" + item.Name + @"} & \texttt{" + item.Type + @"} & \texttt{" + item.Value + @"} \\");
            }
            text.AppendLine(@"\hline");
            text.AppendLine(@"\end{tabular}" + "\n");
        }

        private static void writePlot(IProblem problem, StringBuilder text, bool showLegend)
        {
            if (text == null)
            {
                text = new StringBuilder();
            }

            text.AppendLine("\n" + @"\begin{large}Problem plot:\end{large}" + "\n");

            text.AppendLine(@"\begin{tikzpicture}");
            text.Append(@"\begin{axis}[axis x line=bottom, axis y line=left,");
            if (!string.IsNullOrEmpty(problem.Result.VisualTitleKey) && !string.IsNullOrEmpty(problem.Result.VisualTitleValue))
            {
                text.Append("xlabel=$" + problem.Result.VisualTitleKey + "$, ylabel=$" + problem.Result.VisualValues + "$,");
            }
            text.Append(" legend pos= north east]\n");

            int titlesCount = 0;

            foreach (var plot in problem.Result.VisualValues)
            {
                text.AppendLine(@"\addplot[mark=none,black,thick] coordinates {");
                for (int i = 0; i < plot.Keys.Length; ++i)
                {
                    text.Append("(" + plot.Keys[i].ToString().Replace(",", ".") + ", " + plot.Values[i].ToString().Replace(",", ".") + ")");

                }
                text.Append("};\n");

                if (!string.IsNullOrEmpty(plot.Title))
                {
                    ++titlesCount;
                }
            }
            if (titlesCount == problem.Result.VisualValues.Count && showLegend)
            {
                text.Append(@"\legend{");
                for (int i = 0; i < problem.Result.VisualValues.Count; ++i)
                {
                    text.Append("$" + problem.Result.VisualValues[i].Title.Replace(" ", "") + "$");
                    if (i < problem.Result.VisualValues.Count - 1)
                    {
                        text.Append(",");
                    }
                }
                text.Append("};\n");
            }
            text.AppendLine(@"\end{axis}");
            text.AppendLine(@"\end{tikzpicture}");
        }

        private static void writeWatermark(StringBuilder text)
        {
            text.AppendLine("\n" + @"\textcolor{light-gray}{" + WatermarkText + "}");
            text.AppendLine("\n" + @"\textcolor{light-gray}{" + DateTime.Today.Date.ToShortDateString() + "}");
        }

        private static void writeEmptyEquation(StringBuilder text)
        {
            text.AppendLine("\n" + @"\begin{equation*}\end{equation*}" + "\n");
        }
    }
}