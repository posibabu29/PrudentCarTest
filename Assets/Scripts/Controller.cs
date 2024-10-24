using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public CSVLoader csvLoader;
    [SerializeField]
    GameObject Car, OrbitObj,ButtonObj,DescriptionPanel;

    [SerializeField]
    Transform ButtonContainer;

    [SerializeField]
    TextMeshProUGUI descriptionText,PartNameText;

    int NumberOfChildren;

    GameObject SelectedPart;
    
    void Start()
    {
        csvLoader = this.GetComponent<CSVLoader>();
        DescriptionPanel.SetActive(false);
        NumberOfChildren = Car.transform.childCount;
        AddColliders(Car);
        CreatingButtons();

    }
    void AddColliders(GameObject car)
    {
        foreach (Transform child in car.transform)
        {
            if (child.GetComponent<MeshCollider>() == null)
            {
                child.gameObject.AddComponent<MeshCollider>();
                child.gameObject.AddComponent<Outline>();
                child.GetComponent<Outline>().enabled = false;
            }
        }
    }

    //void PartsSelection(Transform child)
    //{     

    //    EventTrigger eventTrigger = child.gameObject.AddComponent<EventTrigger>();

    //    // Create and set up the PointerClick event
    //    EventTrigger.Entry clickEntry = new EventTrigger.Entry
    //    {
    //        eventID = EventTriggerType.PointerClick
    //    };

    //    clickEntry.callback.AddListener((eventData) => OnPointerClick(child));
    //    eventTrigger.triggers.Add(clickEntry);

    //}
    //void OnPointerClick(Transform obj)
    //{
    //    Debug.Log("Clicked");
    //    if (SelectedPart != null)
    //    {
    //        ResetSelectionParts();
    //    }
    //    obj.GetComponent<Outline>().enabled = true;
    //    SelectedPart = obj.gameObject;
    //}

    void ResetSelectionParts()
    {
        SelectedPart.GetComponent<Outline>().enabled = false;
    }
  
    void CreatingButtons() 
    {
        for (int i = 0; i < NumberOfChildren; i++)
        {
            GameObject newButton = Instantiate(ButtonObj);
            newButton.transform.SetParent(ButtonContainer, false);
            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            GameObject part = Car.transform.GetChild(i).gameObject;
            if (buttonText != null)
            {
                buttonText.text = part.name; 
            }
            Button button = newButton.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => OnButtonClick(part, button));
            }
        }
    }




    void OnButtonClick(GameObject part, Button button)
    {
        if (part.activeSelf)
        {
            part.SetActive(false);
            button.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            part.SetActive(true);
            button.GetComponent<Image>().color = Color.white;
        }

    }

    void Update()
    {
       
        if (Input.GetMouseButtonDown(0)) 
        {
            HandleLeftClick();
        }
        else if (Input.GetMouseButtonDown(1)) 
        {
            HandleRightClick();
        }

        if(SelectedPart != null)
        {
            Vector3 p = OrbitObj.GetComponent<orbit>().target.transform.position;
            p = Vector3.Lerp(p, SelectedPart.transform.position,Time.deltaTime);
            OrbitObj.GetComponent<orbit>().target.transform.position = p;
        }
        else
        {
            Vector3 p = OrbitObj.GetComponent<orbit>().target.transform.position;
            p = Vector3.Lerp(p, Vector3.zero, Time.deltaTime);
            OrbitObj.GetComponent<orbit>().target.transform.position = p;
        }
    }

    void HandleLeftClick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
           GameObject Obj = hit.collider.gameObject;
          //  Debug.Log("Left mouse button clicked on: " + Obj.name);

            if (SelectedPart != null)
            {
                ResetSelectionParts();
            }

            if ((Obj != SelectedPart) && (Obj.GetComponent<Outline>().enabled == false))
            {
                Obj.GetComponent<Outline>().enabled = true;
                SelectedPart = Obj.gameObject;

                string description = csvLoader.GetDescription(Obj.name);
                descriptionText.text = description;
                PartNameText.text = Obj.name;
                Debug.Log(description);
                DescriptionPanel.SetActive(true);
            }
            else
            {
                Obj.GetComponent<Outline>().enabled = false;
                SelectedPart = null;
                DescriptionPanel.SetActive(false);
                //  OrbitObj.GetComponent<orbit>().transform.position = Vector3.zero;
            }
            

        }
    }

    void HandleRightClick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            GameObject Obj = hit.collider.gameObject;
            Obj.SetActive(false);

            int childIndex = Obj.transform.GetSiblingIndex();
            ButtonContainer.GetChild(childIndex).GetComponent<Image>().color = Color.gray;

        }
    }

}
