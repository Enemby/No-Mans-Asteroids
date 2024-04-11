var link : String = "https://docs.google.com/document/u/0/export?format=txt&id=1nov4VLgqCvkd-Pf-jETXeV26And5FifPW6J5KvWj2v4&token=AC4w5Vi3SiKTrGfWIlqizygaKRgg5IxtPQ%3A1502060542977&includes_info_params=true";
var web : WWW;
function Start () {
	web = WWW(link);
}

function Update () {
	if(web.isDone){
		var mytext = this.GetComponent(UI.Text);
		mytext.text = web.text;
	}
}