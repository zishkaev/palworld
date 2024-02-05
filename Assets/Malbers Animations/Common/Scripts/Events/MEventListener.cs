using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditorInternal;
using UnityEditor;
#endif

namespace MalbersAnimations.Events
{
    ///<summary>
    /// Listener to use with the GameEvents
    /// Based on the Talk - Game Architecture with Scriptable Objects by Ryan Hipple
    /// </summary>
    [AddComponentMenu("Malbers/Events/Event Listener")]
    public class MEventListener : MonoBehaviour
    {
        public List<MEventItemListener> Events = new List<MEventItemListener>();
        // public bool debug;

        private void OnEnable()
        {
            foreach (var item in Events)
            {
                if (item.Event) item.Event.RegisterListener(item);
            }
        }

        private void OnDisable()
        {
            foreach (var item in Events)
            {
                if (item.Event) item.Event.UnregisterListener(item);
            }
        }
    }



    [System.Serializable]
    public class MEventItemListener
    {
        public MEvent Event;


        [HideInInspector]
        public bool useInt = false, useFloat = false, useVoid = true, useString = false, useBool = false,
            useGO = false, useTransform = false, useVector3, useVector2 = false, useComponent = false, useSprite = false;

        public UnityEvent Response = new UnityEvent();

        public UnityEvent ResponseNull = new UnityEvent();

        public FloatEvent ResponseFloat = new FloatEvent();
        public IntEvent ResponseInt = new IntEvent();

        public BoolEvent ResponseBool = new BoolEvent();
        public UnityEvent ResponseBoolFalse = new UnityEvent();
        public UnityEvent ResponseBoolTrue = new UnityEvent();

        public StringEvent ResponseString = new StringEvent();
        public GameObjectEvent ResponseGO = new GameObjectEvent();
        public TransformEvent ResponseTransform = new TransformEvent();
        public ComponentEvent ResponseComponent = new ComponentEvent();
        public SpriteEvent ResponseSprite = new SpriteEvent();
        public Vector3Event ResponseVector3 = new Vector3Event();
        public Vector2Event ResponseVector2 = new Vector2Event();

        public List<AdvancedIntegerEvent> IntEventList = new List<AdvancedIntegerEvent>();
        public bool AdvancedInteger = false;
        public bool AdvancedBool = false;
        [Tooltip("Inverts the value of the Bool Event")]
        public bool InvertBool = false;

        public float multiplier = 1;

        public virtual void OnEventInvoked()
        {
           if (useVoid)
                Response.Invoke();
        }

        public virtual void OnEventInvoked(string value)
        {
           if (useString) 
                ResponseString.Invoke(value);
        }

        public virtual void OnEventInvoked(float value)
        {
            if (useFloat) 
                ResponseFloat.Invoke(value * multiplier);
        }

        public virtual void OnEventInvoked(int value)
        {
            if (useInt)
            {
                ResponseInt.Invoke(value);

                if (AdvancedInteger)
                {
                    foreach (var item in IntEventList)
                        item.ExecuteAdvanceIntegerEvent(value);
                }
            }
        }

        public virtual void OnEventInvoked(bool value)
        {
            if (useBool)
            {
                ResponseBool.Invoke(InvertBool ? !value : value);

                if (AdvancedBool)
                {
                    if (value)
                        ResponseBoolTrue.Invoke();
                    else
                        ResponseBoolFalse.Invoke();
                }
            }
        }
        public virtual void OnEventInvoked(Vector3 value)
        {
           if (useVector3) ResponseVector3.Invoke(value);
        }

        public virtual void OnEventInvoked(Vector2 value)
        {
          if (useVector2)  ResponseVector2.Invoke(value);
        }

        public virtual void OnEventInvoked(GameObject value)
        {
            if (useGO)
            {
                if (value) ResponseGO.Invoke(value);
                else ResponseNull.Invoke();
            }
        }

        public virtual void OnEventInvoked(Transform value)
        {
            if (useTransform)
            {
                ResponseTransform.Invoke(value);
                if (!value) ResponseNull.Invoke();
            }
        }

