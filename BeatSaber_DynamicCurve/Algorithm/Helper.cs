using static BeatmapSaveDataVersion3.BeatmapSaveData;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BeatSaber_DynamicCurve.Algorithm
{
    internal class Helper
    {
        #region Array

        public static int[] VerticalSwing = { 0, 1, 4, 5, 6, 7 };
        public static int[] HorizontalSwing = { 2, 3, 4, 5, 6, 7 };
        public static int[] DiagonalSwing = { 4, 5, 6, 7 };
        public static int[] PureVerticalSwing = { 0, 1 };
        public static int[] PureHorizontalSwing = { 2, 3 };

        public static double[] UpSwing = { 0, 4, 5 };
        public static double[] DownSwing = { 1, 6, 7 };
        public static double[] LeftSwing = { 2, 4, 6 };
        public static double[] RightSwing = { 3, 5, 7 };
        public static double[] UpLeftSwing = { 0, 2, 4 };
        public static double[] DownLeftSwing = { 1, 2, 6 };
        public static double[] UpRightSwing = { 0, 3, 5 };
        public static double[] DownRightSwing = { 1, 3, 7 };

        #endregion

        public static void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            (list[indexB], list[indexA]) = (list[indexA], list[indexB]);
        }

        public static void SwapValue(List<Cube> list, int indexA, int indexB)
        {
            (list[indexB].Head, list[indexA].Head) = (list[indexA].Head, list[indexB].Head);
            (list[indexB].Reset, list[indexA].Reset) = (list[indexA].Reset, list[indexB].Reset);
        }

        public static void FixPatternHead(List<Cube> cubes)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int i = 1; i < cubes.Count(); i++)
                {
                    if (cubes[i].Note.beat == cubes[i - 1].Note.beat)
                    {
                        switch (cubes[i - 1].Direction)
                        {
                            case 0:
                                if (cubes[i - 1].Layer > cubes[i].Layer)
                                {
                                    Swap(cubes, i - 1, i);
                                    SwapValue(cubes, i - 1, i);
                                }
                                break;
                            case 1:
                                if (cubes[i - 1].Layer < cubes[i].Layer)
                                {
                                    Swap(cubes, i - 1, i);
                                    SwapValue(cubes, i - 1, i);
                                }
                                break;
                            case 2:
                                if (cubes[i - 1].Line < cubes[i].Line)
                                {
                                    Swap(cubes, i - 1, i);
                                    SwapValue(cubes, i - 1, i);
                                }
                                break;
                            case 3:
                                if (cubes[i - 1].Line > cubes[i].Line)
                                {
                                    Swap(cubes, i - 1, i);
                                    SwapValue(cubes, i - 1, i);
                                }
                                break;
                            case 4:
                                if (cubes[i - 1].Line < cubes[i].Line)
                                {
                                    Swap(cubes, i - 1, i);
                                    SwapValue(cubes, i - 1, i);
                                }
                                else if (cubes[i - 1].Layer > cubes[i].Layer)
                                {
                                    Swap(cubes, i - 1, i);
                                    SwapValue(cubes, i - 1, i);
                                }
                                break;
                            case 5:
                                if (cubes[i - 1].Line > cubes[i].Line)
                                {
                                    Swap(cubes, i - 1, i);
                                    SwapValue(cubes, i - 1, i);
                                }
                                else if (cubes[i - 1].Layer > cubes[i].Layer)
                                {
                                    Swap(cubes, i - 1, i);
                                    SwapValue(cubes, i - 1, i);
                                }
                                break;
                            case 6:
                                if (cubes[i - 1].Line < cubes[i].Line)
                                {
                                    Swap(cubes, i - 1, i);
                                    SwapValue(cubes, i - 1, i);
                                }
                                else if (cubes[i - 1].Layer < cubes[i].Layer)
                                {
                                    Swap(cubes, i - 1, i);
                                    SwapValue(cubes, i - 1, i);
                                }
                                break;
                            case 7:
                                if (cubes[i - 1].Line > cubes[i].Line)
                                {
                                    Swap(cubes, i - 1, i);
                                    SwapValue(cubes, i - 1, i);
                                }
                                else if (cubes[i - 1].Layer < cubes[i].Layer)
                                {
                                    Swap(cubes, i - 1, i);
                                    SwapValue(cubes, i - 1, i);
                                }
                                break;
                        }
                    }
                }
            }
        }

        public static void FindReset(List<Cube> cubes)
        {
            for (int i = 1; i < cubes.Count(); i++)
            {
                if (cubes[i].Pattern && !cubes[i].Head)
                {
                    continue;
                }

                if (SameDirection(cubes[i - 1].Direction, cubes[i].Direction))
                {
                    cubes[i].Reset = true;
                    continue;
                }
            }
        }

        public static bool SameDirection(double before, double after)
        {
            switch (before)
            {
                case 0:
                    if (UpSwing.Contains(after)) return true;
                    break;
                case 1:
                    if (DownSwing.Contains(after)) return true;
                    break;
                case 2:
                    if (LeftSwing.Contains(after)) return true;
                    break;
                case 3:
                    if (RightSwing.Contains(after)) return true;
                    break;
                case 4:
                    if (UpLeftSwing.Contains(after)) return true;
                    break;
                case 5:
                    if (UpRightSwing.Contains(after)) return true;
                    break;
                case 6:
                    if (DownLeftSwing.Contains(after)) return true;
                    break;
                case 7:
                    if (DownRightSwing.Contains(after)) return true;
                    break;
            }

            return false;
        }

        public static void FindNoteDirection(List<Cube> cubes, List<BombNoteData> bombs, float bpm)
        {
            if (cubes[0].Assumed)
            {
                var c = cubes.Where(ca => !ca.Assumed).FirstOrDefault();
                if (c != null)
                {
                    int temp = 1;
                    for (int i = 0; i < cubes.IndexOf(c); i++)
                    {
                        temp = ReverseCutDirection((int)c.Note.cutDirection);
                    }
                    cubes[0].Direction = temp;
                }
                else
                {
                    if (cubes[0].Note.layer == 2)
                    {
                        cubes[0].Direction = 0;
                    }
                    else
                    {
                        cubes[0].Direction = 1;
                    }
                }
            }
            else
            {
                cubes[0].Direction = (int)cubes[0].Note.cutDirection;
            }

            bool pattern = false;

            FixPatternHead(cubes);

            BombNoteData bo = null;

            for (int i = 1; i < cubes.Count(); i++)
            {
                if (cubes[i].Beat - cubes[i - 1].Beat <= (0.25 / 200 * bpm) && (cubes[i].Note.cutDirection == cubes[i - 1].Note.cutDirection ||
                    cubes[i].Assumed || cubes[i - 1].Assumed || SameDirection((int)cubes[i - 1].Note.cutDirection, (int)cubes[i].Note.cutDirection)))
                {
                    if (!pattern)
                    {
                        cubes[i - 1].Head = true;
                        if (cubes[i].Beat - cubes[i - 1].Beat < 0.26 && cubes[i].Beat - cubes[i - 1].Beat >= 0.01)
                        {
                            cubes[i - 1].Slider = true;
                        }
                    }

                    cubes[i - 1].Pattern = true;
                    cubes[i].Pattern = true;
                    pattern = true;
                }
                else
                {
                    pattern = false;
                }

                bo = bombs.LastOrDefault(b => cubes[i - 1].Beat < b.beat && cubes[i].Beat >= b.beat && cubes[i].Line == b.line);

                if (bo != null)
                {
                    cubes[i].Bomb = true;
                }

                if (cubes[i].Pattern && cubes[i - 1].Bomb)
                {
                    cubes[i].Bomb = cubes[i - 1].Bomb;
                }

                if (cubes[i].Assumed && !cubes[i].Pattern && !cubes[i].Bomb)
                {
                    cubes[i].Direction = ReverseCutDirection((int)cubes[i - 1].Note.cutDirection);
                }
                else if (cubes[i].Assumed && cubes[i].Pattern)
                {
                    cubes[i].Direction = cubes[i - 1].Direction;
                }
                else if (cubes[i].Assumed && cubes[i].Bomb)
                {
                    if (bo.layer == 0)
                    {
                        cubes[i].Direction = 1;
                    }
                    else if (bo.layer == 1)
                    {
                        if (cubes[i].Layer == 0)
                        {
                            cubes[i].Direction = 0;
                        }
                        else
                        {
                            cubes[i].Direction = 1;
                        }
                    }
                    else if (bo.layer == 2)
                    {
                        cubes[i].Direction = 0;
                    }
                }
                else
                {
                    cubes[i].Direction = (int)cubes[i].Note.cutDirection;
                }
            }
        }

        public static int ReverseCutDirection(int direction)
        {
            switch(direction)
            {
                case 0: return 1;
                case 1: return 0;
                case 2: return 3;
                case 3: return 2;
                case 4: return 7;
                case 5: return 6;
                case 6: return 5;
                case 7: return 4;
                default: return 8;
            }
        }

        public static bool IsLinearAndNotInverted(Cube previous, Cube current)
        {
            if(SameDirection(previous.Direction, current.Direction))
            {
                return false;
            }

            switch (previous.Direction)
            {
                case 0: 
                    if(current.Layer <= previous.Layer && current.Line == previous.Line && (current.Direction == 1 || current.Direction == 6 || current.Direction == 7))
                    {
                        return true;
                    }
                    if(current.Layer < previous.Layer)
                    {
                        if(current.Line < previous.Line && (current.Direction == 1 || current.Direction == 2 || current.Direction == 6))
                        {
                            return true;
                        }
                        if (current.Line > previous.Line && (current.Direction == 1 || current.Direction == 3 || current.Direction == 7))
                        {
                            return true;
                        }
                    }
                    break;
                case 1:
                    if (current.Layer >= previous.Layer && current.Line == previous.Line && (current.Direction == 0 || current.Direction == 4 || current.Direction == 5))
                    {
                        return true;
                    }
                    if (current.Layer > previous.Layer)
                    {
                        if (current.Line < previous.Line && (current.Direction == 0 || current.Direction == 2 || current.Direction == 4))
                        {
                            return true;
                        }
                        if (current.Line > previous.Line && (current.Direction == 0 || current.Direction == 3 || current.Direction == 5))
                        {
                            return true;
                        }
                    }
                    break;
                case 2:
                    if (current.Layer == previous.Line && current.Line >= previous.Line && (current.Direction == 3 || current.Direction == 5 || current.Direction == 7))
                    {
                        return true;
                    }
                    if (current.Line > previous.Line)
                    {
                        if (current.Layer < previous.Layer && (current.Direction == 1 || current.Direction == 3 || current.Direction == 7))
                        {
                            return true;
                        }
                        if (current.Layer > previous.Layer && (current.Direction == 0 || current.Direction == 3 || current.Direction == 5))
                        {
                            return true;
                        }
                    }
                    break;
                case 3:
                    if (current.Layer == previous.Line && current.Line <= previous.Line && (current.Direction == 2 || current.Direction == 4 || current.Direction == 6))
                    {
                        return true;
                    }
                    if (current.Line < previous.Line)
                    {
                        if (current.Layer < previous.Layer && (current.Direction == 1 || current.Direction == 2 || current.Direction == 6))
                        {
                            return true;
                        }
                        if (current.Layer > previous.Layer && (current.Direction == 0 || current.Direction == 2 || current.Direction == 4))
                        {
                            return true;
                        }
                    }
                    break;
                case 4:
                    if (current.Layer <= previous.Layer && current.Line >= previous.Line)
                    {
                        if(current.Direction == 7)
                        {
                            return true;
                        }
                        if(current.Layer < previous.Layer && current.Direction == 1)
                        {
                            return true;
                        }
                        if (current.Layer == previous.Layer && current.Direction == 5)
                        {
                            return true;
                        }
                        if (current.Line > previous.Line && current.Direction == 3)
                        {
                            return true;
                        }
                        if (current.Line == previous.Line && current.Direction == 6)
                        {
                            return true;
                        }
                    }
                    break;
                case 5:
                    if (current.Layer <= previous.Layer && current.Line <= previous.Line)
                    {
                        if (current.Direction == 6)
                        {
                            return true;
                        }
                        if (current.Layer < previous.Layer && current.Direction == 1)
                        {
                            return true;
                        }
                        if (current.Layer == previous.Layer && current.Direction == 4)
                        {
                            return true;
                        }
                        if (current.Line < previous.Line && current.Direction == 2)
                        {
                            return true;
                        }
                        if (current.Line == previous.Line && current.Direction == 7)
                        {
                            return true;
                        }
                    }
                    break;
                case 6:
                    if (current.Layer >= previous.Layer && current.Line >= previous.Line)
                    {
                        if (current.Direction == 5)
                        {
                            return true;
                        }
                        if (current.Layer > previous.Layer && current.Direction == 0)
                        {
                            return true;
                        }
                        if (current.Layer == previous.Layer && current.Direction == 7)
                        {
                            return true;
                        }
                        if (current.Line > previous.Line && current.Direction == 3)
                        {
                            return true;
                        }
                        if (current.Line == previous.Line && current.Direction == 4)
                        {
                            return true;
                        }
                    }
                    break;
                case 7:
                    if (current.Layer >= previous.Layer && current.Line >= previous.Line)
                    {
                        if (current.Direction == 4)
                        {
                            return true;
                        }
                        if (current.Layer > previous.Layer && current.Direction == 0)
                        {
                            return true;
                        }
                        if (current.Layer == previous.Layer && current.Direction == 6)
                        {
                            return true;
                        }
                        if (current.Line < previous.Line && current.Direction == 2)
                        {
                            return true;
                        }
                        if (current.Line == previous.Line && current.Direction == 5)
                        {
                            return true;
                        }
                    }
                    break;
                default: return false;
            }

            return false;
        }

        public static bool IsInLinearPath(Cube previous, Cube current, Cube next)
        {
            if(previous.Line == current.Line)
            {
                if(current.Line == next.Line)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if (previous.Layer == current.Layer)
            {
                if (current.Layer == next.Layer)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            float a = (current.Layer - previous.Layer) / (current.Line - previous.Line);
            float b = previous.Layer - a * previous.Line;
            if (Math.Abs(next.Layer - (a * next.Line + b)) < 0.001f)
            {
                return true;
            }

            return false;
        }

        public static int count = 0;
        public static int linear = 0;
        public static int slider = 0;

        // Consider exact same direction for 3 notes in a row "linear"
        public static void DetectLinear(List<Cube> cubes)
        {
            count = 0;
            linear = 0;
            slider = 0;

            // First note will always be considered linear
            cubes[0].Linear = true;
            linear++;
            count++;

            if (IsLinearAndNotInverted(cubes[0], cubes[1]))
            {
                cubes[1].Linear = true;
                linear++;
                count++;
            }

            for (int i = 2; i < cubes.Count(); i++)
            {
                if (cubes[i].Head || !cubes[i].Pattern)
                {
                    if (cubes[i].Slider)
                    {
                        slider++;
                    }
                    count++;
                    if (cubes[i].Reset)
                    {
                        // Reset will be considered non-linear unless the placement is exactly the same
                        if (cubes[i - 1].Line == cubes[i].Line && cubes[i - 1].Layer == cubes[i].Layer)
                        {
                            cubes[i].Linear = true;
                            linear++;
                        }
                    }
                    else // Not a reset
                    {
                        if (IsLinearAndNotInverted(cubes[i - 1], cubes[i]))
                        {
                            if (IsInLinearPath(cubes[i - 2], cubes[i - 1], cubes[i]))
                            {
                                cubes[i].Linear = true;
                                linear++;
                            }
                        }
                    }
                }
            }
        }

        public static bool IsInverted(Cube previous, Cube current)
        {
            if (RightSwing.Contains(current.Direction) && current.Line < previous.Line)
            {
                return true;
            }
            if (LeftSwing.Contains(current.Direction) && current.Line > previous.Line)
            {
                return true;
            }
            if (DownSwing.Contains(current.Direction) && current.Layer > previous.Line)
            {
                return true;
            }
            if (UpSwing.Contains(current.Direction) && current.Layer < previous.Line)
            {
                return true;
            }

            return false;
        }

        public static void CalculateMovement(List<Cube> cubes)
        {
            for(int i = 1; i < cubes.Count(); i++)
            {
                if ((!cubes[i].Linear && !cubes[i].Pattern) || (!cubes[i].Linear && cubes[i].Head))
                {
                    var distance = Math.Sqrt(Math.Pow(cubes[i].Line - cubes[i - 1].Line, 2) + Math.Pow(cubes[i].Layer - cubes[i - 1].Layer, 2));
                    if (IsInverted(cubes[i - 1], cubes[i]))
                    {
                        Curve.movement += distance * 1.5;
                    }
                    else
                    {
                        Curve.movement += distance;
                    }
                }
            }

            Curve.movement /= count;
        }
    }
}
