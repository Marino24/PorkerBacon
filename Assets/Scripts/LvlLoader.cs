using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LvlLoader : MonoBehaviour
{

    public Animator animator;
    public float animTime;

    public static LvlLoader instance;

    void Awake()
    {
        instance = this;
    }

    public void LoadScene(int buildIndex)
    {
        StartCoroutine(LoadTheLevel(buildIndex));
    }

    IEnumerator LoadTheLevel(int buildIndex)
    {
        animator.SetTrigger("StartLoad");
        yield return new WaitForSeconds(animTime);
        SceneManager.LoadScene(buildIndex);
    }
}
