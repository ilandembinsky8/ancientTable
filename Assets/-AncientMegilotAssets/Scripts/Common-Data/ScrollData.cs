using System;
using System.Collections.Generic;
using UnityEngine;

// One scroll's text data, as Dr.slayer specified. Field names match the JSON keys (JsonUtility).
[Serializable]
public class ScrollData
{
    public string puzzleId;              // מזהה של פאזל
    public string nameHebrew;            // שם מגילה בעברית
    public string numberAndNameEnglish;  // מספר ושם מגילה באנגלית
    public List<PieceData> pieces = new List<PieceData>(); // רשימה של חלקים
    public string site;                  // אתר
    public string period;                // תקופה
    public string material;              // חומר
    public string handwritingType;       // סוג כתב יד
    public string compositionType;       // סוג חיבור (shown on the microscope cards)
    public string language;              // שפת כתב
    public string script;                // כתב
    public string info;                  // מידע על המגילה
    public string translationHeader;     // כותרת של התרגום
    public string translation;           // תרגום של המגילה
}

[Serializable]
public class PieceData
{
    public string pieceId;               // מזהה של חלק
    public Vector2 startPosition;        // מיקום התחלתי
    public float startRotation;          // רוטציה התחלתית
    public Vector2 endPosition;          // מיקום סופי
    public float endRotation;            // רוטציה סופית
}
