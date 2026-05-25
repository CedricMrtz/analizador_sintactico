using Avalonia.Controls;
using Avalonia.Interactivity;

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

    }
}
