// Copyright eeGeo Ltd (2012-2014), All Rights Reserved
using System;
using UnityEngine;

namespace Wrld.MapInput.Mouse
{
    public class MouseTapGesture
    {
        int m_tapDownCount;
        int m_tapUpCount;
        int m_currentPointerTrackingStack;
        float m_tapAnchorX;
        float m_tapAnchorY;
        Int64 m_tapUnixTime;

        private const long DoubleTapMaxDelayMillisecond = 200;
        private const long TapDeadZonePixels = 40;
        private const long TapDeadZonePixelsSq = TapDeadZonePixels * TapDeadZonePixels;

        private IUnityInputHandler m_tapHandler;

        private void DispatchSingleTap()
        {
            if (NoPointerDown())
            {
                AppInterface.TapData tap = new AppInterface.TapData();
                tap.point.Set(m_tapAnchorX, m_tapAnchorY);
                m_tapHandler.Event_TouchTap(tap);
            }

            Reset();
        }

        private void DispatchDoubleTap()
        {
            if (NoPointerDown())
            {
                AppInterface.TapData tap = new AppInterface.TapData();
                tap.point.Set(m_tapAnchorX, m_tapAnchorY);
                m_tapHandler.Event_TouchDoubleTap(tap);
            }

            Reset();
        }

        private bool WithinAnchorDeadzone(MouseInputEvent mouseEvent)
        {
            Debug.Assert(IsTapping());

            Vector2 tapLocation = new Vector2(m_tapAnchorX, m_tapAnchorY);
            Vector2 currentLocation = new Vector2(mouseEvent.x, mouseEvent.y);

            if ((tapLocation - currentLocation).sqrMagnitude < TapDeadZonePixelsSq)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsTapping()
        {
            return m_tapDownCount > 0;
        }

        private void Reset()
        {
            m_tapDownCount = 0;
            m_tapUpCount = 0;
            m_currentPointerTrackingStack = 0;
        }

        private void TrackPointerDown()
        {
            ++m_currentPointerTrackingStack;
        }

        private void TrackPointerUp()
        {
            --m_currentPointerTrackingStack;
            m_currentPointerTrackingStack = Math.Max(0, m_currentPointerTrackingStack);
        }

        private bool NoPointerDown()
        {
            return m_currentPointerTrackingStack == 0;
        }

        public MouseTapGesture(IUnityInputHandler tapHandler)
        {
            m_tapHandler = tapHandler;
            m_tapDownCount = 0;
            m_tapUpCount = 0;
            m_currentPointerTrackingStack = 0;
            m_tapAnchorX = 0.0f;
            m_tapAnchorY = 0.0f;
        }

        public void PointerDown(MouseInputEvent mouseEvent)
        {
            TrackPointerDown();

            if (m_tapDownCount == 0)
            {
                m_tapAnchorX = mouseEvent.x;
                m_tapAnchorY = mouseEvent.y;
            }

            ++m_tapDownCount;
            m_tapUnixTime = MillisecondsSinceEpoch();
        }

        private static long MillisecondsSinceEpoch()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
        }

        public void PointerUp(MouseInputEvent mouseEvent)
        {
            TrackPointerUp();

            if (IsTapping())
            {
                ++m_tapUpCount;

                if (m_tapDownCount > m_tapUpCount)
                {
                    Reset();
                    return;
                }

                if (m_tapDownCount >= 2)
                {
                    if (WithinAnchorDeadzone(mouseEvent))
                    {
                        DispatchDoubleTap();
                    }
                    else
                    {
                        // Two taps have occurred in the time window, but too far apart on screen, should be two single taps.

                        DispatchSingleTap();

                        m_tapAnchorX = mouseEvent.x;
                        m_tapAnchorY = mouseEvent.y;

                        DispatchSingleTap();
                    }
                }
            }
        }

        public void PointerMove(MouseInputEvent mouseEvent)
        {
            if (IsTapping() && !WithinAnchorDeadzone(mouseEvent))
            {
                Reset();
            }
        }

        public void Update(float deltaSeconds)
        {
            if (IsTapping())
            {
                long timeSinceFirstTapMilliseconds = MillisecondsSinceEpoch() - m_tapUnixTime;

                if (timeSinceFirstTapMilliseconds >= DoubleTapMaxDelayMillisecond)
                {
                    if (m_tapDownCount == 1 && m_tapUpCount == 1)
                    {
                        DispatchSingleTap();
                    }
                    else
                    {
                        Reset();
                    }
                }
            }
            else
            {
                Debug.Assert(m_tapUpCount == 0, "Invalid tap gesture state, a pointer is up having never went down.\n");
            }
        }
    };
}
