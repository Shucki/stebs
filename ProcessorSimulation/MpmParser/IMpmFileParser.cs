﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorSimulation.MpmParser
{
    /// <summary>
    /// Parser, which takes raw configuration strings or configuration files and extract information for the mpm (micro program memory) and the opcode decoder.
    /// </summary>
    public interface IMpmFileParser : IMpmParser
    {
        /// <summary>
        /// Decodes the given instructions file.
        /// </summary>
        /// <param name="instructions">Instructions file in a csv format.</param>
        /// <returns>Parsed instructions.</returns>
        IEnumerable<IInstruction> ParseInstructionsFile(string filename);

        /// <summary>
        /// Decodes the given micro instructions file.
        /// </summary>
        /// <param name="microInstructions">
        /// Logisim ram export file containing the micro instructions.
        /// (This format has to be used beacause of compatibility reasons.)
        /// </param>
        /// <returns>Parsed micro instructions in a dictionary. The key is the micro instructions address.</returns>
        IDictionary<byte, IMicroInstruction> ParseMicroInstructionsFile(string filename);
    }
}
