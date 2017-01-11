	/*
	 *        ****************************
	 *        ******* TOP PRIORITY *******
	 *        ****************************
	 *
	 *  TODO FIX INDIRECT COLLISION BETWEEN ENNEMIES AND BUTTONS (raycast for position button check + (trigger + col) for indirect det + col for direct det 
	 *
	 *
	 *  TODO tweak/increase botFaceTrigger to hal the height of ennemy, in case of weirds diagnoal tricky placement
     *  eg:                
     *               \------\   ||
	 *        |------|\      \  ||
	 *        |      | \      \ ||
	 *        |      |  \------\||
	 *        |------|  	    ||   
	 *
	 *    check how to diynamically resize botFaceBehavior => DO THE MATH
	 *
	 *
	 * TODO fix rotation of botFaceTrigger
	 *
	 *
	 * TODO removeList indirect function recursive call check 
	 *
	 *        ****************************
	 *        ******* MED PRIORITY *******
	 *        ****************************
	 *
	 *  TODO door opening and button animation
	 *  TODO stop moveTowardPlayer action before it reach THE CENTER of the player
	 *  TODO add : step offset for ennemy => custom box collision with round"ish" edges + custom gravity 
	 *             => NOPE , justs set friction is enough => TOWIGLE
	 *  TODO add shocwave effect to move ennemies
	 * 
	 *  TODO : FIX POSITION when stun
	 *  TODO: add repulsion force during 
	 *  TOFIX bump effect while standing on ennemy => correct physic material & weight for player and ennemy +
	 *           + change way of applying force so it won't force to Y when player is at top
	 *  TOFIX : stop sliding / ice effect for ennemies ( LINKED /\ ? )
	 *  TOFIX when in the corner, friction is acting weird, can jump much higher
	 *  TODO  delete/move a maximum of GetComponent() to a private variable initialized in start()
	 *
	 *  TODO replace collision nested if by switches
	 *  TODO replace List<GameObject> by List<EnnemyBehavior> in EnnemyBehavior
	 *  TODO replace GetComponent<EnnemyBehavior> in removeFromButtonList()
	 *
	 *
	 *        ****************************
	 *        ******* LOW PRIORITY *******
	 *        ****************************
	 *
	 */