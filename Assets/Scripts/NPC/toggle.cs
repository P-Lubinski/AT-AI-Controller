using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggle : MonoBehaviour
{
    private Movement script;                    // Script responsible for moving the AI
    private Light light;                        // Light component on each AI representing if they're selected or not

	void Start ()
    {
        script = GetComponent<Movement>();
        light = GetComponent<Light>();
	}

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo); // Using raycast to see what Player clicked on
            if (hit)
            {
                if (hitInfo.transform.gameObject.tag == "AI")       // If player clicked on an AI character
                {
                    if (script.enabled)                             // and the Movement script is enabled
                    {
                        this.script.enabled = false;                // disable that script
                        this.light.enabled = false;                 // and disable the light indicating deselection of the AI
                    }
                    else if (!script.enabled)                       // if script is diabled
                    {
                        this.script.enabled = true;                 // enable it
                        this.light.enabled = true;                  // and indicate with a light that this AI character is now active
                    }
                }
            }
        }
    }
}
