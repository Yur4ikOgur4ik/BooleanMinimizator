namespace BooleanMinimizator.Models
{
    public class BooleanMinimizatorModel
    {
        public string InputFunction { get; set; }  // сюда пользователь вводит функцию

        public string ResultMessage { get; set; }  // сюда выводится результат: успех или текст ошибки

        public string? PolizOutput { get; set; }   // сюда выводится ПОЛИЗ

        public string? VectorOutput { get; set; }  // сюда выводится вектор функции
    }
}
