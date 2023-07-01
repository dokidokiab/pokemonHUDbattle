using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    public Transform pauseMenu, ConfirmMenu;
    private pokemonPlayer pokemonplayer;
    private pokemonEnemy pokemonenemy;
    public GameObject menuA, menuB;
    public AudioSource[] backgroundMusic;

    public float x, y, z, amount1, speed, infoEinicial, infoPinicial;

    public int idState;
    public string talk;                                                                         
    public Text text;

    private Transform trainer, pokemonP, pokemonE, posA, posB, infoPlayer, infoEnemy;

    private Vector3 pokemonPPosition, pokemonEPosition;

    // Start is called before the first frame update
    void Start()
    {

        backgroundMusic[0].Play();

        pokemonplayer = FindObjectOfType(typeof(pokemonPlayer)) as pokemonPlayer;
        pokemonenemy = FindObjectOfType(typeof(pokemonEnemy)) as pokemonEnemy;

        trainer = GameObject.Find("Player").transform;
        posA = GameObject.Find("posA").transform;
        posB = GameObject.Find("posB").transform;
        infoPlayer = GameObject.Find("infoPlayer").transform;
        infoEnemy = GameObject.Find("infoEnemy").transform;
        
        pokemonP = pokemonplayer.transform;
        pokemonE = pokemonenemy.transform;

        pokemonPPosition = pokemonplayer.transform.position;
        pokemonEPosition = pokemonenemy.transform.position;
        pokemonEPosition.x = pokemonenemy.transform.position.x;

        infoPinicial = infoPlayer.position.y;
        infoEinicial = infoEnemy.position.y;

        menuA.SetActive(false);
        menuB.SetActive(false);

        idState = 0;
        speed = 2f;
        amount1 = .15f;

        talk = "A "+ pokemonenemy.named + " appeared out of nowhere!";

        StartCoroutine(toDialog(talk));
    }

    // Update is called once per frame
    void Update()
    {
        if(idState == 1){//to animate trainer go out and chikorita go in
            trainer.GetComponent<Animator>().SetBool("throw", true);

            float step = 75 * Time.deltaTime; //vel de movimento
            trainer.position = Vector3.MoveTowards(trainer.position, posA.position, step);
            
            pokemonP.position = Vector3.MoveTowards(pokemonP.position, posB.position, step);

            pokemonPPosition.x = pokemonplayer.transform.position.x;
            
        }
        else if(idState > 1){
            
            x = pokemonPPosition.x+Mathf.Sin(Time.time*-speed)*amount1;
            y = pokemonplayer.transform.position.y;
            z = pokemonplayer.transform.position.z;

            pokemonplayer.transform.position = new Vector3(x, y, z);
            
            x = pokemonEPosition.x+Mathf.Sin(Time.time*speed)*amount1;
            y = pokemonenemy.transform.position.y;
            z = pokemonenemy.transform.position.z;

            pokemonenemy.transform.position = new Vector3(x, y, z);

            x = infoPlayer.position.x;
            y = infoPinicial+Mathf.Sin(Time.time*speed)*.1f;

            infoPlayer.position = new Vector3(x, y, 0);

            x = infoEnemy.position.x;
            y = infoEinicial+Mathf.Sin(Time.time*-speed)*.1f;

            infoEnemy.position = new Vector3(x, y, 0);
            }



        if(Input.GetKeyDown(KeyCode.Escape)){
            if(pauseMenu.gameObject.activeSelf){
                pauseMenu.gameObject.SetActive(false);
                Time.timeScale = 1;

            }
            else{
                pauseMenu.gameObject.SetActive(true);
                Time.timeScale = 0;

            }
        }

        
        
        
            /*
            float step = 75 / Time.deltaTime; //vel de movimento
            Vector3 position1 = new Vector3(pokemonP.position.x, pokemonP.position.y+2, pokemonP.position.z);
            Vector3 position2 = pokemonP.position;
            if (y_player == 2){
                
                pokemonP.position = Vector3.MoveTowards(position2, position1, step);

                y_player = 0;
            }else
            {
                pokemonP.position = Vector3.MoveTowards(position1, position2, step);
                y_player = 2;
            }}
            

            
        
    }

    public void ShakeUnitDown(Transform unit, float minus){
        Vector3 position2 = new Vector3(unit.position.x, unit.position.y-minus, unit.position.z);
        Vector3 position1 = new Vector3(unit.position.x, unit.position.y, unit.position.z);

        float step = 5 / Time.deltaTime; //vel de movimento

        
        unit.position = Vector3.MoveTowards(position1, position2, step);
        
    }

    public void ShakeUnitUp(Transform unit, float plus){
        Vector3 position2 = new Vector3(unit.position.x, unit.position.y+plus, unit.position.z);
        Vector3 position1 = new Vector3(unit.position.x, unit.position.y, unit.position.z);

        float step = 5 / Time.deltaTime; //vel de movimento

        
        unit.position = Vector3.MoveTowards(position1, position2, step);
     */   
    }

    public IEnumerator toDialog(string txt){
        int letter = 0;
        text.text = "";
        while(letter < txt.Length-1){
            
            text.text += txt[letter];
            letter += 1;

            yield return new WaitForSeconds(0.05f);

        }

        yield return new WaitForSeconds(1);

        idState += 1;
               
        //Determina o que acontecerá pós 
        switch (idState)
        {
            
            case 1:
                talk = "Go "+pokemonplayer.named+"!";
                StartCoroutine(toDialog(talk));
                break;

            case 2:
                pokemonplayer.inicial_command();
                break;


        }

    }
    
    //Função que troca os menus e renomeia de acordo com os ataques do pokemon
    public void toFight(){

        menuA.SetActive(false);
        menuB.SetActive(true);

        pokemonplayer.renameButtons();

    }

    //Função do botão de ataque
    public void toAttack(int idAttack){
        menuB.SetActive(false);

        pokemonplayer.StartCoroutine("command", idAttack);
    }

    //Botão de retornar ao jogo
    public void BttnResume(){
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    //Botão de saída
    public void BttnExit(){
        ConfirmMenu.gameObject.SetActive(true);
    }

    public void yesButton(){
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void noButton(){
        ConfirmMenu.gameObject.SetActive(false);
    }

}
