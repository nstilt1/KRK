using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

/**
 * Names: Joseph Giambrone, Jacob Cosgrove, Omer Tariq, Noah Stiltner
 */

// namespace line added to make it a program
namespace KRK {
    //public class Calculator : MonoBehaviour {
    public class Calculator { 
        private const double g = 9.80665;
        public Calculator(){}


        /**
         * Stores the small tanks in order by size, by wet mass being 
         * even, and by dry mass by odd index.
         **/
        private double[] smallTanks =
        {
            0.225, // tiny tank wet mass
            0.025, // tiny tank dry mass
            0.45,  // 2*tiny wet
            0.05,  // 2*tiny dry
            0.5625,// 1st 1.5m tank wet mass
            0.0625,// 1st 1.5m tank dry mass
            0.7875,// 1st + tiny wet
            0.0875,// 1st + tiny dry
            1.0125,// 1st + 2*tiny wet
            0.1125,// 1st + 2*tiny dry
            1.125, // 2nd-largest 1.5m tank wet mass
            0.125, //                       dry mass
            1.35,  // 2nd + tiny wet
            0.15,  // 2nd + tiny dry
            1.575, // 2nd + 2*tiny wet
            0.175, // 2nd + 2*tiny dry
            1.6875,// 2nd + 1st wet
            0.1875,// 2nd + 1st dry
            1.9125,// 2nd + 1st + tiny wet
            0.2125,// 2nd + 1st + tiny dry
            2.1375,// 2nd + 1st + 2*tiny wet
            0.2375,// 2nd + 1st + 2*tiny dry
            2.25, // 3rd-largest 1.5m tank wet mass
            0.25, // 3rd-largest 1.5m tank dry mass
            2.475,// 3rd + tiny wet
            0.275,// 3rd + tiny dry
            2.7,  // 3rd + 2*tiny wet
            0.3,  // 3rd + 2*tiny dry
            2.8125,//3rd + 1st wet
            0.3125,//3rd + 1st dry
            3.0375,//3rd + 1st + tiny wet
            0.3375,//3rd + 1st + tiny dry
            3.2625,//3rd + 1st + 2*tiny wet
            0.3625,//3rd + 1st + 2*tiny dry
            3.375, //3rd + 2nd wet
            0.375, //3rd + 2nd dry
            3.6,   //3rd + 2nd + tiny wet
            0.4,   //3rd + 2nd + tiny dry
            3.825, //3rd + 2nd + 2*tiny wet
            0.425, //3rd + 2nd + 2*tiny dry
            3.9375,//3rd + 2nd + 1st wet
            0.4375,//3rd + 2nd + 1st dry
            4.1625,//3rd + 2nd + 1st + tiny wet
            0.4625,//3rd + 2nd + 1st + tiny dry
            4.3875,//3rd + 2nd + 1st + 2*tiny wet
            0.4875,//3rd + 2nd + 1st + 2*tiny dry
            4.5,  // 4th-largest 1.5m tank wet mass
            0.5   // 4th                   dry mass
        };

        /**
         * Makes an engine given the parameters
         * string n is the name
         * double ispA is the ispASL
         * double ispV is the ispVac
         * double thrustA is the thrustASL
         * double thrustV is the thrustVac
         * double m is the mass
         **/
        private Engine[] engines = new Engine[]{
            new Engine("T-1 Toroidal Aerospike", 290, 340, 153.529, 180, 1),
            new Engine("LV-T45", 250, 320, 167.969, 215, 1.5),
            new Engine("LV-909", 85, 345, 14.783, 60, 0.5),
            new Engine("LV-T30", 265, 310, 205.161, 240, 1.25),
            new Engine("S3 KS-25", 295, 315, 936.508, 1000, 4),
            new Engine("LV-N", 185, 800, 13.875, 60, 3)
        };

