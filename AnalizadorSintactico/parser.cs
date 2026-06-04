using System.Collections.Generic;
namespace AnalizadorSintactico;

public class DerivationStep {
    public string Rule { get; set; } = "";
    public string SententialForm { get; set; } = "";
}

public class Parser {
    string input;
    int pos = 0;
    public List<DerivationStep> Steps { get; } = new();
    private string currentForm = "S";

    public Parser(string input) { this.input = input; }

    public bool IsAtEnd() => pos >= input.Length;
    char current() => pos < input.Length ? input[pos] : '\0';

    bool Consume(char c) {
        if (current() == c){ 
            pos++; return true;
        }
        return false;
    }
    bool isDigit() => current() >= '0' && current() <= '9';

    void RecordStep(string nonTerminal, string production) {
        string replacement = production == "ε" ? "" : production;
        int idx = currentForm.IndexOf(nonTerminal);
        if (idx >= 0)
            currentForm = currentForm[..idx] + replacement + currentForm[(idx + nonTerminal.Length)..];
        Steps.Add(new DerivationStep {
            Rule = $"{nonTerminal} → {production}",
            SententialForm = currentForm == "" ? "ε" : currentForm
        });
    }

    void Save(out int p, out int sc, out string sf) {
        p = pos; sc = Steps.Count; sf = currentForm;
    }
    void Restore(int p, int sc, string sf) {
        pos = p;
        if (Steps.Count > sc) Steps.RemoveRange(sc, Steps.Count - sc);
        currentForm = sf;
    }

    // S → CA
    public bool S() {
        Save(out var p, out var sc, out var sf);
        RecordStep("S", "CA");
        if (C() && A()) return true;
        Restore(p, sc, sf);
        return false;
    }

    // A → +CA | -CA | epsilon
    bool A() {
        Save(out var p, out var sc, out var sf);

        RecordStep("A", "+CA");
        if (Consume('+') && C() && A()){
            return true;
        }
        Restore(p, sc, sf);

        RecordStep("A", "-CA");
        if (Consume('-') && C() && A()){
            return true;
        }
        Restore(p, sc, sf);

        RecordStep("A", "ε");
        return true;
    }

    // C → PB
    bool C() {
        Save(out var p, out var sc, out var sf);
        RecordStep("C", "PB");
        if (P() && B()){
            return true;
        }
        Restore(p, sc, sf);
        return false;
    }

    // B → *PB | /PB | ε
    bool B() {
        Save(out var p, out var sc, out var sf);

        RecordStep("B", "*PB");
        if (Consume('*') && P() && B()){
            return true;
        }
        Restore(p, sc, sf);

        RecordStep("B", "/PB");
        if (Consume('/') && P() && B()){
            return true;
        }
        Restore(p, sc, sf);

        RecordStep("B", "ε");
        return true;
    }

    // P → E^P | E
    bool P() {
        Save(out var p, out var sc, out var sf);

        RecordStep("P", "E^P");
        if (E() && Consume('^') && P()){
            return true;
        }
        Restore(p, sc, sf);

        RecordStep("P", "E");
        if (E()){
            return true;
        }
        Restore(p, sc, sf);
        return false;
    }

    // E → -F | F
    bool E() {
        Save(out var p, out var sc, out var sf);

        RecordStep("E", "-F");
        if (Consume('-') && F()){
            return true;
        }
        Restore(p, sc, sf);

        RecordStep("E", "F");
        if (F()){
            return true;
        }
        Restore(p, sc, sf);
        return false;
    }

    // F → (S) | N
    bool F() {
        Save(out var p, out var sc, out var sf);

        RecordStep("F", "(S)");
        if (Consume('(') && S() && Consume(')')){
            return true;
        }
        Restore(p, sc, sf);

        RecordStep("F", "N");
        if (N()){
            return true;
        }
        Restore(p, sc, sf);
        return false;
    }

    // N → DN | D.N | D
    bool N() {
        Save(out var p, out var sc, out var sf);

        RecordStep("N", "D.N");
        if (D() && Consume('.') && N()){
            return true;
        }
        Restore(p, sc, sf);

        RecordStep("N", "DN");
        if (D() && N()){
            return true;
        }
        Restore(p, sc, sf);

        RecordStep("N", "D");
        if (D()){
            return true;
        }
        Restore(p, sc, sf);

        return false;
    }

    // D → 0 | 1 | ... | 9
    bool D() {
        if (!isDigit()){
            return false;
        }
        RecordStep("D", current().ToString());
        pos++;
        return true;
    }
}