using UnityEngine;

namespace MalbersAnimations.Scriptables
{
    ///<summary>  Float Scriptable Variable. Based on the Talk - Game Architecture with Scriptable Objects by Ryan Hipple  </summary>
    [CreateAssetMenu(menuName = "Malbers Animations/Variables/Float Formula", order = 1000)]
    public class FloatFormula : FloatVar
    {
        public FloatOperation[] values;

        /// <summary>Value of the Float Scriptable variable </summary>
        public override float Value
        {
            get
            {
                var result = value;

                foreach (var v in values)
                    result = v.GetResult(result);

                if (debug) Debug.Log($"<B>{name} -> [<color=red> {result} </color>] </B>", this);

                return result;
            }
        }

        

        private void OnValidate()
        {
            var displayOld = $"{value}";
            foreach (var item in values)
            {
                item.display = displayOld + $" {item.GetOperation()} {item.value.Value}";

                displayOld = item.display;
            }
        }

        [System.Serializable]
        public class FloatOperation
        {
            [HideInInspector] public string display;
            public FloatReference value = new();
            public MathOperation operation = MathOperation.Add;

            public float GetResult(float MainValue)
            {
                return operation switch
                {
                    MathOperation.Add => MainValue + value,
                    MathOperation.Substract => MainValue - value,
                    MathOperation.Multiply => MainValue * value,
                    MathOperation.Divide => MainValue / value,
                    _ => 0,
                };
            }

            public string GetOperation()
            {
                return operation switch
                {
                    MathOperation.Add => "+",
                    MathOperation.Substract => "-",
                    MathOperation.Multiply => "*",
                    MathOperation.Divide => "/",
                    _ => "",
                };
            }
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects, UnityEditor.CustomEditor(typeof(FloatFormula))]
    public class FloatFormulaEditor : VariableEditor
    {
        public override void OnInspectorGUI() => PaintInspectorGUI("Float Formula (Add extra operations to the Float Var)");

        public override void ExtraValues()
        {
            UnityEditor. EditorGUI.indentLevel++;
            UnityEditor.EditorGUILayout.PropertyField(serializedObject.FindProperty("values"),true);
            UnityEditor.EditorGUI.indentLevel--;
        }
    }
#endif
}