using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Xml.Linq;
using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;

public class LevelExport : EditorWindow
{
    [MenuItem("Custom Editor/Export Level")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(LevelExport));
    }

    Vector2 scrollPosition = Vector2.zero;
    int noOfEnemy1;
    int noOfEnemy2;
    int noOfEnemy3;
    int noOfEnemy4;
    int noOfEnemy5;
    int noOfEnemy6;
    int initialMoney;
    int MinSpawnTime, MaxSpawnTime;
    string filename = "LevelX.xml";
    int waypointsCount;
    int pathsCount;
    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        EditorGUILayout.LabelField("Total Rounds created:" + rounds.Count);
        for (int i = 0; i < rounds.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Round " + (i + 1));
            EditorGUILayout.LabelField("# 1 " + rounds[i].NoOfEnemy1);
            EditorGUILayout.LabelField("# 2 " + rounds[i].NoOfEnemy2);
            EditorGUILayout.LabelField("# 3 " + rounds[i].NoOfEnemy3);
            EditorGUILayout.LabelField("# 4 " + rounds[i].NoOfEnemy4);
            EditorGUILayout.LabelField("# 5 " + rounds[i].NoOfEnemy5);
            EditorGUILayout.LabelField("# 6 " + rounds[i].NoOfEnemy6);
            if (GUILayout.Button("Delete"))
            {
                rounds.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.LabelField("Add a new round", EditorStyles.boldLabel);
        noOfEnemy1 = EditorGUILayout.IntSlider("Number of enemy1", noOfEnemy1, 0, 20);
        noOfEnemy2 = EditorGUILayout.IntSlider("Number of enemy2", noOfEnemy2, 0, 20);
        noOfEnemy3 = EditorGUILayout.IntSlider("Number of enemy3", noOfEnemy3, 0, 20);
        noOfEnemy4 = EditorGUILayout.IntSlider("Number of enemy4", noOfEnemy4, 0, 20);
        noOfEnemy5 = EditorGUILayout.IntSlider("Number of enemy5", noOfEnemy5, 0, 20);
        noOfEnemy6 = EditorGUILayout.IntSlider("Number of enemy6", noOfEnemy6, 0, 20);

        if (GUILayout.Button("Add new round"))
        {
            rounds.Add(new Round() { 
                NoOfEnemy1 = noOfEnemy1,
                NoOfEnemy2 = noOfEnemy2,
                NoOfEnemy3 = noOfEnemy3,
                NoOfEnemy4 = noOfEnemy4,
                NoOfEnemy5 = noOfEnemy5,
                NoOfEnemy6 = noOfEnemy6
                });
        }
        initialMoney = EditorGUILayout.IntSlider("Initial Money", initialMoney, 200, 400);
        MinSpawnTime = EditorGUILayout.IntSlider("MinSpawnTime", MinSpawnTime, 1, 10);
        MaxSpawnTime = EditorGUILayout.IntSlider("MaxSpawnTime", MaxSpawnTime, 1, 10);
        filename = EditorGUILayout.TextField("Filename:", filename);
        EditorGUILayout.LabelField("Export Level", EditorStyles.boldLabel);
        if (GUILayout.Button("Export"))
        {
            Export();
        }
    }

    XDocument doc;
    List<Round> rounds = new List<Round>();

    // The export method
    void Export()
    {
        // Create a new output file stream
        doc = new XDocument();
        doc.Add(new XElement("Elements"));
        XElement elements = doc.Element("Elements");


        XElement pathPiecesXML = new XElement("PathPieces");
        var paths = GameObject.FindGameObjectsWithTag("Path");
       
        foreach (var item in paths)
        {
            XElement path = new XElement("Path");
            XAttribute attrX = new XAttribute("X", item.transform.position.x);
            XAttribute attrY = new XAttribute("Y", item.transform.position.y);
            XAttribute attrZ = new XAttribute("Z", item.transform.position.z);
            path.Add(attrX, attrY,attrZ);
            XAttribute attrX2 = new XAttribute("X2", item.transform.eulerAngles.x);
            XAttribute attrY2 = new XAttribute("Y2", item.transform.eulerAngles.y);
            XAttribute attrZ2 = new XAttribute("Z2", item.transform.eulerAngles.z);
            path.Add(attrX2, attrY2,attrZ2);
            pathPiecesXML.Add(path);
        }
        pathsCount = paths.Length;
        elements.Add(pathPiecesXML);

        XElement waypointsXML = new XElement("Waypoints");
        var waypoints = GameObject.FindGameObjectsWithTag("Waypoints");
        if (!WaypointsAreValid(waypoints))
        {
            return;
        }
        //order by user selected order
        waypoints = waypoints.OrderBy(x => x.GetComponent<OrderedWaypointForEditor>().Order).ToArray();
        foreach (var item in waypoints)
        {
            XElement waypoint = new XElement("Waypoints");
            XAttribute attrX = new XAttribute("X", item.transform.position.x);
            XAttribute attrY = new XAttribute("Y", item.transform.position.y);
            XAttribute attrZ = new XAttribute("Z", item.transform.position.z);
            waypoint.Add(attrX, attrY, attrZ);
            waypointsXML.Add(waypoint);
        }
        waypointsCount = waypoints.Length;
        elements.Add(waypointsXML);

        XElement roundsXML = new XElement("Rounds");
        foreach (var item in rounds)
        {
            XElement round = new XElement("Round");
            XAttribute NoOfEnemy1 = new XAttribute("NoOfEnemy1", item.NoOfEnemy1);
            XAttribute NoOfEnemy2 = new XAttribute("NoOfEnemy2", item.NoOfEnemy2);
            XAttribute NoOfEnemy3 = new XAttribute("NoOfEnemy3", item.NoOfEnemy3);
            XAttribute NoOfEnemy4 = new XAttribute("NoOfEnemy4", item.NoOfEnemy4);
            XAttribute NoOfEnemy5 = new XAttribute("NoOfEnemy5", item.NoOfEnemy5);
            XAttribute NoOfEnemy6 = new XAttribute("NoOfEnemy6", item.NoOfEnemy6);
            round.Add(NoOfEnemy1);
            round.Add(NoOfEnemy2);
            round.Add(NoOfEnemy3);
            round.Add(NoOfEnemy4);
            round.Add(NoOfEnemy5);
            round.Add(NoOfEnemy6);
            roundsXML.Add(round);
        }
        elements.Add(roundsXML);

        XElement enemyXML = new XElement("Enemy");
        XAttribute enemyX = new XAttribute("X", waypoints[0].transform.eulerAngles.x);
        XAttribute enemyY = new XAttribute("Y", waypoints[0].transform.eulerAngles.y);
        XAttribute enemyZ = new XAttribute("Z", waypoints[0].transform.eulerAngles.z);
        enemyXML.Add(enemyX,enemyY,enemyZ);
        elements.Add(enemyXML);

        XElement towerXML = new XElement("Tower");
        var tower = GameObject.FindGameObjectWithTag("Tower");
        if(tower == null)
        {
            ShowErrorForNull("Tower");
            return;
        }
        XAttribute towerX = new XAttribute("X", tower.transform.position.x);
        XAttribute towerY = new XAttribute("Y", tower.transform.position.y);
        XAttribute towerZ = new XAttribute("Z", tower.transform.position.z);
        towerXML.Add(towerX, towerY, towerZ);
        XAttribute towerX2 = new XAttribute("X2", tower.transform.eulerAngles.x);
        XAttribute towerY2 = new XAttribute("Y2", tower.transform.eulerAngles.y);
        XAttribute towerZ2 = new XAttribute("Z2", tower.transform.eulerAngles.z);
        towerXML.Add(towerX2, towerY2, towerZ2);
        elements.Add(towerXML);
        
        XElement EnemyGeneratorXML = new XElement("EnemyGenerator");
        var EnemyGenerator = GameObject.FindGameObjectWithTag("EnemyGenerator");
        if(EnemyGenerator == null)
        {
            ShowErrorForNull("EnemyGenerator");
            return;
        }
        XAttribute EnemyGeneratorX = new XAttribute("X", EnemyGenerator.transform.position.x);
        XAttribute EnemyGeneratorY = new XAttribute("Y", EnemyGenerator.transform.position.y);
        XAttribute EnemyGeneratorZ = new XAttribute("Z", EnemyGenerator.transform.position.z);
        EnemyGeneratorXML.Add(EnemyGeneratorX, EnemyGeneratorY, EnemyGeneratorZ);
        XAttribute EnemyGeneratorX2 = new XAttribute("X2", EnemyGenerator.transform.eulerAngles.x);
        XAttribute EnemyGeneratorY2 = new XAttribute("Y2", EnemyGenerator.transform.eulerAngles.y);
        XAttribute EnemyGeneratorZ2 = new XAttribute("Z2", EnemyGenerator.transform.eulerAngles.z);
        EnemyGeneratorXML.Add(EnemyGeneratorX2, EnemyGeneratorY2, EnemyGeneratorZ2);
        elements.Add(EnemyGeneratorXML);

        XElement otherStuffXML = new XElement("OtherStuff");
        otherStuffXML.Add(new XAttribute("InitialMoney", initialMoney));
        otherStuffXML.Add(new XAttribute("MinSpawnTime", MinSpawnTime));
        otherStuffXML.Add(new XAttribute("MaxSpawnTime", MaxSpawnTime));
        elements.Add(otherStuffXML);


        if (!InputIsValid())
            return;



        if (EditorUtility.DisplayDialog("Save confirmation",
            "Are you sure you want to save level " + filename +"?", "OK", "Cancel"))
        {
            doc.Save("Assets/Resources/" + filename);
            EditorUtility.DisplayDialog("Saved", filename + " saved!", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("NOT Saved", filename + " not saved!", "OK");
        }
    }

    private bool WaypointsAreValid(GameObject[] waypoints)
    {
        //first check whether whey all have a OrderedWaypoint component
        if (!waypoints.All(x => x.GetComponent<OrderedWaypointForEditor>() != null))
        {
            EditorUtility.DisplayDialog("Error", "All waypoints must have an ordered waypoint component", "OK");
            return false;
        }
        //check if all Order fields on the orderwaypoint components are different

        if (waypoints.Count() != waypoints.Select(x=>x.GetComponent<OrderedWaypointForEditor>().Order).Distinct().Count())
        {
            EditorUtility.DisplayDialog("Error", "All waypoints must have a different order", "OK");
            return false;
        }
        return true;
    }

    private void ShowErrorForNull(string gameObjectName)
    {
        EditorUtility.DisplayDialog("Error", "Cannot find gameobject " + gameObjectName, "OK");
    }

    private bool InputIsValid()
    {
        if (MinSpawnTime > MaxSpawnTime)
        {
            EditorUtility.DisplayDialog("Error", "MinSpawnTime must be less or equal "
            + " to MaxSpawnTime", "OK");
            return false;
        }

        if (rounds.Count == 0)
        {
            EditorUtility.DisplayDialog("Error", "You cannot have 0 rounds", "OK");
            return false;
        }

        if (waypointsCount == 0)
        {
            EditorUtility.DisplayDialog("Error", "You cannot have 0 waypoints", "OK");
            return false;
        }

        if (pathsCount == 0)
        {
            EditorUtility.DisplayDialog("Error", "You cannot have 0 paths", "OK");
            return false;
        }

        return true;
    }

}
