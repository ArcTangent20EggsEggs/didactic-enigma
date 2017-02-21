using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Sprites;

public class EnemyTool : EditorWindow
{

    public List<Enemies> enemyList = new List<Enemies>();

    public List<string> enemyNameList = new List<string>();

    public string[] enemyStringArray;
    string myName = "";
    int myHealth = 0;
    int myAttack = 0;
    int myDefense = 0;
    int myAgility = 0;
    float myAttackTime = 0;
    bool magicUser = false;
    int myMana = 0;
    Sprite mySprite;



    bool spriteFlag;
    bool nameFlag = false;
    bool nameExists = false;

    int currentChoice = 0;
    int lastChoice = 0;



    //stuff
    [MenuItem("Custom Tool/Enemy Tools %g")]
    private static void meleeeeeeeeeeee()
    {
        EditorWindow.GetWindow<EnemyTool> ();
    }

    void Awake()
    {
        GetEnemies();
    }

    void OnGUI()
    {
        currentChoice = EditorGUILayout.Popup(currentChoice, enemyStringArray);
        if(mySprite != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            Texture2D myTexture = SpriteUtility.GetSpriteTexture(mySprite, false);
            GUILayout.Label(myTexture);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        mySprite = EditorGUILayout.ObjectField(mySprite, typeof(Sprite), false) as Sprite;
        myName = EditorGUILayout.TextField("Name: ", myName);
        myHealth = EditorGUILayout.IntSlider("Health: ", myHealth, 1, 300);
        myAttack = EditorGUILayout.IntSlider("Attack: ", myAttack, 1, 100);
        myDefense = EditorGUILayout.IntSlider("Defense: ", myDefense, 1, 100);
        myAgility = EditorGUILayout.IntSlider("Agility: ", myAgility, 1, 100);
        myAttackTime = EditorGUILayout.Slider("Attack Time: ", myAttackTime, 1.0f, 20.0f);
        magicUser = EditorGUILayout.Toggle("Is Magic User", magicUser);
        if(magicUser)
            myMana = EditorGUILayout.IntSlider("Mana: ", myMana, 1, 100);

        //use slider for float

        if (currentChoice == 0)
        {
            if (GUILayout.Button("Create"))
            {
               CreateEnemy();
            }
           

        }
        else
        {
            if (GUILayout.Button("Save"))
            {
                SaveCurrentEnemy();
            }
        }
        if (currentChoice != lastChoice)
        {
            if (currentChoice == 0)
            {
                NewEnemy();
            }
            else
            {
                CurrentEnemy();
            }
            lastChoice = currentChoice;
        }
        if(nameFlag)
        {
            EditorGUILayout.HelpBox("Put in a name dummy", MessageType.Error);
        }
        if (nameExists)
        {
            EditorGUILayout.HelpBox("Put in a new name dummy", MessageType.Error);
        }
        if (spriteFlag)
        {
            EditorGUILayout.HelpBox("Put in a sprite dummy", MessageType.Error);
        }

    }

    private void GetEnemies()
    {
        enemyList.Clear();
        enemyNameList.Clear();
        string[] haxMoney = AssetDatabase.FindAssets("t:Enemies");
        foreach(string guid in haxMoney)
        {
            string mang0 = AssetDatabase.GUIDToAssetPath(guid);
            Enemies leffen = AssetDatabase.LoadAssetAtPath(mang0, typeof(Enemies)) as Enemies;
            enemyNameList.Add(leffen.emname);
            enemyList.Add(leffen);
        }
        enemyNameList.Insert(0, "New");
        enemyStringArray = enemyNameList.ToArray();
    }

    private void NewEnemy()
    {
        myHealth = 1;
        myName = "";
        myAttack = 1;
        myDefense = 1;
        myAgility = 1;
        myAttackTime = 1.0f;
        magicUser = false;
        myMana = 1;
        mySprite = null;
        nameExists = false;
        nameFlag = false;
    }

    private void CurrentEnemy()
    {
        myName = enemyList[currentChoice - 1].emname;
        myHealth = enemyList[currentChoice - 1].health;
        myAttack = enemyList[currentChoice - 1].atk;
        myDefense = enemyList[currentChoice - 1].def;
        myAgility = enemyList[currentChoice - 1].agi;
        myAttackTime = enemyList[currentChoice - 1].atkTime;
        magicUser = enemyList[currentChoice - 1].isMagic;
        myMana = enemyList[currentChoice - 1].manaPool;
        mySprite = enemyList[currentChoice - 1].mySprite;
        //EditorUtility.SetDirty(enemyList[currentChoice - 1]);
        //AssetDatabase.SaveAssets();
    }

    private void SaveCurrentEnemy()
    {
        enemyList[currentChoice - 1].emname = myName;
        enemyList[currentChoice - 1].health = myHealth;
        enemyList[currentChoice - 1].atk = myAttack;
        enemyList[currentChoice - 1].def = myDefense;
        enemyList[currentChoice - 1].agi = myAgility;
        enemyList[currentChoice - 1].atkTime = myAttackTime;
        enemyList[currentChoice - 1].isMagic = magicUser;
        enemyList[currentChoice - 1].manaPool = myMana;
        enemyList[currentChoice - 1].mySprite = mySprite;
        EditorUtility.SetDirty(enemyList[currentChoice - 1]);
        AssetDatabase.SaveAssets();
    }

    private void CreateEnemy()
    {
        if (myName == "")
        {
            nameFlag = true;
        }
        else
        {
            if (mySprite == null)
            {
                spriteFlag = true;
            }
            else
            {
                string[] assetString = AssetDatabase.FindAssets(myName);
                if (assetString.Length > 0)
                {
                    nameExists = true;
                }
                else
                {
                    Enemies myEnemy = ScriptableObject.CreateInstance<Enemies>();
                    myEnemy.emname = myName;
                    myEnemy.health = myHealth;
                    myEnemy.atk = myAttack;
                    myEnemy.def = myDefense;
                    myEnemy.agi = myAgility;
                    myEnemy.atkTime = myAttackTime;
                    myEnemy.isMagic = magicUser;
                    myEnemy.manaPool = myMana;
                    myEnemy.mySprite = mySprite;
                    AssetDatabase.CreateAsset(myEnemy, "Assets/Resources/Data/EnemyData/" + myEnemy.emname.Replace(" ", "_") + ".asset");
                    nameFlag = false;
                    nameExists = false;
                    spriteFlag = false;
                    NewEnemy();
                    GetEnemies();
                }
            }
        }
    }
}
