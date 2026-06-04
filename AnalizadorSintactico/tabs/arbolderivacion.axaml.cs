using Avalonia.Controls;
using System.Collections.ObjectModel;

namespace AnalizadorSintactico.Tabs;

public partial class ArbolDerivacion : UserControl
{
    public class Nodo
    {
        public string Nombre { get; set; }

        public ObservableCollection<Nodo> Hijos { get; set; }
            = new();

        public Nodo(string nombre)
        {
            Nombre = nombre;
        }
    }

    private static readonly ObservableCollection<Nodo> SharedRoot
        = new();

    public ObservableCollection<Nodo> RootNodes => SharedRoot;

    public ArbolDerivacion()
    {
        InitializeComponent();
        DataContext = this;
    }

    public static void GenerarArbol(string expresion)
{
    SharedRoot.Clear();

    var raiz = new Nodo("Expresión");

    int i = 0;

    while (i < expresion.Length)
    {
        char c = expresion[i];

        // NUMEROS COMPLETOS
        if (char.IsDigit(c))
        {
            string numero = "";

            while (i < expresion.Length &&
                  (char.IsDigit(expresion[i]) || expresion[i] == '.'))
            {
                numero += expresion[i];
                i++;
            }

            raiz.Hijos.Add(new Nodo($"Número: {numero}"));

            continue;
        }

        // OPERADORES
        raiz.Hijos.Add(new Nodo($"Operador: {c}"));

        i++;
    }

    SharedRoot.Add(raiz);
}
}