using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Names: Joseph Giambrone, Jacob Cosgrove, Omer Tariq, Noah Stiltner
 */

public class Calculator : MonoBehaviour {
    // The minimum change in delta-m
    private const double min_dm = 0.05;
    private const double g = 9.80665;

    private const double min_wet_dry_ratio = 1.05;
    private const double delta_wet_dry = 0.05;
    
    /**
     * Stores the small tanks in order by size, by wet mass being 
     * even, and by dry mass by odd index.
     **/
    private double[] smallTanks =
    {
        0.225, // tiny tank wet mass
        0.025, // tiny tank dry mass
        0.5625,// smallest 1.5m tank wet mass
        0.0625,// smallest 1.5m tank dry mass
        1.125, // next-largest 1.5m tank wet mass
        0.125, // ...
        2.25,
        0.25,
        4.5,
        0.5
    };

    private double[,] wetTanks = new double[7, 17];
    private double[,] dryTanks = new double[7, 17];
    private double wetOffset;
    private double dryOffset;
    // Start is called before the first frame update
    void Start() {
        // row is the amount of fuel tank stacks
        for (int row = 0; row < 7; row++) {
            // column is the amount of tanks
            int col;
            for (col = 0; col < 10; col += 2) {
                wetTanks[row, col] = (row + 1) * smallTanks[col];

                dryTanks[row, col] = (row + 1) * smallTanks[col + 1];
            }
            wetOffset = wetTanks[row, col - 1];
            dryOffset = dryTanks[row, col - 1];
            // col starts at 10
            for (; col < 17; col += 2) {
                wetTanks[row, col] = (row + 1) * smallTanks[col - 10] + wetOffset;
                dryTanks[row, col] = (row + 1) * smallTanks[col - 9];
            }
        }

        int rowLength = wetTanks.GetLength(0);
        int colLength = wetTanks.GetLength(1);

        for (int i = 0; i < rowLength; i++) {
            for (int j = 0; j < colLength; j++) {
                System.Console.Write(string.Format("{0} ", wetTanks[i, j]));
            }
            System.Console.Write(System.Environment.NewLine + System.Environment.NewLine);
        }
        System.Console.ReadLine();

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
        double[] config = new double[3];
        /**
         * A list of configurations for all engines
         * row index    details
         * 0            mass
         * 1            amount of engines
         * 2            fuel tank configuration
         **/
        double[,] configs = new double[3, engines.Length()];
        foreach(Engine engine in engines){
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
            double engineMass = engine.getModuleMass();
            double thrust;
            if (isASL)
                thrust = engine.thrustASL;
            else
                thrust = engine.thrustVac;
            int col = 0;
            double totalThrust = thrust;
            double totalEngineMass = 0;
            for(int numEngines = 1; numEngines <= 7; numEngines++) {
                totalEngineMass += engineMass;

                double tempWetMass = baseWetMass + totalEngineMass;
                double tempDryMass = baseDryMass + totalEngineMass;

                double prevTWR = 0;
                double currentTWR = 1;
                if (firstIndex != -1) {
                    double t = wetTanks[numEngines - 2, firstIndex];
                    for(int i = 0; i < 17; i++) {
                        if(wetTanks[numEngines - 1, i] >= t) {
                            col = i;
                            i = 17;
                        }
                    }
                }
                for(; col < 10; col++) {
                    double tempWetMass2 = tempWetMass + wetTanks[numEngines - 1, col];
                    double tempDryMass2 = tempDryMass + dryTanks[numEngines - 1, col];
                    currentTWR = totalThrust / tempWetMass2 / g;
                    if (prevTWR > currentTWR) {
                        // breaks all for loops if the TWR is decreasing when fuel is added
                        col = 99;
                    } else {
                        if (tempWetMass2 / tempDryMass2 >= targetRatio) {
                            if (currentTWR > minTWR) {
                                if (firstIndex != -1)
                                    firstIndex = col;
                                if (configs[0, engine.index] != 0) {
                                    if (configs[0, engine.index] > tempWetMass2) {
                                        configs[0, engine.index] = tempWetMass2;
                                        configs[1, engine.index] = numEngines;
                                        configs[2, engine.index] = col;
                                    }
                                }
                            }
                        }
                    }
                    
                }
                if(col != 99) {
                    double baseWetMass2 = tempWetMass + wetTanks[numEngines - 1, 9];
                    double baseDryMass2 = tempDryMass + dryTanks[numEngines - 1, 9];
                    for(;col < 17; col++) {
                        double tempWetMass2 = baseWetMass2 + wetTanks[numEngines - 1, col - 10];
                        double tempDryMass2 = baseDryMass2 + dryTanks[numEngines - 1, col - 10];
                        currentTWR = totalThrust / tempWetMass2 / g;
                        if (prevTWR > currentTWR)
                            col = 99;
                        else {
                            if(tempWetMass/tempDryMass >= targetRatio) {
                                if(currentTWR > minTWR) {
                                    if (firstIndex != -1)
                                        firstIndex = col;
                                    if(configs[0,engine.index] != 0) {
                                        configs[0, engine.index] = tempWetMass2;
                                        configs[1, engine.index] = numEngines;
                                        configs[2, engine.index] = col;
                                    }
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
        for(int i = 1; i < configs.Length; i++) {
            if(configs[0,i] < minValue) {
                minimumIndex = i;
                minValue = configs[0, i];
            }
        }
        config[0] = minimumIndex;
        config[1] = configs[1, minimumIndex];
        config[2] = configs[2, minimumIndex];

        return config;
    }
    

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
