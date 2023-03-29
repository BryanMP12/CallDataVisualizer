using System;
using UI.Gallery;
using UnityEngine;

namespace UI.FilterGallery {
    public class FilterElementRep : ElementRep {
        //
        public readonly string Name;
        public readonly int TotalCount;
        public readonly int OfficerInitiatedCount;
        public readonly int[] PriorityCount;
        public int NonOfficerInitiatedCount() => TotalCount - OfficerInitiatedCount;
        //Ratio
        public float NonOfficerInitiatedRatio => (float) NonOfficerInitiatedCount() / TotalCount;
        public float OfficerInitiatedRatio => (float) OfficerInitiatedCount / TotalCount;
        public float PriorityRatio(int priority) => (float) PriorityCount[priority] / TotalCount;
        //
        //
        readonly Action<bool> SwitchButtonClicked;
        public bool SwitchedOn = true;
        public FilterElementRep(string n, int tc, int oic, int[] pc, Action<bool> sbc) =>
            (Name, TotalCount, OfficerInitiatedCount, PriorityCount, SwitchButtonClicked) = (n, tc, oic, pc, sbc);
        public bool Switch() {
            SwitchedOn = !SwitchedOn;
            SwitchButtonClicked?.Invoke(SwitchedOn);
            return SwitchedOn;
        }
        //Dimensions
        static Vector2 RectLimits;
        public static void SetRectLimits(Vector2 limits) => RectLimits = new Vector2(10 + limits.x, 10 + limits.y);
        public override Vector2 PositionLimits => RectLimits;
        public override float YSize => 20;
        public FilterElementRep() { }
        //
    }
}