using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




namespace BarrageGame
{
    // 兵分天下
    // A.暂停 30 秒
    // B.40倍加速到200年
    // C.4倍速到只剩一个国家
    // 剩余2个国家时，主动敌对
    public class DivideTheWorld : MonoBehaviour
    {
        public enum StageType
        {
            None,
            // A.暂停 30 秒
            A,
            // B.40倍加速到200年
            B,
            // C.2倍速到只剩一个国家
            C,
        }
        // 阶段
        public StageType stageType = StageType.None;

        public float secondsTime = 0f;

        public float aTimer = 0f;

        private bool taskCompleted = false;

        void Awake()
        {
            Debug.Log(">>>>>>>>>>> DivideTheWorld.Awake");

        }

        void Start()
        {
            stageType = StageType.A;
            GameHelper.Paused(true);
        }

        void Update()
        {
            secondsTime += Time.deltaTime;
            if(secondsTime >= 1f)
            {
                secondsTime -= 1f;
                // 流逝1秒
                SecondsUpdate();
            }



        }


        void SecondsUpdate()
        {
            switch(stageType)
            {
                case StageType.A:
                {
                    aTimer += 1f;
                    if(aTimer >= 30f)
                    {
                        stageType = StageType.B;
                        GameHelper.Paused(false);
                        GameHelper.SetTimeScale(40f);
                    }
                    break;
                }
                case StageType.B:
                {
                    if(MapBox.instance.mapStats.year >= 200)
                    {
                        stageType = StageType.C;
                        GameHelper.SetTimeScale(4f);
                    }
                    break;
                }
                case StageType.C:
                {
                    if(MKingdomManager.instance.allKingdoms.Count <= 1)
                    {
                        if(taskCompleted == false)
                        {
                            taskCompleted = true;
                            OnCompeleted();
                        }
                    }
                    if(MKingdomManager.instance.allKingdoms.Count == 2)
                    {
                        if(MKingdomManager.instance.allKingdoms.ElementAt(0).Value.IsEnemy(MKingdomManager.instance.allKingdoms.ElementAt(1).Value) == false)
                        {
                            MKingdomManager.instance.allKingdoms.ElementAt(0).Value.StartWar(MKingdomManager.instance.allKingdoms.ElementAt(1).Value);
                        }
                    }
                    break;
                }
            }




            if(stageType == StageType.C)
            {
                if(MKingdomManager.instance.allKingdoms.Count > 2){
                    // 简易ai
                    {
                        int index = UnityEngine.Random.Range(0,MKingdomManager.instance.allKingdoms.Count);
                        var mKingdom =MKingdomManager.instance.allKingdoms.ElementAt(index).Value;
                        if(mKingdom.kingPlayerUid != 0)
                        {
                            // 这是玩家控制的
                            return;
                        }
                        if(mKingdom.HasEnemies())
                        {
                            // 有敌人了，先把现在的敌人解决掉
                            return;
                        }
                        index = UnityEngine.Random.Range(0,MKingdomManager.instance.allKingdoms.Count);
                        var targetMKingdom = MKingdomManager.instance.allKingdoms.ElementAt(index).Value;
                        if(mKingdom.IsEnemy(targetMKingdom))
                        {
                            // 已经是敌人了
                            return;
                        }
                        if(!mKingdom.ForceComparison(targetMKingdom))
                        {
                            // 没他厉害，和他宣战，我傻吗？
                            return;
                        }
                        mKingdom.StartWar(targetMKingdom);
                    }

                }
            }
        }

        void OnCompeleted()
        {
            // 任务完成
            Destroy(gameObject);
            ControlHelper.GameOver();
        }
        void OnDisable()
        {
            Debug.Log(">>>>>>>>>>> DivideTheWorld.OnDisable");
        }
    }
} 