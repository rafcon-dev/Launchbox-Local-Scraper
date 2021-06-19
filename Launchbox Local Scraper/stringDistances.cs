using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Launchbox_Local_Scraper
{
    static class stringDistances
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int minimumOf(ref int b, ref int a, ref int c)
        {
            if (a < b && a < c)
                return a;
            if (a < b)
                return c;
            if (b < c)
                return b;
            return c;
        }

        public static int levenshteinDistance(string hori, string verti)
        {
            int i, j, jMOne, iMOne, horiLen, vertiLen;

            if (String.IsNullOrEmpty(hori))
                if (String.IsNullOrEmpty(verti))
                    return 0;
                else
                    return verti.Length;
            else
                if (String.IsNullOrEmpty(verti))
                return hori.Length;

            horiLen = hori.Length;
            int horiLenPOne = horiLen + 1;

            vertiLen = verti.Length;

            char[] horiChar = hori.ToCharArray();
            char[] vertiChar = verti.ToCharArray();

            int[] d = new int[(horiLenPOne) * (vertiLen + 1)];

            for (i = 0; i <= horiLen; i++)
                d[i] = i;

            for (j = 0; j <= vertiLen; j++)
                d[(j * (horiLenPOne)) + 0] = j;

            for (i = 1; i <= horiLen; i++)
            {
                iMOne = i - 1;

                for (j = 1; j <= vertiLen; j++)
                {
                    jMOne = j - 1;

                    if (horiChar[iMOne] == vertiChar[jMOne])
                        d[j * (horiLenPOne) + i] = d[(jMOne) * (horiLenPOne) + iMOne];
                    else
                    {
                        d[j * (horiLenPOne) + i] = minimumOf(
                        ref d[(j) * (horiLenPOne) + iMOne], //Deletion
                        ref d[(jMOne) * (horiLenPOne) + i], //Insertion
                        ref d[(jMOne) * (horiLenPOne) + iMOne] //Substitution
                        )
                        + 1;
                    }
                }
            }
            return d[(j - 1) * (horiLenPOne) + i - 1];
        }
        private static List<int> getLeveshteinDistancesBetweenListAndString(List<string> strings, string str)
        {
            List<int> leveshteinDistances = new List<int>();

            foreach (string s in strings)
            {
                leveshteinDistances.Add(levenshteinDistance(s, str));
            }
            return leveshteinDistances;
        }

        private static int leveshteinDistanceDiscardingCommon(string s1, string s2)
        {
            string s1Temp = generalUtils.removeAccents(s1).ToUpper();

            string s2Temp = generalUtils.removeAccents(s2).ToUpper();

            int equalWordsTotalLenght = 0;

            //bool startAgain = false;
            while (true)
            {
                startAgain:;
                string[] s1Words = generalUtils.splitAtWords(s1Temp);
                string[] s2Words = generalUtils.splitAtWords(s2Temp);

                for (int i = 0; i < s1Words.Length; i++)
                {
                    for (int j = 0; j < s2Words.Length; j++)
                    {
                        if (s1Words[i].Equals(s2Words[j]))
                        {
                            equalWordsTotalLenght += s1Words[i].Length;
                            s1Temp = generalUtils.eliminateFirstInstanceOfString(s1Temp, s2Words[j]);
                            s2Temp = generalUtils.eliminateFirstInstanceOfString(s2Temp, s2Words[j]);
                            goto startAgain;
                        }
                    }
                }
                //if we reached here, we haven't found any more equal words
                break;
            }

            int leveshtein = levenshteinDistance(s1Temp, s2Temp);
            return leveshtein - 3 * equalWordsTotalLenght;
        }
        private static void getShortestAndLargestStrings(string s1, string s2, ref string shortest, ref string largest)
        {
            if (s1.Length <= s2.Length)
            {
                shortest = s1;
                largest = s2;
            }
            else
            {
                shortest = s2;
                largest = s1;
            }
        }

        private static int lowestLeveshteinDistanceBetweenIntersecting(string s1, string s2)
        {
            List<int> distances = new List<int>();

            distances.Add(stringDistances.levenshteinDistance(s1, s2)); //first add full distances, then add partials

            string shortest = string.Empty, longest = string.Empty;
            getShortestAndLargestStrings(s1, s2, ref shortest, ref longest);

            for (int i = 0; i + shortest.Length < longest.Length; i++)
            {
                string largestSubstring = longest.Substring(i, shortest.Length);
                distances.Add(stringDistances.levenshteinDistance(shortest, largestSubstring));
            }
            return distances.Min();
        }

        private static List<int> getLowestLeveshteinDistancesBetweenListAndString(List<string> strings, string str)
        {
            List<int> lowestLeveshteinDistances = new List<int>();

            foreach (string s in strings)
            {
                lowestLeveshteinDistances.Add(
                    lowestLeveshteinDistanceBetweenIntersecting(s, str));
            }
            return lowestLeveshteinDistances;
        }

        public static List<int> leveshteinDistancesDiscardingCommonBetweenListAndString(List<string> strings, string str)
        {
            List<int> leveshteinDistances = new List<int>();

            foreach (string s in strings)
            {
                leveshteinDistances.Add(
                    leveshteinDistanceDiscardingCommon(s, str));
                // leveshteinDistances.Add(1);
            }
            return leveshteinDistances;
        }
    }
}
