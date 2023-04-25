using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    Material buttonMaterial;
    public Shader shader;
    SFXManager sfxManager;

    public float XSize = 0.3f, YSize = 0.2f;
    public Vector2 Offset;

    Vector3 bound;
    float XboundButton, YboundButton;

    bool Hovering;

    public UnityEvent eventAction;

    public AudioClip OnPressedSound;

    Color CurrentColorButton = Color.white;

    private void Awake()
    {
        if (buttonMaterial == null) buttonMaterial = new Material(shader);

        sfxManager = new SFXManager();
    }

    private void Start()
    {
        buttonMaterial.SetVector("_Offset", Offset);

        bound = new Vector3(
            buttonMaterial.GetVector("_Offset").x,
            buttonMaterial.GetVector("_Offset").y,
            buttonMaterial.GetVector("_Offset").z);

        buttonMaterial.SetFloat("_Width", XSize);
        buttonMaterial.SetFloat("_Height", YSize);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, buttonMaterial);
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        XboundButton = (Camera.main.orthographicSize - bound.x) * buttonMaterial.GetFloat("_Width");
        YboundButton = (Camera.main.orthographicSize - bound.y ) * buttonMaterial.GetFloat("_Height");

        Hovering = Mathf.Abs(ray.origin.x - (bound.x*-10)) < Mathf.Abs(XboundButton) &&
                   Mathf.Abs((ray.origin.y - (bound.y * -10))-1) < Mathf.Abs(YboundButton);


        if (Hovering && !Input.GetMouseButton(0))
        {
            buttonMaterial.SetColor("_ButtonColor", Color.blue);
        }
        else if(Hovering && Input.GetMouseButton(0))
        {
            buttonMaterial.SetColor("_ButtonColor", Color.yellow);
            if (Hovering && Input.GetMouseButtonDown(0))
            {
                eventAction.Invoke();
            }
        }
        else
        {
            buttonMaterial.SetColor("_ButtonColor", CurrentColorButton);
        }
    }

    public void StartGame()
    {
        if(OnPressedSound == null)
        {
            GetComponent<Snake>().running = true;
            GetComponent<Snake>().enabled = false;
            GetComponent<Snake>().enabled = true;

            foreach (Button button in GetComponents<Button>())
            {
                Destroy(button);
            }
        }
        else
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            sfxManager.PlaySFX(source, OnPressedSound, .8f);
            Destroy(source, 2f);
            GetComponent<Snake>().running = true;
            GetComponent<Snake>().enabled = false;
            GetComponent<Snake>().enabled = true;
            foreach (Button button in GetComponents<Button>())
            {
                Destroy(button);
            }
        }
    }

    public void RandomizePlayerColor()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        sfxManager.PlaySFX(source, OnPressedSound, .8f);
        Destroy(source, 2f);
        Snake snakeGame = GetComponent<Snake>();

        if(snakeGame != null)
        {
            CurrentColorButton = Random.ColorHSV();
            snakeGame.playerColor = CurrentColorButton;
            buttonMaterial.SetColor("_ButtonColor", CurrentColorButton);
        }
    }

    public void RandomizeBackgroundColor()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        sfxManager.PlaySFX(source, OnPressedSound, .8f);
        Destroy(source, 2f);
        Snake snakeGame = GetComponent<Snake>();

        if (snakeGame != null)
        {
            CurrentColorButton = Random.ColorHSV();
            snakeGame.backGroundColor = CurrentColorButton;
            buttonMaterial.SetColor("_ButtonColor", CurrentColorButton);
        }
    }

    public void RandomizePointColor()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        sfxManager.PlaySFX(source, OnPressedSound, .8f);
        Destroy(source, 2f);
        Snake snakeGame = GetComponent<Snake>();

        if (snakeGame != null)
        {
            CurrentColorButton = Random.ColorHSV();
            snakeGame.pointColor = CurrentColorButton;
            buttonMaterial.SetColor("_ButtonColor", CurrentColorButton);
        }
    }

}