        private string[] decoded = {
            "Oscar-B Fuel tank",
            "2 Oscar-B Fuel tanks",
            "FL-T100 Fuel tank",
            "FL-T100 Fuel tank + Oscar-B Fuel tank",
            "FL-T100 Fuel tank + 2 Oscar-B Fuel tanks",
            "FL-T200 Fuel tank",
            "FL-T200 Fuel tank + Oscar-B Fuel tank",
            "FL-T200 Fuel tank + 2 Oscar-B Fuel tanks",
            "FL-T200 Fuel tank + FL-T100 Fuel tank",
            "FL-T200 Fuel tank + FL-T100 Fuel tank + Oscar-B Fuel tank",
            "FL-T200 Fuel tank + FL-T100 Fuel tank + 2 Oscar-B Fuel tanks",
            "FL-T400 Fuel tank",
            "FL-T400 Fuel tank + Oscar-B Fuel tank",
            "FL-T400 Fuel tank + 2 Oscar-B Fuel tanks",
            "FL-T400 Fuel tank + FL-T100 Fuel tank",
            "FL-T400 Fuel tank + FL-T100 Fuel tank + Oscar-B Fuel tank",
            "FL-T400 Fuel tank + FL-T100 Fuel tank + 2 Oscar-B Fuel tanks",
            "FL-T400 Fuel tank + FL-T200 Fuel tank",
            "FL-T400 Fuel tank + FL-T200 Fuel tank + Oscar-B Fuel tank",
            "FL-T400 Fuel tank + FL-T200 Fuel tank + 2 Oscar-B Fuel tanks",
            "FL-T400 Fuel tank + FL-T200 Fuel tank + FL-T100 Fuel tank",
            "FL-T400 Fuel tank + FL-T200 Fuel tank + FL-T100 Fuel tank + Oscar-B Fuel tank",
            "FL-T400 Fuel tank + FL-T200 Fuel tank + FL-T100 Fuel tank + 2 Oscar-B Fuel tanks",
            "FL-T800 Fuel tank"
        };

