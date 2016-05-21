﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolverApp.Classes
{
    public class ProblemExporter
    {
        public string WatermarkText { get { return "Generated by MathProblemSolver application"; } }

        public void SaveToTex(ProblemItem problem, string filename)
        {
            StringBuilder fileContent = new StringBuilder();

            // documentclass
            // usepackage

            fileContent.Append("\\documentclass[a4paper]{article}\\usepackage[utf8]{inputenc}" +
                "\\usepackage[left=3cm,right=3cm,top=3cm,bottom=3cm]{geometry}\\usepackage[english]{babel}" +
                "\\usepackage[T1,T2A]{fontenc}\\usepackage{amssymb,amsmath}\\usepackage{tikz}\\usepackage{pgfplots}" +
                "\\usepackage{color}\n\n");

            fileContent.Append("\\definecolor{light-gray}{gray}{0.5}\\pagenumbering{gobble}\n\n");

            // begin document
            // header

            fileContent.Append("\\begin{document}\\center\\textcolor{light-gray}{" + WatermarkText +
                "\\linebreak" + DateTime.Today.Date.ToShortDateString() + "}\n\n\\vspace{15pt}\n\n");

            // title
            // equation

            fileContent.Append("\\begin{LARGE}\\textbf{" + problem.Problem.Name + "}\\end{LARGE}\n\n\\begin{equation*}" +
                problem.Problem.Equation + "\\end{equation*}\n\n\\vspace{20pt}\n\n");

            // input data
            fileContent.Append("\\begin{large}Input data:\\end{large}\n\n\\vspace{10pt}\n\n" +
                "\\begin{tabular}{l l l}\nParameter & Data Type & Value" + @"\\ \hline");
            foreach (var i in problem.Problem.InputData)
            {
                switch (i.Type)
                {
                    case ProblemDevelopmentKit.ProblemDataItemType.Function:
                        fileContent.Append("\n$" + i.Name + "$ & \\texttt{" + i.Type + "} & \\texttt{" + i.Value + @"} \\");
                        break;

                    default:
                        fileContent.Append("\n" + i.Name + " & \\texttt{" + i.Type + "} & \\texttt{" + i.Value + @"} \\");
                        break;
                }
            }

            fileContent.Append("\n\\hline\\end{tabular}\n\n\\vspace{20pt}\n\n");

            fileContent.Append("\\begin{large}Plot:\\end{large}\n\n\\vspace{20pt}");

            // plot
            var title = new string[] { problem.Problem.Result.VisualTitleKey, problem.Problem.Result.VisualTitleValue };
            fileContent.Append(@"\begin{tikzpicture}\begin{axis}[width=\textwidth,xlabel=" + title[0].ToString() +
                ",ylabel=" + title[1].ToString() + "]");

            foreach (var plot in problem.Problem.Result.VisualValues)
            {
                fileContent.Append(@"\addplot[black] coordinates {");
                // TODO: Check if Keys and Values have the same length
                for (int i = 0; i < plot.Keys.Length; ++i)
                {
                    fileContent.Append("(" + plot.Keys[i].ToString().Replace(",", ".") + "," + plot.Values[i].ToString().Replace(",", ".") + ")");
                }
            }

            //var values = ((object[,])problem.Problem.Result.VisualValues[0].);

            //fileContent.Append("(-2.5,2.5) (-1,1) (0,0) (1,1) (2.5,2.5)");

            fileContent.Append("};\n\\end{axis}\\end{tikzpicture}");

            // end
            fileContent.Append("\n\\end{document}");

            using (StreamWriter outfile = new StreamWriter(filename))
            {
                outfile.Write(fileContent.ToString());
            }
        }
    }
}
