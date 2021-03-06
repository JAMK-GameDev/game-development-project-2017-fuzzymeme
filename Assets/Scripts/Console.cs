﻿using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Interface;
using Assets.Scripts.Weapon_Inventary;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Assets.Script;
using UnityEngine.AI;
using UnityEngine.Assertions.Comparers;

public class Console : MonoBehaviour
{

    const int NormalHash = -2016234814;
    const int GodHash = 2071335238;
    const int DropSomethingHash = -262007715;
    const int HalfGodHash = -208539993;
    const int LaterHash = 2071488744;
    const int NotAvailableHash = -210791430;
    const int ShowMeTheBossHash = 649304737;
    const int OpenHash = -371437091;
    const int FpsCounter = 101609;
    const int FreeBeer = -489312271;
    const int FreeBeerForAYear = -86716964;
    const int NoFreeBeer = 1230423493;

    private GameManager gameManager;


    private static InputField consoleInput;
    private RectTransform rectTransform;

    private Text messageBoxText;
    private GameObject messageBoxBorder;

    private GameObject fpsCounter;
    private bool fps = false;

    private Weapon stick;
    private Weapon godsFist;

    private bool consoleVisible = false;
    private float lastPressedButtonTime = 0;
	// Use this for initialization

    private static Console instance = null;
	void Start ()
	{

	    instance = this;
        GameObject find = GameObject.Find("ConsoleInput");
        consoleInput=find.GetComponent<InputField>();
	    consoleInput.enabled = false;
	    rectTransform = find.GetComponent<RectTransform>();
	    rectTransform.localScale = new Vector3(0,0,0);

        consoleInput.onEndEdit.AddListener(AnalyseCommand);
        GameObject messageBox = GameObject.Find("MessageBox");
	    messageBoxText = messageBox.GetComponent<Text>();
	    messageBoxBorder = messageBox.transform.parent.gameObject;
	    messageBoxBorder.SetActive(false);
        fpsCounter = GameObject.FindGameObjectWithTag("FPS");
        fpsCounter.SetActive(false);

    }

    private void AnalyseCommand(string arg0)
    {
        arg0 = arg0.ToLower();
        int hash = arg0.GetHashCode();
        

        if (GodHash.Equals(hash))
        {
            StartGodMode(true);
            ShowMessage("It´s EPPU time");

        }
        else if (NormalHash.Equals(hash))
        {
            StartGodMode(false);
            ShowMessage("We are proud, that you dont use cheat power anymore!");

        }
        else if (HalfGodHash.Equals(hash))
        {
            StartGodMode(false);
            StartHalfGodMode();
            ShowMessage("Joeli....."+Environment.NewLine+"halfgod, pro designer and true inventor of the m building party");

        }
        else if (DropSomethingHash.Equals(hash))
        {
            DropSomething();
            ShowMessage("Something dropped :D");

        }
        consoleInput.text = "";

        if (LaterHash.Equals(hash))
        {
            ShowMessage("I will do it... later...somewhen...who knows...", 5);

        }else if (NotAvailableHash.Equals(hash))
        {
            ShowMessage("That is not available. maybe eppu can help.", 5);

        }else if (FreeBeer.Equals(hash))
        {
            SetPlayerSpeed(17);
            ShowMessage("Hurry Up - There is free beer ! :D");

        }
        else if (FreeBeerForAYear.Equals(hash))
        {
            SetPlayerSpeed(30);
            ShowMessage("I will be as fast as I can");

        }
        else if (NoFreeBeer.Equals(hash))
        {
            SetPlayerSpeed(8);
            ShowMessage("There is no reason to hurry then");


        }
        if (OpenHash.Equals(hash))
        {
            CompleteLevel();
            
        }
        if(ShowMeTheBossHash.Equals(hash))
        {
            TeleportToBossRoom();
        }
        if (FpsCounter.Equals(hash))
        {
            fps = !fps;
            fpsCounter.SetActive(fps);
        }
        consoleInput.enabled = false;
        consoleVisible = false;
        rectTransform.localScale = new Vector3(0, 0, 0);
    }

    private void SetPlayerSpeed(int speed)
    {
        PlayerMovement playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerMovement.speed = speed;
    }

