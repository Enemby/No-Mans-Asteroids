//This is just game specific stuff the other scripts can reference...
enum ShipType {
	Fighter = 0, //Can fire at other ships
	Harvester = 1, //Player only
	Kamikaze = 2, //AI only?
	Carrier = 3, //Spawns ships
	Supressor = 4, //Freezes other ships (We might not do this)
	Station = 5,
	Turret = 6
}
enum AIMode{ //Actions other squad members will take when not selected....
	Sentry = 0, //Stop and fire at nearby enemies/asteroids
	Follow = 1, //Follow the selected ship
	Drift = 2 //Don't change velocity, stay course.
}
static function getDebug(){
	//Do stuff here
}