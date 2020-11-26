using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System.Linq;

public class People : MonoBehaviour, Photon.Pun.IPunObservable
{
    public GameObject ArrowPrefab;
    public GameObject effectPrefab;
    public GameObject effect;
    GameObject targetEnemy;
    public float ShootWaitTime = 2f;
    public float ShootTimes = 1.0f;
    private float LastShootTime = 0f;
    private float InitialArrowForce = 50f;
    public int createCost;
    public float arrowDamage;
    public int level=1;
    public int attackType;
    Animator ator;
    private PeopleState currentPeopleState;

    // Start is called before the first frame update
    void Start()
    {
        //TODO************************************
        currentPeopleState = PeopleState.Searching;
        // ArrowPosition = transform.Find
    }

    // Update is called once per frame
    void Update()
    {
        ator = this.gameObject.GetComponent<Animator>();
        //if we're in the last round and we've killed all enemies, do nothing
        if(GameManager.Instance.FinalRoundFinished && GameManager.Instance.Enemies.Where(x=>x!=null).Count()==0){
            currentPeopleState = PeopleState.Inactive;
        }

        //searching for an enemy
        if(currentPeopleState==PeopleState.Searching){
            if(GameManager.Instance.Enemies.Where(x=>x!=null).Count()==0){
                return;
            }
            // find the closest ememy
            targetEnemy = GameManager.Instance.Enemies.Where(x=>x!=null)
            .Aggregate((current,next)=>Vector3.Distance(current.transform.position,transform.position)
            <Vector3.Distance(next.transform.position,transform.position)?current:next);

            //if there is an enemy and is close to us
            if(targetEnemy!=null && targetEnemy.activeSelf && Vector3.Distance(transform.position,targetEnemy.transform.position)<Constants.MinDistanceForPeopleToShoot){
                currentPeopleState = PeopleState.Targeting;
            }
        }else if(currentPeopleState == PeopleState.Targeting){
            // if the targeted enemy is still close to us, look at it and shoot
            if (targetEnemy!=null && Vector3.Distance(transform.position,targetEnemy.transform.position)<Constants.MinDistanceForPeopleToShoot){
                LookAndShoot();
            }else{
                currentPeopleState = PeopleState.Searching;
            }
        }
    }

    public void Disable(){
        CancelInvoke();
        ator.SetBool("isPunching_Left",false);
    }

    private void LookAndShoot(){
        //look at the enemy
        Quaternion diffRotation = Quaternion.LookRotation(targetEnemy.transform.position-transform.position,Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation,diffRotation,Time.deltaTime*2000);
        transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
        //make sure we're almost looking at the enemy before we start shooting
        Vector3 direction = targetEnemy.transform.position - transform.position;
        float axisDif = Vector3.Angle(transform.forward,direction);
        // shoot only if we have 20 degrees rotation difference
        if(axisDif <= 20f){
            if(Time.time - LastShootTime > ShootWaitTime*ShootTimes){
                Shoot(direction);
                LastShootTime = Time.time;
            }
        }
    }

    private void Shoot(Vector3 dir){
        //if the enemy is still close to us
        if(targetEnemy != null && targetEnemy.activeSelf &&Vector3.Distance(transform.position,targetEnemy.transform.position)<Constants.MinDistanceForPeopleToShoot){
            // create a new arrow
            CancelInvoke();
            Invoke("Disable",1f);   
            ator.SetBool("isPunching_Left",true);
            GameObject go;
            if(attackType==3){
                //go = Instantiate(ArrowPrefab,targetEnemy.transform);
                go = GameNetworkManager.KkInstantiate(ArrowPrefab, targetEnemy.transform.position, targetEnemy.transform.rotation);
            }
            else{
                //go = Instantiate(ArrowPrefab,transform.position,transform.rotation);
                go = GameNetworkManager.KkInstantiate(ArrowPrefab, transform.position, transform.rotation);
            }
            ((Arrow)go.GetComponent(typeof(Arrow))).ChangeTarget(targetEnemy,attackType);
            go.name = ((int)(arrowDamage)).ToString();
            // Shoot it!
            AudioManager.Instance.PlayArrowSound();
            if(attackType==2) GameManager.AlterMoney(20);
        }else{
            currentPeopleState = PeopleState.Searching;
        }
    }

    public int getLevel(){
        return level;
    }

    public void updateLevel(Transform trans){
        if(level>1) return;
        level += 1;
        createEffect(trans);
        arrowDamage = 1.25f*arrowDamage;
    }

    public void createEffect(Transform trans){
        //Instantiate(effectPrefab,trans);
        
        effect = GameNetworkManager.KkInstantiate(effectPrefab,trans.position, Quaternion.Euler(90,0,0));
    }

    public int getCreateCost(){
        return createCost;
    }

    public void alterCreateCost(int alter){
        createCost += alter;
    }

    public void remove(){
        GameNetworkManager.KkDestroy(this.gameObject);
        GameNetworkManager.KkDestroy(effect);
    }

    public void MulShootWaitTimes(float mul,float time){
        ShootTimes *= mul;
        Invoke("resetShootTime",time);
    }

    public void resetShootTime(){
        ShootTimes = 1.0f;
    }


    #region IPunObservable implementation

    public void OnPhotonSerializeView(Photon.Pun.PhotonStream stream, Photon.Pun.PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext(level);
        }
        else
        {
            this.level = (int)stream.ReceiveNext();  
        }
    }

    #endregion
}
