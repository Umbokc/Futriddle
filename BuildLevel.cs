﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// строим уровень
public class BuildLevel : MonoBehaviour {
	
	public Image bg_level;
	public GameObject bg_change;
	public GameObject riddleimg;
	public GameObject Level_text;
	public GameObject Answer;
	public GameObject EnterChars;
	public GameObject Riddle;
	
	public GameObject Next_btn;
	public GameObject Back_btn;

	private float Anim_Time = 0.6f;

	void Start () {
	}

	void Update () {
		if(U.GAME_STATUS == 1){
			
			Invoke("LevelText", Anim_Time);

			// Фон
			SetBg();

			riddleimg.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("levels/"+ U.Global_Level.ToString() +"/uroven-"+ U.current_level.ToString());
			
			// строим поля для ввода ответа
			SetFieldAnswerChars();

			// клава
			SetChars();

			// передаем эстафету дальше
			U.GAME_STATUS = 2;
		}
		if(U.GAME_STATUS == 5){
			Invoke("LevelText", Anim_Time);
			ToNextLevel();
			U.GAME_STATUS  = 3;
		}
	}

	void SetFieldAnswerChars (){

		// удаляем обьекты предыдущего уровня, если они есть
		for(int k = 0; k < U._FIELD_ANSWER_CHARS.transform.childCount; k++ ){
			U._FIELD_ANSWER_CHARS.transform.GetChild (k).gameObject.SetActive(false);
		}

		// получаем колличество символов ответа
		int len_word = U._LEVELS_ANSWER[U.Global_Level-1,U.current_level-1].Length;

		bool[] how_btn = new bool[len_word];
		
		for(int ki = 0; ki < len_word; ki++) how_btn[ki] = false;

		U.HOW_BUTTONS_DONE = how_btn;

		U._FIELD_ANSWER_CHARS.transform.GetChild (len_word-3).gameObject.SetActive(true);
		for(int k = 0; k < U._FIELD_ANSWER_CHARS.transform.GetChild (len_word-3).gameObject.transform.childCount; k++ ){
			U._FIELD_ANSWER_CHARS.transform.GetChild (len_word-3).gameObject.transform.GetChild (k).gameObject.GetComponentInChildren<TextMesh>().text = "";
		}

		// if(!U.PreviewField)
			// U.Allow_click_field = true;
	}

	void SetChars () {
		for(int k = 0; k < U._ENTER_CHARS.transform.childCount; k++ ){
			GameObject TheChar = U._ENTER_CHARS.transform.GetChild (k).gameObject;
			TheChar.GetComponentInChildren<TextMesh>().text =  U._LEVELS_CHARS[U.Global_Level-1,U.current_level-1][k].ToString().ToUpper();
			
			ClickChar TheChar_click = TheChar.GetComponentInChildren<ClickChar>();

			if(TheChar.transform.position.y != TheChar_click.the_tp.y && 0 != TheChar_click.the_tp.y){
				TheChar_click.Restart_the_fast();
				// U.tp_to(TheChar, 999, TheChar_click.the_tp.y, TheChar_click.the_tp.z);
			}
		}
	}

	void SetBg(){
		bg_change.GetComponent<Image>().sprite = Resources.Load<Sprite>("levels/"+ U.Global_Level.ToString() + "/bg");
		bg_change.SetActive(true);
		bg_change.GetComponent<Animation>().Play("Change_bg");
		Invoke("End_SetBg", Anim_Time);
	}

	void End_SetBg(){
		bg_change.SetActive(false);
		bg_level.sprite = Resources.Load<Sprite>("levels/"+ U.Global_Level.ToString() + "/bg");
	}

	void ToNextLevel(){

		U.current_level = U.PP_Level;

		riddleimg.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("levels/"+ U.Global_Level.ToString() +"/uroven-"+ U.current_level.ToString());
		
		// // строим поля для ввода ответа
		SetFieldAnswerChars ();

		// U.tp_to(U._FIELD_ANSWER_CHARS, 999, 0);

		// клава
		SetChars();
		
		Riddle.SetActive(true);

		// анимаци 
		Riddle.GetComponent<Animation>().Play("ToNextLevel_Riddle");
		Answer.GetComponent<Animation>().Play("ToNextLevel_Answer");

		Invoke("answer_to_place", Anim_Time);

		// // передаем эстафету дальше
		U.GAME_STATUS = 3;
	}

	void answer_to_place(){
		U.tp_to(Answer, 0);
		Answer.SetActive(false);
		Destroy(U._FIELD_ANSWER_CHARS_CLONE);
	}

	void LevelText(){

		Level_text.SetActive(true);

		Level_text.GetComponentInChildren<TextMesh>().text = 	U.current_level.ToString() + "/30";

		U.Anim_go(Level_text.GetComponent<Animation>(), "ToggleColor_LevelText");
		
		Invoke("LevelText_hide", 3f);
	}

	void LevelText_hide(){
		U.Anim_go(Level_text.GetComponent<Animation>(), "ToggleColor_LevelText", false);
		Invoke("LevelText_false", Anim_Time);
	}
	void LevelText_false(){
		Level_text.SetActive(false);
	}

}