        public virtual void OnEventInvoked(Component value)
        {
            if (useComponent)
            {
                if (value) ResponseComponent.Invoke(value);
                else ResponseNull.Invoke();
            }
        }

        public virtual void OnEventInvoked(Sprite value)
        {
            if (useSprite)
            {
                if (value) ResponseSprite.Invoke(value);
                else ResponseNull.Invoke();
            }
        }

        public MEventItemListener()
        {
            useVoid = true;
            useInt = useFloat = useString = useBool = useGO = useTransform = useVector3 = useVector2 = useSprite = useComponent = false;
        }
    }


    /// <summary> CustomPropertyDrawer</summary>

#if UNITY_EDITOR 
    [CanEditMultipleObjects, CustomEditor(typeof(MEventListener))]
    public class MEventListenerEditor : Editor
    {
        private ReorderableList list;
        private SerializedProperty eventsListeners,
            useFloat, useBool, useInt, useString, useVoid, useGo, useTransform, useVector3, useSprite, useVector2, useComponent;
        private MEventListener M;
       // MonoScript script;

        private readonly Dictionary<string, ReorderableList> innerListDict = new();



        private static GUIContent _icon_short;
        public static GUIContent Icon_Short
        {
            get
            {
                if (_icon_short == null)
                {
                    _icon_short = EditorGUIUtility.IconContent("d_Shortcut Icon", "Shortcut");
                    _icon_short.tooltip = "Show/Hide Description";
                }

                return _icon_short;
            }
        }

        private void OnEnable()
        {
            M = ((MEventListener)target);
         //   script = MonoScript.FromMonoBehaviour(M);

            eventsListeners = serializedObject.FindProperty("Events");
            // debug = serializedObject.FindProperty("debug");

            list = new ReorderableList(serializedObject, eventsListeners, true, true, true, true)
            {
                drawElementCallback = DrawElementCallback,
                drawHeaderCallback = HeaderCallbackDelegate,
                onAddCallback = OnAddCallBack
            };

        }


        GUIStyle TypeStyle;

        void HeaderCallbackDelegate(Rect rect)
        {
            EditorGUI.LabelField(rect, "   Malbers Events");
        }

