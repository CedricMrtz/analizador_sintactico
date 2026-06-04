using Avalonia.Controls;
using AnalizadorSintactico;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AnalizadorSintactico.Tabs;

public partial class TablaDerivacion : UserControl
{
    private static readonly ObservableCollection<DerivationStep> SharedSteps = new();
    public ObservableCollection<DerivationStep> Steps => SharedSteps;

    public TablaDerivacion()
    {
        InitializeComponent();
        DataContext = this;
    }

    public static void SetSteps(IEnumerable<DerivationStep> steps)
    {
        SharedSteps.Clear();
        foreach (var step in steps)
            SharedSteps.Add(step);
    }
}