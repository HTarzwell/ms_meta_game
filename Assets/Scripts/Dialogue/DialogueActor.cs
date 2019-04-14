using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActor : MonoBehaviour {
	public UnityEngine.UI.Image expressionSprite;
	public UnityEngine.UI.Image speechBubbleSprite;
	public UnityEngine.UI.Text speechBubbleText;

	[SerializeField]
	public ExpressionDictionary expressions;
	public string defaultExpression = "neutral";

	[Yarn.Unity.YarnCommand( "setExpression" )]
	public void setExpression(string expressionName) {
		if( ( expressions.ContainsKey( expressionName ) ) )
			expressionSprite.sprite = expressions[expressionName];
		else
			Debug.LogError( "Unknown Expression " + expressionName );
	}

	public void hide() {
		this.gameObject.SetActive( false );
	}

	public void show() {
		expressionSprite.sprite = expressions[defaultExpression];
		if( speechBubbleSprite )
			speechBubbleSprite.gameObject.SetActive( false );
		this.gameObject.SetActive( true );
	}

	public void speak(string text) {
		if( speechBubbleSprite )
			speechBubbleSprite.gameObject.SetActive( true );
		speechBubbleText.text = text;
	}

	public void stopSpeaking() {
		if( speechBubbleSprite )
			speechBubbleSprite.gameObject.SetActive( false );
		speechBubbleText.text = "";
	}
}
