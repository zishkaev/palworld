using MalbersAnimations.Events;
using UnityEngine;
namespace MalbersAnimations
{
    [HelpURL("https://malbersanimations.gitbook.io/animal-controller/secondary-components/scriptables/tags")]
    [AddComponentMenu("Malbers/Utilities/Tools/Tags Comparer")]
    public class TagsComparer : MonoBehaviour 
    {
        public Tag[] tags;
        public GameObjectEvent HasTag = new();

        public void CheckTag(GameObject gameObject)
        {
            if (gameObject.HasMalbersTag(tags))
                HasTag.Invoke(gameObject);
        }

        public void CheckTagInParent(GameObject gameObject)
        {
            if (gameObject.HasMalbersTagInParent(tags))
                HasTag.Invoke(gameObject);
        }
         
        public void CheckTag(Component co) => CheckTag(co);
        public void CheckTagInParent(Component co) => CheckTagInParent(co);
    }
}