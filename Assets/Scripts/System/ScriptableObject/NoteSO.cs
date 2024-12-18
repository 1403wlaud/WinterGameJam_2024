using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Note", menuName = "NoteSetting")]
public class NoteSO : ScriptableObject
{
    [SerializeField] private string NoteContents;

    public string NoteContents_SO=>NoteContents;
}
