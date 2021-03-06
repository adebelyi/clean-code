﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Markdown.Md.TagHandlers
{
    public class EmphasisHandler : TokenHandler
    {
        public override Token Handle(string str, int position, ImmutableStack<Token> openingTokens)
        {
            var result = Recognize(str, position);

            if (result == null)
            {
                if (Successor == null)
                {
                    throw new InvalidOperationException(
                        "Can't transfer control to the next chain element because it was null");
                }

                return Successor.Handle(str, position, openingTokens);
            }

            result.Value = MdSpecification.Tags[result.Type];

            return result;
        }

        private static Token Recognize(string str, int position)
        {
            if (MdSpecification.IsEscape(str, position))
            {
                return null;
            }

            if (IsEmphasis(str, position)
                && IsClosedEmphasis(str, position)
                && (position - 1 < 0 || str[position - 1] != '_'))
            {
                return new Token(MdSpecification.Emphasis, "", TokenPairType.Close);
            }

            if (IsEmphasis(str, position)
                && IsOpenedEmphasis(str, position))
            {
                return new Token(MdSpecification.Emphasis, "", TokenPairType.Open);
            }

            return null;
        }

        private static bool IsOpenedEmphasis(string str, int position)
        {
            return
                position + 1 < str.Length
                && !char.IsWhiteSpace(str[position + 1])
                && !char.IsNumber(str[position + 1])
                && (position - 1 < 0 || !char.IsLetter(str[position - 1]));
        }

        private static bool IsClosedEmphasis(string str, int position)
        {
            return
                position - 1 >= 0
                && !char.IsWhiteSpace(str[position - 1])
                && !char.IsNumber(str[position - 1])
                && (position + 1 >= str.Length || !char.IsLetter(str[position + 1]));
        }

        private static bool IsEmphasis(string str, int position)
        {
            return str[position] == '_';
        }
    }
}