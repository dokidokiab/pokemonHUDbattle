using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pokemonEnemy : MonoBehaviour
{
    private pokemonPlayer pokemonplayer;
    private BattleSystem BS;

    public string named;
    public int lvl;
    public int xp;
    public float HPmax, HPcurrent, percentual;
    public string[] attacks;
    public AudioSource[] attackSound;

    public int[] damage;

    public GameObject[] animations;

    private string speech;
    public Transform HPbar;

    private Vector3 vector3;

    private int idAttack, idCommand, hit, idState;

    // Start is called before the first frame update
    void Start()
    {
        pokemonplayer = FindObjectOfType(typeof(pokemonPlayer)) as pokemonPlayer;
        BS = FindObjectOfType(typeof(BattleSystem)) as BattleSystem;
        
        //HPcurrent = HPmax;

        

        HPbar = GameObject.Find("HPEnemy").transform;
        percentual = HPcurrent / HPmax;

        vector3 = HPbar.localScale;
        vector3.x = percentual;
        HPbar.localScale = vector3;

        xp = 35;



    }

    void Update(){
        //BS.ShakeUnitDown(this.transform, -0.1f);
        //BS.ShakeUnitUp(this.transform, 0.1f);
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

    }

    public IEnumerator inicial_attack(){
        int idAttack = Random.Range(0, attacks.Length);


        yield return new WaitForSeconds(2f);

        StartCoroutine(command(idAttack));

    }

    public IEnumerator attack(){
        GameObject tempPrefab = Instantiate(animations[idCommand]) as GameObject;
        tempPrefab.transform.position = pokemonplayer.transform.position;


        pokemonplayer.transform.GetComponent<Animator>().SetBool("damaged", true);



        hit = Random.Range(1, damage[idCommand]);
        speech = named+" used "+attacks[idCommand]+" and took "+hit+" of damage!";

        attackSound[idCommand].Play();

        
        StartCoroutine("toDialog", speech);
        yield return new WaitForSeconds(1f);

        pokemonplayer.takeDamage(hit);
        Destroy(tempPrefab);

        pokemonplayer.transform.GetComponent<Animator>().SetBool("damaged", false);


        if(pokemonplayer.HPcurrent <= 0){
            BS.speed = 0;
            BS.amount1 = 0;
            StartCoroutine(toDialog(pokemonplayer.named+" is defeated!!"));
            idState = 4;

            yield return new WaitForSeconds(5);

            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else{
            idState = 2;
        }
        
        

    }
    public void updateHP(int hit){
        HPcurrent -= hit;

        percentual = HPcurrent / HPmax;

        vector3 = HPbar.localScale;
        vector3.x = percentual;
        HPbar.localScale = vector3;
    }

    public IEnumerator command(int idAttack){
         switch (idAttack)
        {
            
            case 0:
                idCommand = 0;
                StartCoroutine(attack());
                break;

            case 1:
                idCommand = 1;
                StartCoroutine(attack());
                break;

            case 2:
                idCommand = 2;
                StartCoroutine(attack());
                break;

            case 3:
                idCommand = 3;
                StartCoroutine(attack());
                break;
            
            case 4:
                idCommand = 4;
                StartCoroutine(attack());
                break;        
        }
        
        yield return new WaitForSeconds(1f);

    }


    public IEnumerator toDialog(string txt){
        int letter = 0;
        BS.text.text = "";
        while(letter < txt.Length-1){
            BS.text.text += txt[letter];
            letter += 1;

            yield return new WaitForSeconds(0.04f);
        }

        yield return new WaitForSeconds(2f);

        switch (idState)
        {
            case 1:
                StartCoroutine(attack());
                break;

            case 2:
                pokemonplayer.inicial_command();
                break;

            case 3:
                break;

            case 4:
                yield return new WaitForSeconds(1f);
                break;

            

            
        }
    }
    
}
