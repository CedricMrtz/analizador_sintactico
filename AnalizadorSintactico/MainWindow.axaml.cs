using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Data;
using AnalizadorSintactico.Tabs;

namespace AnalizadorSintactico;

public partial class MainWindow : Window
{
    string expression = "";
    bool justCalculated = false;
    bool isValid = true;

    public MainWindow() => InitializeComponent();

    public void Input(string value)
    {
        if (justCalculated)
        {
            expression = "";
            justCalculated = false;
        }

        expression += value;

        Display.Text = expression;

        ValidarSintaxis(expression);
    }

    public void Operator(string symbol)
    {
      var op = symbol switch { "÷" => "/", "x" => "*", "-" => "-", _ => symbol };
      if (justCalculated)
          {
            justCalculated = false;
          }
    expression += op;
      Display.Text = expression;
      ValidarSintaxis(expression);

      char ultimo = expression[^1];
      if ("+-*/^".Contains(ultimo))
      return;
    }

    public void Clear(){
      expression = "";
      Display.Text = "0";
      ValidarSintaxis(expression);
    }

    public void Backspace(){
      if (expression.Length == 0) return;
      expression = expression[..^1];
      Display.Text = expression == "" ? "0" : expression;
      ValidarSintaxis(expression);
    }
    
    void ValidarSintaxis(string expression){
      if (string.IsNullOrWhiteSpace(expression)){
        StatusText.Text = "";
        TablaDerivacion.SetSteps(Array.Empty<DerivationStep>());
        return;
      }

      char ultimo = expression[^1];
      if ("+-*/^(".Contains(ultimo)){
        StatusText.Text = "Expresión incorrecta";
        TablaDerivacion.SetSteps(Array.Empty<DerivationStep>());
        return;
      }

      try {
        Parser parser = new Parser(expression);
        bool valido = parser.S() && parser.IsAtEnd();
        StatusText.Text = valido ? "Expresión correcta" : "Expresión incorrecta";
        TablaDerivacion.SetSteps(valido ? parser.Steps : Array.Empty<DerivationStep>());
      }
      catch {
        StatusText.Text = "Expresión incorrecta";
        TablaDerivacion.SetSteps(Array.Empty<DerivationStep>());
      }
    }

  public void Equals(){
    Parser parser = new Parser(expression);

    bool valido = parser.S() && parser.IsAtEnd();
    TablaDerivacion.SetSteps(valido ? parser.Steps : Array.Empty<DerivationStep>());

    if (!valido)
    {
        Display.Text = "Error";
        StatusText.Text = "Expresión incorrecta";
        return;
    }
        ArbolDerivacion.GenerarArbol(expression);
try
{
    string originalExpression = expression;

    var expr = expression.Replace("^", "*");

    var resultado = new System.Data.DataTable()
        .Compute(expr, null);

    Display.Text = resultado.ToString();

    Historial.AddEntry($"{expression} = {resultado}");

    expression = resultado.ToString()!;

    ArbolDerivacion.GenerarArbol(originalExpression);

    StatusText.Text = "Expresión correcta";

    justCalculated = true;
}
    catch
    {
        Display.Text = "Error";
    }
}
}
