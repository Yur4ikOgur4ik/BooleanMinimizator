using BooleanMinimizerLibrary;
using static BooleanMinimizerLibrary.KarnaughMapBuilder;

namespace BooleanMinimizator.Models
{
    public class BooleanMinimizatorModel
    {
        public string InputFunction { get; set; }  // сюда пользователь вводит функцию

        public string ResultMessage { get; set; }  // сюда выводится результат: успех или текст ошибки

        public string? PolizOutput { get; set; }   // сюда выводится ПОЛИЗ

        public bool IsSolved { get; set; }

        public string? VectorOutput { get; set; }  // сюда выводится вектор функции

        public List<Dictionary<string, bool>> TruthTable { get; set; } // Таблица истинности

        public string FunctionExpression { get; set; } // Новое поле для хранения функции

        public string SKNFOutput { get; set; }      // Совершенная конъюнктивная нормальная форма

        public string SDNFOutput { get; set; }      // Совершенная дизъюнктивная нормальная форма
        
        public string MKNFOutput { get; set; }      // Минимальная конъюнктивная нормальная форма

        public string MDNFOutput { get; set; }      // Минимальная дизъюнктивная нормальная форма

        public List<List<string>> KarnaughMap { get; set; }

        public List<KarnaughStep> KarnaughSteps { get; set; }

        public List<KarnaughMapBuilder.Area> Areas { get; set; }

        public List<Area> ZeroAreas { get; set; }

        public List<string> Variables { get; set; }
        public string GetExpressionForArea(Area area, List<string> variables)
        {
            if (area == null || variables == null || variables.Count == 0 || KarnaughMap == null)
                return "";

            int rowCount = KarnaughMap.Count - 1; // без заголовков
            int colCount = KarnaughMap[0].Count - 1;

            var literals = new List<string>();

            for (int r = area.StartRow; r < area.StartRow + area.Height; r++)
            {
                for (int c = area.StartCol; c < area.StartCol + area.Width; c++)
                {
                    int wrappedRow = ((r % (rowCount - 1)) + 1); // пропуск заголовка строк
                    int wrappedCol = ((c % (colCount - 1)) + 1); // пропуск заголовка столбцов

                    if (KarnaughMap[wrappedRow][wrappedCol] == "1")
                    {
                        for (int i = 0; i < variables.Count; i++)
                        {
                            bool isFixed = false;
                            if (i == 0) // переменная по строкам
                            {
                                isFixed = (r / Math.Pow(2, variables.Count - 1 - i)) % 2 == 1;
                            }
                            else // переменные по столбцам
                            {
                                isFixed = (c / Math.Pow(2, variables.Count - 1 - i)) % 2 == 1;
                            }

                            if (isFixed)
                            {
                                literals.Add(variables[i]);
                            }
                            else
                            {
                                literals.Add($"¬{variables[i]}");
                            }
                        }
                        return $"({string.Join(" ∧ ", literals)})";
                    }
                }
            }

            return "";
        }
        public string GetExpressionForZeroArea(Area area, List<string> variables)
        {
            if (area == null || variables == null || variables.Count == 0 || KarnaughMap == null)
                return "";

            int rowCount = KarnaughMap.Count - 1;
            int colCount = KarnaughMap[0].Count - 1;

            var literals = new List<string>();
            bool found = false;

            for (int r = area.StartRow; r < area.StartRow + area.Height && !found; r++)
            {
                for (int c = area.StartCol; c < area.StartCol + area.Width && !found; c++)
                {
                    int wrappedRow = ((r % (rowCount - 1)) + 1); // Пропуск заголовка строк
                    int wrappedCol = ((c % (colCount - 1)) + 1); // Пропуск заголовка столбцов

                    if (KarnaughMap[wrappedRow][wrappedCol] == "0")
                    {
                        for (int i = 0; i < variables.Count; i++)
                        {
                            bool isFixed = false;
                            if (i == 0) // первая переменная по строкам
                            {
                                isFixed = (r / Math.Pow(2, variables.Count - 1 - i)) % 2 == 1;
                            }
                            else // остальные по столбцам
                            {
                                isFixed = (c / Math.Pow(2, variables.Count - 1 - i)) % 2 == 1;
                            }

                            if (isFixed)
                            {
                                literals.Add(variables[i]);
                            }
                            else
                            {
                                literals.Add($"¬{variables[i]}");
                            }
                        }
                        found = true;
                    }
                }
            }

            return $"({string.Join(" ∨ ", literals)})";
        }
    }
}