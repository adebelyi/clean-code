﻿using System;
using Markdown.Renderers;

namespace Markdown.Md.HtmlTagHandlers
{
    public class StrongEmphasisHandler : TagHandler
    {
        public override string Handle(Tag tag, bool isOpeningTag)
        {
            if (tag.Type == MdSpecification.StrongEmphasis)
            {
                return isOpeningTag ? "<strong>" : "</strong>";
            }

            if (Successor == null)
            {
                throw new InvalidOperationException(
                    "Can't transfer control to the next chain element because it was null");
            }

            return Successor.Handle(tag, isOpeningTag);
        }
    }
}