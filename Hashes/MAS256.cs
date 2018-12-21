using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
// This ideally should take in an arbitrary string and spit out another string representing a hexadecimal 
// number. I might try out a couple different hashing algorithms.
namespace HashApp {
    public class MAS256 {
        public static string[] hexaDigRef = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f"};

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

        // Making a new hashing function, hopefully better...
        public static string mas256(string input) {
            if(input == " ") {
                return "0000000000000000000000000000000000000000000000000000000000000000";
            }

            // 1) Convert String to bitArray
            byte[] inputBytes;
            inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            Array.Reverse(inputBytes);
            BitArray bits = new BitArray(inputBytes);

            // 2) Convert bitArray to int32 Array
            int[] nums = bitIntConversion(bits);
            
            // 3) Use Sine Function
            int[] x = sineArray(nums);

            // 4) "Flip" bit array.
            BitArray bitStar = bits.Not();

            // 5) Convert new bit array.
            int[] numsStar = bitIntConversion(bitStar);

            // 6) Use Sine Function on b*.
            int[] xStar = sineArray(numsStar);

            // 7) Combine x and x*
            int[] y = new int[x.Length];
            for(int ii = 0; ii < x.Length; ii++) {
                y[ii] = (x[ii] + xStar[ii]) / 2;
            }

            // 8) Convert y's to hexadecimal.
            string[] h = new string[y.Length];
            for(int ii = 0; ii < y.Length; ii++) {
                h[ii] = toHexa(y[ii]);
            }

            // 9) Concatenate h values.
            string final = "";
            for(int ii = 0; ii < h.Length; ii++) {
                final += h[ii];
            }
            return final;
        }

        //  ******************************************************************************************************
        // Builds an integer using the bits and the digits for a number in base 2.
        // So long as bits.Lengths < 32, this will always return a positive integer, otherwise you will have overflow.
        private static int bitsToInt(BitArray bits) {
            int sum = 0;
            for(int ii = 0; ii < bits.Length; ii++) {
                sum = sum * 2;
                if(bits[ii]) {
                    sum += 1;
                }
            }
            return sum;
        }

        private static int[] bitsToIntArray(BitArray bits) {
            int[] nums = new int[16];
            // int tracker = 0;
            BitArray tempArr = new BitArray(32, false);
            for(int ii = 0; ii < nums.Length; ii++) {
                for(int jj = 0; jj < tempArr.Length; jj++) {
                    tempArr[jj] = bits[((32 * ii) + jj) % bits.Length];
                }
                nums[ii] = bitsToInt(tempArr);
            }
            return nums;
        }

        // Total function taking a bit array, and doing the funky stuff to it to create an array of nums.
        private static int[] bitIntConversion (BitArray bits) {
            int[] nums = new int[16];
            if(bits.Length <= 32) {
                BitArray ext = new BitArray(32, false);
                for(int ii = 0; ii < ext.Length; ii++) {
                    ext[ii] = bits[ii % bits.Length];
                }
                int temp = bitsToInt(ext);
                for(int ii = 0; ii < nums.Length; ii++) {
                    nums[ii] = temp;
                }
            } else if(bits.Length > 32 && bits.Length <= (nums.Length * 32)) {
                nums = bitsToIntArray(bits);
            } else { // bits.Length > nums.Length * 32
                BitArray temp1;
                BitArray temp2;
                BitArray shorter = bits;
                int half;
                while(shorter.Length > (nums.Length * 32)) {
                    half = shorter.Length / 2;
                    temp1 = new BitArray(half, false);
                    temp2 = new BitArray(half, true);
                    for(int ii = 0; ii < half; ii++) {
                        temp1[ii] = shorter[ii];
                        temp2[ii] = shorter[ii + half];
                    }
                    shorter = new BitArray(half, false);
                    for(int ii = 0; ii < half; ii++) {
                        shorter[ii] = temp1[ii] ^ temp2[ii];
                    }
                }
                nums = bitsToIntArray(bits);
            }

            return nums;
        }

        private static int[] sineArray(int[] nums) {
            int[] x = new int[nums.Length];
            int halfMax = (int) Math.Floor(Math.Pow(2, 16));
            for(int ii = 0; ii < nums.Length; ii++) {
                // x_i = Floor(2^16 * sin(n_i + i))
                x[ii] = (int) Math.Floor(halfMax * Math.Abs(Math.Sin(nums[ii] + ii)));
            }
            return x;
        }

        // this function takes in a number and converts it into a string representing a hexadecimal number. 
        // This function specifically only works on numbers between 0 and 2^16.
        private static string toHexa(int bTen) {
            // this array represents the coefficient for each power of 16 between 16^0 and 16^3.
            int[] hexaDigits = new int[4];
            int temp;
            for(int ii = hexaDigits.Length - 1; ii >= 0; ii--) {
                temp = (int) Math.Pow(16, ii);
                hexaDigits[ii] = bTen / temp;
                bTen = bTen % temp;
            }
            string num = "";
            for(int ii = 0; ii < hexaDigits.Length; ii++) {
                num += hexaDigRef[hexaDigits[ii]];
            }
            return num;
        }
    }
}