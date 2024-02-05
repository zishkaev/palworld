using System;
using UnityEngine; 


namespace MalbersAnimations.Reactions
{
    [Serializable]
    public abstract class Reaction
    {
        /// <summary>Instant Reaction ... without considering Active or Delay parameters</summary>
        protected abstract bool _TryReact(Component reactor);

        /// <summary>Get the Type of the reaction</summary>
        public abstract Type ReactionType { get; }

        public void React(Component component) => TryReact(component);

        public void React(GameObject go) => TryReact(go.transform);

        [Tooltip("Enable or Disable the Reaction")]
        [HideInInspector] public bool Active = true;

        [Tooltip("Delay the Reaction this ammount of seconds")]
        [Min(0)] public float delay = 0;

        [Tooltip("The component assigned is verified. Which means is the Correct type")]
        protected Component Verified;


        /// <summary>  Checks and find the correct component to apply a reaction  </summary>  
        public Component VerifyComponent(Component component)
        {
            Component TrueComponent;

            if (ReactionType.IsAssignableFrom(component.GetType())) //Find if the component is the same 
            {
                TrueComponent = component;
            }
            else
            {
                //Debug.Log($"Component {component.name} REACTION TYPE: {ReactionType.Name}");

                TrueComponent = component.GetComponent(ReactionType);
               
                if (TrueComponent == null)
                    TrueComponent = component.GetComponentInParent(ReactionType);
                if (TrueComponent == null)
                    TrueComponent = component.GetComponentInChildren(ReactionType);
            }

            return TrueComponent;
        }

        public bool TryReact(Component component)
        {
            if (Active && component != null)
            {
                //Check if the component is the correct component.. a first time
                if (Verified == null || Verified != component)
                {
                    Verified = VerifyComponent(component);
                    if (Verified == null) return false;
                }

                //If the Reaction has a Delay
                if (delay > 0 && component.TryGetComponent<MonoBehaviour>( out var Mono))
                {
                    Mono.Delay_Action(delay, () => _TryReact(Verified));
                    return true;
                }
                else
                {
                    return _TryReact(Verified);
                }
            }
            return false;
        }

        //React to multiple components
        public bool TryReact(params Component[] components)
        {
            if (Active && components != null && components.Length>0)
            {
                foreach (var component in components)
                {
                    var comp = VerifyComponent(component);
                   _TryReact(comp);
                }
            }
            return true;
        }
    }
}