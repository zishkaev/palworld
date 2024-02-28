
using UnityEngine;

public class LevelFadingScreen : MonoBehaviour
{

    [SerializeField] Animator animator;

   

    //void Update()
    //{
    //    if(Input.GetMouseButtonDown(0))
    //    {
    //        FadeToLevel(1);
    //    }

    //}


    public void FadeToLevel()
    {

        animator.SetTrigger("FadeOut");

    }



}
