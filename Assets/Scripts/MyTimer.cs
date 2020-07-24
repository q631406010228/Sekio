using UnityEngine;

namespace Assets
{
    public class MyTimer
    {
        public enum STATE
        {
            IDLE,
            RUN,
            FINSIHED
        }

        public STATE state;
        public float duration = 1;

        private float elapsedTime = 0;

        public void Tick()
        {
            switch (state)
            {
                case STATE.IDLE:
                    {
                        break;
                    }
                case STATE.RUN:
                    {
                        elapsedTime += Time.deltaTime;
                        if (elapsedTime >= duration)
                        {
                            state = STATE.FINSIHED;
                        }
                        break;
                    }
                case STATE.FINSIHED:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public void Go()
        {
            elapsedTime = 0;
            state = STATE.RUN;
        }
    }
}
