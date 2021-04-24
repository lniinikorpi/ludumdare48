﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

// References: https://www.gamasutra.com/blogs/AAdonaac/20150903/252889/Procedural_Dungeon_Generation_Algorithm.php

namespace Assets.Scripts.ProceduralSystem
{
    [Serializable]
    public class Dungeon : IDungeon
    {
        public Gateway startGateway;
        public Gateway exitGateway;
        /// <summary>
        /// for entry to multiple dungeons
        /// </summary>
        public List<Gateway> subDungeons;

        public int id { get ; set ; }
        public IDungeon parentDungeon { get ; set ; }
        public List<Rect> rooms;

        public DungeonConfig config;
        private float mTileSize = 1;

        public Dungeon(DungeonConfig config , float tileSize)
        {
            this.config = config;
            this.mTileSize = tileSize;

            
        }

        public IEnumerator Generate(GameObject simulationCube)
        {
            GenerateRoom();
            yield return SeparateRoom(simulationCube);
            //DetermineMainRooms();
            //TriangulateRoom();
            //GenerateGraph();
            //MinSpanningTree();
            //DetermineHallway();
            //TriangulateRoom();
            //GenerateGraph();
            //MinSpanningTree();
            //DetermineHallway();
        }

        private void GenerateRoom()
        {
            this.rooms = new List<Rect>();

            var roomCount = this.config.roomGenerateCountRange.GetRandom();
            var roomSize = this.config.roomGenerateSizeRange;
            
            for(var i = 0; i < roomCount; i++)
            {
                var position = GetRandomPointInCircle(this.config.dungeonRadius);
                var rect = new Rect(position.x, position.y, roomSize.GetRandom(), roomSize.GetRandom());

                this.rooms.Add(rect);
            }
        }

        private IEnumerator SeparateRoom(GameObject simulationCube)
        {
            var rigidbodies = new List<Rigidbody2D>();
            var shouldBreak = true;

            for(var i = 0; i < this.rooms.Count ; i++)
            {
                var room = this.rooms[i];
                var clone = GameObject.Instantiate(simulationCube, new Vector3(room.center.x, room.center.y, 0), Quaternion.identity).GetComponent<Rigidbody2D>();
                clone.transform.localScale = new Vector3(room.width, room.height, 1);

                rigidbodies.Add(clone);
            }

            while (shouldBreak)
            {
                shouldBreak = false;
                yield return new WaitForSeconds(1f);


                for (var i = 0; i < rigidbodies.Count; i++)
                {
                    var body = rigidbodies[i];
                    if (!body.IsSleeping())
                    {
                        shouldBreak = true;
                    }
                    var position = body.transform.position;

                    //body.transform.position = new Vector3(RoundM(position.x, this.mTileSize), RoundM(position.y, this.mTileSize), position.z);
                }
            }

            for (var i = 0; i < rigidbodies.Count; i++)
            {
                var body = rigidbodies[i];
                var position = body.transform.position;
                var room = this.rooms[i];

                var x = position.x - room.width / 2; 
                var y = position.y - room.height / 2;

                //this.rooms[i] = new Rect(RoundM(x , this.mTileSize) , RoundM(y , this.mTileSize) , room.width, room.height);
                this.rooms[i] = new Rect(x , y , room.width, room.height);

                GameObject.Destroy(body.gameObject);
            }

            Debug.Log("Everybody is sleeping now");
        }

        private void DetermineMainRooms()
        {
            throw new System.NotImplementedException();
        }

        private void TriangulateRoom()
        {
            throw new System.NotImplementedException();
        }

        private void GenerateGraph()
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


        /// <summary>
        /// Check out TKdev's algorithm
        /// </summary>
        private Vector2 GetRandomPointInCircle(float radius)
        {
            float t = 2 * Mathf.PI * UnityEngine.Random.Range(0, 1f);
            float u = UnityEngine.Random.Range(0, 1f) + UnityEngine.Random.Range(0, 1f);
            float r = 0f;

            if (u > 1)
            {
                r = 2 - u;
            }
            else
            {
                r = u;
            }

            r *= radius;

            return new Vector2(
                RoundM(r * Mathf.Cos(t) , this.mTileSize),
                RoundM(r * Mathf.Sin(t) , this.mTileSize)
            );
        }

        private float RoundM(float value , float pixelSize){
            return Mathf.Floor(((value + pixelSize - 1)/pixelSize))*pixelSize;
        }
    }
}
