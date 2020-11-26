using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Simple class to hold all our level details
    /// </summary>
    public class LevelStuffFromXML
    {
        public float MinSpawnTime;
        public float MaxSpawnTime;
        public int InitialMoney;
        public Vector3 Enemy;
        public List<Round> Rounds;
        public List<List<Vector3>> Paths;
        public List<Vector3> Waypoints;
        public List<Vector3> Tower;
        public List<Vector3> EnemyGenerator;
        public LevelStuffFromXML()
        {
            Paths = new List<List<Vector3>>();
            Waypoints = new List<Vector3>();
            Rounds = new List<Round>();
        }

    }

    /// <summary>
    /// Some basic information about each game round
    /// </summary>
    public class Round
    {
        public int NoOfEnemy1 { get; set; }
        public int NoOfEnemy2 { get; set; }
        public int NoOfEnemy3 { get; set; }
        public int NoOfEnemy4 { get; set; }
        public int NoOfEnemy5 { get; set; }
        public int NoOfEnemy6 { get; set; }
    }


    
}
