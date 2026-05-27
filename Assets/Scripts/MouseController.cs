using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MouseController : MonoBehaviour
{
    private NPCController activeNPC;
    private Vector3 raycastPosition;
    
    //inspector drag references
    [SerializeField] private Camera cam;

    [Header("UI and Sliders")]
    [SerializeField] private Image propertiesMenu;
    
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider idleSlider;
    [SerializeField] private Slider rerouteSlider;
    [SerializeField] private Slider walkSlider;
    
    [Header("Obstacles")]
    [SerializeField] private Transform[] obstacles = new Transform [5];
    [SerializeField] private int obstacleIndex;
    [SerializeField] private float fallHeight = 10;
    
    
    private void OnLeftClick()
    {
        //if mouse is over UI Element, Do not raycast 
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        //raycast from the camera to mouse position
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        //new layer mask including ground and NPC layers
        int targetedLayers = LayerMask.GetMask("NPC","Ground");
        
        //raycast using targeted layers
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, targetedLayers))
        {
            //check what we hit, go into appropriate functions
            if (hit.collider.CompareTag("NPC"))
            {
                //get a reference to the selected npc
                activeNPC = hit.collider.gameObject.GetComponent<NPCController>();
                ClickNPC();
            }
            else if (hit.collider.CompareTag("Ground"))
            {
                //get the reference to where we clicked in world space
                raycastPosition = hit.point;
                ClickGround();
            }
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

        activeNPC = null;
        propertiesMenu.gameObject.SetActive(false);
    }
    
    

    private void ClickGround()
    {
        SpawnObstacle();
        
        //reset stored variable
        raycastPosition = Vector3.zero;
        
        //go to next obstacle
        obstacleIndex++;
        
        //loop back around to the front of the array
        if (obstacleIndex > obstacles.Length-1)
            obstacleIndex = 0;
    }

    private void SpawnObstacle()
    {
        //store obstacle transform
        var tempTransform = obstacles[obstacleIndex];
        //go to mouse raycast
        tempTransform.position = raycastPosition;
        
        //add fall height
        var tempPos = tempTransform.position;
        tempPos.y = tempTransform.position.y + fallHeight;
        tempTransform.position = tempPos;
        
        //add random rotation
        var tempRot = tempTransform.rotation;
        tempRot = Quaternion.Euler(Random.Range(0,180),Random.Range(0,180),0);
        tempTransform.rotation = tempRot;
        
        //plug temp value back in
        obstacles[obstacleIndex] = tempTransform;
    }



}
