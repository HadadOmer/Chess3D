using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using UnityEngine.Android;
using UnityEngine.UI;

public class VoiceControl : MonoBehaviour
{

    public BoardManager gameEngine;
    public Text voiceLog;
    public List<string> keywords;
    public bool speaking;

    // Use this for initialization
    void Start()
    {
        //Inserts valid kewords
        keywords = GenerateKeywords();

        //Initailizes the Speech to speak instance which handles the voice recognition
        SpeechToText.instance.Setting("en-US");
        SpeechToText.instance.onResultCallback = OnResultBack;
        SpeechToText.instance.onPartialResultsCallback = OnResultBack;
                
        gameEngine = GameObject.Find("Board").GetComponent<BoardManager>();
    }
    public List<string> GenerateKeywords()
    {        
        List<string> kewords = new List<string>();

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                kewords.Add(((char)(65 + i)).ToString() + (j + 1).ToString());
                kewords.Add(((char)(97 + i)).ToString() + (j + 1).ToString());
            }
        }
        //The valid kewords contains A-H or a-h in the first char and 1-8 in the second char
        return kewords;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpeaking()
    {
        //Asking for microphone permission iof needed
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            Permission.RequestUserPermission(Permission.Microphone);

        SpeechToText.instance.StartRecording();
        
        //Makes the button pulse red and white while recording
        StartCoroutine(PulseButton());
    }
    public IEnumerator PulseButton()
    {
        speaking = true;
        Image img = transform.GetComponent<Image>();
        while(speaking)
        {
            img.color = img.color == Color.white ? Color.red : Color.white;
            yield return new WaitForSeconds(0.2f);
        }
        //Reverts back to white color a the end of the pulsing 
        img.color = Color.white;
    }
    public void StopSpeaking()
    {      
        SpeechToText.instance.StopRecording();
    }
    public void OnResultBack(string result)
    {
        speaking = false;
        //Displays the voice result on screen
        voiceLog.text = result;
        if (keywords.Contains(result))
        {
            //Adds a v for a valid voice result
            voiceLog.text += " V";

            //Converts the lower case to upper
            result=result.ToUpper();

            //Picks a tile in the game engine based on the voice input
            gameEngine.VoicePickTile(result);
        }
    }


}
