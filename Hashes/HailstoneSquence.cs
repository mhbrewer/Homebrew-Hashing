using System;
using System.Collections.Generic;
using System.Numerics;

namespace HashApp {
    public class HailstoneSequence {

        public HailstoneSequence() {

        }

        // One iteration of the sequence. Returns the outcome of that iteration.
        public static int oneIter(int n) {
            // Even
            if(n % 2 == 0) {
                return n / 2;
            }
            // Odd
            else {
                return 3 * n + 1;
            }
        }

        // A number of iterations of the sequence as determined by the iter variable. 
        // Returns the outcome of that many iterations.
        public static int manyIter(int start, int iter) {
            int temp = start;
            for(int ii = 0; ii < iter; ii++) {
                temp = oneIter(temp);
            }
            return temp;
        }

        // Runs the entire sequence until 1 comes up as an outcome. Returns the entire sequence.
        public static List<int> entireSeq(int start) {
            List<int> returnList = new List<int>();
            returnList.Add(start);
            int temp = start;
            while(temp != 1) {
                temp = oneIter(temp);
                returnList.Add(temp);
            }
            return returnList;
        }


        // Need to make these same methods with BigInteger objects instead of ints.
        public static BigInteger oneIter(BigInteger n) {
            // Even
            if(BigInteger.ModPow(n, 1, 2).Equals(0)) {
                return BigInteger.Divide(n, 2);
            }
            // Odd
            else {
                return BigInteger.Add(BigInteger.Multiply(3, n), 1);
            }
        }

        // Iterator does not need to be Big b/c we are only going to do on the order of 10^2 number of
        //  iterations in general.
        public static BigInteger manyIter(BigInteger start, int iter) {
            BigInteger temp = start;
            for(int ii = 0; ii < iter; ii++) {
                temp = oneIter(temp);
            }
            return temp;
        }

        public static List<BigInteger> entireSeq(BigInteger start) {
            List<BigInteger> returnList = new List<BigInteger>();
            returnList.Add(start);
            BigInteger temp = start;
            while(temp != 1) {
                temp = oneIter(temp);
                returnList.Add(temp);
            }
            return returnList;
        }
    }
}