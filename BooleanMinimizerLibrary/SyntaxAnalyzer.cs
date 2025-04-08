namespace BooleanMinimizerLibrary
{
    public class SyntaxAnalyzer
    {
        private string expr;
        private char currentChar;
        private int pos;

        public bool Parse(string input)
        {
            
            // Проверка на пустой ввод
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Ошибка: Ввод не может быть пустым.");
            }
            
            expr = input;
            pos = 0;
            NextChar();
            Expression();
            if (currentChar != '\0')
                ThrowError("лишний(е) символы в конце выражения");

            return true;
        }

        private void NextChar()
        {
            currentChar = (pos < expr.Length) ? expr[pos++] : '\0';
        }

        private void ThrowError(string message)
        {
            throw new ArgumentException($"Ошибка на символе № {pos}: {message}");
        }

        private void Expression()
        {
            Implies();
            while (currentChar == '↔')
            {
                NextChar();
                Implies();
            }
        }

        private void Implies()
        {
            Xor();
            while (currentChar == '→')
            {
                NextChar();
                Xor();
            }
        }

        private void Xor()
        {
            Or();
            while (currentChar == '⊕')
            {
                NextChar();
                Or();
            }
        }

        private void Or()
        {
            And();
            while (currentChar == '∨')
            {
                NextChar();
                And();
            }
        }

        private void And()
        {
            Not();
            while (currentChar == '∧')
            {
                NextChar();
                Not();
            }
        }

        private void Not()
        {
            if (currentChar == '¬')
            {
                NextChar();
                Not();
            }
            else
            {
                Primary();
            }
        }

        private void Primary()
        {
            if (IsVariable(currentChar))
            {
                NextChar();
            }
            else if (currentChar == '0' || currentChar == '1')
            {
                if (IsVector())
                    Vector();
                else
                    NextChar();
            }
            else if (currentChar == '(')
            {
                NextChar();
                Expression();
                if (currentChar != ')')
                    ThrowError("Ожидалось ')'");
                NextChar();
            }
            else
            {
                ThrowError($"Неожиданный символ");
            }
        }

        private bool IsVariable(char ch)
        {
            return ch == 'w' || ch == 'x' || ch == 'y' || ch == 'z';
        }

        private bool IsVector()
        {
            int tempPos = pos;
            int length = 1; // считаем текущий символ

            while (tempPos < expr.Length && (expr[tempPos] == '0' || expr[tempPos] == '1'))
            {
                tempPos++;
                length++;
            }

            return length >= 2;
        }

        private void Vector()
        {
            int length = 1; // уже есть первый символ (currentChar == '0' или '1')

            while (currentChar == '0' || currentChar == '1')
            {
                NextChar();
                length++;
            }

            length--; // убираем лишний +1 после последнего NextChar()

            if (!IsPowerOfTwo(length))
                ThrowError("Вектор должен быть длины степени два (1, 2, 4, 8, 16, ...)");
        }

        private bool IsPowerOfTwo(int n)
        {
            return n > 0 && (n & (n - 1)) == 0;
        }
    }
}