    private void CompleteLevel()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().OpenCurrentAreasEntries();
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GetCurrentArea().EnemySpawner.KillAllEnemies();
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RevealCurrentMiniMap();
    }

    private void TeleportToBossRoom()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<NavMeshAgent>().Warp(GameObject.FindGameObjectWithTag("BossArea").transform.FindDeepChild("Entry 1").GetComponent<EntryPoint>().playerTeleportPoint.position);
        GameManager gameMananger = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        GameObject.FindGameObjectWithTag("Sun").GetComponent<Light>().color = new Color(1f, 0.4f, 0.4f);
        gameMananger.SetCurrentArea(GameObject.FindGameObjectWithTag("BossArea").GetComponent<Level>());
        gameMananger.progression = gameMananger.levelsToBoss;
        gameMananger.UpdateProgressionText();
        gameMananger.UpdateDropLevel();
    }

    private  string message;
    private  float messageDuration;

    private  List<MessageItem> messageQueue = new List<MessageItem>();

    public static void ShowMessage(String message,float time=4)
    {

       instance.messageQueue.Add(new MessageItem() {duration = time,Message = message});


    }


    

  

    private void DropSomething()
    {
        Object[] loadAll = Resources.LoadAll("Dropables/");

        int length = loadAll.Length;
        int index = UnityEngine.Random.Range(0, length);
        GameObject o = (GameObject) loadAll[index];

        GameObject player = GameObject.Find("Player");

        o.GetComponent<InventoryItem>().Drop(player.transform);
    }

    private void StartHalfGodMode()
    {
        GameObject player = GameObject.Find("Player");
        Stats stats = player.GetComponent<Stats>();
        
            stats.LifeEnergy = 500;
            stats.CurrentLifeEnergy = 250;
        
    }
    private void StartGodMode(bool flag)
    {
        GameObject player = GameObject.Find("Player");
        Stats stats = player.GetComponent<Stats>();
        if (flag)
        {
            stats.LifeEnergy = 99999;
            stats.CurrentLifeEnergy = 99999;
        }
        else
        {
            stats.LifeEnergy = 100;
            stats.CurrentLifeEnergy = 100;
        }
        Weapon[] weapons = player.GetComponents<Weapon>();
        foreach (Weapon weapon in weapons)
        {
            if (weapon.InventaryItemName.Equals("Stick"))
            {
                stick = weapon;
            }else if (weapon.InventaryItemName.Equals("Gods Fist"))
            {
                this.godsFist = weapon;
            }

        }

        if (godsFist == null)
        {
            var godsFist1 = Resources.Load<Weapon>("Dropables/10_Gods Fist");
            godsFist = (Weapon)godsFist1.CreateCopy(player);
            godsFist.InitWeaponHolder();
        }
     

        InventoryItem inventoryItem;
        if (flag)
        {
            inventoryItem = godsFist;
        }
        else
        {
            inventoryItem = stick;
        }


        Inventory inventory = player.GetComponent<Inventory>();
        inventory.Items.Remove(inventory.Items[0]);
        inventory.Items.Insert(0, inventoryItem);
        inventory.ChangeIndex(0);


    }

    // Update is called once per frame
    void Update ()
	{

        bool keyUp = GetKeyDown(KeyCode.I);

        if (keyUp)
	    {
	        float diff = Time.time   - lastPressedButtonTime;
	        if (diff > 0.3)
	        {
	            if (consoleVisible)
	            {
	                consoleInput.enabled = false;
	                consoleVisible = false;
                    rectTransform.localScale=new Vector3(0,0,0);
	            }
	            else
	            {
                    consoleInput.enabled = true;
                    consoleVisible = true;
                    rectTransform.localScale = new Vector3(1,1,1);
	                consoleInput.ActivateInputField();

	            }
            }
	        lastPressedButtonTime = Time.time;

	    }



        if (messageItem != null)
        {

            if (Time.time > messageEndTime)
            {
                messageItem = null;

                messageBoxBorder.SetActive(false);
                messageBoxText.text = "";

            }
        }
        else
        {
            if (messageQueue.Count != 0)
            {
                messageItem = messageQueue[0];
                messageQueue.Remove(messageItem);
                messageEndTime = Time.time+messageItem.duration;
                messageBoxBorder.SetActive(true);
                messageBoxText.text = messageItem.Message;
            }
        }
	}

    private float messageEndTime;
    private MessageItem messageItem;

    public static bool GetKeyDown(KeyCode keyCode)
    {
        if (consoleInput == null)
        {
            return Input.GetKeyDown(keyCode);
        }
        if (consoleInput.isFocused)
        {
            return false;
        }
        else
        {
            return Input.GetKeyDown(keyCode);

        }
    }

    public static bool GetKeyUp(KeyCode keyCode)
    {
        if (consoleInput.isFocused)
        {
            return false;
        }
        else
        {
            return Input.GetKeyUp(keyCode);
        }
    }

    public static bool GetKey(KeyCode keyCode)
    {
        if (consoleInput.isFocused)
        {
            return false;
        }
        else
        {
            return Input.GetKey(keyCode);
        }
    }

    public static float GetAxisRaw(String axis) 
    {
        if (consoleInput.isFocused)
        {
            return 0.0f;
        }
        else
        {
            return Input.GetAxisRaw(axis);
        }
    }

    private class MessageItem
    {
        public String Message { get; set; }
        public float duration { get; set; }
    }

}
