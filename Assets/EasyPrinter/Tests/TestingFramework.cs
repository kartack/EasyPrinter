using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

#if UNITY_EDITOR
namespace EasyPrinter.Test {
    public enum TestResultCode {
        PASSED,
        ERROR,
        TIME_OUT,
        WRONG_OUTPUT,
        SLOW
    }

    public delegate string TestCase(System.Object toSerialize);

    public static class ThreadStateExtensions {
        public static bool IsStillRunningState(this System.Threading.ThreadState a) {
            switch (a) {
                case ThreadState.Background:
                case ThreadState.Running:
                case ThreadState.Suspended:
                case ThreadState.SuspendRequested:
                case ThreadState.Unstarted:
                    return true;
                default:
                    return false;
            }
        }
    }


    public class ExpectedTestResult {
        public const long TIME_OUT_FACTOR = 100;

        public string testName = null;
        public string expectedOutput = null;
        public long expectedMS = 0;
        public TestCase toPerform = null;
        public System.Object input = null;

        private static long startTime = -1;
        private static long endTime = -1;
        private static volatile string output = null;
        private static volatile string error = null;
        public static Thread testThread = null;

        public ActualTestResult PerformTest() { 
            startTime = -1;
            endTime = -1;
            output = null;
            error = null;

            long stoppedTime = -1;

            testThread = new Thread(this.runTest);
            testThread.Start();
            testThread.Priority = System.Threading.ThreadPriority.Highest;

            testThread.Join((int)(this.expectedMS * TIME_OUT_FACTOR));

            long testTime = endTime - startTime;

            testThread.Abort();

            if (System.Environment.TickCount - startTime >= this.expectedMS * TIME_OUT_FACTOR) {
                stoppedTime = System.Environment.TickCount;
                testThread.Abort();
#if UNITY_EDITOR_WIN
                Debug.LogError("Due to a bug in Mono Thread.Abort does not work properly in the Unity Windows Editor (and on some target platforms). The testing framework has just detected an infinite loop. " +
                    "The tests will continue but you will not be able to hit play in Unity (or even quit Unity gracefully) as whenever you try to do either next it will lock up. Instead open Program Manager " +
                    "and shut down Unity Forcably after you are done looking at the report.");
#endif
                testTime = stoppedTime - startTime;
            }
            
            //if we didn't kill the thread because it ran for too long then get more accurate timing info by running it on this thread
            if(stoppedTime <= 0) {
                this.runTest();
                testTime = endTime - startTime;
            }
            
            TestResultCode testResultCode = TestResultCode.PASSED;
            if (stoppedTime > 0) {
                testResultCode = TestResultCode.TIME_OUT;
            } else if (error != null) {
                testResultCode = TestResultCode.ERROR;
            } else if (output != this.expectedOutput) {
                testResultCode = TestResultCode.WRONG_OUTPUT;
            } else if (testTime > expectedMS) {
                testResultCode = TestResultCode.SLOW;
            }
            
            return new ActualTestResult() {
                expectedResult = this,
                testResultCode = testResultCode,
                output = output,
                actualMS = testTime,
                error = error
            };
        }

        private void runTest() { 
            try {
                startTime = System.Environment.TickCount;
                output = toPerform(input);
            } catch(ThreadAbortException) {
                //this one exception case we don't really want to speak about because we have other ways of handling timeout
            } catch (System.Exception e) {
                error = e.Message + "\n" + e.StackTrace;
            }
            endTime = System.Environment.TickCount;
        }
    }

    public class ActualTestResult {
        public ExpectedTestResult expectedResult;
        public TestResultCode testResultCode;
        public string output;
        public long actualMS;
        public string error;
        
