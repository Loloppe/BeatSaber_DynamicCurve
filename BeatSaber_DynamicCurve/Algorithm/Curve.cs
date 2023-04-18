using System;
using System.Collections.Generic;
using System.Linq;
using static BeatmapSaveDataVersion3.BeatmapSaveData;

namespace BeatSaber_DynamicCurve.Algorithm
{
    internal class Curve
    {
        public const double curveMultiplier = 0.1;
        public const double sliderMultiplier = 0.15;
        public const int sliderThreshold = 15; // At which X should the multipler switch from positive to negative
        public const double linear = 0.33; // Value to nerf linear by when it come to distance calculation

        // Base curve used to calculate the new curve
        public static double[] curveX = { 1, 0.999, 0.9975, 0.995, 0.9925, 0.99, 0.9875, 0.985, 0.9825, 0.98, 0.9775, 0.975, 0.9725, 0.97, 0.965, 0.96, 0.955, 0.95, 0.94, 0.93, 0.92, 0.91, 0.9, 0.875, 0.85, 0.825, 0.8, 0.75, 0.7, 0.65, 0.6, 0 };
        public static double[] curveY = { 7.059, 5.936, 4.905, 3.813, 3.083, 2.567, 2.19, 1.909, 1.698, 1.539, 1.417, 1.323, 1.251, 1.194, 1.11, 1.044, 0.991, 0.95, 0.884, 0.83, 0.785, 0.746, 0.712, 0.645, 0.588, 0.542, 0.506, 0.449, 0.408, 0.371, 0.337, 0 };
        
        public static double[] finalY = new double[32];
        public static double distance = 0;

        public static double[] Curves(List<ColorNoteData> notes, List<BombNoteData> bombs, float bpm)
        {
            // Reset
            distance = 0;
            finalY = new double[32];

            // Find pattern per hand
            List<Cube> cube = new List<Cube>();

            foreach (var note in notes)
            {
                cube.Add(new Cube(note));
            }

            cube.OrderBy(c => c.Beat);
            var red = cube.Where(c => (int)c.Note.color == 0).ToList();
            var blue = cube.Where(c => (int)c.Note.color == 1).ToList();

            // Everything is calculated per hand, as it's way easier that way
            if (red.Count() > 0)
            {
                Helper.FindNoteDirection(red, bombs, bpm);
                Helper.FixPatternHead(red);
                Helper.CalculateDistance(red);
            }

            if (blue.Count() > 0)
            {
                Helper.FindNoteDirection(blue, bombs, bpm);
                Helper.FixPatternHead(blue);
                Helper.CalculateDistance(blue);
            }
            
            // Slider multipler * (slider count / swing count)
            for (int i = 0; i < curveX.Count(); i++)
            {
                if(i <= sliderThreshold)
                {
                    finalY[i] = curveY[i] * (1 + (sliderMultiplier * ((double)cube.Where(c => c.Slider).Count() / cube.Where(c => c.Head || !c.Pattern).Count())));
                }
                else
                {
                    finalY[i] = curveY[i] * (1 + (-sliderMultiplier * ((double)cube.Where(c => c.Slider).Count() / cube.Where(c => c.Head || !c.Pattern).Count())));
                }
            }

            // Distance multiplier * (sum of distances / swing count)
            foreach(var c in cube)
            {
                distance += c.Distance;
            }
            distance /= cube.Where(c => c.Head || !c.Pattern).Count();

            for (int i = 0; i < curveX.Count(); i++)
            {
                finalY[i] *= 1 + (curveMultiplier * distance);
            }

            Plugin.Log.Info(Math.Round((double)cube.Where(c => c.Linear).Count() / cube.Where(c => c.Head || !c.Pattern).Count() * 100, 2).ToString() + "% linear " + Math.Round((double)cube.Where(c => c.Slider).Count() / cube.Where(c => c.Head || !c.Pattern).Count() * 100, 2).ToString() + "% slider " + Math.Round(distance, 2) + " distance");

            return finalY;
        }
    }
}
