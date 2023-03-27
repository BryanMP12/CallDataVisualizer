using System;
using UI.Gallery;
using UnityEngine;

namespace UI.DescriptionGallery {
    public class DescriptionElementRep : ElementRep {
        //
        public readonly string Name;
        public readonly int TotalCount;
        public readonly int OfficerInitiatedCount;
        public readonly int[] PriorityCount;
        //
        readonly Action<bool> SwitchButtonClicked;
        public bool SwitchedOn = true;
        public DescriptionElementRep(string n, int tc, int oic, int[] pc, Action<bool> sbc) =>
            (Name, TotalCount, OfficerInitiatedCount, PriorityCount, SwitchButtonClicked) = (n, tc, oic, pc, sbc);
        public bool Switch() {
            SwitchedOn = !SwitchedOn;
            SwitchButtonClicked?.Invoke(SwitchedOn);
            return SwitchedOn;
        }
        //Dimensions
        static Vector2 RectLimits;
        public static void SetRectLimits(Vector2 limits) => RectLimits = new Vector2(10.5f + limits.x, 10.5f + limits.y);
        public override Vector2 PositionLimits => RectLimits;
        public override float YSize => 21;
        public DescriptionElementRep() { }
        //
    }
}