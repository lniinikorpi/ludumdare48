﻿using UnityEngine;
using System.Collections.Generic;

// References: https://www.gamasutra.com/blogs/AAdonaac/20150903/252889/Procedural_Dungeon_Generation_Algorithm.php

namespace Assets.Scripts.ProceduralSystem
{
    public class Dungeon : IDungeon
    {
        public Gateway startGateway;
        public Gateway exitGateway;
        /// <summary>
        /// for entry to multiple dungeons
        /// </summary>
        public System.Collections.Generic.List<Gateway> subDungeons;

        public int id { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public IDungeon parentDungeon { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void GenerateDungeon()
        {
            GenerateRoom();
            SeparateRoom();
            DetermineMainRooms();
            TriangulateRoom();
            GenerateGraph();
            MinSpanningTree();
            DetermineHallway();
            TriangulateRoom();
            GenerateGraph();
            MinSpanningTree();
            DetermineHallway();
        }
        public void GenerateRoom()
        {
            throw new System.NotImplementedException();
        }

        public void SeparateRoom()
        {
            throw new System.NotImplementedException();
        }

        public void DetermineMainRooms()
        {
            throw new System.NotImplementedException();
        }

        public void TriangulateRoom()
        {
            throw new System.NotImplementedException();
        }

        public void GenerateGraph()
        {
            throw new System.NotImplementedException();
        }

        public void MinSpanningTree()
        {
            throw new System.NotImplementedException();
        }

        public void DetermineHallway()
        {
            throw new System.NotImplementedException();
        }
    }
}