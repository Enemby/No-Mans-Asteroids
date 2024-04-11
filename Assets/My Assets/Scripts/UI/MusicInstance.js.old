@script RequireComponent(AudioSource)
var myClips :AudioClip[];
var myNames : String[];
private var mySource : AudioSource;
var lastClipIndex : int = 0;
var fade : boolean = false;
var updatedInfo : boolean = false;
var currentIndex : int = 0;
var skiptrack : boolean = false;
function Start(){
	mySource = this.GetComponent(AudioSource);
}
function musicPlaying(){
	if(mySource.isPlaying == true){
		return true;
	}
	else{
		return false;
	}
}
function Deactivate(){
	fade = true;
}
function Activate(){
	fade = false;
}
function pickRandomClip(){
	var rand;
	if(myClips.Length <= 1){
		rand = Random.Range(0,myClips.Length+1);
	}
	else{
		rand = 0;
	}
	if(rand != lastClipIndex){
		lastClipIndex = rand;
		currentIndex = rand;
		return myClips[rand];
	}
	else{
		rand = Random.Range(0,myClips.Length+1);
		currentIndex = rand;
		return myClips[rand];	
	}
}
function updateMusicInfo(){
	var myInfo = GameObject.FindGameObjectWithTag("MusicInfo");
	if(myInfo != null){
		myInfo.GetComponent(UI.Text).text = myNames[currentIndex];
		myInfo.GetComponent("MusicInfo").BroadcastMessage("startPopUp");
	}
}
function Update(){
	if(Input.GetButtonDown("Skip")){
		if(fade == false){
			skiptrack = true;
		}
	}
	if(Application.isLoadingLevel){
		Debug.Log("Loading level! Fading music");
		fade = true;
	}
}
function LateUpdate(){
	if(musicPlaying() == false||skiptrack == true){
		mySource.clip = pickRandomClip();
		mySource.Play();
		updatedInfo = false;
		skiptrack = false;
	}
	if(fade == true){
		if(mySource.volume >= 0){
			mySource.volume-=Time.deltaTime;
		}
	}
	else{
		if(updatedInfo == false){
			updateMusicInfo();
			updatedInfo = true;
		}
		if(mySource.volume < PlayerPrefs.GetFloat("musicVolume")){
			mySource.volume+=Time.deltaTime;
		}
		if(mySource.volume > PlayerPrefs.GetFloat("musicVolume")){
			mySource.volume-=Time.deltaTime;
			if(mySource.volume - PlayerPrefs.GetFloat("musicVolume") <= 0.1){
				mySource.volume = PlayerPrefs.GetFloat("musicVolume"); //This prevents jitter
			}
		}
	}
}