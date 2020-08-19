using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] Renderer rend;
    [SerializeField] GameObject matchFX;
    [SerializeField] Material[] mats;
    [SerializeField] BoxCollider[] screwHoles, screws;
    public int type;
    bool set;

    private void Start()
    {
        rend = GetComponentInChildren<MeshRenderer>();

        DifficultyAdjustments();
    }

    void DifficultyAdjustments()
    {
        if (screwHoles.Length > 0)
        {
            foreach (var hole in screwHoles)
            {
                hole.size *= GameManager.instance.difficultyLevel;
            }
        }

        if (screws.Length > 0)
        {
            foreach (var screw in screws)
            {
                screw.size *= GameManager.instance.difficultyLevel;
            }
        }
    }

    public void SelectPiece()
    {
        if (!set)
            rend.material = mats[1];
    }

    public void DeselectPiece()
    {
        rend.material = mats[0];
    }

    public void ScrewIn(Transform receiver, Transform screw)
    {
        GameObject fx = Instantiate(matchFX, screw.position, screw.rotation);
        Destroy(fx, 2f);

        transform.SetParent(receiver);
        transform.localPosition = Vector3.zero;
        DeselectPiece();
        GameManager.instance.UpdateGameState(0);
        set = true;
    }
    
}
