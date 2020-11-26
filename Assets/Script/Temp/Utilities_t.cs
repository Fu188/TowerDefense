// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Text;
// using System.Xml.Linq;
// using UnityEngine;

// namespace Assets.Scripts
// {
//     public static class Utilities
//     {
//         /// <summary>
//         /// Found here
//         /// http://www.bensilvis.com/?p=500
//         /// </summary>
//         /// <param name="screenWidth"></param>
//         /// <param name="screenHeight"></param>
//         public static void AutoResize(int screenWidth, int screenHeight)
//         {
//             Vector2 resizeRatio = new Vector2((float)Screen.width / screenWidth, (float)Screen.height / screenHeight);
//             GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resizeRatio.x, resizeRatio.y, 1.0f));
//         }

//         /// <summary>
//         /// Reads the XML file
//         /// </summary>
//         /// <returns>A new FileStuffFromXML object</returns>
//         public static LevelStuffFromXML ReadXMLFile()
//         {
//             LevelStuffFromXML ls = new LevelStuffFromXML();
//             //we're directly loading the level1 file, change if appropriate
//             TextAsset ta = Resources.Load("LevelX") as TextAsset;
//             //LINQ to XML rulez!
//             XDocument xdoc = XDocument.Parse(ta.text);
//             XElement el = xdoc.Element("Elements");
//             var paths = el.Element("PathPieces").Elements("Path");

//             foreach (var item in paths)
//             {
//                 ls.Paths.Add(new Vector3(float.Parse(item.Attribute("X").Value), float.Parse(item.Attribute("Y").Value), float.Parse(item.Attribute("Z").Value)));
//             }

//             var waypoints = el.Element("Waypoints").Elements("Waypoints");
//             foreach (var item in waypoints)
//             {
//                 ls.Waypoints.Add(new Vector3(float.Parse(item.Attribute("X").Value), float.Parse(item.Attribute("Y").Value), float.Parse(item.Attribute("Z").Value)));
//             }

//             var rounds = el.Element("Rounds").Elements("Round");
//             foreach (var item in rounds)
//             {
//                 ls.Rounds.Add(new Round()
//                 {
//                     NoOfEnemies_low = int.Parse(item.Attribute("NoOfEnemies_low").Value),
//                     NoOfEnemies_mid = int.Parse(item.Attribute("NoOfEnemies_mid").Value),
//                     NoOfEnemies_high = int.Parse(item.Attribute("NoOfEnemies_high").Value)
//                 });
//             }

//             XElement tower = el.Element("Tower");
//             ls.Tower = new Vector3(float.Parse(tower.Attribute("X").Value), float.Parse(tower.Attribute("Y").Value), float.Parse(tower.Attribute("Z").Value));

//             XElement otherStuff = el.Element("OtherStuff");
//             ls.InitialMoney = int.Parse(otherStuff.Attribute("InitialMoney").Value);
//             ls.MinSpawnTime = float.Parse(otherStuff.Attribute("MinSpawnTime").Value);
//             ls.MaxSpawnTime = float.Parse(otherStuff.Attribute("MaxSpawnTime").Value);

//             return ls;
//         }
//     }
// }