        public StringBuilder AddTestReport(StringBuilder toAddTo) {
            toAddTo.Append("  ").Append(this.expectedResult.testName).Append(": ");
            switch (this.testResultCode) {
                case TestResultCode.PASSED:
                    toAddTo.Append("Passed");
                    break;

                case TestResultCode.WRONG_OUTPUT:
                    toAddTo.Append("Failed Wrong Output\n")
                        .Append("  Expected:\n    ").Append(this.expectedResult.expectedOutput == null ? "expected output is null" : this.expectedResult.expectedOutput.Replace("\n","    \n")).Append("\n")
                        .Append("  Actual:\n    ").Append(this.output == null ? "output is null" : this.output.Replace("\n", "    \n"));
                    break;

                case TestResultCode.ERROR:
                    toAddTo.Append("Failed Exception\n    ").Append(this.error == null ? "null" : this.error.Replace("\n", "    \n"));
                    break;

                case TestResultCode.TIME_OUT:
                    toAddTo.Append("Timed Out: Expected: ").Append(this.expectedResult.expectedMS).Append(" ms, Stopped After: ").Append(this.actualMS).Append(" ms");
                    break;

                case TestResultCode.SLOW:
                    toAddTo.Append("Failed Slow, Excpected: ").Append(this.expectedResult.expectedMS).Append(" ms, Actual: ").Append(this.actualMS).Append(" ms");
                    break;

                default:
                    throw new System.Exception("In printing test case unknown TestResultCode " + this.testResultCode.ToString());
            }
            return toAddTo;
        }
    }

    public abstract class TestingFramework : MonoBehaviour {

        private const long TIME_BEFORE_STOPPING = 3;

        protected abstract List<ExpectedTestResult> getExpectedResults();

        private static List<ActualTestResult> actualResults = new List<ActualTestResult>();
        
        IEnumerator Start() {
            List<ExpectedTestResult> expectedResults = this.getExpectedResults();
            long lastBatchStartTime = System.Environment.TickCount;
            foreach (var curTest in expectedResults) {
                actualResults.Add(curTest.PerformTest());
                if (System.Environment.TickCount - lastBatchStartTime >= TIME_BEFORE_STOPPING) {
                    yield return null;
                    lastBatchStartTime = System.Environment.TickCount;
                }
            }

            Dictionary<TestResultCode, List<ActualTestResult>> testSums = new Dictionary<TestResultCode, List<ActualTestResult>>();
            foreach (var curCode in (TestResultCode[])(System.Enum.GetValues(typeof(TestResultCode)))){
                testSums.Add(curCode, new List<ActualTestResult>());
            }

            foreach(var curTest in actualResults) {
                testSums[curTest.testResultCode].Add(curTest);
            }

            StringBuilder toPrint = new StringBuilder();
            toPrint.Append("Passed: ").Append(testSums[TestResultCode.PASSED].Count)
                .Append(", Error: ").Append(testSums[TestResultCode.ERROR].Count)
                .Append(", Time Out: ").Append(testSums[TestResultCode.TIME_OUT].Count)
                .Append(", Wrong Output: ").Append(testSums[TestResultCode.WRONG_OUTPUT].Count)
                .Append(", Slow: ").Append(testSums[TestResultCode.SLOW].Count)
                .Append(", Total: ").Append(actualResults.Count)
                .Append("\nDetailed Report:\n");
            System.Action<string, TestResultCode> addAllOfAType = (header, testResultCode) => {
                if(testSums[testResultCode].Count > 0) {
                    toPrint.Append(header).Append("\n");
                    foreach (var curTest in testSums[testResultCode]) {
                        curTest.AddTestReport(toPrint).Append("\n");
                    }
                }
            };
            addAllOfAType("Error:", TestResultCode.ERROR);
            addAllOfAType("Timed Out:", TestResultCode.TIME_OUT);
            addAllOfAType("Wrong Output:", TestResultCode.WRONG_OUTPUT);
            addAllOfAType("Slow:", TestResultCode.SLOW);
            addAllOfAType("Passed:", TestResultCode.PASSED);
            Debug.Log(toPrint.ToString());
        }
    }
}
#endif
