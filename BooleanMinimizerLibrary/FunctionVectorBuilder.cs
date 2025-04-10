namespace BooleanMinimizerLibrary
{
    public class FunctionVectorBuilder
    {
        public string BuildVector(Node root)
        {
            if (root.Type == NodeType.Vector)
                return root.Value;

            var variables = GetVariables(root).OrderBy(v => v).ToList();
            int n = variables.Count;
            int vectorLength = 1 << n; // 2^n
            var resultVector = "";

            for (int mask = 0; mask < vectorLength; mask++)
            {
                var variableValues = new Dictionary<string, bool>();

                for (int i = 0; i < n; i++)
                    variableValues[variables[i]] = ((mask >> (n - i - 1)) & 1) == 1;

                bool value = Evaluate(root, variableValues);
                resultVector += value ? "1" : "0";
            }

            return resultVector;
        }

        private HashSet<string> GetVariables(Node node)
        {
            var variables = new HashSet<string>();

            if (node == null)
                return variables;

            if (node.Type == NodeType.Variable)
                variables.Add(node.Value);

            foreach (var child in new[] { node.Left, node.Right })
                if (child != null)
                    variables.UnionWith(GetVariables(child));

            return variables;
        }

        private bool Evaluate(Node node, Dictionary<string, bool> variables)
        {
            return node.Type switch
            {
                NodeType.Variable => variables[node.Value],
                NodeType.Constant => node.Value == "1",
                NodeType.Not => !Evaluate(node.Right, variables),
                NodeType.And => Evaluate(node.Left, variables) && Evaluate(node.Right, variables),
                NodeType.Or => Evaluate(node.Left, variables) || Evaluate(node.Right, variables),
                NodeType.Xor => Evaluate(node.Left, variables) ^ Evaluate(node.Right, variables),
                NodeType.Implies => !Evaluate(node.Left, variables) || Evaluate(node.Right, variables),
                NodeType.Equivalent => Evaluate(node.Left, variables) == Evaluate(node.Right, variables),
                _ => throw new Exception("Неизвестный тип узла")
            };
        }
    }
}
