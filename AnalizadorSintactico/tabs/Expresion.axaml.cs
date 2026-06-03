using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using System.Linq;

namespace AnalizadorSintactico.Tabs;

public partial class Expresion : UserControl
{
	public Expresion()
	{
		InitializeComponent();
	}

	void OnInput(object sender, RoutedEventArgs e)
	{
		if (sender is Button button && button.Content is not null)
		{
			FindMainWindow()?.Input(button.Content.ToString()!);
		}
	}

	void OnOperator(object sender, RoutedEventArgs e)
	{
		if (sender is Button button && button.Content is not null)
		{
			FindMainWindow()?.Operator(button.Content.ToString()!);
		}
	}

	void OnClear(object sender, RoutedEventArgs e)
	{
		FindMainWindow()?.Clear();
	}

	void OnBackspace(object sender, RoutedEventArgs e)
	{
		FindMainWindow()?.Backspace();
	}

	void OnEquals(object sender, RoutedEventArgs e)
	{
		FindMainWindow()?.Equals();
	}

	MainWindow? FindMainWindow()
	{
		return this.GetVisualAncestors().OfType<MainWindow>().FirstOrDefault();
	}
}
