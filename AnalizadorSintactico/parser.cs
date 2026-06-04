using System.Collections.Generic;
namespace AnalizadorSintactico;

class Parser{
  string input;
  int pos = 0;
  public List<string> Tokens { get; } = new List<string>();

  public Parser(string input){
    this.input = input;
  }

  public bool IsAtEnd() => pos >= input.Length;
  char current() => pos< input.Length ? input[pos] : '\0';

  bool Consume(char c){
    if (current() == c){
      pos++;
      return true;
    }
    return false;
  }

  bool isDigit() => current() >= '0' && current() <= '9';

  //S -> CA
  public bool S(){
    int save = pos;
    if(C() && A()){
      Tokens.Add("S -> CA");
      return true;
    }
    pos = save;
    return false;
  }

  //A -> +CA | -CA | epsilon
  bool A(){
    int save = pos;
    if(Consume('+') && C() && A()){
      Tokens.Add("A -> +CA");
      return true;
    }
    if(Consume('-') && C() && A()){
      Tokens.Add("A -> -CA");
      return true;
    }
    pos = save;
    return true; //epsilon
  }
  
  //C -> PB
  bool C(){
    int save = pos;
    if(P() && B()){
      Tokens.Add("C -> PB");
      return true;
    }
    pos = save;
    return false;
  }

  //B -> *PB | /PB | epsilon
  bool B(){
    int save = pos;
    if(Consume('*') && P() && B()){
      Tokens.Add("B -> *PB");
      return true;
    }
    if(Consume('/') && P() && B()){
      Tokens.Add("B -> /PB");
      return true;
    }
    pos = save;
    return true;
  }
  
//P -> E^P | E
bool P()
{
    int save = pos;

    if (E())
    {
        if (Consume('^'))
        {
            Tokens.Add("P -> E^P");
            return P();
        }

        Tokens.Add("P -> E");
        return true;
    }

    pos = save;
    return false;
}

  //E -> -F | F
  bool E(){
    int save = pos;
    if(Consume('-') && F()){
      Tokens.Add("E -> -F");
      return true;
    }
    if(F()){
      Tokens.Add("E -> F");
      return true;
    }
    pos = save;
    return false;
  }

  //F -> (S) | N
  bool F(){
    int save = pos;
    if(Consume('(') && S() && Consume(')')){
      Tokens.Add("F -> (S)");
      return true;
    }
    if(N()){
      Tokens.Add("F -> N");
      return true;
    }
    pos = save;
    return false;
  }

  //N -> DN | D | D.N
  bool N(){
    int save = pos;

    if(ConsumeDigit()){
      Tokens.Add("N -> D");
      return true;
    }

    if(ConsumeDigit() && N()){
      Tokens.Add("N -> DN");
      return true;
    }

      if(ConsumeDigit() && Consume('.') && ConsumeDigit()){
      Tokens.Add("N -> D.N");
      return true;
    }
    pos = save;
    return false;
  }

    bool ConsumeDigit(){
      if (isDigit()){
        pos++;
        return true;
      }
      return false;
    }

}
