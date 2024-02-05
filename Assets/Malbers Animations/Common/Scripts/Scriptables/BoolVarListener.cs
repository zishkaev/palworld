using MalbersAnimations.Events;
using MalbersAnimations.Scriptables;
using UnityEngine;
using UnityEngine.Events;


namespace MalbersAnimations
{
    [DefaultExecutionOrder(750)]
    [AddComponentMenu("Malbers/Variables/Bool Listener (Local Bool)")]
    [HelpURL("https://malbersanimations.gitbook.io/animal-controller/secondary-components/variable-listeners-and-comparers")]
    public class BoolVarListener : VarListener
    {
        public BoolReference value = new();
        [Tooltip("When the events are invoked the value will be inverted")]
        public bool invert;
        public BoolEvent OnValueChanged = new();
        public UnityEvent OnTrue = new();
        public UnityEvent OnFalse = new();

        public bool Value
        {
            get => value;
            set
            {
                this.value.Value = value;
                if (Auto) Invoke(value);
            }
        }

        void OnEnable()
        {
            if (value.Variable != null && Auto) value.Variable.OnValueChanged += Invoke;
            if (InvokeOnEnable)  Invoke(value);
        }

        void OnDisable()
        {
            if (value.Variable != null && Auto) value.Variable.OnValueChanged -= Invoke;
        }

        public virtual void Invoke(bool value)
        {
            if (invert) value ^= true; //Invert the value

            if (Enable)
            {
                OnValueChanged.Invoke(value);

                if (value)
                    OnTrue.Invoke();
                else OnFalse.Invoke();

                Debuggin(value);
            }
        }
        public virtual void Invoke(int value) => Invoke(value != 0);
        public virtual void Invoke(float value) => Invoke(value != 0);
        public virtual void Invoke(string value) => Invoke(string.IsNullOrEmpty(value));
        public virtual void InvokeVectorX(Vector3 value) => Invoke(value.x != 0);
        public virtual void InvokeVectorY(Vector3 value) => Invoke(value.y != 0);
        public virtual void InvokeVectorZ(Vector3 value) => Invoke(value.z != 0);

        private void Debuggin(bool value)
        {
#if UNITY_EDITOR
            if (debug) Debug.Log($"<B><Color=cyan>[{name}] Bool Listener [{ID.Value}] -> [{value}]</color></B>");
#endif
        }

        public virtual void Invoke() => Invoke(Value);

        public virtual void Toggle_Value()
        {
            if (Enable) Value ^= true; //toggle the value
        }

        /// <summary> Used to use turn Objects to True or false </summary>
        public virtual void Invoke(Object value)
        {
            if (Enable)
            {
                OnValueChanged.Invoke(value != null);

                if (value != null)
                    OnTrue.Invoke();
                else 
                    OnFalse.Invoke();
            }
        }

        public void ShowCursor(bool value) => UnityUtils.ShowCursor(value);
    }




    //INSPECTOR
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(BoolVarListener)), UnityEditor.CanEditMultipleObjects]
    public class BoolVarListenerEditor : VarListenerEditor
    {
        private UnityEditor.SerializedProperty OnTrue, OnFalse, OnValueChanged, invert;

        private void OnEnable()
        {
            base.SetEnable();
            OnTrue = serializedObject.FindProperty("OnTrue");
            invert = serializedObject.FindProperty("invert");
            OnFalse = serializedObject.FindProperty("OnFalse");
            OnValueChanged = serializedObject.FindProperty("OnValueChanged");
        }

        protected override void DrawEvents()
        {
            using (new UnityEditor.EditorGUILayout.VerticalScope(UnityEditor.EditorStyles.helpBox))
            {
                UnityEditor.EditorGUILayout.PropertyField(invert);
                UnityEditor.EditorGUILayout.PropertyField(OnValueChanged);
                UnityEditor.EditorGUILayout.PropertyField(OnTrue);
                UnityEditor.EditorGUILayout.PropertyField(OnFalse);
            }
        }
    }
#endif
}
