using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("References")]
    [SerializeField] Camera cam;
    [SerializeField] LayerMask clickableObjects;

    [Header("UI")]
    [SerializeField] Color[] stateColors;
    [SerializeField] Image rotationIcon, moveIcon;
    [SerializeField] GameObject[] moveAxis, rotateAxis;

    [Header("Configurations")]
    [SerializeField] float moveSpeed;
    public bool canMisplace;
    [Range(1,4)]
    public int difficultyLevel;
    [SerializeField] float selectionHoldDuration;

    [Header("Stats")]
    public int gameState;
    public int rotationMode, moveMode;
    public GameObject selectedObject;

    Touch touch;
    Vector3 lastMousePosition;
    float selectTimer;
    bool selecting;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Update()
    {
        if (gameState == 1 && selectedObject != null)
            Move();

        if (selecting)
        {
            selectTimer += Time.deltaTime;

            if (selectTimer >= selectionHoldDuration)
            {
                selecting = false;
            }
        }
    }

    public void ClickObjects()
    {
        var Ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(Ray, out hit, 1000f, clickableObjects))
        {
            if (selectedObject != hit.collider.transform.parent.gameObject)
                SelectObject(hit.collider.transform.parent.gameObject);
            else
            {
                if (gameState == -1)
                    Rotate();
            }
        }
    }

    public void PointerDown()
    {
        selectTimer = 0;
        selecting = true;
    }

    public void PointerUp()
    {
        if (selectTimer > selectionHoldDuration)
            ClickObjects();
    }

    void SelectObject(GameObject obj)
    {
        if (selectedObject != null)
            selectedObject.GetComponentInParent<Piece>().DeselectPiece();

        selectedObject = obj;
        selectedObject.GetComponentInParent<Piece>().SelectPiece();
        UpdateGameState(1);
    }

    void Rotate()
    {
        switch (rotationMode)
        {
            case 0:
                selectedObject.transform.DOBlendableLocalRotateBy(new Vector3(-90, 0, 0), 0.25f);
                break;
            case 1:
                selectedObject.transform.DOBlendableLocalRotateBy(new Vector3(0, -90, 0), 0.25f);
                break;
            case 2:                
                selectedObject.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, 90), 0.25f);
                break;
        }
    }

    void Move()
    {
        if (Input.GetMouseButtonDown(0))
            lastMousePosition = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            Vector2 deltaPosition = Input.mousePosition - lastMousePosition;

            if (deltaPosition.magnitude > 0)
            {
                if (moveMode == 0)
                {
                    selectedObject.transform.Translate(0, deltaPosition.y * moveSpeed, 0);
                }
                else if (moveMode == 1)
                {
                    selectedObject.transform.Translate(deltaPosition.x * moveSpeed, 0, deltaPosition.y * moveSpeed);
                }
            }

            lastMousePosition = Input.mousePosition;
        }
    }

    public void UpdateGameState(int i)
    {
        if(selectedObject != null)
        {
            if (gameState != i)
                gameState = i;

            if (gameState == 1)
            {
                foreach (var axis in rotateAxis)
                {
                    axis.SetActive(false);
                }

                moveIcon.GetComponent<Outline>().enabled = true;
                rotationIcon.GetComponent<Outline>().enabled = false;

                rotationIcon.color = stateColors[0];
                moveIcon.color = stateColors[1];
            }
            if (gameState == -1)
            {
                foreach (var axis in moveAxis)
                {
                    axis.SetActive(false);
                }

                moveIcon.GetComponent<Outline>().enabled = false;
                rotationIcon.GetComponent<Outline>().enabled = true;

                rotationIcon.color = stateColors[1];
                moveIcon.color = stateColors[0];
            }

            UpdaterMoveMode();
            UpdaterRotationMode();
        }
    }

    public void UpdaterRotationMode()
    {
        if(gameState == -1)
        {
            if (rotationMode + 1 < 3)
                rotationMode++;
            else
                rotationMode = 0;

            foreach (var axis in rotateAxis)
            {
                axis.SetActive(false);
            }

            rotateAxis[rotationMode].SetActive(true);
        }
    }

    public void UpdaterMoveMode()
    {
        if (gameState == 1)
        {
            if (moveMode + 1 < 2)
                moveMode++;
            else
                moveMode = 0;

            foreach (var axis in moveAxis)
            {
                axis.SetActive(false);
            }

            moveAxis[moveMode].SetActive(true);
        }
    }
}
