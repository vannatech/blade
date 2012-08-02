﻿using System.Linq;
using Roslyn.Compilers.Common;
using Roslyn.Compilers.CSharp;

namespace Blade.Compiler.Validation.CSharp
{
    [SyntaxValidator]
    internal class ClassDeclarationValidator: SyntaxValidator<ClassDeclarationSyntax>
    {
        /// <summary>
        /// Validates the syntax node.
        /// </summary>
        /// <param name="node">The node to validate.</param>
        /// <param name="tree">The tree containing the node.</param>
        /// <param name="result">The compilation result.</param>
        public override void Validate(ClassDeclarationSyntax node, CommonSyntaxTree tree, CompilationResult result)
        {
            // check for overloaded constructors
            var ctors = node.Members.OfType<ConstructorDeclarationSyntax>();
            var ctorCount = ctors.Count();

            if (ctors.Any(c => c.Modifiers.Any(m => m.ValueText == "static")))
                ctorCount--;

            if (ctorCount > 1)
                AddError("Constructor overloading is not supported. Consider using optional parameters instead.");
        }
    }
}