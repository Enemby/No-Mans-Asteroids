var destroyTime : float = 7.5;
function Start(){

}
function Update(){
	destroyTime-=Time.deltaTime;
	if(destroyTime <= 0){
		Destroy(this.gameObject);
	}
}