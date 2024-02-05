using MalbersAnimations.Reactions;
using UnityEngine;

namespace MalbersAnimations.Controller.AI
{
    [CreateAssetMenu(menuName = "Malbers Animations/Pluggable AI/Tasks/Reaction Task")]
    public class ReactionTask : MTask
    {
        public override string DisplayName => "General/Reaction";

        [Space, Tooltip("Apply the Task to the Animal(Self) or the Target(Target)")]
        public Affected affect = Affected.Self;

        [SerializeReference,SubclassSelector]
        public Reaction reaction;

        public override void StartTask(MAnimalBrain brain, int index)
        {
            if (affect == Affected.Self)
            {
                reaction?.React(brain.Animal);
            }
            else
            {
                if (brain.Target)
                    reaction?.React(brain.Target);
            }
             brain.TaskDone(index);
        }

        private void Reset()
        => Description = "Add a Reaction to the Target or the Animal";

    }
}
