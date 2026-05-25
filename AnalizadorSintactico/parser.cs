namespace AnalizadorSintactico;

class Parser{
  string input;
  int pos = 0;

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
      return true;
    }
    pos = save;
    return false;
  }

  //A -> +CA | -CA | epsilon
  bool A(){
    int save = pos;
    if(Consume('+') && C() && A()){
      return true;
    }
    if(Consume('-') && C() && A()){
      return true;
    }
    pos = save;
    return true; //epsilon
  }
  
  //C -> PB
  bool C(){
    int save = pos;
    if(P() && B()){
      return true;
    }
    pos = save;
    return false;
  }

  //B -> *PB | /PB | epsilon
  bool B(){
    int save = pos;
    if(Consume('*') && P() && B()){
      return true;
    }
    if(Consume('/') && P() && B()){
      return true;
    }
    pos = save;
    return true;
  }
  
  //P -> E^P | E
  bool P(){
    int save = pos;
    if(E() && Consume('^') && P()){
      return true;
    }
    if(E()){
      return true;
    }
    pos = save;
    return false;
  }

  //E -> -F | F
  bool E(){
    int save = pos;
    if(Consume('-') && F()){
      return true;
    }
    if(F()){
      return true;
    }
    pos = save;
    return false;
  }

  //F -> (S) | N
  bool F(){
    int save = pos;
    if(Consume('(') && S() && Consume(')')){
      return true;
    }
    if(N()){
      return true;
    }
    pos = save;
    return false;
  }

  //N -> DN | D | D.N
  bool N(){
    if(!isDigit()) return false;
    while(isDigit()){
      pos++;
    }  
    if(Consume('.')){
      if(!isDigit()) return false;
      while(isDigit()){
        pos++;
      }
    }
    return true;
  }

}
