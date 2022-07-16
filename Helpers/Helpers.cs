﻿using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gmtkjame2022rollthedice.Helpers
{
    public static class Helpers
    {

        /// <summary>
        /// Get all children of a given type from a specified root node
        /// </summary>
        /// <typeparam name="ChildType"></typeparam>
        /// <param name="rootNode"></param>
        /// <returns></returns>
        public static IEnumerable<ChildType> GetChildrenOfType<ChildType>(Node rootNode)
        {
            foreach (ChildType child in rootNode.GetChildren())
            {
                if (child is ChildType)
                {
                    yield return child;
                }

            }
        }

    }
}