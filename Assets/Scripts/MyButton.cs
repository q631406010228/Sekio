using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class MyButton
    {
        public bool isPressing = false;
        public bool onPressed = false;
        public bool onReleased = false;
        /// <summary>
        /// 刚放开键后限定时间内为true，其余为false
        /// </summary>
        public bool isExtending = false;    
        /// <summary>
        /// 刚按下键后限定时间内为true，其余为false
        /// </summary>
        public bool isDelaying = false;     

        public float extendingDuration = 0.15f;
        public float delayingDuration = 0.15f;

        private bool curState = false;
        private bool lastState = false;

        private MyTimer extTimer = new MyTimer();
        private MyTimer delayTimer = new MyTimer();

        public void Tick(bool input, float extendingDuration = 0.15f, float delayingDuration = 0.15f)
        {
            this.extendingDuration = extendingDuration;
            this.delayingDuration = delayingDuration;
            extTimer.Tick();
            delayTimer.Tick();

            curState = input;
            isPressing = curState;
            onPressed = false;
            onReleased = false;
            isExtending = false;
            isDelaying = false;

            if (curState != lastState)
            {
                if (curState)
                {
                    onPressed = true;
                    StartTimer(delayTimer, this.delayingDuration);
                }
                else
                {
                    onReleased = true;
                    StartTimer(extTimer, this.extendingDuration);
                }
            }
            lastState = curState;

            if(extTimer.state == MyTimer.STATE.RUN)
            {
                isExtending = true;
            }
            
            if(delayTimer.state == MyTimer.STATE.RUN)
            {
                isDelaying = true;
            }
        }

        private void StartTimer(MyTimer timer, float duration)
        {
            timer.duration = duration;
            timer.Go();
        }
    }
}
