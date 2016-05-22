using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace day07
{
    class MainClass
    {
        public static Dictionary<string, Instruction> wires = new Dictionary<string, Instruction>();
        public static void Main(string[] args)
        {
            // Part 1
            parseData();
            ushort answer = evaluateData("a", 0);
            Console.WriteLine("Answer for part 1 is " + answer);
            // Part 2
            wires["b"].op1 = answer.ToString();
            foreach (KeyValuePair<string, Instruction> kvp in wires)
                kvp.Value.defined = false;
            answer = evaluateData("a", 0);
            Console.WriteLine("Answer for part 2 is " + answer);
        }

        public static ushort calc(Operation operation, ushort op1)
        {
            if (operation == Operation.NOT)
                return (ushort)~op1;
            else return op1;
        }
        public static ushort calc(Operation operation, ushort op1, ushort op2)
        {
            if (operation == Operation.AND)
                return (ushort)(op1 & op2);
            else if (operation == Operation.OR)
                return (ushort)(op1 | op2);
            else if (operation == Operation.LSHIFT)
                return (ushort)(op1 << op2);
            else
                return (ushort)(op1 >> op2);
        }

        public static ushort evaluateData(string key, int depth)
        {
            // If we have been passed a number instead of a key, return it as number
            ushort num;
            if (UInt16.TryParse(key, out num))
                return num;

            // Get a reference to the instruction
            Instruction instruction = wires[key];

            // If we already have an instruction with defined value, return the value
            if (instruction.defined)
                return wires[key].val;
            // Else, calculate it recursively
            else
            {
                ushort retVal;
                ushort op1 = evaluateData(instruction.op1, depth+1);
                if (instruction.singleOp)
                    retVal = calc(instruction.operation, op1);
                else
                {
                    ushort op2 = evaluateData(instruction.op2, depth+1);
                    retVal = calc(instruction.operation, op1, op2);
                }
                instruction.defined = true;
				instruction.val = retVal;
                return retVal;
            }
        }

        public static void parseData()
        {
            // Input data
            string[] instructions = File.ReadAllLines("../../input");
            /*
             * Regex for matching EVERYTHING
             * Groups 1, 2, 3 match with AND|OR|LSHIFT|RSHIFT instructions
             *	 Group 1: op1
             *	 Group 2: instruction
             *	 Group 3: op2
             *	 Example: "4 AND ab -> c" ==> Group 1: 4, Group 2: AND, Group 3: ab
             * Group 4 matches "ab" for the case ab-> b
             * Group 5 matches "13" for the case 13 -> a
             * Group 6 matches "ab" for the case NOT ab -> a
             * Group 7 matches the assigned wire "a" in b AND c -> a
             */
            Regex r = new Regex(@"(?:([\w\d]+) (\w+) ([\w\d]+)|([a-z]+)|(\d+)|NOT ([a-z]+)) -> (\w+)");
            foreach (string s in instructions)
            {
                GroupCollection groups = r.Match(s).Groups;
                // The wire we assign to
                string dest = groups[7].ToString();


                // We have an AND|OR|LSHIFT|RSHIFT instruction
                if (groups[2].ToString() != "")
                {
                    string lvalue = groups[1].ToString();
                    string rvalue = groups[3].ToString();
                    string instruction = groups[2].ToString();

                    switch (instruction)
                    {
                        case "AND":
                            wires.Add(dest, new Instruction(Operation.AND, lvalue, rvalue, dest));
                            break;
                        case "OR":
                            wires.Add(dest, new Instruction(Operation.OR, lvalue, rvalue, dest));
                            break;
                        case "LSHIFT":
                            wires.Add(dest, new Instruction(Operation.LSHIFT, lvalue, rvalue, dest));
                            break;
                        case "RSHIFT":
                            wires.Add(dest, new Instruction(Operation.RSHIFT, lvalue, rvalue, dest));
                            break;
                    }
                }
                // We have a wire assigned to a wire
                else if (groups[4].ToString() != "")
                {
					Instruction inst = new Instruction(Operation.ASSIGN, groups[4].ToString(), dest);
					wires [dest] = inst;
                }
                // We have a number assigned to a wire
                else if (groups[5].ToString() != "")
                {
                    Instruction instruction = new Instruction(Operation.ASSIGN, groups[5].ToString(), dest);
                    instruction.defined = true;
                    instruction.val = getNumber(groups[5].ToString());
                    wires[dest] = instruction;
                }
                // We have NOT a wire
                else if (groups[6].ToString() != "")
				{
					Instruction inst = new Instruction(Operation.NOT, groups[6].ToString(), dest);
					wires [dest] = inst;

                }
            }
        }

        public static ushort getNumber(string s)
        {
            ushort num;
            if (UInt16.TryParse(s, out num))
                return num;
            if (wires.ContainsKey(s))
                return wires[s].val;
            else return (ushort)0;
        }

        public enum Operation
        {
            AND,
            OR,
            LSHIFT,
            RSHIFT,
            NOT,
            ASSIGN
        }

        public class Instruction
        {
            public ushort val;
            public string op1;
            public string op2;
            public string destWire;
            public bool defined;
            public bool singleOp;
            public Operation operation;

            public Instruction(Operation operation, string op1, string destWire)
            {
                this.op2 = "";
                setVals(operation, op1, destWire);
            }

            public Instruction(Operation operation, string op1, string op2, string destWire)
            {
                this.op2 = op2;
                setVals(operation, op1, destWire);
            }

            private void setVals(Operation operation, string op1, string destWire)
            {
                defined = false;
                this.destWire = destWire;
                this.operation = operation;
                this.op1 = op1;
                if (operation == Operation.NOT || operation == Operation.ASSIGN)
                    singleOp = true;
                else singleOp = false;
            }
        }

    }
}
