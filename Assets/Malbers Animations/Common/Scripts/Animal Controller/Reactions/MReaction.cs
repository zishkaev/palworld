
using System;

namespace MalbersAnimations.Reactions
{
    /// <summary> Reaction Script for Making the Animal do something </summary>
    [System.Serializable]
    public abstract class MReaction : Reaction
    {
        public override Type ReactionType => typeof(Controller.MAnimal);
    }
}