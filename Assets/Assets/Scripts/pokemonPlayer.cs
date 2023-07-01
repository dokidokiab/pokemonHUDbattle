using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class pokemonPlayer : MonoBehaviour
{
    private pokemonEnemy pokemonenemy;
    private BattleSystem BS;


    public string named;
    public int lvl;
    public int xp;
    public float HPmax, HPcurrent, percentual;

    public GameObject[] animations;
    public GameObject buttonA, buttonB, buttonC, buttonD;


    public string[] attacks;

    public AudioSource[] attackSound;
    
    public int[] damage;
    private string speech;
    public int idAttack, hit, idState;
    public int idCommand;
    public Transform HPbar, XPbar;

    private Vector3 vector3;


    // Start is called before the first frame update
    void Start()
    {
        pokemonenemy = FindObjectOfType(typeof(pokemonEnemy)) as pokemonEnemy;
        BS = FindObjectOfType(typeof(BattleSystem)) as BattleSystem;
        
        //HPcurrent = HPmax;

        HPbar = GameObject.Find("HPPlayer").transform;
        XPbar = GameObject.Find("XPPlayer").transform;

        percentual = HPcurrent / HPmax;

        vector3 = HPbar.localScale;
        vector3.x = percentual;
        HPbar.localScale = vector3;

        percentual = xp / 100f ;

        vector3 = XPbar.localScale;
        vector3.x = percentual;
        XPbar.localScale = vector3;



    }

    public void takeDamage(int hit){

        
        HPcurrent -= hit;

        if(HPcurrent <= 0){
            HPcurrent=0;
            GetComponent<SpriteRenderer>().enabled = false;
            }

        percentual = HPcurrent / HPmax;

        vector3 = HPbar.localScale;
        vector3.x = percentual;
        HPbar.localScale = vector3;

        percentual = xp / 100f ;

        vector3 = XPbar.localScale;
        vector3.x = percentual;
        XPbar.localScale = vector3;
    }
    public void renameButtons(){

        buttonA = GameObject.Find("Attack1");
        buttonB = GameObject.Find("Attack2");
        buttonC = GameObject.Find("Attack3");
        buttonD = GameObject.Find("Attack4");

        buttonA.GetComponent<Text>().text = attacks[0];
        buttonB.GetComponent<Text>().text = attacks[1];
        buttonC.GetComponent<Text>().text = attacks[2];
        buttonD.GetComponent<Text>().text = attacks[3];
    
    }

    public IEnumerator attack(){

        GameObject tempPrefab = Instantiate(animations[idCommand]) as GameObject;
        tempPrefab.transform.position = pokemonenemy.transform.position;

        pokemonenemy.transform.GetComponent<Animator>().SetBool("damaged", true);


        hit = Random.Range(1, damage[idCommand]);
        speech = named+" used "+attacks[idCommand]+" and took "+hit+" of damage!";

        attackSound[idCommand].Play();
        
        StartCoroutine(toDialog(speech));

        yield return new WaitForSeconds(1f);
        pokemonenemy.takeDamage(hit);

        Destroy(tempPrefab);

        pokemonenemy.transform.GetComponent<Animator>().SetBool("damaged", false);


        if(pokemonenemy.HPcurrent <= 0){
            idState = 4;
        }
        else{
            idState = 2;
        }

        
    }

    public IEnumerator earnXP(int xpEarned){
        StartCoroutine(toDialog( named+" earned "+xpEarned+" xp!!"));
        idState = 5;

        xp += xpEarned;

        percentual = xp / 100 ;

        vector3 = XPbar.localScale;
        vector3.x = percentual;
        XPbar.localScale = vector3;
        yield return new WaitForSeconds(1);
    }



    public IEnumerator command(int idAttack){
         switch (idAttack)
        {
            
            case 0:
                idCommand = 0;
                speech = named+" use "+attacks[idAttack]+"!";
                StartCoroutine(toDialog(speech));   
                break;

            case 1:
                idCommand = 1;
                speech = named+" use "+attacks[idAttack]+"!";
                StartCoroutine(toDialog(speech));               
                break;

            case 2:
                idCommand = 2;
                speech = named+" use "+attacks[idAttack]+"!";
                StartCoroutine(toDialog(speech));               
                break;

            case 3:
                idCommand = 3;
                speech = named+" use "+attacks[idAttack]+"!";
                StartCoroutine(toDialog(speech));   
                break;
            
            case 4:
                idCommand = 4;
                speech = named+" use "+attacks[idAttack]+"!";
                StartCoroutine(toDialog(speech));   
                break;
            
        }
        
        idState = 1;
        
        return null;
    }


    public IEnumerator toDialog(string txt){
        int letter = 0;
        BS.text.text = "";
        while(letter < txt.Length-1){
            
            BS.text.text += txt[letter];
            letter += 1;

            yield return new WaitForSeconds(0.04f);

        }
        

        yield return new WaitForSeconds(1f);
        switch (idState)
        {
            case 1:
                StartCoroutine(attack());
                break;
            case 2:
                pokemonenemy.StartCoroutine("inicial_attack");
                break;
            case 3:
                BS.text.text = "";
                BS.menuA.SetActive(true);
                break;
            case 4:
                BS.speed = 0;
                BS.amount1 = 0;
                BS.backgroundMusic[0].Stop();
                BS.backgroundMusic[1].Play();
                StartCoroutine(toDialog(pokemonenemy.named+" is defeated!!"));
                idState = 5;
                break;
            case 5:
                StartCoroutine(earnXP(pokemonenemy.xp));
                idState = 6;

                yield return new WaitForSeconds(5);

                Application.Quit();
                UnityEditor.EditorApplication.isPlaying = false;
                break;

                
        }
    }

    public void inicial_command(){
        StartCoroutine(toDialog("What do you wanna do??"));
        
        idState = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
