﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blamite.Blam.Resources;
using Blamite.Blam.Resources.Models;
using Blamite.Blam.ThirdGen.Resources.Models;
using Blamite.Flexibility;
using Blamite.IO;
using Blamite.Util;

namespace Blamite.Blam.ThirdGen.Resources
{
    public class ThirdGenResourceMetaLoader : IResourceMetaLoader
    {
        private BuildInformation _buildInfo;
        private FileSegmentGroup _metaArea;

        public ThirdGenResourceMetaLoader(BuildInformation buildInfo, FileSegmentGroup metaArea)
        {
            _buildInfo = buildInfo;
            _metaArea = metaArea;
        }

        public bool SupportsRenderModels
        {
            get { return _buildInfo.HasLayout("render model"); }
        }

        public IRenderModel LoadRenderModelMeta(ITag modeTag, IReader reader)
        {
            if (modeTag.MetaLocation == null || modeTag.Class == null || modeTag.Class.Magic != ModeMagic)
                throw new ArgumentException("modeTag does not point to metadata for a renderable model");

            if (!SupportsRenderModels)
                throw new NotSupportedException("Render model metadata loading is not supported for the cache file's engine.");

            reader.SeekTo(modeTag.MetaLocation.AsOffset());
            var layout = _buildInfo.GetLayout("render model");
            var values = StructureReader.ReadStructure(reader, layout);
            return new ThirdGenRenderModel(values, reader, _metaArea, _buildInfo);
        }

        private static int ModeMagic = CharConstant.FromString("mode");
    }
}
