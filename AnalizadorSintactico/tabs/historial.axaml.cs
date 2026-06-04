using Avalonia.Controls;
using System.Collections.ObjectModel;

namespace AnalizadorSintactico.Tabs;

public partial class Historial : UserControl
{
	private static readonly ObservableCollection<string> SharedHistory = new();

	public ObservableCollection<string> History => SharedHistory;

	public Historial()
	{
		InitializeComponent();
		DataContext = this;
	}

	public static void AddEntry(string entry)
	{
		SharedHistory.Insert(0, entry);
	}
}
