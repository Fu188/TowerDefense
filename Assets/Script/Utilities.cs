using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Utilities
    {
        /// <summary>
        /// Found here
        /// http://www.bensilvis.com/?p=500
        /// </summary>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        public static void AutoResize(int screenWidth, int screenHeight)
        {
            Vector2 resizeRatio = new Vector2((float)Screen.width / screenWidth, (float)Screen.height / screenHeight);
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resizeRatio.x, resizeRatio.y, 1.0f));
        }

        /// <summary>
        /// Reads the XML file
        /// </summary>
        /// <returns>A new FileStuffFromXML object</returns>
        public static LevelStuffFromXML ReadXMLFile()
        {
            LevelStuffFromXML ls = new LevelStuffFromXML();
            //we're directly loading the level1 file, change if appropriate
            TextAsset ta = Resources.Load("Levels/Level"+HomePageManager.GetHomePageManagerInstance().DungeonNumber.ToString()) as TextAsset;
            Debug.Log(ta);
            //LINQ to XML rulez!
            XDocument xdoc = XDocument.Parse(ta.text);
            XElement el = xdoc.Element("Elements");
            var paths = el.Element("PathPieces").Elements("Path");

            foreach (var item in paths)
            {
                Vector3 position = new Vector3(float.Parse(item.Attribute("X").Value), float.Parse(item.Attribute("Y").Value), float.Parse(item.Attribute("Z").Value));
                Vector3 rotation = new Vector3(float.Parse(item.Attribute("X2").Value), float.Parse(item.Attribute("Y2").Value), float.Parse(item.Attribute("Z2").Value));
                List<Vector3> attr = new List<Vector3>();
                attr.Add(position);
                attr.Add(rotation);
                ls.Paths.Add(attr);
            }

            var waypoints = el.Element("Waypoints").Elements("Waypoints");
            foreach (var item in waypoints)
            {
                ls.Waypoints.Add(new Vector3(float.Parse(item.Attribute("X").Value), float.Parse(item.Attribute("Y").Value), float.Parse(item.Attribute("Z").Value)));
            }

            var rounds = el.Element("Rounds").Elements("Round");
            foreach (var item in rounds)
            {
                ls.Rounds.Add(new Round()
                {
                    NoOfEnemy1 = int.Parse(item.Attribute("NoOfEnemy1").Value),
                    NoOfEnemy2 = int.Parse(item.Attribute("NoOfEnemy2").Value),
                    NoOfEnemy3 = int.Parse(item.Attribute("NoOfEnemy3").Value),
                    NoOfEnemy4 = int.Parse(item.Attribute("NoOfEnemy4").Value),
                    NoOfEnemy5 = int.Parse(item.Attribute("NoOfEnemy5").Value),
                    NoOfEnemy6 = int.Parse(item.Attribute("NoOfEnemy6").Value)
                });
            }

            XElement enemy = el.Element("Enemy");
            Vector3 enemydir = new Vector3(float.Parse(enemy.Attribute("X").Value),float.Parse(enemy.Attribute("Y").Value),float.Parse(enemy.Attribute("Z").Value));
            ls.Enemy = enemydir;

            XElement tower = el.Element("Tower");
            Vector3 towerposition = new Vector3(float.Parse(tower.Attribute("X").Value), float.Parse(tower.Attribute("Y").Value), float.Parse(tower.Attribute("Z").Value));
            Vector3 towerrotation = new Vector3(float.Parse(tower.Attribute("X2").Value), float.Parse(tower.Attribute("Y2").Value), float.Parse(tower.Attribute("Z2").Value));
            List<Vector3> towerattr = new List<Vector3>();
            towerattr.Add(towerposition);
            towerattr.Add(towerrotation);
            ls.Tower = towerattr;

            XElement EnemyGenerator = el.Element("EnemyGenerator");
            Vector3 EnemyGeneratorposition = new Vector3(float.Parse(EnemyGenerator.Attribute("X").Value), float.Parse(EnemyGenerator.Attribute("Y").Value), float.Parse(EnemyGenerator.Attribute("Z").Value));
            Vector3 EnemyGeneratorrotation = new Vector3(float.Parse(EnemyGenerator.Attribute("X2").Value), float.Parse(EnemyGenerator.Attribute("Y2").Value), float.Parse(EnemyGenerator.Attribute("Z2").Value));
            List<Vector3> EnemyGeneratorattr = new List<Vector3>();
            EnemyGeneratorattr.Add(EnemyGeneratorposition);
            EnemyGeneratorattr.Add(EnemyGeneratorrotation);
            ls.EnemyGenerator = EnemyGeneratorattr;

            XElement otherStuff = el.Element("OtherStuff");
            ls.InitialMoney = int.Parse(otherStuff.Attribute("InitialMoney").Value);
            ls.MinSpawnTime = float.Parse(otherStuff.Attribute("MinSpawnTime").Value);
            ls.MaxSpawnTime = float.Parse(otherStuff.Attribute("MaxSpawnTime").Value);

            return ls;
        }
    }
}
