using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private StageData stageData;
    [SerializeField]
    private KeyCode keyCodeAttack = KeyCode.Space;
    private Movement2D movement2D;
    private Weapon weapon;

    private void Awake(){
        movement2D = GetComponent<Movement2D>();
        weapon = GetComponent<Weapon>();
    }

    private void Update(){

        float x=Input.GetAxisRaw("Horizontal");
        float y=Input.GetAxisRaw("Vertical");
        
        movement2D.MoveTo(new Vector3(x,y,0));

        if(Input.GetKeyDown(keyCodeAttack)){
            weapon.StartFiring();
        }
        else if(Input.GetKeyUp(keyCodeAttack)){
            weapon.StopFiring();
        }
    }

    private void LateUpdate(){

        transform.position=new Vector3(Mathf.Clamp(transform.position.x,stageData.LimitMin.x,stageData.LimitMax.x),Mathf.Clamp(transform.position.y,stageData.LimitMin.y,stageData.LimitMax.y));
    }
}
