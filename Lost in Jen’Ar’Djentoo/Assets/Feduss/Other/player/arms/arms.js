#pragma strict

var anim : Animator;

var on : boolean = false; 

function Start () {

}

function Update () {




if (Input.GetButtonDown("Equip"))

{

on = true;

anim.SetBool("Equipped",true);

}

if (Input.GetButtonDown("Equip2"))

{

on = false;

anim.SetBool("Equipped",false);

}



if (Input.GetButtonDown("Fire1") && on == true)

{

anim.SetTrigger("Hit");

}


}

function Hit ()

{

Debug.Log("HIT");

}