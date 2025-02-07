using System;
using System.Collections.Generic;

namespace ResidentsFarmWithYou
{
    public class AIWork_FarmOmega : AIWork
    {
        public override void OnPerformWork(bool realtime)
        {
            if (!realtime)
            {
                base.SetDestination();
                this.SetDestPos();
            }
        }
        
        public override void SetDestPos()
        {
            if (EClass.core?.IsGameStarted == false ||
                EClass._zone?.IsPCFaction == false ||
                this.destThing == null ||
                this.destThing?.ExistsOnMap == false)
            {
                return;
            }
            
            Point destPos = FindDestinationPosition(destThing: this.destThing);
            
            this.destPos = destPos;
        }
        
        private Point FindDestinationPosition(Thing destThing)
        {
            List<Point> destPoints = destThing?.trait?.ListPoints();
            if (destPoints == null || destPoints.Count == 0)
            {
                return null;
            }

            int maxTries = destPoints.Count;

            for (int i = 0; i < maxTries; i++)
            {
                Point randomPoint = destPoints.RandomItem<Point>();
                
                if (randomPoint != null && 
                    this.owner.CanMoveTo(p: randomPoint) == true &&
                    randomPoint.HasChara == false &&
                    randomPoint.IsFarmField == true &&
                    randomPoint.cell?.isWatered == false)
                {
                    return randomPoint;
                }
            }

            return null;
        }

        public override AIAct GetWork(Point p)
        {
            this._previousWork = this;
            
            return new AI_FarmOmega
            {
                pos = p,
                PreviousWork = this._previousWork
            };
        }
        
        public override IEnumerable<AIAct.Status> Run()
        {
            yield return base.DoIdle(repeat: 100);
            AIWork.Work_Type workType = this.WorkType;
            this.SetDestPos();
            if (this.destPos != null)
            {
                yield return base.DoGoto(pos: this.destPos, dist: this.destDist, ignoreConnection: false, _onChildFail: null);
                AIAct work = this.GetWork(p: this.destPos);
                if (work != null)
                {
                    this.owner.Talk(idTopic: "work_" + this.sourceWork.talk, ref1: null, ref2: null, forceSync: false);
                    yield return base.Do(_seq: work, _onChildFail: new Func<AIAct.Status>(base.KeepRunning));
                }
            }
            else
            {
                yield return this.Cancel();
            }
            yield return base.Restart();
            yield break;
        }
        
        private AIWork_FarmOmega _previousWork;
    }
}