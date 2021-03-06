using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Gamemanager : MonoBehaviour
{
    [HideInInspector] public bool MoveByTouch, StartTheGame,EndGame;
    private Vector3 _mouseStartPos, PlayerStartPos;
    [SerializeField] private float RoadSpeed, SwipeSpeed, Distance;
    public static Gamemanager GameManagerInstance;

    [SerializeField] Text Scor_Text;
    public int scor = 0;
    [SerializeField] GameObject start_text;
    [SerializeField] private Camera mine;
    [SerializeField] private Camera animcamera;
    [SerializeField] private Rigidbody enmy;

    public TimeManager slow;



    [SerializeField] bool start;
    [SerializeField] GameObject Game_over_panel;




    void Start()
    {
        GameManagerInstance = this;
        Scor_Text.text = scor.ToString();
        start = true;
        EndGame = false;
        animcamera.enabled = false;
        Game_over_panel.SetActive(false);
    }

    void Update()
    {
        if (!EndGame)
        {


            if (Input.GetMouseButtonDown(0))
            {
                StartTheGame = MoveByTouch = true;
                Debug.Log("true");
                if (start)
                {
                    start_text.SetActive(false);
                    start = false;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                MoveByTouch = false;
                Debug.Log("false");
            }

            if (MoveByTouch)
            {
                var plane = new Plane(Vector3.up, 0f);

                float distance;

                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (plane.Raycast(ray, out distance))
                {
                    Vector3 mousePos = ray.GetPoint(distance);
                    Vector3 desirePso = mousePos - _mouseStartPos;
                    Vector3 move = PlayerStartPos + desirePso;

                    move.x = Mathf.Clamp(move.x, -3.2f, 3.2f);
                    move.z = -7f;

                    var player = transform.position;

                    player = new Vector3(Mathf.Lerp(player.x, move.x, Time.deltaTime * (SwipeSpeed + 10f)), player.y, player.z);

                    transform.position = player;
                }
            }

            if (StartTheGame)
            {
               
                gameObject.transform.Translate(Vector3.forward * (RoadSpeed * Time.deltaTime));

              

            }
              

        }

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("scor"))
        {

            scor += 1;
            Destroy(other.gameObject);
            Scor_Text.text = scor.ToString();
        }

        if (other.CompareTag("wall"))
        {
            if (scor>=1)
            {
                scor -= 1;
                Scor_Text.text = scor.ToString();
            }
            else
            {
                Game_over();
                   

            }

            Destroy(other.gameObject);


        }

        if (other.CompareTag("end"))
        {
            EndGame = true;
            Invoke("Game_ended", 3f);
        }
    }   
    void Game_over()
    {
        EndGame = true;
        Game_over_panel.SetActive(true);

    }

  public  void restartbrn()
    {
        SceneManager.LoadScene("SampleScene");

    }

    void Game_ended()
    {
        enmy.AddForce(new Vector3(0, 300, scor*50));
        mine.enabled = false;
        animcamera.enabled = true;
    }
 

    }
