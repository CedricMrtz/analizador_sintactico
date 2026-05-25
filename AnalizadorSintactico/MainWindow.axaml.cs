using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Data;

namespace AnalizadorSintactico;

public partial class MainWindow : Window
{
    string expression = "";
    bool isValid = true;

    public MainWindow() => InitializeComponent();

    void OnInput(object sender, RoutedEventArgs e){
      expression += ((Button)sender).Content;
      Display.Text = expression;
      ValidarSintaxis(expression);
    }

    void OnOperator(object sender, RoutedEventArgs e){
      var symbol = ((Button)sender).Content!.ToString()!;
      var op = symbol switch { "÷" => "/", "×" => "*", "−" => "-", _ => symbol };
      expression += op;
      Display.Text = expression;
      ValidarSintaxis(expression);

      char ultimo = expression[^1];
      if ("+-*/^".Contains(ultimo))
      return;
    }

    void OnClear(object sender, RoutedEventArgs e){
      expression = "";
      Display.Text = "0";
      ValidarSintaxis(expression);
    }

    void OnBackspace(object sender, RoutedEventArgs e){
      if (expression.Length == 0) return;
      expression = expression[..^1];
      Display.Text = expression == "" ? "0" : expression;
      ValidarSintaxis(expression);
    }
    
    void ValidarSintaxis(string expression){
      if (string.IsNullOrWhiteSpace(expression)){
        StatusText.Text = "";
        return;
      }

      char ultimo = expression[^1];
      if ("+-*/^(".Contains(ultimo)){
        StatusText.Text = "Expresión incorrecta";
        return;
      }

      try {
        Parser parser = new Parser(expression);
        bool valido = parser.S() && parser.IsAtEnd();
        StatusText.Text = valido ? "Expresión correcta" : "Expresión incorrecta";
      }
      catch {
        StatusText.Text = "Expresión incorrecta";
      }
    }

   void OnEquals(object sender, RoutedEventArgs e){
    Parser parser = new Parser(expression);

    bool valido = parser.S() && parser.IsAtEnd();

    if (!valido)
    {
        Display.Text = "Error";
        StatusText.Text = "Expresión incorrecta";
        return;
    }

    try
    {
        var expr = expression.Replace("^", "*");

        var resultado = new System.Data.DataTable()
            .Compute(expr, null);

        Display.Text = resultado.ToString();

        expression = resultado.ToString()!;
        StatusText.Text = "Expresión correcta";
    }
    catch
    {
        Display.Text = "Error";
    }
}
}
