using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace Fosc.Dolphin.Test.Pattern.Behavioral.Command
{
    public class User
    {
        // Initializers
        Calculator calculator = new Calculator();
        ArrayList commands = new ArrayList();

        int current = 0;

        public void Redo(int levels)
        {
            string str = string.Format("\n---- Redo {0} levels ", levels);
            MessageBox.Show(str);
            // Perform redo operations
            for (int i = 0; i < levels; i++)
            {
                if (current < commands.Count - 1)
                {
                    Command command = commands[current++] as Command;
                    command.Execute();
                }
            }
        }

        public void Undo(int levels)
        {
            string str = string.Format("\n---- Undo {0} levels ", levels);
            MessageBox.Show(str);
            // Perform undo operations
            for (int i = 0; i < levels; i++)
            {
                if (current > 0)
                {
                    Command command = commands[--current] as Command;
                    command.UnExecute();
                }
            }
        }

        public void Compute(char @operator, int operand)
        {
            // Create command operation and execute it
            Command command = new CalculatorCommand(
              calculator, @operator, operand);
            command.Execute();

            // Add command to undo list
            commands.Add(command);
            current++;
        }
    }
}
