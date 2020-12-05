using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeCoolDown : MonoBehaviour
{
    public Image imageCoolDown;
    public float cooldown = 4.5f;
    bool isCoolDown;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            isCoolDown = true;
        }
        if (isCoolDown)
        {
            imageCoolDown.fillAmount += 1 / cooldown * Time.deltaTime;

            if(imageCoolDown.fillAmount >= 1)
            {
                imageCoolDown.fillAmount = 0;
                isCoolDown = false;
            }

        }
    }
}
