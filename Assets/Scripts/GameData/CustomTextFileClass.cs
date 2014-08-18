using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;


public class CustomTextFileClass {
    IList<string> textFileRecord = new List<string>();

    public string get_next_line(StreamReader textToRead)
    {
        string rawString;
        rawString = textToRead.ReadLine();
        textFileRecord.Add(rawString);
        while (rawString.Length == 0 ||
            rawString[0] == '#' || rawString == null)
        {
            rawString = textToRead.ReadLine();
            textFileRecord.Add(rawString);
        }
        return rawString;
    }
}
