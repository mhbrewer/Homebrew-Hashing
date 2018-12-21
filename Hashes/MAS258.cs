using System;
using System.Collections.Generic;
using System.Numerics;
// This ideally should take in an arbitrary string and spit out another string representing a hexadecimal 
// number. I might try out a couple different hashing algorithms.
namespace HashApp {
    public class MAS257 {

        // This is almost what I want, but it only takes in integers and returns a hashed integer, so the
        // only other thing to do are some conversions. This hash is based on the hailstone sequence.
        // This only works for small numbers so I need to do another with BigInteger objects 
        // (since I need numbers that cover up to 2^256 since I will be having 16 digit hexadecimal numbers).
        public static int hailIntHash(int input) {
            // This is to make it so that nearby inputs don't produce nearby outputs.
            // 104729 is prime.
            double doubStart = (1000003) * Math.Sin((double) input) + 104729;
            int start = (int) doubStart;
            int end = HailstoneSequence.manyIter(start, 25);
            return end + input;
        }

        // This takes in a string and outputs a hexadecimal number, represented as a string.
        public static string hailStrHash(string input) {
            int strInt = parseSum(input);
            double doubStart = (1000003) * Math.Sin((double) strInt) + 104729;
            int start = (int) doubStart;
            int end = HailstoneSequence.manyIter(start, 25);
            int sum = end + strInt;
            string hex = sum.ToString("X");
            return hex;
        }

        public static int parseSum(string str) {
            int sum = 0;
            for(int ii = 0; ii < str.Length; ii++) {
                sum += str[ii];
            }
            return sum;
        }
    }
}