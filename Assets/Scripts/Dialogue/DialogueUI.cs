/*
 * Original code supplied by Secret Lab Pty. Ltd. and Yarn Spinner contributors
 * under MIT License and later edited and adapted.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;

/// Displays dialogue lines to the player, and sends
/// user choices back to the dialogue system.

/** Note that this is just one way of presenting the
 * dialogue to the user. The only hard requirement
 * is that you provide the RunLine, RunOptions, RunCommand
 * and DialogueComplete coroutines; what they do is up to you.
 */
public class DialogueUI : Yarn.Unity.DialogueUIBehaviour {

	public Yarn.Unity.DialogueRunner dialogueRunner;
	/// The object that contains the dialogue and the options.
	/** This object will be enabled when conversation starts, and 
     * disabled when it ends.
     */
	public GameObject dialogueContainer;

	/// A UI element that appears after lines have finished appearing
	public GameObject continuePrompt;
	public UnityEngine.Events.UnityEvent onStart, onEnd;

	/// A delegate (ie a function-stored-in-a-variable) that
	/// we call to tell the dialogue system about what option
	/// the user selected
	private Yarn.OptionChooser SetSelectedOption;

	/// How quickly to show the text, in seconds per character
	[Tooltip( "How quickly to show the text, in seconds per character" )]
	public float textSpeed = 0.025f;


	/* 
    ==================
    Added variables
    ==================
     */
	public ActorDictionary actors;
	public DialogueActor[] hudActors;

	/// The buttons that let the user choose an option
	public List<Button> optionButtons;

	/// Make it possible to temporarily disable the controls when
	/// dialogue is active and to restore them when dialogue ends
	public RectTransform gameControlsContainer;

	private bool continueDialogue = false;
	private bool awaitingContinue = false;

	private DialogueActor speakingActor = null;

	void Awake() {
		// Start by hiding the container, line and option buttons
		if( dialogueContainer != null )
			dialogueContainer.SetActive( false );

		foreach( var button in optionButtons ) {
			button.gameObject.SetActive( false );
		}

		// Hide the continue prompt if it exists
		if( continuePrompt != null )
			continuePrompt.SetActive( false );

		//Hide the actors and their text
		hideActors();
	}

	/// Show a line of dialogue, gradually
	public override IEnumerator RunLine(Yarn.Line line) {
		//Get the current speaking actor actor by name
		var nextActor = actors[line.text.Substring( 0, line.text.IndexOf( ':' ) )];
		line.text = line.text.Substring( line.text.IndexOf( ':' ) + 1 );
		line.text.TrimStart( ' ' );
		if( nextActor == null ) {
			Debug.LogError( "[DialogueUI] Invalid actor name: " + line.text.Substring( 0, line.text.IndexOf( ':' ) ) );
			yield return null;
		}
		// Switch actors if necessary
		if( nextActor != speakingActor ) {
			if( speakingActor )
				speakingActor.stopSpeaking();
			speakingActor = nextActor;
		}

		if( textSpeed > 0.0f ) {
			// Display the line one character at a time
			var stringBuilder = new StringBuilder();

			foreach( char c in line.text ) {
				stringBuilder.Append( c );
				speakingActor.speak( stringBuilder.ToString() );
				yield return new WaitForSeconds( textSpeed );
			}
		}
		else {
			// Display the line immediately if textSpeed == 0
			speakingActor.speak( line.text );
		}

		// Show the 'Continue' prompt when done, if we have one
		if( continuePrompt != null )
			continuePrompt.SetActive( true );

		// Wait for any user input
		while( continueDialogue == false ) {
			awaitingContinue = true;
			yield return null;
		}
		continueDialogue = false;
		awaitingContinue = false;

		if( continuePrompt != null )
			continuePrompt.SetActive( false );

	}

	public void ContinueDialogue() {
		if( awaitingContinue )
			continueDialogue = true;
		// Debug.Log( "Continue" );
	}

	/// Show a list of options, and wait for the player to make a selection.
	public override IEnumerator RunOptions(Yarn.Options optionsCollection, Yarn.OptionChooser optionChooser) {
		Debug.Log( "Here" );
		// Do a little bit of safety checking
		if( optionsCollection.options.Count > optionButtons.Count ) {
			Debug.LogWarning( "There are more options to present than there are" +
							 "buttons to present them in. This will cause problems." );
		}

		// Display each option in a button, and make it visible
		int i = 0;
		foreach( var optionString in optionsCollection.options ) {
			optionButtons[i].gameObject.SetActive( true );
			optionButtons[i].GetComponentInChildren<Text>().text = optionString;
			i++;
		}

		// Record that we're using it
		SetSelectedOption = optionChooser;

		// Wait until the chooser has been used and then removed (see SetOption below)
		while( SetSelectedOption != null ) {
			yield return null;
		}

		// Hide all the buttons
		foreach( var button in optionButtons ) {
			button.gameObject.SetActive( false );
		}
	}

	/// Called by buttons to make a selection.
	public void SetOption(int selectedOption) {

		// Call the delegate to tell the dialogue system that we've
		// selected an option.
		SetSelectedOption( selectedOption );

		// Now remove the delegate so that the loop in RunOptions will exit
		SetSelectedOption = null;
	}

	/// Run an internal command.
	public override IEnumerator RunCommand(Yarn.Command command) {
		// "Perform" the command
		// dialogueRunner.?
		// command.
		Debug.Log( "Command: " + command.text );

		if( command.text.Contains( "end" ) )
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
         	Application.Quit();
#endif

		yield break;
	}

	/// Called when the dialogue system has started running.
	public override IEnumerator DialogueStarted() {
		Debug.Log( "Dialogue starting!" );

		// Enable the dialogue controls.
		if( dialogueContainer != null )
			dialogueContainer.SetActive( true );

		// Hide the game controls.
		if( gameControlsContainer != null ) {
			gameControlsContainer.gameObject.SetActive( false );
		}

		if( onStart != null )
			onStart.Invoke();

		yield break;
	}

	/// Called when the dialogue system has finished running.
	public override IEnumerator DialogueComplete() {
		Debug.Log( "Complete!" );

		// Hide the dialogue interface.
		if( dialogueContainer != null )
			dialogueContainer.SetActive( false );

		// Show the game controls.
		if( gameControlsContainer != null ) {
			gameControlsContainer.gameObject.SetActive( true );
		}

		hideActors();

		if( onEnd != null )
			onEnd.Invoke();

		yield break;
	}

	[Yarn.Unity.YarnCommand( "setActors" )]
	public void setActors(string leftActor, string rightActor, string hudActor) {
		hideActors();

		if( leftActor != null && leftActor != "null" )
			showActor( leftActor );
		if( rightActor != null && rightActor != "null" )
			showActor( rightActor );
		if( hudActor != null && hudActor != "null" )
			showActor( hudActor );
	}

	private void showActor(string name) {
		if( actors.ContainsKey( name ) ) {
			var obj = actors[name];
			obj.show();
		}
		else
			Debug.LogError( "[DialogueUI] Unknown actor " + name );
	}

	private void hideActors() {
		foreach( var pair in actors ) {
			pair.Value.hide();
		}

		foreach( var actor in hudActors ) {
			actor.hide();
		}
	}

}


