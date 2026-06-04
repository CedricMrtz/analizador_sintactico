using Avalonia.Controls;
using System.Collections.ObjectModel;

namespace AnalizadorSintactico.Tabs;

public partial class ArbolDerivacion : UserControl
{
    public class Nodo
    {
        public string Nombre { get; set; } = "";

        public ObservableCollection<Nodo> Hijos { get; } = new();

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

        if (string.IsNullOrWhiteSpace(expresion))
        {
            return;
        }

        var builder = new TreeBuilder(expresion);
        if (builder.TryBuild(out var raiz))
        {
            var expresionRoot = new Nodo("Expresión");
            expresionRoot.Hijos.Add(raiz);
            SharedRoot.Add(expresionRoot);
        }
    }

    private static Nodo CreateNode(string nombre, params Nodo[] hijos)
    {
        var nodo = new Nodo(nombre);
        foreach (var hijo in hijos)
        {
            nodo.Hijos.Add(hijo);
        }

        return nodo;
    }

    private sealed class TreeBuilder
    {
        private readonly string input;
        private int pos;

        public TreeBuilder(string input)
        {
            this.input = input;
        }

        public bool TryBuild(out Nodo root)
        {
            var start = pos;

            if (ParseS(out root) && pos == input.Length)
            {
                return true;
            }

            pos = start;
            root = new Nodo("Expresión inválida");
            return false;
        }

        private char Current => pos < input.Length ? input[pos] : '\0';

        private bool Consume(char value)
        {
            if (Current == value)
            {
                pos++;
                return true;
            }

            return false;
        }

        private bool ParseS(out Nodo node)
        {
            var start = pos;

            if (ParseC(out var c) && ParseA(out var a))
            {
                node = CreateNode("S → CA", c, a);
                return true;
            }

            pos = start;
            node = null!;
            return false;
        }

        private bool ParseA(out Nodo node)
        {
            var start = pos;

            if (Consume('+') && ParseC(out var suma) && ParseA(out var colaSuma))
            {
                node = CreateNode("A → +CA", new Nodo("+"), suma, colaSuma);
                return true;
            }

            pos = start;

            if (Consume('-') && ParseC(out var resta) && ParseA(out var colaResta))
            {
                node = CreateNode("A → -CA", new Nodo("-"), resta, colaResta);
                return true;
            }

            pos = start;
            node = CreateNode("A → ε", new Nodo("ε"));
            return true;
        }

        private bool ParseC(out Nodo node)
        {
            var start = pos;

            if (ParseP(out var p) && ParseB(out var b))
            {
                node = CreateNode("C → PB", p, b);
                return true;
            }

            pos = start;
            node = null!;
            return false;
        }

        private bool ParseB(out Nodo node)
        {
            var start = pos;

            if (Consume('*') && ParseP(out var multiplicacion) && ParseB(out var colaMultiplicacion))
            {
                node = CreateNode("B → *PB", new Nodo("*"), multiplicacion, colaMultiplicacion);
                return true;
            }

            pos = start;

            if (Consume('/') && ParseP(out var division) && ParseB(out var colaDivision))
            {
                node = CreateNode("B → /PB", new Nodo("/"), division, colaDivision);
                return true;
            }

            pos = start;
            node = CreateNode("B → ε", new Nodo("ε"));
            return true;
        }

        private bool ParseP(out Nodo node)
        {
            var start = pos;

            if (ParseE(out var baseExpresion) && Consume('^') && ParseP(out var potencia))
            {
                node = CreateNode("P → E^P", baseExpresion, new Nodo("^"), potencia);
                return true;
            }

            pos = start;

            if (ParseE(out var expresionSimple))
            {
                node = CreateNode("P → E", expresionSimple);
                return true;
            }

            pos = start;
            node = null!;
            return false;
        }

        private bool ParseE(out Nodo node)
        {
            var start = pos;

            if (Consume('-') && ParseF(out var negado))
            {
                node = CreateNode("E → -F", new Nodo("-"), negado);
                return true;
            }

            pos = start;

            if (ParseF(out var simple))
            {
                node = CreateNode("E → F", simple);
                return true;
            }

            pos = start;
            node = null!;
            return false;
        }

        private bool ParseF(out Nodo node)
        {
            var start = pos;

            if (Consume('(') && ParseS(out var expresion) && Consume(')'))
            {
                node = CreateNode("F → (S)", new Nodo("("), expresion, new Nodo(")"));
                return true;
            }

            pos = start;

            if (ParseN(out var numero))
            {
                node = CreateNode("F → N", numero);
                return true;
            }

            pos = start;
            node = null!;
            return false;
        }

        private bool ParseN(out Nodo node)
        {
            var start = pos;

            if (!char.IsDigit(Current))
            {
                node = null!;
                return false;
            }

            while (char.IsDigit(Current))
            {
                pos++;
            }

            if (Current == '.')
            {
                pos++;

                if (!char.IsDigit(Current))
                {
                    pos = start;
                    node = null!;
                    return false;
                }

                while (char.IsDigit(Current))
                {
                    pos++;
                }
            }

            node = CreateNode($"N → {input[start..pos]}");
            return true;
        }
    }
}