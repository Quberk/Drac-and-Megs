using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{

    private Animator comboUi;
    private Animator comboText;

    [HideInInspector]
    public int comboNum;

    private Text combo;

    private GameManaging myGame;

    // Start is called before the first frame update
    void Start()
    {
        myGame = FindObjectOfType<GameManaging>();

        comboUi = GameObject.Find("Combo_UI").GetComponent<Animator>();
        comboText = GameObject.Find("Combo_Number").GetComponent<Animator>();
        combo = GameObject.Find("Combo_Number").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        string str = comboNum.ToString();
        combo.text = "X" + str;
    }

    public void ComboController()
    {
        //comboCounter += Time.deltaTime;


        //if (comboCounter >= comboTime)
        //{
        myGame.score += (comboNum * comboNum);
        comboNum = 0;
        //comboCounter = 0f;
        //}
    }

    public void ComboAdder(int number)
    {
        comboUi.SetTrigger("add");
        comboText.SetTrigger("add");
        comboNum++;
        //comboCounter = 0f;
    }
}
