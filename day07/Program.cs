using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace day07
{
    class MainClass
    {
        public static Dictionary<string, Instruction> wires = new Dictionary<string, Instruction>();
        public static void Main(string[] args)
        {
            Console.WriteLine("Start parsing.");
            parseData();
            Console.WriteLine("Finished parsing.");
            Console.WriteLine("Start extremely inefficient recursion.");
            ushort answer = evaluateData("a", 0);
            Console.WriteLine("Yay, we're done. Answer is " + answer);
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
            string input = "af AND ah -> ai:NOT lk -> ll:hz RSHIFT 1 -> is:NOT go -> gp:du OR dt -> dv:x RSHIFT 5 -> aa:at OR az -> ba:eo LSHIFT 15 -> es:ci OR ct -> cu:b RSHIFT 5 -> f:fm OR fn -> fo:NOT ag -> ah:v OR w -> x:g AND i -> j:an LSHIFT 15 -> ar:1 AND cx -> cy:jq AND jw -> jy:iu RSHIFT 5 -> ix:gl AND gm -> go:NOT bw -> bx:jp RSHIFT 3 -> jr:hg AND hh -> hj:bv AND bx -> by:er OR es -> et:kl OR kr -> ks:et RSHIFT 1 -> fm:e AND f -> h:u LSHIFT 1 -> ao:he RSHIFT 1 -> hx:eg AND ei -> ej:bo AND bu -> bw:dz OR ef -> eg:dy RSHIFT 3 -> ea:gl OR gm -> gn:da LSHIFT 1 -> du:au OR av -> aw:gj OR gu -> gv:eu OR fa -> fb:lg OR lm -> ln:e OR f -> g:NOT dm -> dn:NOT l -> m:aq OR ar -> as:gj RSHIFT 5 -> gm:hm AND ho -> hp:ge LSHIFT 15 -> gi:jp RSHIFT 1 -> ki:hg OR hh -> hi:lc LSHIFT 1 -> lw:km OR kn -> ko:eq LSHIFT 1 -> fk:1 AND am -> an:gj RSHIFT 1 -> hc:aj AND al -> am:gj AND gu -> gw:ko AND kq -> kr:ha OR gz -> hb:bn OR by -> bz:iv OR jb -> jc:NOT ac -> ad:bo OR bu -> bv:d AND j -> l:bk LSHIFT 1 -> ce:de OR dk -> dl:dd RSHIFT 1 -> dw:hz AND ik -> im:NOT jd -> je:fo RSHIFT 2 -> fp:hb LSHIFT 1 -> hv:lf RSHIFT 2 -> lg:gj RSHIFT 3 -> gl:ki OR kj -> kk:NOT ak -> al:ld OR le -> lf:ci RSHIFT 3 -> ck:1 AND cc -> cd:NOT kx -> ky:fp OR fv -> fw:ev AND ew -> ey:dt LSHIFT 15 -> dx:NOT ax -> ay:bp AND bq -> bs:NOT ii -> ij:ci AND ct -> cv:iq OR ip -> ir:x RSHIFT 2 -> y:fq OR fr -> fs:bn RSHIFT 5 -> bq:0 -> c:14146 -> b:d OR j -> k:z OR aa -> ab:gf OR ge -> gg:df OR dg -> dh:NOT hj -> hk:NOT di -> dj:fj LSHIFT 15 -> fn:lf RSHIFT 1 -> ly:b AND n -> p:jq OR jw -> jx:gn AND gp -> gq:x RSHIFT 1 -> aq:ex AND ez -> fa:NOT fc -> fd:bj OR bi -> bk:as RSHIFT 5 -> av:hu LSHIFT 15 -> hy:NOT gs -> gt:fs AND fu -> fv:dh AND dj -> dk:bz AND cb -> cc:dy RSHIFT 1 -> er:hc OR hd -> he:fo OR fz -> ga:t OR s -> u:b RSHIFT 2 -> d:NOT jy -> jz:hz RSHIFT 2 -> ia:kk AND kv -> kx:ga AND gc -> gd:fl LSHIFT 1 -> gf:bn AND by -> ca:NOT hr -> hs:NOT bs -> bt:lf RSHIFT 3 -> lh:au AND av -> ax:1 AND gd -> ge:jr OR js -> jt:fw AND fy -> fz:NOT iz -> ja:c LSHIFT 1 -> t:dy RSHIFT 5 -> eb:bp OR bq -> br:NOT h -> i:1 AND ds -> dt:ab AND ad -> ae:ap LSHIFT 1 -> bj:br AND bt -> bu:NOT ca -> cb:NOT el -> em:s LSHIFT 15 -> w:gk OR gq -> gr:ff AND fh -> fi:kf LSHIFT 15 -> kj:fp AND fv -> fx:lh OR li -> lj:bn RSHIFT 3 -> bp:jp OR ka -> kb:lw OR lv -> lx:iy AND ja -> jb:dy OR ej -> ek:1 AND bh -> bi:NOT kt -> ku:ao OR an -> ap:ia AND ig -> ii:NOT ey -> ez:bn RSHIFT 1 -> cg:fk OR fj -> fl:ce OR cd -> cf:eu AND fa -> fc:kg OR kf -> kh:jr AND js -> ju:iu RSHIFT 3 -> iw:df AND dg -> di:dl AND dn -> do:la LSHIFT 15 -> le:fo RSHIFT 1 -> gh:NOT gw -> gx:NOT gb -> gc:ir LSHIFT 1 -> jl:x AND ai -> ak:he RSHIFT 5 -> hh:1 AND lu -> lv:NOT ft -> fu:gh OR gi -> gj:lf RSHIFT 5 -> li:x RSHIFT 3 -> z:b RSHIFT 3 -> e:he RSHIFT 2 -> hf:NOT fx -> fy:jt AND jv -> jw:hx OR hy -> hz:jp AND ka -> kc:fb AND fd -> fe:hz OR ik -> il:ci RSHIFT 1 -> db:fo AND fz -> gb:fq AND fr -> ft:gj RSHIFT 2 -> gk:cg OR ch -> ci:cd LSHIFT 15 -> ch:jm LSHIFT 1 -> kg:ih AND ij -> ik:fo RSHIFT 3 -> fq:fo RSHIFT 5 -> fr:1 AND fi -> fj:1 AND kz -> la:iu AND jf -> jh:cq AND cs -> ct:dv LSHIFT 1 -> ep:hf OR hl -> hm:km AND kn -> kp:de AND dk -> dm:dd RSHIFT 5 -> dg:NOT lo -> lp:NOT ju -> jv:NOT fg -> fh:cm AND co -> cp:ea AND eb -> ed:dd RSHIFT 3 -> df:gr AND gt -> gu:ep OR eo -> eq:cj AND cp -> cr:lf OR lq -> lr:gg LSHIFT 1 -> ha:et RSHIFT 2 -> eu:NOT jh -> ji:ek AND em -> en:jk LSHIFT 15 -> jo:ia OR ig -> ih:gv AND gx -> gy:et AND fe -> fg:lh AND li -> lk:1 AND io -> ip:kb AND kd -> ke:kk RSHIFT 5 -> kn:id AND if -> ig:NOT ls -> lt:dw OR dx -> dy:dd AND do -> dq:lf AND lq -> ls:NOT kc -> kd:dy AND ej -> el:1 AND ke -> kf:et OR fe -> ff:hz RSHIFT 5 -> ic:dd OR do -> dp:cj OR cp -> cq:NOT dq -> dr:kk RSHIFT 1 -> ld:jg AND ji -> jj:he OR hp -> hq:hi AND hk -> hl:dp AND dr -> ds:dz AND ef -> eh:hz RSHIFT 3 -> ib:db OR dc -> dd:hw LSHIFT 1 -> iq:he AND hp -> hr:NOT cr -> cs:lg AND lm -> lo:hv OR hu -> hw:il AND in -> io:NOT eh -> ei:gz LSHIFT 15 -> hd:gk AND gq -> gs:1 AND en -> eo:NOT kp -> kq:et RSHIFT 5 -> ew:lj AND ll -> lm:he RSHIFT 3 -> hg:et RSHIFT 3 -> ev:as AND bd -> bf:cu AND cw -> cx:jx AND jz -> ka:b OR n -> o:be AND bg -> bh:1 AND ht -> hu:1 AND gy -> gz:NOT hn -> ho:ck OR cl -> cm:ec AND ee -> ef:lv LSHIFT 15 -> lz:ks AND ku -> kv:NOT ie -> if:hf AND hl -> hn:1 AND r -> s:ib AND ic -> ie:hq AND hs -> ht:y AND ae -> ag:NOT ed -> ee:bi LSHIFT 15 -> bm:dy RSHIFT 2 -> dz:ci RSHIFT 2 -> cj:NOT bf -> bg:NOT im -> in:ev OR ew -> ex:ib OR ic -> id:bn RSHIFT 2 -> bo:dd RSHIFT 2 -> de:bl OR bm -> bn:as RSHIFT 1 -> bl:ea OR eb -> ec:ln AND lp -> lq:kk RSHIFT 3 -> km:is OR it -> iu:iu RSHIFT 2 -> iv:as OR bd -> be:ip LSHIFT 15 -> it:iw OR ix -> iy:kk RSHIFT 2 -> kl:NOT bb -> bc:ci RSHIFT 5 -> cl:ly OR lz -> ma:z AND aa -> ac:iu RSHIFT 1 -> jn:cy LSHIFT 15 -> dc:cf LSHIFT 1 -> cz:as RSHIFT 3 -> au:cz OR cy -> da:kw AND ky -> kz:lx -> a:iw AND ix -> iz:lr AND lt -> lu:jp RSHIFT 5 -> js:aw AND ay -> az:jc AND je -> jf:lb OR la -> lc:NOT cn -> co:kh LSHIFT 1 -> lb:1 AND jj -> jk:y OR ae -> af:ck AND cl -> cn:kk OR kv -> kw:NOT cv -> cw:kl AND kr -> kt:iu OR jf -> jg:at AND az -> bb:jp RSHIFT 2 -> jq:iv AND jb -> jd:jn OR jo -> jp:x OR ai -> aj:ba AND bc -> bd:jl OR jk -> jm:b RSHIFT 1 -> v:o AND q -> r:NOT p -> q:k AND m -> n:as RSHIFT 2 -> at";
            string[] instructions = input.Split(':');
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
