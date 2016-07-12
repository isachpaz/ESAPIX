﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESAPIX.Enums;
using ESAPIX.Types;

namespace ESAPIX.Extensions
{
    public static class DVHDataExtensions
    {
        /// <summary>
        /// Gets the volume that recieves the input dose
        /// </summary>
        /// <param name="dvh">the dvhPoint array that is queried</param>
        /// <param name="doseGy">the dose at which to find the volume</param>
        /// <returns>the volume in the same units as the DVH point array</returns>
        public static double GetVolumeAtDose(this DVHPoint[] dvh, double dose, DoseUnit units = DoseUnit.Gy)
        {
            var curve = dvh.Select(d => new { Dose = d.DoseValue.GetDose(units), Volume = d.Volume, VolumeUnit = d.VolumeUnit });
            var maxDose = curve.Max(d => d.Dose);
            var minDose = curve.Min(d => d.Dose);

            if (maxDose < dose || dose< minDose)
            {
                return maxDose < dose ? 0 : dvh.Max(d=>d.Volume);
            }
            else
            {
                //Interpolate
                var higherPoints = curve.Where(p => p.Dose > dose);
                var lowerPoints = curve.Where(p => p.Dose <= dose);

                var point1 = higherPoints.First();
                var point2 = lowerPoints.Last();
                var volumeAtPoint = Interpolate(point1.Dose, point2.Dose, point1.Volume, point2.Volume, dose);
                return volumeAtPoint;
            }
        }

        public static DoseValue GetDoseAtVolume(this DVHPoint[] dvh, double volume)
        {
            var minVol = dvh.Min(d => d.Volume);
            var maxVol = dvh.Max(d => d.Volume);

            //Check for max point dose scenario
            if (volume <= minVol) { return dvh.FirstOrDefault(dos => dos.DoseValue.Dose == dvh.Max(d => d.DoseValue.Dose)).DoseValue; }
            var dvhList = dvh.ToList();

            //Check dose to total volume scenario
            if (volume >= maxVol)
            {
                return dvh.First(d => d.Volume == maxVol).DoseValue;
            }

            var minVolumeDiff = dvhList.Min(d => Math.Abs(d.Volume - volume));
            var closestPoint = dvhList.First(d => Math.Abs(d.Volume - volume) == minVolumeDiff);

            if (minVolumeDiff < 0.001) { return closestPoint.DoseValue; }
            else
            {
                //Interpolate
                var index1 = dvhList.IndexOf(closestPoint);
                var index2 = closestPoint.Volume < volume ? index1 - 1 : index1 + 1;

                if (index1 > 0 && index2 < dvh.Count())
                {
                    var point1 = dvhList[index1];
                    var point2 = dvhList[index2];
                    var doseAtPoint = Interpolate(point1.Volume, point2.Volume, point1.DoseValue.Dose, point2.DoseValue.Dose, volume);
                    return new DoseValue(doseAtPoint, point1.DoseValue.Unit);
                }
                return new DoseValue(double.NaN, closestPoint.DoseValue.Unit);
                throw new Exception(string.Format("Interpolation failed. Index was : {0}, DVH Point Count : {1}, Volume was {2}, ClosestVol was {3}", index1, dvh.Count(), volume, minVolumeDiff));
            }
        }

        public static DoseValue GetMinimumDoseAtVolume(this DVHPoint[] dvh, double volume)
        {
            var maxVol = dvh.Max(d => d.Volume);
            var volOfInterest = maxVol - volume;
            return GetDoseAtVolume(dvh, volOfInterest);
        }

        private static double Interpolate(double x1, double x3, double y1, double y3, double x2)
        {
            return (x2 - x1) * (y3 - y1) / (x3 - x1) + y1;
        }
    }
}