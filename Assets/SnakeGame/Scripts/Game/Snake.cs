using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SnakeDirection { UP, DOWN, LEFT, RIGHT }
public class Snake : MonoBehaviour
{
    Texture2D gameWindow;
    float timer;
    SFXManager soundManager;

    [HideInInspector]public int[] x;
    [HideInInspector]public int[] y;
    Vector2 currentPointPos;

    int bodyCount = 1;

    public Color backGroundColor = Color.black, playerColor = Color.white, pointColor = Color.yellow;

    public float maxTimeUpdate = 0.17f;

    public int gameUnitsSize = 60;
    SnakeDirection currentDir;

    [HideInInspector]public bool running = false;

    public AudioClip pointSound, DeadSound;

    private void Awake()
    {
        soundManager = new SFXManager();
        Camera.main.backgroundColor = backGroundColor;
        Screen.SetResolution(800, 800, FullScreenMode.Windowed);
    }

    private void OnEnable()
    {
        if (running)
        {
            InitializeGame();
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        CheckInput();

        if(timer >= maxTimeUpdate && running)
        {
            CheckIfNewPoint();
            MoveSnake();
            PaintBackground();
            PaintPoint();
            PaintPlayer();
            CheckPlayerOutOfBounds();

            gameWindow.Apply();

            timer = 0;
        }
    }
    private void AccelerateTimerUpdateTime()
    {
        maxTimeUpdate = maxTimeUpdate * 90 / 100f;

        if (maxTimeUpdate <= 0.07f)
        {
            maxTimeUpdate = 0.04f;
        }
    }
    void InitializeGame()
    {
        running = true;
        CreateGameWindow();
        PaintBackground();
        RandomizePointPos();
        PaintPoint();
        SetPixelsPositionsArray();
        PaintPlayer();
        gameWindow.Apply();
    }

    private void PaintPlayer()
    {
        for (int i = 0; i < bodyCount; i++)
        {
            if(i == 0)
            {
                gameWindow.SetPixel(x[i], y[i], playerColor);
            }
            else
            {
                gameWindow.SetPixel(x[i], y[i], Color.Lerp(backGroundColor * 80 / 100, playerColor * 90/100, (bodyCount - i) * 0.05f));
            }

            if (bodyCount > 1)
            {
                if (x[0] == x[i + 1] && y[0] == y[i + 1])
                {
                    AudioSource source = CreateAudioSource();
                    soundManager.PlaySFX(source, DeadSound, 0.8f);
                    Destroy(source, 2f);
                    running = false;
                }
            }
        }
    }

    void CheckIfNewPoint()
    {
        for (int i = 0; i < bodyCount; i++)
        {
            if (x[0] == currentPointPos.x && y[0] == currentPointPos.y)
            {
                AudioSource source = CreateAudioSource();
                soundManager.PlaySFX(source, pointSound, 0.8f);
                Destroy(source, 2f);
                RandomizePointPos();
                if(currentPointPos.x == x[i] && currentPointPos.y == y[i])
                {
                    RandomizePointPos();
                }
                AccelerateTimerUpdateTime();
                bodyCount++;
            }
        }
    }
    void CheckPlayerOutOfBounds()
    {
        if(x[0] > gameWindow.width || x[0] < 0 ||
            y[0] > gameWindow.height || y[0] < 0)
        {
            AudioSource source = CreateAudioSource();
            soundManager.PlaySFX(source, DeadSound, 0.8f);
            Destroy(source, 2f);
            running = false;
        }
            
    }

    void CreateGameWindow()
    {
        gameWindow = new Texture2D((int)Screen.width / gameUnitsSize, (int)Screen.height / gameUnitsSize, TextureFormat.RGB24, false);

        gameWindow.filterMode = FilterMode.Point;
        gameWindow.wrapMode = TextureWrapMode.Clamp;
    }

    void PaintBackground()
    {
        for (int i = 0; i < gameWindow.width; i++)
        {
            for (int j = 0; j < gameWindow.height; j++)
            {
                gameWindow.SetPixel(i, j, backGroundColor);
            }
        }
    }

    void PaintPoint()
    {
        gameWindow.SetPixel((int)currentPointPos.x, (int)currentPointPos.y, pointColor);
    }

    void RandomizePointPos()
    {
        int x = Random.Range(0, gameWindow.width);
        int y = Random.Range(0, gameWindow.height);
        currentPointPos = new Vector2(x, y);
    }

    void SetPixelsPositionsArray()
    {
        int gameUnits = (Screen.width * Screen.height) / gameUnitsSize;
        x = new int[gameUnits];
        y = new int[gameUnits];
    }

    private void MoveSnake()
    {
        for (int i = bodyCount - 1; i > 0; i--)
        {
            x[i] = x[i - 1];
            y[i] = y[i - 1];
        }

        switch (currentDir)
        {
            case SnakeDirection.UP:
                y[0] = y[0] + 1;
                break;
            case SnakeDirection.DOWN:
                y[0] = y[0] - 1;
                break;
            case SnakeDirection.LEFT:
                x[0] = x[0] - 1;
                break;
            case SnakeDirection.RIGHT:
                x[0] = x[0] + 1;
                break;
        }
    }

    void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) && currentDir != SnakeDirection.DOWN)
        {
            currentDir = SnakeDirection.UP;
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow) && currentDir != SnakeDirection.UP)
        {
            currentDir = SnakeDirection.DOWN;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && currentDir != SnakeDirection.RIGHT)
        {
            currentDir = SnakeDirection.LEFT;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && currentDir != SnakeDirection.LEFT)
        {
            currentDir = SnakeDirection.RIGHT;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(gameWindow, destination);
    }

    AudioSource CreateAudioSource()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        return source;
    }
}