        void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.y += 2;
            rect.height -= 5;
            SerializedProperty Element = eventsListeners.GetArrayElementAtIndex(index).FindPropertyRelative("Event");
            eventsListeners.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, Element, GUIContent.none);
        }

        void OnAddCallBack(ReorderableList list)
        {
            if (M.Events == null)  M.Events = new List<MEventItemListener>();
            M.Events.Add(new MEventItemListener());
        }

        GUIStyle Description_Style;


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

           // MalbersEditor.DrawDescription("Listen to [MEvents] and reacts when those events are invoked");
            using (new GUILayout.VerticalScope())
            {
                list.DoLayoutList();

                if (list.index != -1)
                {
                    if (list.index < list.count)
                    {
                        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                        {
                            var ev = M.Events[list.index].Event;

                            string evName = ev != null ? ev.name : "<Add Event!>";

                            SerializedProperty Element = eventsListeners.GetArrayElementAtIndex(list.index);
                            using (new GUILayout.HorizontalScope())
                            {
                                EditorGUI.indentLevel++;
                                EditorGUILayout.PropertyField(Element, new GUIContent($"{evName} [{list.index}]"), false);

                                useBool = Element.FindPropertyRelative("useBool");


                                var dC = GUI.color;
                                GUI.color = useBool.isExpanded ? dC : dC * 0.8f;


                                useBool.isExpanded = GUILayout.Toggle(useBool.isExpanded, Icon_Short,
                                    EditorStyles.label, GUILayout.Width(25), GUILayout.Height(25));


                                GUI.color = dC;
                                EditorGUI.indentLevel--;
                            }


                            if (M.Events[list.index].Event != null && Element.isExpanded)
                            {
                                if (useBool.isExpanded)
                                {
                                    var Descp = M.Events[list.index].Event.Description;

                                    if (Descp != string.Empty)
                                    {
                                        Description_Style ??= new GUIStyle(MTools.StyleGreen)
                                            {
                                                fontSize = 12,
                                                fontStyle = FontStyle.Bold,
                                                alignment = TextAnchor.MiddleLeft,
                                                stretchWidth = true
                                            };

                                        Description_Style.normal.textColor = EditorStyles.label.normal.textColor;

                                        M.Events[list.index].Event.Description = UnityEditor.EditorGUILayout.TextArea(Descp, Description_Style);
                                    }
                                    EditorGUILayout.Space();
                                }


                                useFloat = Element.FindPropertyRelative("useFloat");
                                useBool = Element.FindPropertyRelative("useBool");
                                useInt = Element.FindPropertyRelative("useInt");
                                useString = Element.FindPropertyRelative("useString");
                                useVoid = Element.FindPropertyRelative("useVoid");
                                useGo = Element.FindPropertyRelative("useGO");
                                useTransform = Element.FindPropertyRelative("useTransform");
                                useComponent = Element.FindPropertyRelative("useComponent");
                                useVector3 = Element.FindPropertyRelative("useVector3");
                                useVector2 = Element.FindPropertyRelative("useVector2");
                                useSprite = Element.FindPropertyRelative("useSprite");


                                TypeStyle ??= new GUIStyle(EditorStyles.objectField)
                                {
                                    alignment = TextAnchor.MiddleCenter,
                                    fontStyle = FontStyle.Bold,
                                };

                                using (new GUILayout.HorizontalScope())
                                {
                                    useVoid.boolValue = GUILayout.Toggle(useVoid.boolValue, new GUIContent("Void", "No Parameters Response"), TypeStyle);
                                    useBool.boolValue = GUILayout.Toggle(useBool.boolValue, new GUIContent("Bool", "Bool Response"), TypeStyle);
                                    useFloat.boolValue = GUILayout.Toggle(useFloat.boolValue, new GUIContent("Float", "Float Response"), TypeStyle);
                                    useInt.boolValue = GUILayout.Toggle(useInt.boolValue, new GUIContent("Int", "Int Response"), TypeStyle);
                                    useString.boolValue = GUILayout.Toggle(useString.boolValue, new GUIContent("String", "String Response"), TypeStyle);
                                    useVector3.boolValue = GUILayout.Toggle(useVector3.boolValue, new GUIContent("V3", "Vector3 Response"), TypeStyle);
                                    useVector2.boolValue = GUILayout.Toggle(useVector2.boolValue, new GUIContent("V2", "Vector2 Response"), TypeStyle);
                                }


                                using (new GUILayout.HorizontalScope())
                                {
                                    useGo.boolValue = GUILayout.Toggle(useGo.boolValue, new GUIContent("GameObject", "Game Object Response"), TypeStyle);
                                    useTransform.boolValue = GUILayout.Toggle(useTransform.boolValue, new GUIContent("Transform", "Transform Response"), TypeStyle);
                                    useComponent.boolValue = GUILayout.Toggle(useComponent.boolValue, new GUIContent("Component", "Component Response"), TypeStyle);
                                    useSprite.boolValue = GUILayout.Toggle(useSprite.boolValue, new GUIContent("Sprite", "Sprite Response"), TypeStyle);
                                }
                                

                                Draw_Void(Element);

                                Draw_Bool(Element);

                                Draw_Float(Element);

                                Draw_Integer(Element);

                                DrawString(Element);

                                Draw_GameObject(Element);

                                DrawTransform(Element);

                                DrawComponent(Element);

                                DrawSprite(Element);

                                DrawVector2(Element);

                                DrawVector3(Element);

                            }
                        }
                    }
                }
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void Draw_Void(SerializedProperty Element)
        {
            if (useVoid.boolValue)
            {
                EditorGUILayout.PropertyField(Element.FindPropertyRelative("Response"));
            }
        }

        private void Draw_Bool(SerializedProperty Element)
        {
            if (useBool.boolValue)
            {
                MalbersEditor.DrawLineHelpBox();
                var useAdvBool = Element.FindPropertyRelative("AdvancedBool");
                // if (!useAdvBool.boolValue)
                {
                    EditorGUILayout.PropertyField(Element.FindPropertyRelative("InvertBool"));
                    EditorGUILayout.PropertyField(Element.FindPropertyRelative("ResponseBool"), new GUIContent("Response"));
                }
                useAdvBool.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Use Advanced Bool", "Uses Separated Unity Events for True and False Entries"), useAdvBool.boolValue);
                if (useAdvBool.boolValue)
                {
                    EditorGUILayout.PropertyField(Element.FindPropertyRelative("ResponseBoolTrue"), new GUIContent("On True"));
                    EditorGUILayout.PropertyField(Element.FindPropertyRelative("ResponseBoolFalse"), new GUIContent("On False"));
                }
            }
        }

        private void Draw_Float(SerializedProperty Element)
        {
            if (useFloat.boolValue)
            {
                MalbersEditor.DrawLineHelpBox();
                EditorGUILayout.PropertyField(Element.FindPropertyRelative("ResponseFloat"), new GUIContent("Response"));
                EditorGUILayout.PropertyField(Element.FindPropertyRelative("multiplier"));
            }
        }

        private void Draw_Integer(SerializedProperty Element)
        {
            if (useInt.boolValue)
            {
                MalbersEditor.DrawLineHelpBox();

                var useAdvInteger = Element.FindPropertyRelative("AdvancedInteger");
                useAdvInteger.boolValue = GUILayout.Toggle(useAdvInteger.boolValue, new GUIContent("Use Integer Comparer",
                    "Compare the Event entry value with a new  one to make a new Int Response"),  EditorStyles.foldoutHeader);

                if (useAdvInteger.boolValue)
                {
                    var compare = Element.FindPropertyRelative("IntEventList");
                    ReorderableList Reo_AbilityList;

                    string listKey = Element.propertyPath;

                    if (innerListDict.ContainsKey(listKey))
                    {
                        // fetch the reorderable list in dict
                        Reo_AbilityList = innerListDict[listKey];
                    }
                    else
                    {
                        Reo_AbilityList = new ReorderableList(serializedObject, compare, true, true, true, true)
                        {
                            drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                            {
                                var element = compare.GetArrayElementAtIndex(index);
                               
                                var name = element.FindPropertyRelative("name");
                                var active = element.FindPropertyRelative("active");
                                var comparer = element.FindPropertyRelative("comparer");
                                var Value = element.FindPropertyRelative("Value");

                                rect.y += 1;
                                var height = UnityEditor.EditorGUIUtility.singleLineHeight;
                                var split = rect.width / 3;
                                var p = 5;

                               

                                var rectActveName = new Rect(rect.x, rect.y, 20, height);
                                var rectName = new Rect(rect.x+20, rect.y, (split + p - 2) * 1.2f-20, height);
                                var rectComparer = new Rect(rect.x + (split + p)*1.2f+20, rect.y, (split - p)*0.75f-20, height);
                                var rectValue = new Rect(rect.x + split * 2 + p + 15+20, rect.y, split - p - 10-20, height);


                                EditorGUI.PropertyField(rectActveName, active, GUIContent.none);
                                EditorGUI.PropertyField(rectName, name, GUIContent.none);
                                EditorGUI.PropertyField(rectComparer, comparer, GUIContent.none);
                                EditorGUI.PropertyField(rectValue, Value, GUIContent.none);
                            },

                            drawHeaderCallback = (Rect rect) =>
                            {
                                rect.y += 1;
                                var height = UnityEditor.EditorGUIUtility.singleLineHeight;
                                var split = rect.width / 3;
                                var p = (split * 0.3f);
                                var rectName = new Rect(rect.x, rect.y, split + p - 2, height);
                                var rectComparer = new Rect(rect.x + split + p, rect.y, split - p + 15, height);
                                var rectValue = new Rect(rect.x + split * 2 + p + 5, rect.y, split - p, height);
                                var DebugRect = new Rect(rect.width, rect.y - 1, 25, height + 2);

                                EditorGUI.LabelField(rectName, "    Name");
                                EditorGUI.LabelField(rectComparer, " Compare");
                                EditorGUI.LabelField(rectValue, " Value");
                            },
                        };


                        innerListDict.Add(listKey, Reo_AbilityList);  //Store it on the Editor

                    }
                    Reo_AbilityList.DoLayoutList(); 

                    int SelectedAbility = Reo_AbilityList.index; 

                    if (SelectedAbility != -1 && SelectedAbility < Reo_AbilityList.count)
                    {
                        var element = compare.GetArrayElementAtIndex(SelectedAbility);
                        if (element != null) 
                        {
                            var Response = element.FindPropertyRelative("Response");
                            var name = element.FindPropertyRelative("name").stringValue;
                            EditorGUILayout.PropertyField(Response, new GUIContent("Response: [" + name + "]   "));
                        }
                    } 
                }
                //   else
                {
                    MalbersEditor.DrawLineHelpBox();
                    EditorGUILayout.PropertyField(Element.FindPropertyRelative("ResponseInt"), new GUIContent("Response"));
                }
            }
        }

        private void DrawString(SerializedProperty Element)
        {
            if (useString.boolValue)
            {
                MalbersEditor.DrawLineHelpBox();
                EditorGUILayout.PropertyField(Element.FindPropertyRelative("ResponseString"), new GUIContent("Response"));
            }
        }

        private void Draw_GameObject(SerializedProperty Element)
        {
            if (useGo.boolValue)
            {
                MalbersEditor.DrawLineHelpBox();
                EditorGUILayout.PropertyField(Element.FindPropertyRelative("ResponseGO"), new GUIContent("Response GO"));
                EditorGUILayout.PropertyField(Element.FindPropertyRelative("ResponseNull"), new GUIContent("Response NULL"));
            }
        }

        private void DrawTransform(SerializedProperty Element)
        {
            if (useTransform.boolValue)
            {
                MalbersEditor.DrawLineHelpBox();
                EditorGUILayout.PropertyField(Element.FindPropertyRelative("ResponseTransform"), new GUIContent("Response T"));
                EditorGUILayout.PropertyField(Element.FindPropertyRelative("ResponseNull"), new GUIContent("Response NULL")); 
            }
        }

        private void DrawComponent(SerializedProperty Element)
        {
            if (useComponent.boolValue)
            {
                MalbersEditor.DrawLineHelpBox();
                EditorGUILayout.PropertyField(Element.FindPropertyRelative("ResponseComponent"), new GUIContent("Response C"));
                EditorGUILayout.PropertyField(Element.FindPropertyRelative("ResponseNull"), new GUIContent("Response NULL"));
            }
        }

        private void DrawSprite(SerializedProperty Element)
        {
            if (useSprite.boolValue)
            {
                MalbersEditor.DrawLineHelpBox();
                EditorGUILayout.PropertyField(Element.FindPropertyRelative("ResponseSprite"), new GUIContent("Response Sprite"));
                EditorGUILayout.PropertyField(Element.FindPropertyRelative("ResponseNull"), new GUIContent("Response NULL"));
            }
        }

        private void DrawVector2(SerializedProperty Element)
        {
            if (useVector3.boolValue)
            {
                MalbersEditor.DrawLineHelpBox();
                EditorGUILayout.PropertyField(Element.FindPropertyRelative("ResponseVector3"), new GUIContent("Response V3"));
            }
        }

        private void DrawVector3(SerializedProperty Element)
        {
            if (useVector2.boolValue)
            {
                MalbersEditor.DrawLineHelpBox();
                EditorGUILayout.PropertyField(Element.FindPropertyRelative("ResponseVector2"), new GUIContent("Response V2"));
            }
        }
    }
#endif
}