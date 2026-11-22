using System;
using System.Collections.Generic;

// Data model for FragmentSets.json. Field names must match the JSON keys exactly.
// Load: FragmentSetsData data = JsonDataLoader.Load<FragmentSetsData>(jsonTextAsset);

[Serializable]
public class FragmentSetsData
{
    public string header;
    // Badge text. {0} is replaced with the set's pieceCount.
    public string piecesLabelFormat;
    public List<FragmentSetData> sets = new List<FragmentSetData>();
}

[Serializable]
public class FragmentSetData
{
    public string title;
    public int pieceCount;
    public List<FragmentDocumentData> documents = new List<FragmentDocumentData>();
}

[Serializable]
public class FragmentDocumentData
{
    // Kept in logical order ("MUR19"), not pre-reversed for RTL text.
    public string id;
    public string description;
}
