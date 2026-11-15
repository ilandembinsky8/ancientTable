using UnityEngine;

public abstract class GameManagerBase : MonoBehaviour
{
    public virtual void Home()
    {
        Debug.Log("'Home' Not Implemented");
    }

    public virtual void Help()
    {
        Debug.Log("'Help' Not Implemented");
    }

    public virtual void ChangeLanguage()
    {
        Debug.Log("'ChangeLanguage' Not Implemented");
    }
}
