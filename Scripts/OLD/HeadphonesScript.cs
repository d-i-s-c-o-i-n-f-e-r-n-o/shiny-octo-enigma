using UnityEngine;

public class HeadphonesScript : MonoBehaviour
{
    public GameObject headphones_Icon, headphones_Text;
    public SaveManager SaveManager;
    Animator anim;
    bool isGameOn;

    void Start()
    {
        anim = GetComponent<Animator>();
        isGameOn = SaveManager.instance.activeSave.isGameOn;
        if (!isGameOn)
        {
            anim.SetBool("Opening", false);
            headphones_Icon.SetActive(true);
            headphones_Text.SetActive(true);
            Invoke("CloseIt", 4f);
        }
        else gameObject.SetActive(false);
    }

    void CloseIt()
    {
        anim.SetBool("Opening", true);
        headphones_Icon.SetActive(false);
        headphones_Text.SetActive(false);
        Invoke("FadeAway", 1f);
    }

    void FadeAway()
    {
        Destroy(gameObject);
        SaveManager.instance.activeSave.isGameOn = true;
        SaveManager.instance.Save();
    }
}