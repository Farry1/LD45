using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputController : MonoBehaviour
{
    void Update()
    {
        //TODO: this can be improved. I'm sure.

        // Get Mouse Input
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Cast a Ray through all Objects to shoot through Indicator Plane of Nodes. If Ray hits "Tile"-Layer, get the coresponding node
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity).OrderBy(h => h.distance).ToArray();

        CheckPlayerNavigation(ray);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            Unit selectedPlayerUnit = PlayerUnitController.Instance.selectedPlayerUnit;

            //If mouse is over a Tile
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Tile"))
            {
                //Check which side of the tile was hit
                Transform objectHit = hit.transform;
                Ray objectSideRay = new Ray(objectHit.transform.position, hit.normal);
                // Debug.DrawRay(clickSide.origin, clickSide.direction * 1.5f, Color.red, 5f);

                // Get the node that is in that direction
                RaycastHit objectSideHit;
                if (Physics.Raycast(objectSideRay, out objectSideHit, 1f))
                {
                    Node hitNode = objectSideHit.collider.gameObject.GetComponent<Node>();
                    if (hitNode != null && selectedPlayerUnit != null)
                    {
                        selectedPlayerUnit.GeneratePathTo(hitNode);
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit leftClickHit;
            if (Physics.Raycast(ray, out leftClickHit))
            {
                //A Selectable is clicked
                ISelectable selectable = leftClickHit.collider.GetComponentInParent<ISelectable>();
                if (selectable != null)
                {
                    selectable.OnSelect();
                }
            }
            // TileMap.Instance.selectedUnit.Move();
        }

        void CheckPlayerNavigation(Ray navigateRay)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (PlayerUnitController.Instance.selectedPlayerUnit != null &&
                    PlayerUnitController.Instance.selectedPlayerUnit.currentPath != null)
                {
                    RaycastHit navigateToHit;
                    if (Physics.Raycast(navigateRay, out navigateToHit))
                    {
                        PlayerUnitController.Instance.selectedPlayerUnit.Move();
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            PlayerUnitController.Instance.UnselectSelectedUnits();
        }
    }

    public void OnSelectMoveAction()
    {
        PlayerUnitController.Instance.EnableMoveAction();
    }
}
