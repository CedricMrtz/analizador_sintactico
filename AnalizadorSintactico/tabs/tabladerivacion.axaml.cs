using Avalonia.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AnalizadorSintactico.Tabs;

public partial class TablaDerivacion : UserControl
{
	private static readonly ObservableCollection<string> SharedTokens = new();

	public ObservableCollection<string> Tokens => SharedTokens;

	public TablaDerivacion()
	{
		InitializeComponent();
		DataContext = this;
	}

	public static void SetTokens(IEnumerable<string> tokens)
	{
		SharedTokens.Clear();
		foreach (var token in tokens)
		{
			SharedTokens.Add(token);
		}
	}
}
