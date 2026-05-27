using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MouseController : MonoBehaviour
{
    private NPCController activeNPC;

    [Header("UI and Sliders")]
    [SerializeField] private Image propertiesMenu;
    
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider idleSlider;
    [SerializeField] private Slider rerouteSlider;
    [SerializeField] private Slider walkSlider;

    private void OnLeftClick()
    {
        //raycast from the camera to mouse position
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 999f))
        {
            //check what we hit, go into appropriate functions
            if (hit.collider.CompareTag("NPC"))
            {
                //get a reference to the selected npc
                activeNPC = hit.collider.gameObject.GetComponent<NPCController>();
                ClickNPC();
            }
            else if (hit.collider.CompareTag("Ground")) ClickGround();
        }
    }

    private void ClickNPC()
    {
        //if ui menu not opened, open it
        if (!propertiesMenu.gameObject.activeSelf) propertiesMenu.gameObject.SetActive(true);
        SliderPlugIn();
    }
    
    //plug in min, max and main values on each slider
    private void SliderPlugIn()
    {
        PlugMinMax(speedSlider, activeNPC.speedMin,activeNPC.speedMax);
        PlugMinMax(idleSlider, activeNPC.idleMin,activeNPC.idleMax);
        PlugMinMax(walkSlider, activeNPC.walkMin,activeNPC.walkMax);

        speedSlider.value = activeNPC.speed;
        idleSlider.value = activeNPC.idleTime;
        rerouteSlider.value = activeNPC.rerouteChance;
        walkSlider.value = activeNPC.walkTime;
    }

    //gets a slider + two values and plugs them in accordingly
    private void PlugMinMax(Slider slider, float minVal, float maxVal)
    {
        slider.minValue = minVal;
        slider.maxValue = maxVal;
    }

    //function for the UI "Apply" button in menu
    public void ApplyProperties()
    {
        activeNPC.speed = speedSlider.value;
        activeNPC.navAgent.speed = activeNPC.speed;
        activeNPC.idleTime = idleSlider.value;
        activeNPC.rerouteChance = (int)rerouteSlider.value;
        activeNPC.walkTime = walkSlider.value;
        propertiesMenu.gameObject.SetActive(false);
    }

    private void ClickGround()
    {
        
    }



}