        private double[,] wetTanks = new double[7, 48];
        private double[,] dryTanks = new double[7, 48];
        //private double wetOffset;
        //private double dryOffset;
        // Start is called before the first frame update
        //void Start() {
        //void Main(string[] args) { 
        public void testStart(){
            System.Console.WriteLine("Let's see here...");
            // row is the amount of fuel tank stacks
            for (int row = 0; row < 7; row++) {
                // column is the amount of tanks, tcol is for the wetTank and dryTank index
                int col, tcol;
                for (tcol = 0, col = 0; col < 47; col += 2, tcol++) { // fixed an error where col+1 = 48
                    // adds #(fuel tank stacks)*(total mass of fuel stack)
                    wetTanks[row, tcol] = (row + 1) * smallTanks[col];
                    dryTanks[row, tcol] = (row + 1) * smallTanks[col + 1];
                }
                // adds the wet and dry mass of the largest tank to their offset
                double wetOffset = smallTanks[smallTanks.Length-2]*(row+1); // fixed potential error
                double dryOffset = smallTanks[smallTanks.Length-1]*(row+1);
                // col starts at 48
                for (; col < 96; col += 2, tcol++) {
                    // finishes off the wetTanks and dryTanks initialization
                    wetTanks[row, tcol] = (row + 1) * smallTanks[col - 48] + wetOffset;
                    dryTanks[row, tcol] = (row + 1) * smallTanks[col - 47] + dryOffset;
                }
            }

            int rowLength = wetTanks.GetLength(0);
            int colLength = wetTanks.GetLength(1);

            // tests the 2d array initialization
            /*
            for (int i = 0; i < rowLength; i++) {
                for (int j = 0; j < colLength; j++) {
                    System.Console.Write(string.Format("{0} ", wetTanks[i, j]));
                }
                System.Console.Write(System.Environment.NewLine + System.Environment.NewLine);
            }
            System.Console.ReadLine();
            */






            // Input parameters:
            // double mass, double dv, double minTWR, bool isASL

            System.Console.Write("Enter the mass of your payload in tonnes> ");
            //double inputMass = System.Console.Read();            
            double inputMass;
            if (!double.TryParse(System.Console.ReadLine(), out inputMass)) {
                // .. error with input
                System.Console.WriteLine("Failed to get a valid value");
                return;
            }
            System.Console.Write("\nEnter the target delta-v in m/s> ");
            //double inputDV = System.Console.Read();
            double inputDV;
            if(!double.TryParse(System.Console.ReadLine(), out inputDV)){
                System.Console.WriteLine("Failed to get a valid value");               
                return;
            }
            System.Console.Write("\nEnter the minimum TWR> ");
            //double inputMinTWR = System.Console.Read();
            double inputMinTWR;
            if(!double.TryParse(System.Console.ReadLine(), out inputMinTWR)){
                System.Console.WriteLine("Failed to get a valid value");
                return;
            }
            System.Console.Write("\nIs the stage in a vacuum (y/n)> ");
            char c = System.Console.ReadKey().KeyChar;
            bool b = false;
            bool valid = false;
            do {
                if (c == 'y' || c == 'Y') {
                    b = false;
                    valid = true;
                } else if (c == 'n' || c == 'N') {
                    b = true;
                    valid = true;
                } else{
                    System.Console.Write("\nPlease enter y or n");
                    c = System.Console.ReadKey().KeyChar;
                }
            } while (valid == false);
            


            
            
            /**
             * Testing: Use the following test parameters and comment out the inputting code above, or 
             * manually input the following values. Expect 99.3% or higher accuracy for up to 4000 m/s delta-v.
             **/ 
            /*
            double inputMass = 0.94;
            double inputDV = 4000;
            double inputMinTWR = 1.4;
            bool b = true;
            */

            interpretConfig(getSet(inputMass, inputDV, inputMinTWR, b));
        }
        /**
         * Gets the optimal configuration of engines, amount of engines, and tanks
         * param mass The mass of the stage before
         * param dv The target delta v
         * param minTWR The minimum TWR to achieve
         * param isASL If the stage starts at sea level or not
         * Returns a configuration of {the rocket engine's index, the
         * amount of engines, the fuel tank configuration}
         **/
        double[] getSet(double mass, double dv, double minTWR, bool isASL) {
            /**
             * The optimal configuration
             **/
            double[] config = new double[4];
            /**
             * A list of configurations for all engines
             * row index    details
             * 0            mass
             * 1            amount of engines
             * 2            fuel tank configuration
             * 3            engine index
             **/
            double[,] configs = new double[3, engines.Length];
            for (int en = 0; en < engines.Length; en++) {
                Engine engine = engines[en];
                int firstIndex = -1;
                /**
                 * The ratio (wetMass/dryMass) required to achieve the delta v, based on
                 * dv = ln(wetMass/dryMass)*g*isp
                 */
                double targetRatio;
                // dv/(g*engine.ispASL)
                // (dv/g) * (1/engine.ispASL)
                if (isASL)
                    targetRatio = System.Math.Exp(dv / g / engine.ispASL);
                else
                    targetRatio = System.Math.Exp(dv / g / engine.ispVac);
                double baseWetMass = mass;
                double baseDryMass = mass;
                double engineMass = engine.mass;
                double thrust;
                if (isASL)
                    thrust = engine.thrustASL;
                else
                    thrust = engine.thrustVac;
                int col = 0;
                double totalThrust = thrust;
                double totalEngineMass = 0;
                for (int numEngines = 1; numEngines <= 7; numEngines++) {
                    totalEngineMass += engineMass;

                    double tempWetMass = baseWetMass + totalEngineMass;
                    double tempDryMass = baseDryMass + totalEngineMass;

                    double prevTWR = 0;
                    double currentTWR = 1;
                    if (firstIndex != -1) {
                        double t = wetTanks[numEngines - 2, firstIndex];
                        for (int i = 0; i < 46; i++) {
                            // finds the first mass with greater than or equal to the mass from the last iteration
                            if (wetTanks[numEngines - 1, i] >= t) {
                                col = i;
                                i = 48;
                            }
                        }
                    }else
                        col = 0;
                    for (; col < 48; col++) {
                        double tempWetMass2 = tempWetMass + wetTanks[numEngines - 1, col];
                        double tempDryMass2 = tempDryMass + dryTanks[numEngines - 1, col];
                        currentTWR = totalThrust / tempWetMass2 / g;
                        if (minTWR > currentTWR) {
                            // breaks for loop if it's impossible to get the desired TWR
                            col = 98;
                        } else {
                            // breaks loop if a good config
                            if (tempWetMass2 / tempDryMass2 >= targetRatio) { 
                                
                                if (firstIndex != -1)
                                    firstIndex = col;
                                if (configs[0, en] == 0) {
                                    configs[0, en] = tempWetMass2;
                                    configs[1, en] = numEngines;
                                    configs[2, en] = col;
                                    col = 95;
                                    numEngines = 15;
                                    // when col = 96, a good config has been found
                                }else if (configs[0, en] > tempWetMass2) {
                                    configs[0, en] = tempWetMass2;
                                    configs[1, en] = numEngines;
                                    configs[2, en] = col;
                                    col = 95;
                                    numEngines = 15;
                                    // when col = 96, a good config has been found
                                }
                            
                            }
                        }

                    }
                    
                    if (col != 99) {
                        if(col != 96){
                            double baseWetMass2 = baseWetMass + wetTanks[numEngines - 1, 47]; // changed to 47, and changed variable to baseWetMass
                            double baseDryMass2 = baseDryMass + dryTanks[numEngines - 1, 47];
                            if(firstIndex != -1)
                                col = firstIndex;
                            else
                                col = 0;
                            for (; col < 48; col++) {
                                /*
                                if(col >= wetTanks.GetLength(1) || col < 0){
                                    System.Console.WriteLine("Index (col) out of bounds: " + col);
                                }
                                if(numEngines - 1 >= wetTanks.GetLength(0) || numEngines - 1 < 0){
                                    System.Console.WriteLine("Index (numEngines-1) out of bounds: " + (numEngines-1));
                                }
                                */
                                double tempWetMass2 = baseWetMass2 + wetTanks[numEngines - 1, col ]; // changed col - 48 to col in order to fix index out of bounds
                                double tempDryMass2 = baseDryMass2 + dryTanks[numEngines - 1, col ]; // ^
                                currentTWR = totalThrust / tempWetMass2 / g;
                                if (currentTWR < minTWR) // fixes TWR issue: TWR is always decreasing as the column increases
                                    col = 98;
                                else {
                                    if (tempWetMass2 / tempDryMass2 >= targetRatio) {
                                        if (firstIndex != -1)
                                            firstIndex = col;
                                        if (configs[0, en] != 0) {
                                            configs[0, en] = tempWetMass2;
                                            configs[1, en] = numEngines;
                                            configs[2, en] = col;
                                            col = 95;
                                            numEngines = 15;
                                        }else if(configs[0,en] > tempWetMass2){
                                            configs[0, en] = tempWetMass2;
                                            configs[1, en] = numEngines;
                                            configs[2, en] = col;
                                            col = 95;
                                            numEngines = 15;
                                            // when col = 96, a good config has been found
                                        }
                                        // the ideal config for the engine has been found
                                        //System.Console.WriteLine("Possible config found!");
                                    
                                    }
                                }
                            }
                        }
                    }

                    prevTWR = currentTWR;
                    totalThrust += thrust;
                }
            }
            int minimumIndex = 0;
            double minValue = configs[0, 0];
            // handles when the first value is 0
            if (minValue == 0) {
                
                for (int i = 1; i < configs.GetLength(0); i++) { // index out of bounds fix
                        if(configs[0,i] > 0){ // fixes 0 stacks error
                        if (configs[0, i] != 0) {
                            minValue = configs[0, i];
                            minimumIndex = i;
                            i = configs.Length;
                        }
                    }
                }
                for (int i = minimumIndex; i < configs.GetLength(0); i++) { // index out of bounds fix
                    if(configs[0,i] > 0){ // fixes 0 stacks error 
                        if (configs[0, i] < minValue) {
                            if (minValue > 0) {
                                minimumIndex = i;
                                minValue = configs[0, i];
                            }
                        }
                    }
                }
                /**
                * row index    details
                * 0            mass
                * 1            amount of engines
                * 2            fuel tank configuration
                * 3            the engine type
                * 4            engine index
                **/
                config[0] = configs[0, minimumIndex];
                config[1] = configs[1, minimumIndex];
                config[2] = configs[2, minimumIndex];
                config[3] = minimumIndex;
                return config;
            } else {
                for (int i = 1; i < configs.GetLength(0); i++) { // index out of bounds fix
                    if(configs[0,i] > 0){ // fixes 0 stacks error
                        if (configs[0, i] < minValue) {
                            if (minValue > 0) {
                                minimumIndex = i;
                                minValue = configs[0, i];
                            }
                        }
                    }
                }
                /**
                * row index    details
                * 0            mass
                * 1            amount of engines
                * 2            fuel tank configuration
                * 3            the engine type
                * 4            engine index
                **/
                config[0] = configs[0, minimumIndex];
                config[1] = configs[1, minimumIndex];
                config[2] = configs[2, minimumIndex];
                config[3] = minimumIndex;
                return config;
            }
            
        }
        void interpretConfig(double[] config) {
            //System.Console.Write(string.Format("{0} ", wetTanks[i, j]));
            if (config[1] == 1) {
                System.Console.Write("Use one fuel stack with ");
            } else {
                System.Console.Write("Use " + config[1] + " fuel stacks with ");
            }
            if (config[2] >= 24) {
                System.Console.Write("a FL-T800 and " + decoded[System.Convert.ToInt16(config[2]) - 24] + " the engine ");
            } else {
                System.Console.Write("a " + decoded[System.Convert.ToInt16(config[2])] + " and an ");
            }
            System.Console.Write(engines[System.Convert.ToInt16(config[3])].name + " on each stack");
        }

        private class Engine {
            public string name;
            public double ispASL;
            public double ispVac;
            public double thrustASL;
            public double thrustVac;
            public double mass;
            public Engine() { }
            /**
             * Makes an engine given the parameters
             * string n is the name
             * double ispA is the ispASL
             * double ispV is the ispVac
             * double thrustA is the thrustASL
             * double thrustV is the thrustVac
             * double m is the mass
             **/
            public Engine(string n, double ispA, double ispV, double thrustA, double thrustV, double m) {
                name = n;
                ispASL = ispA;
                ispVac = ispV;
                thrustASL = thrustA;
                thrustVac = thrustV;
                mass = m;
            }
        }
    }
}