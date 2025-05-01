using UnityEngine;


public class musicAdapt : MonoBehaviour
{
    private RobotController controller;
    public RobotMode currentMode = RobotMode.Balanced;

    public AudioSource source; 

    private AudioClip[] balancedClips;
    private AudioClip[] combatClips;
    private AudioClip[] stealthClips;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<RobotController>();
        balancedClips = Resources.LoadAll<AudioClip>("balanced");
        combatClips = Resources.LoadAll<AudioClip>("combat");
        stealthClips = Resources.LoadAll<AudioClip>("stealth");
    }

    // Update is called once per frame
    void Update()
    {

        if (controller.currentMode != currentMode){
            currentMode = controller.currentMode;

            if (currentMode == RobotMode.Balanced){

                int songChoice = Random.Range(0,balancedClips.Length);
                source.clip = balancedClips[songChoice];
                source.Play();

            }

            if (currentMode == RobotMode.Stealth){

                int songChoice = Random.Range(0,stealthClips.Length);
                source.clip = stealthClips[songChoice];
                source.Play();

            }

            if (currentMode == RobotMode.Combat){

                int songChoice = Random.Range(0,combatClips.Length);
                source.clip = combatClips[songChoice];
                source.loop = true;
                source.Play();

            }
        }
    }
}
