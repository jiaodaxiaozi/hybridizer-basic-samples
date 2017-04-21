﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixCusparse
{
    class VectorReader
    {
        public static float[] GetRandomVector(int size)
        {
            float[] res = new float[size];
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < size; ++i)
            {
                res[i] = rand.NextFloat();
            }
            return res;
        }

        public static float[] GetSplatVector(int size, float value)
        {
            float[] res = new float[size];
            for (int i = 0; i < size; ++i)
            {
                res[i] = value;
            }

            return res;
        }

        public static float[] ReadVectorFromFile(String filePath)
        {
            string path = GetPath(filePath);

            int rowCount;
            string line;

            StreamReader reader = new StreamReader(path);

            while ((line = reader.ReadLine()) != null && line.StartsWith("%")) { }

            ReadVectorSize(line, out rowCount);

            float[] res = new float[rowCount];
            int cpt = 0;
            while ((line = reader.ReadLine()) != null)
            {
                string[] lineSplitted = line.Split(' ');
                float data;
                ReadData(lineSplitted, out data);
                res[cpt] = data;
                ++cpt;
            }

            return res;
        }

        private static string GetPath(string filePath)
        {
            string path;
            if (Path.IsPathRooted(filePath))
            {
                path = filePath;
            }
            else
            {
                path = Path.Combine(Environment.CurrentDirectory, filePath);
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }

            return path;
        }

        private static void ReadVectorSize(string line, out int rowCount)
        {
            string[] lineSplitted = line.Split(' ');
            if (lineSplitted.Length != 2)
            {
                throw new ApplicationException("cannot read matrix size");
            }
            else
            {
                if (!int.TryParse(lineSplitted[0], out rowCount))
                {
                    throw new ApplicationException("cannot read matrix size");
                }
            }
        }

        private static void ReadData(string[] lineSplitted, out float data)
        {
            if (!Single.TryParse(lineSplitted[0], NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out data))
            {
                throw new ApplicationException("invalid line");
            }
        }
    }
}
