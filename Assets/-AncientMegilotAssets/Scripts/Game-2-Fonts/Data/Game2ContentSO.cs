using System.Collections.Generic;
using UnityEngine;

// All Game 2 content: the script types (left list) and the scrolls (right list). Looked up by id.
[CreateAssetMenu(fileName = "Game2Content", menuName = "Ancient Megilot/Game 2/Content")]
public class Game2ContentSO : ScriptableObject
{
    public List<ScriptTypeSO> scriptTypes = new List<ScriptTypeSO>();
    public List<ScrollSO> scrolls = new List<ScrollSO>();

    // Returns the script type whose id matches, or null.
    public ScriptTypeSO GetScriptType(string id)
    {
        foreach (ScriptTypeSO scriptType in scriptTypes)
            if (scriptType != null && scriptType.id == id) return scriptType;
        return null;
    }

    // Returns the scroll whose catalogId matches, or null.
    public ScrollSO GetScroll(string catalogId)
    {
        foreach (ScrollSO scroll in scrolls)
            if (scroll != null && scroll.catalogId == catalogId) return scroll;
        return null;
    }
}
