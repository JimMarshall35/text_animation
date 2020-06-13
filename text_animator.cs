using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class text_animator : MonoBehaviour
{
    private GameObject[] character_array;
    private bool animate_on;

    [SerializeField] private float period = 0.4f;                                     // period of the sine wave used for moving the text
    [SerializeField] private Vector3 movement_vector = new Vector3(0f, 70f, 0f);      // the amount and direction by which the text moves per period
    [SerializeField] private float waitbetweenletters = 0.1f;                         // the time the script waits in seconds before triggering the next letters movement
    [SerializeField] private float waitafterfinish = 0.5f;                            // the time in seconds the script waits after all the letters of the text have been triggered
    [SerializeField] private bool use_tau = false;                                    // use tau or pi for the sine wave. If tau is used then the letters initial position is the middle of its animation, ie the letters will move "up", then back down to where they started, then down and back up again 
    [SerializeField] private bool play_on_awake = false;                                      

    void Awake()
    {
        animate_on = false;
        character_array = splitTMPRO(GetComponent<TextMeshProUGUI>());

        if (play_on_awake)
        {
            toggleAnimation(true);
        }
    }
    private GameObject[] splitTMPRO(TextMeshProUGUI mytextmesh)
    {
        string mytext = mytextmesh.text;
        GameObject[] array = new GameObject[mytext.Length];
        Vector2 worddims = mytextmesh.GetPreferredValues();
        mytextmesh.rectTransform.sizeDelta = new Vector2(worddims.x, worddims.y);
        //mytextmesh.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, worddims.x);
        RectTransform r = mytextmesh.rectTransform;
        float startx = r.position.x - worddims.x / 2;
        float starty = r.position.y;
        float startz = r.position.z;

        float offset = startx;
        int index = 0;
        foreach (char c in mytext)
        {
            GameObject char_obj = Instantiate(Resources.Load<GameObject>("blank letter"), transform);
            TextMeshProUGUI char_mesh = char_obj.GetComponent<TextMeshProUGUI>();
            char_mesh.SetText(c.ToString());      // set text to correct letter of the original string
            Vector2 dims = char_mesh.GetPreferredValues();
            offset += dims.x;
            Vector3 character_position = new Vector3(offset - dims.x / 2, starty, startz);

            char_obj.transform.position = character_position;     // instantiate blank template as child of this gameobject and set position
            char_mesh.rectTransform.sizeDelta = new Vector2(dims.x, dims.y);
            array[index] = char_obj;
            index++;
        }
        Destroy(mytextmesh); // destroy original text
        return array;
    }
    public void toggleAnimation(bool on) // call to toggle animation on or off
    {
        if (on)
        {
            StartCoroutine(letterAnimator());
        }
        else
        {
            StopCoroutine(letterAnimator());
            animate_on = false;
        }
    }
    private IEnumerator animateSingle(GameObject character) // controls the motion of a single letter
    {

        float movetime = 0f;
        float cycles;
        
        float tau_or_pi;
        if (use_tau)
        {
            tau_or_pi = 2* Mathf.PI;
        }
        else
        {
            tau_or_pi = Mathf.PI;
        }

        Vector3 starting_pos = new Vector3(character.transform.position.x, character.transform.position.y, character.transform.position.z);
        bool move = true;
        while (move)
        {
            yield return null;
            movetime += Time.deltaTime;
            if (movetime > period)
            {
                movetime = period;
            }
            cycles = movetime / period;
            float rawsinewave = Mathf.Sin(cycles * tau_or_pi);
            Vector3 movement = rawsinewave * movement_vector;
            character.transform.position = starting_pos + movement;
            if (movetime == period)
            {
                move = false;
            }
        }
        yield return null;
        
    }
    private IEnumerator letterAnimator() // controls the triggering of each letters animation
    {

        animate_on = true;
        int i = 0;
        while (animate_on)
        {
            StartCoroutine(animateSingle(character_array[i]));
            i++;

            float waittime;
            if(i >= character_array.Length)
            {
                waittime = waitafterfinish;
                i = 0;
            }
            else
            {
                waittime = waitbetweenletters;
            }
            yield return new WaitForSeconds(waittime);
        }
    }

    private IEnumerator testAnimate() // Just my initial test- each letter animates at a time.
    {
        bool animate = true;
        while (animate)
        {
            foreach(GameObject character in character_array)
            {
                float movetime = 0f;
                float cycles;
                bool move = true;
                const float tau = Mathf.PI * 2;

                Vector3 starting_pos = new Vector3(character.transform.position.x, character.transform.position.y, character.transform.position.z);
                while (move)
                {
                    yield return null;
                    movetime += Time.deltaTime;
                    if(movetime > period)
                    {
                        movetime = period;
                    }
                    cycles = movetime / period;
                    float rawsinewave = Mathf.Sin(cycles * Mathf.PI);//tau);
                    Vector3 movement = rawsinewave * movement_vector;
                    character.transform.position = starting_pos + movement;
                    if (movetime == period)
                    {
                        move = false;
                    }
                }
            }
        }
    }
}
