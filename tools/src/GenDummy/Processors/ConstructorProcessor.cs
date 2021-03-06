﻿/*
 * Copyright (c) 2018 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the License);
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an AS IS BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GenDummy.Processors
{
    public class ConstructorProcessor : IProcessor
    {
        public BlockSyntax DummyBlock { get; set; }

        public MemberDeclarationSyntax Process(MemberDeclarationSyntax member)
        {
            MemberDeclarationSyntax newMember = null;

            if (!(member is ConstructorDeclarationSyntax constructor))
            {
                return null;
            }

            if (constructor.Modifiers.ToString().Contains("static"))
            {
                newMember = constructor.WithBody(SyntaxFactory.Block()).WithTrailingTrivia(SyntaxFactory.Whitespace("\r\n"));
            }
            else
            {
                newMember = constructor.WithBody(DummyBlock).WithTrailingTrivia(SyntaxFactory.Whitespace("\r\n"));
            }

            return newMember;
        }
    }
}
