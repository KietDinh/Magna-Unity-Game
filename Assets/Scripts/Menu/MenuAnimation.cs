using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public Image cogWheel;
    public Text title;
    public Button start, controls, options, quit;
    public float upDistance;
    bool cogWheelIsClear;
    bool titleIsClear;
    bool menuIsClear;

    float entryTime;
    void Start()
    {
        clearOpacity();
        setClearBoolean();

        title.CrossFadeAlpha(1, 3, false);
        entryTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - entryTime > 3 && titleIsClear == true)
        {
            Vector3 target = new Vector3(title.rectTransform.position.x, title.rectTransform.position.y + upDistance, title.rectTransform.position.z);
            StartCoroutine(MoveTitle(title.rectTransform, title.rectTransform.position, target));
            titleIsClear = false;
        }
        if (Time.time - entryTime > 4.5 && cogWheelIsClear == true)
        {
            cogWheel.CrossFadeAlpha(1, 4, false);
            cogWheelIsClear = false;
        }
        if (Time.time - entryTime > 5 && menuIsClear == true)
        {
            setActiveButtons(true);
            start.GetComponentInChildren<Text>().canvasRenderer.SetAlpha(0f);
            controls.GetComponentInChildren<Text>().canvasRenderer.SetAlpha(0f);
            options.GetComponentInChildren<Text>().canvasRenderer.SetAlpha(0f);
            quit.GetComponentInChildren<Text>().canvasRenderer.SetAlpha(0f);
        }
        if (Time.time - entryTime > 5.5 && menuIsClear == true)
        {

            start.GetComponentInChildren<Text>().CrossFadeAlpha(1, 1, false);
            controls.GetComponentInChildren<Text>().CrossFadeAlpha(1, 1, false);
            options.GetComponentInChildren<Text>().CrossFadeAlpha(1, 1, false);
            quit.GetComponentInChildren<Text>().CrossFadeAlpha(1, 1, false);
            menuIsClear = false;
        }
    }
    void setClearBoolean()
    {
        cogWheelIsClear = true;
        titleIsClear = true;
        menuIsClear = true;
    }
    void clearOpacity()
    {
        cogWheel.canvasRenderer.SetAlpha(0f);
        title.canvasRenderer.SetAlpha(0f);
        setActiveButtons(false);
    }

    void setActiveButtons(bool status)
    {
        start.gameObject.SetActive(status);
        controls.gameObject.SetActive(status);
        options.gameObject.SetActive(status);
        quit.gameObject.SetActive(status);
    }
    private IEnumerator MoveTitle(RectTransform transform, Vector3 from, Vector3 to)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(from, to, Mathf.SmoothStep(0f, 1f, t));
            // Mathf.SmoothStep(0f, 1f, Mathf.SmoothStep(0f, 1f, Mathf.SmoothStep(0f, 1f, t)))
            yield return null;
        }
    }
}
